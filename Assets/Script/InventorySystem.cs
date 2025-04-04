using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();

    public void PickUp(GameObject item)
    {
        items.Add(item);
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
}
