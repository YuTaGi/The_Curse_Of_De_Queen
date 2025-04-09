using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public int maxSlot = 4;
    public List<GameObject> items = new List<GameObject>();
   public bool isOpen;
    public static InventorySystem instance;
    public bool[] isFull;
    [Header("UI Item")]
    public GameObject ui_Window;
    public Image[] itemImage;

    [Header("UI description")]
    public GameObject ui_Description;
    public Image DescriptionImage;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI Itemname;
    [Header("UI Warning")]
    public GameObject fullInventoryPopup;
    public float popupDuration = 2f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
    void ToggleInventory()
    {
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);
    }

    public void PickUp(GameObject item)
    {
        if (IsFull())
        {
            ShowFullInventoryPopup();
            return;
        }
        items.Add(item);

        UpdateInventoryUI();

        item.SetActive(false);
    }
    public bool TryPickUp(GameObject item)
    {
        if (IsFull())
        {
            ShowFullInventoryPopup();
            return false;
        }

        items.Add(item);
        UpdateInventoryUI();
        return true;
    }
    public bool HasItem(string itemName)
    {
        foreach (GameObject item in items)
        {
            if (item.name == itemName)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsFull()
    {
        return items.Count >= itemImage.Length;
    }

    public void UpdateInventoryUI()
    {
        HideAll();
        for (int i = 0; i < items.Count && i < itemImage.Length; i++)
        {
            Sprite sprite = items[i].GetComponent<SpriteRenderer>()?.sprite;
            if (sprite != null)
            {
                itemImage[i].sprite = sprite;
                itemImage[i].gameObject.SetActive(true);
            }

        }
    }
    void HideAll()
    {
        foreach(var i in itemImage)
        {
            i.gameObject.SetActive(false);
        }
    }
    public void ShowDescription(int id)
    {
        if (id < 0 || id >= items.Count)
        {
            Debug.LogWarning("No ID");
            return;
        }

        DescriptionImage.sprite = itemImage[id].sprite;
        Itemname.text = items[id].name;
        descriptionText.text = items[id].name;

        DescriptionImage.gameObject.SetActive(true);
        Itemname.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);

    }
    public void ShowFullInventoryPopup()
    {
        if (fullInventoryPopup == null) return;

        fullInventoryPopup.SetActive(true);
        CancelInvoke(nameof(HideFullInventoryPopup));
        Invoke(nameof(HideFullInventoryPopup), popupDuration);
    }

    void HideFullInventoryPopup()
    {
        fullInventoryPopup.SetActive(false);
    }
    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == itemName)
            {
                items.RemoveAt(i);
                UpdateInventoryUI();
                break;
            }
        }
    }
   
   

}
