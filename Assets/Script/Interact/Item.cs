using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionTypr { NONE, PickUp, Examine }
    public InteractionTypr type;

    public string itemID;
    public string descriptionText;
    public UnityEvent customEvent;

   
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }
   

    public void Interact()
    {
        switch (type)
        {
            case InteractionTypr.PickUp:
                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);
                if (!string.IsNullOrEmpty(itemID))
                {
                    CollectItem.instance.MarkCollected(itemID);
                }
                gameObject.SetActive(false);
               
                break;
            case InteractionTypr.Examine:
                FindObjectOfType<InteractionSystem>().Examine(this);
                break;
            default:
                break;
        }

    }
    
}
