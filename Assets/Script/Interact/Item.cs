using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionTypr { NONE, PickUp, Examine }
    public InteractionTypr InteractType;

    public string itemID;
    public string descriptionText;

    public AudioClip pickupSound; // เสียงเก็บไอเท็ม
    private AudioSource audioSource;

    private void Awake()
    {
        // ตรวจสอบว่ามี AudioSource หรือไม่
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }

    public void Interact()
    {
        switch (InteractType)
        {
            case InteractionTypr.PickUp:
                if (!FindObjectOfType<InventorySystem>().CanPickUp(gameObject))
                {
                    return;
                }

             
                if (pickupSound != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }

                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);

              
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CollectItem();
                }

                
                StartCoroutine(DisableAfterSound());
                break;

            case InteractionTypr.Examine:
                FindObjectOfType<InteractionSystem>().Examine(this);
                break;

            default:
                break;
        }
    }

    private IEnumerator DisableAfterSound()
    {
        yield return new WaitForSeconds(pickupSound != null ? pickupSound.length : 0f);
        gameObject.SetActive(false);
    }
}