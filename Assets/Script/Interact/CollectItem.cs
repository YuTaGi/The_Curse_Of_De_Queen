using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectItem : MonoBehaviour
{

    private bool interactionAllowed;
    private Interactable interactableObject;

    void Update()
    {
        if (interactionAllowed && Input.GetKeyDown(KeyCode.E))
        {
            IInteract();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionAllowed = true;
        }

        
        interactableObject = collision.GetComponent<Interactable>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionAllowed = false;
        }

        interactableObject = null;
    }

    private void IInteract()
    {
        if (interactableObject != null)
        {
            interactableObject.Interact();
        }
        else
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        Destroy(gameObject);
        Debug.Log("PickUp Success");
    }
    
}
