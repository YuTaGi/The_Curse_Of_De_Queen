using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public bool[] isFull;
    public GameObject[] slots;
    public void PickUp(GameObject item)
    {
        for (int i = 0; i < isFull.Length; i++)
        {
            if (!isFull[i])
            {
               
                items.Add(item);
                isFull[i] = true;

               
                Image icon = slots[i].transform.GetChild(0).GetComponent<Image>();
                Sprite itemSprite = item.GetComponent<SpriteRenderer>()?.sprite;

                if (itemSprite != null)
                {
                    icon.sprite = itemSprite;
                    icon.enabled = true;
                }

               
                item.SetActive(false);
                return;
            }
        }

        UpdateInventoryUI();
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
    public void UpdateInventoryUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Image icon = slots[i].transform.GetChild(0).GetComponent<Image>();

            if (i < items.Count)
            {
                Sprite itemSprite = items[i].GetComponent<SpriteRenderer>()?.sprite;
                icon.sprite = itemSprite;
                icon.enabled = true;
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
            }
        }
    }
}
