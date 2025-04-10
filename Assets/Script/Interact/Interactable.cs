
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered interactable area.");


            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OpenInteractableIcon();
            }
            else
            {
                Debug.LogWarning("PlayerController not found on the player object!");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player left interactable area.");
            collision.GetComponent<PlayerController>().CloseInteractableIcon();
        }
    }

}
