using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    public Transform detectionPoint;
    private const float detectionRadius = 2.0f;
    public LayerMask detectionLayer;
    public GameObject detectedObjects;
    [Header("Examine")]
    public GameObject examineWindow;
    public Image examineImage;
    public TextMeshProUGUI examineText;
    public TextMeshProUGUI ItemNamae;

    public bool isExamining;
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

    public void Examine(Item item)
    {
        if (isExamining)
        {
            examineWindow.SetActive(false);
            isExamining = false;
        }
        else
        {
            
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            ItemNamae.text = item.name;
            examineText.text = item.descriptionText;
            examineWindow.SetActive(true);
            isExamining = true;
        }
    }
}