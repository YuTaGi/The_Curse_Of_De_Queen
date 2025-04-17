using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PointerController : MonoBehaviour
{
    public GameObject gameCanvasToClose;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI hintText;
   
    public Transform PointA;
    public Transform PointB;
    public RectTransform Safexone;
    public float moveSpeed = 100f;

   
    private int successCount = 0; 
    private int requiredSuccesses = 3;
    private float direction = 1f;
    private RectTransform pointerTranform;
    private Vector3 targetposition;
    public GameObject successPopup;
    public float popupDuration = 2f;

    public PlayerHealth Health;
    public float damagePerMismatch = 10f;

    public string rewardItem = "Pot";
    public static string LastReward = null;
    public static bool hasSucceeded = false;
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
    void Start()
    {
        pointerTranform = GetComponent<RectTransform>();
        targetposition = PointB.position;

        successCount = 0; 
        UpdateStatusText();

        hintText.text = "Press Spacebar to play!";
    }

    public void StartMiniGame()
    {
        successCount = 0;
        UpdateStatusText();
        hasSucceeded = false;
        pointerTranform.position = PointA.position;
        targetposition = PointB.position;
        if (gameCanvasToClose != null)
            gameCanvasToClose.SetActive(true);
    }

    void UpdateStatusText()
    {
        if (statusText != null)
        {
            statusText.text = $"Success: {successCount} / {requiredSuccesses}";
        }
    }
    // Update is called once per frame
    void Update()
    {
        pointerTranform.position = Vector3.MoveTowards(pointerTranform.position, targetposition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTranform.position, PointA.position) < 0.1f)
        {
            targetposition = PointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTranform.position, PointB.position) < 0.1f)
        {
            targetposition = PointA.position;
            direction = -1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            CheckSuccess();
        }
       
    }

    void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(Safexone, pointerTranform.position, null))
        {
            successCount++;
            Debug.Log("✅ Success " + successCount + "/" + requiredSuccesses);
            UpdateStatusText();
            LastReward = rewardItem;

            if (successCount >= requiredSuccesses)
            {
                GiveReward();
                hasSucceeded = true;
                ShowSuccessPopup();
                StartCoroutine(WaitAndContinue());
            }
        }
        else
        {
            // พลาด รีเซ็ตใหม่
            successCount = 0;
            Debug.Log("❌ Missed! Reset to 0.");
            if (Health != null)
            {
                Health.TakeDamage(damagePerMismatch); // 💥 ลดเลือดเมื่อจับผิด
            }
            UpdateStatusText();
        }
    }

    void GiveReward()
    {
        Debug.Log(" Trying to give reward: " + rewardItem);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            InventorySystem inventory = player.GetComponent<InventorySystem>();
            if (inventory != null)
            {
                GameObject itemPrefab = Resources.Load<GameObject>("Items/" + rewardItem);
                if (itemPrefab != null)
                {
                    GameObject spawnedItem = Instantiate(itemPrefab);

                    if (inventory.CanPickUp(spawnedItem))
                    {
                        FindObjectOfType<InteractionSystem>().PickUpItem(spawnedItem);
                        Debug.Log("✅ Added item to inventory: " + rewardItem);
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ Cannot pick up item (inventory full?): " + rewardItem);
                        Destroy(spawnedItem);
                    }
                }
                else
                {
                    Debug.LogWarning("⚠️ Could not find item prefab in Resources/Items/: " + rewardItem);
                }
            }
            else
            {
                Debug.LogWarning("⚠️ InventorySystem not found on Player.");
            }
        }

        // 🎯 ทำลายวัตถุผ่าน Roommanager
        if (roomManager != null)
        {
            roomManager.DestroyAfterMiniGameWithSound();
        }
        else
        {
            Debug.LogWarning("⚠️ roomManager ไม่ถูกตั้งค่าใน Inspector!");
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

    IEnumerator WaitAndContinue()
    {
        yield return new WaitForSeconds(1f);

        if (gameCanvasToClose != null)
        {
            gameCanvasToClose.SetActive(false);
        }
    }
}

