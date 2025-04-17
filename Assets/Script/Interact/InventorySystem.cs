using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public GameObject itemPrefab;
        public int quantity;

        public InventoryItem(GameObject prefab)
        {
            itemPrefab = prefab;
            quantity = 1;
        }
    }

    public static InventorySystem instance;

    [Header("Settings")]
    public int maxSlot = 4;

    [Header("Inventory Data")]
    public List<InventoryItem> items = new List<InventoryItem>();
    public bool isOpen;

    [Header("UI")]
    public GameObject ui_Window;
    public Image[] itemImage;
    public TextMeshProUGUI[] itemCountTexts;

    [Header("Description")]
    public GameObject ui_Description;
    public Image DescriptionImage;
    public TextMeshProUGUI Itemname;
    public TextMeshProUGUI DescriptionItem;

    [Header("Popup")]
    public GameObject fullInventoryPopup;
    public float popupDuration = 2f;

    public GameObject itemReceivedPopup;
    public Image GotItem;
    public TextMeshProUGUI itemReceivedText;
    public float itemReceivedPopupDuration = 2f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool CanPickUp(GameObject item)
    {
        // กรณีรองรับ Stack
        string itemName = item.name.Replace("(Clone)", "");

        foreach (var invItem in items)
        {
            if (invItem.itemPrefab.name == itemName)
            {
                return true; // ซ้อนเพิ่มได้
            }
        }

        return !IsFull(); // ถ้าไม่มีของนี้เลย ต้องมีช่องว่าง
    }
    private void Start()
    {
        LoadInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("🧪 เรียก Popup ทดสอบ");
            ShowFullInventoryPopup();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);
        UpdateInventoryUI();
    }

    public void AddItem(string itemName)
    {
        GameObject itemToAddPrefab = Resources.Load<GameObject>("Items/" + itemName);

        if (itemToAddPrefab == null)
        {
            Debug.LogError("Item not found: " + itemName);
            return;
        }

        Sprite itemSprite = itemToAddPrefab.GetComponent<SpriteRenderer>()?.sprite;

        foreach (var invItem in items)
        {
            if (invItem.itemPrefab.name == itemToAddPrefab.name)
            {
                invItem.quantity++;
                UpdateInventoryUI();
                ShowItemReceivedPopup(itemName, itemSprite);

                // ✅ แจ้ง GameManager ว่าเราเก็บไอเท็มนี้แล้ว
                Gamemanager.Instance?.CollectItem(itemName);

                return;
            }
        }

        if (IsFull())
        {
            ShowFullInventoryPopup();
            return;
        }

        InventoryItem newItem = new InventoryItem(itemToAddPrefab);
        items.Add(newItem);
        UpdateInventoryUI();
        ShowItemReceivedPopup(itemName, itemSprite);

        // ✅ แจ้ง GameManager
        Gamemanager.Instance?.CollectItem(itemName);
    }

    public void PickUp(GameObject item)
    {
        string itemName = item.GetComponent<Item>().itemID;
        AddItem(itemName);
        //item.SetActive(false);
    }

    public bool IsFull()
    {
        return items.Count >= itemImage.Length;
    }

    public void UpdateInventoryUI()
    {
        HideAll();

        for (int i = 0; i < items.Count && i < itemImage.Length && i < itemCountTexts.Length; i++)
        {
            Sprite sprite = items[i].itemPrefab.GetComponent<SpriteRenderer>()?.sprite;

            if (sprite != null)
            {
                itemImage[i].sprite = sprite;
                itemImage[i].gameObject.SetActive(true);

                if (items[i].quantity > 1)
                {
                    itemCountTexts[i].text = items[i].quantity.ToString();
                    itemCountTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    itemCountTexts[i].text = "";
                    itemCountTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public bool HasItem(string itemName)
    {
        foreach (var invItem in items)
        {
            if (invItem.itemPrefab.name == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemPrefab.name == itemName)
            {
                items[i].quantity--;
                if (items[i].quantity <= 0)
                {
                    items.RemoveAt(i);
                }

                UpdateInventoryUI();
                return;
            }
        }
    }
    void HideAll()
    {
        for (int i = 0; i < itemImage.Length; i++)
        {
            if (i < itemImage.Length)
                itemImage[i].gameObject.SetActive(false);

            if (i < itemCountTexts.Length)
                itemCountTexts[i].gameObject.SetActive(false);
        }
    }
    public void ShowDescription(int id)
    {
        if (id < 0 || id >= items.Count)
        {
            Debug.LogWarning("Invalid item ID");
            return;
        }

        DescriptionImage.sprite = itemImage[id].sprite;
        Itemname.text = items[id].itemPrefab.name;
        DescriptionItem.text = items[id].itemPrefab.GetComponent<Item>().descriptionText;

        ui_Description.SetActive(true);
    }

    public void HideDescription()
    {
        ui_Description.SetActive(false);
    }

    public void ShowFullInventoryPopup()
    {
        Debug.Log("showFullInventoryPopup!");

        if (fullInventoryPopup == null)
        {
            Debug.LogWarning("❗ fullInventoryPopup no!");
            return;
        }
        fullInventoryPopup.SetActive(true);
        CancelInvoke(nameof(HideFullInventoryPopup));
        Invoke(nameof(HideFullInventoryPopup), popupDuration);
    }

    void HideFullInventoryPopup()
    {
        fullInventoryPopup.SetActive(false);
    }

    public void ShowItemReceivedPopup(string itemName, Sprite itemSprite)
    {
        if (itemReceivedPopup == null) return;

        itemReceivedText.text = "You Got " + itemName;

        if (GotItem != null && itemSprite != null)
        {
            GotItem.sprite = itemSprite;
            GotItem.gameObject.SetActive(true);
        }

        itemReceivedPopup.SetActive(true);
        CancelInvoke(nameof(HideItemReceivedPopup));
        Invoke(nameof(HideItemReceivedPopup), itemReceivedPopupDuration);
    }

    void HideItemReceivedPopup()
    {
        itemReceivedPopup.SetActive(false);
    }

    public void SaveInventory()
    {
        List<string> itemNames = new List<string>();
        List<int> itemQuantities = new List<int>();

        foreach (var invItem in items)
        {
            itemNames.Add(invItem.itemPrefab.name.Replace("(Clone)", ""));
            itemQuantities.Add(invItem.quantity);
        }

        string json = JsonUtility.ToJson(new InventoryData(itemNames, itemQuantities));
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory")) return;

        string json = PlayerPrefs.GetString("Inventory");
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);

        items.Clear();

        for (int i = 0; i < data.itemNames.Count; i++)
        {
            GameObject prefab = Resources.Load<GameObject>("Items/" + data.itemNames[i]);
            if (prefab != null)
            {
                InventoryItem loadedItem = new InventoryItem(prefab);
                loadedItem.quantity = data.itemQuantities[i];
                items.Add(loadedItem);
            }
        }

        UpdateInventoryUI();
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<string> itemNames;
        public List<int> itemQuantities;

        public InventoryData(List<string> names, List<int> quantities)
        {
            itemNames = names;
            itemQuantities = quantities;
        }
    }
}
