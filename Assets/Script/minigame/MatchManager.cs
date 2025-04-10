using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [SerializeField] LineRenderer linePrefab;
    [SerializeField] Transform linecontainer;

    [SerializeField] LineRenderer draggingline;
    MatchCard draggingcard;


    public int Paircout = 4;
    LineRenderer[] lines;

    List<Connection> connectedPairs = new List<Connection>();
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateLines();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(draggingcard != null || draggingline.enabled)
            {
                draggingcard.Hightlight(false);
                draggingcard = null;
                draggingline.enabled = false;
            }
        }
    }

    void GenerateLines()
    {
        lines = new LineRenderer[Paircout];
        for (int i = 0; i < Paircout; i++)
        {
            var line = Instantiate(linePrefab, linecontainer);
            line.enabled = false;
            lines[i] = line;
        }
    }
    LineRenderer GetLineFromPool()
    {
        foreach (LineRenderer line in lines)
        {
            if (line.enabled == false)
            {
                return line;
            }
        }
        return null;
    }
    public void CardPicked(MatchCard card)
    {
        RemoveConnectionAlready(card);
        draggingcard = card;
        draggingcard.Hightlight(true);

        Vector2 cardPos = card.transform.position;
        draggingline.SetPosition(0, cardPos);
        draggingline.SetPosition(1, cardPos);

        if(draggingline.enabled == false)
        {
            draggingline.enabled = true;
        }
    }

    public void Dragging()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        draggingline.SetPosition(1, pos);
    }
    public void DroppedOnCard(MatchCard card)
    {
        if (draggingcard == null || card == null)
        {
            Debug.LogWarning("DroppedOnCard called, but draggingcard or target card is null.");
            return;
        }

        if (draggingcard.GroupName == card.GroupName)
        {
            return;
        }

        RemoveConnectionAlready(card);

        if (draggingline.enabled == true)
        {
            draggingline.enabled = false;
        }

        Connection connection = new Connection(draggingcard, card, GetLineFromPool());
        connectedPairs.Add(connection);
        draggingcard = null;

        ValidatePairs();
    }
    void RemoveConnectionAlready(MatchCard card)
    {
        foreach (Connection connection in connectedPairs)
        {
            if (connection.A == card || connection.B == card)
            {
                connection.A.Hightlight(false);
                connection.B.Hightlight(false);

                connection.connectedWith.enabled = false;
                connection.connectedWith = null;
                connection.A = null;
                connection.B = null;

                connectedPairs.Remove(connection);
                break;
            }
           
        }
    }

    void ValidatePairs()
    {
        if (connectedPairs.Count >= Paircout)
        {
            bool allRight = true;
            foreach (var pair in connectedPairs)
            {
                if (pair.A.name != pair.B.name)
                {
                    allRight = false;
                    pair.connectedWith.startColor = Color.red;
                    pair.connectedWith.endColor = Color.red;
                }
                else
                {
                    pair.connectedWith.startColor = Color.green;
                    pair.connectedWith.endColor = Color.green;
                }
            }

            if (allRight)
            {
                Debug.Log("✅ All pairs matched correctly!");

                // 🎁 แจกไอเท็มหลังชนะ
                string rewardItemName = "Pot";
                GameObject itemPrefab = Resources.Load<GameObject>("Items/" + rewardItemName);
                if (itemPrefab != null)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    InventorySystem inventory = player.GetComponent<InventorySystem>();

                    if (inventory != null)
                    {
                        GameObject newItem = Instantiate(itemPrefab);
                        if (inventory.CanPickUp(newItem))
                        {
                            FindObjectOfType<InteractionSystem>().PickUpItem(newItem);
                            Debug.Log("🎁 Rewarded item: " + rewardItemName);
                        }
                        else
                        {
                            Debug.Log("❌ Inventory full!");
                            Destroy(newItem);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ InventorySystem not found on Player.");
                    }
                }
                else
                {
                    Debug.LogWarning("⚠️ Could not find item prefab: " + rewardItemName);
                }

                // 🔥 ทำลายวัตถุ
                Roommanager roomManager = FindObjectOfType<Roommanager>();
                if (roomManager != null)
                {
                    roomManager.DestroyAfterMiniGameWithSound();

                    // ✅ ปิด Matching Canvas
                    if (roomManager.canvasToOpen != null)
                    {
                        Debug.Log("Closing Matching Canvas...");
                        roomManager.canvasToOpen.SetActive(false);
                    }

                    // ✅ เปิด Canvas ปกติกลับขึ้นมา (ถ้ามี)
                    if (roomManager.canvasToClose != null)
                    {
                        Debug.Log("Reopening main canvas...");
                        roomManager.canvasToClose.SetActive(true);
                    }
                }
            }
        }
    }

        public void OnDestroy()
    {
        Instance = null;
    }
}
public class Connection
{
    public MatchCard A;
    public MatchCard B;
    public LineRenderer connectedWith;
    public Connection(MatchCard A, MatchCard B, LineRenderer connectedWith)
    {
        this.A = A;
        this.B = B;
        this.connectedWith = connectedWith;

        Vector2 CardAPos = A.transform.position;
        Vector2 CardBPos = B.transform.position;

        A.Hightlight(true);
        B.Hightlight(true);

        connectedWith.SetPosition(0, CardAPos);
        connectedWith.SetPosition(1, CardBPos);
        connectedWith.enabled = true;
    }
}
