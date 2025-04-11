using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CardController : MonoBehaviour
{
    [SerializeField] Transform gridTransform;
    [SerializeField] Card cardPrefab;
    [SerializeField] Sprite[] sprites;

    public GameObject miniGamePanel;
    private List<Sprite> spritePairs;
    private Card firstSelected;
    private Card secondSelected;
    private int matchedPairs = 0;
    public GameObject successPopup;
    public float popupDuration = 2f;

    public Roommanager roomManager;

    public void OnMiniGameSuccess()
    {
        Debug.Log("✅ Mini-game complete!");

        // เรียกทำลายออบเจกต์หลังมินิเกม
        if (roomManager != null)
        {
            roomManager.DestroyAfterMiniGameWithSound();
        }
    }
    private void Start()
    {
        PrepareSprite();
        CreatCards();
    }

    


    private void PrepareSprite()
    {
        spritePairs = new List<Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }
        ShuffleSprites(spritePairs);
    }

    void CreatCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.Cardcontroller = this;
        }
    }

    public void SetSelected(Card card)
    {
        if (!card.isSelected)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }
            if (secondSelected == null)
            {
                secondSelected = card;
                StartCoroutine(CheckMatching(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }
        }
    }

    IEnumerator CheckMatching(Card a, Card b)
    {
        yield return new WaitForSeconds(0.5f);

        if (a.GetIconSprite() == b.GetIconSprite())
        {
            matchedPairs++;
            a.Disable(); // ปิดไม่ให้คลิกได้อีก (คุณอาจมี method นี้ใน Card)
            b.Disable();

            // เช็คว่าจบเกมหรือยัง
            if (matchedPairs == sprites.Length)
            {
                yield return new WaitForSeconds(0.5f);
                miniGamePanel.SetActive(false);

                // แจกไอเท็ม
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
                            Debug.Log("✅ Received item: " + rewardItemName);
                        }
                        else
                        {
                            Debug.Log("❌ Inventory full!");
                            Destroy(newItem);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("⚠️ Could not find item prefab in Resources/Items/: " + rewardItemName);
                }

                Roommanager roomManager = FindObjectOfType<Roommanager>();
                if (roomManager != null)
                {
                    roomManager.DestroyAfterMiniGameWithSound();
                }
            }
        }
        else
        {
            a.Hide(); // กลับการ์ด
            b.Hide();
        }
    }
    void ShowSuccessPopup()
    {
        if (successPopup != null)
        {
            successPopup.SetActive(true);
            CancelInvoke(nameof(HideSuccessPopup));
            Invoke(nameof(HideSuccessPopup), popupDuration);
        }
    }

    void HideSuccessPopup()
    {
        successPopup.SetActive(false);
    }

    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}
