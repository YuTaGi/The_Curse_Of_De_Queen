using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    public Transform detectionPoint;
    private const float detectionRadius = 0.2f;
    public LayerMask detectionLayer;
    public GameObject detectedObjects;
    
   

    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {

                GameObject detectedObject = GetDetectedObject();
                if (detectedObject != null)
                {
                    detectedObject.GetComponent<Item>().Interact();
                }
            }
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }


    bool DetectObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(detectionPoint.position, detectionPoint.right, detectionRadius, detectionLayer);
        return hit.collider != null;
    }


    GameObject GetDetectedObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(detectionPoint.position, detectionPoint.right, detectionRadius, detectionLayer);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    public void PickUpItem(GameObject item)
    {
       FindObjectOfType<InventorySystem>().PickUp(item);  
    }
   
   
}