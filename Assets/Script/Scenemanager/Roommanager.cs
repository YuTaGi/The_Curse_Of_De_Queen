using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Roommanager : MonoBehaviour
{
    public string requiredItem = "Key";
    private bool isPlayerInRange = false;
    public GameObject missingItemPopup;
    public float popupDuration = 2f;
    public TextMeshProUGUI missingItemText;

    public CardController cardController;

    public GameObject objectToDestroyAfterMiniGame;
    public AudioClip destroySound;
    public AudioSource audioSource;

    private GameObject player;
    public GameObject canvasToClose;
    public GameObject canvasToOpen;

    public void SavePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat("PlayerPosX", pos.x);
            PlayerPrefs.SetFloat("PlayerPosY", pos.y);
            PlayerPrefs.Save();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject;
            Debug.Log("Press 'E' to interact.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interact!");
            CheckPlayerInventory();
        }
    }

    private void CheckPlayerInventory()
    {
        if (player == null) return;

        InventorySystem playerInventory = player.GetComponent<InventorySystem>();
        if (playerInventory.HasItem(requiredItem))
        {
            playerInventory.RemoveItem(requiredItem);
            InventorySystem.instance.SaveInventory();

            StartCoroutine(HandleTransition());
        }
        else
        {
            ShowMissingItemPopup();
        }
    }

    public void DestroyAfterMiniGameWithSound()
    {
        if (objectToDestroyAfterMiniGame != null)
        {
            StartCoroutine(DelayedDestroy(objectToDestroyAfterMiniGame, 1.5f, destroySound));
        }
    }

    private IEnumerator DelayedDestroy(GameObject target, float delay, AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }

        yield return new WaitForSeconds(delay);

        Destroy(target);
    }

    IEnumerator HandleTransition()
    {
        Debug.Log("Starting transition...");

        if (canvasToClose != null) canvasToClose.SetActive(false);

        if (canvasToOpen != null)
        {
            Debug.Log("Trying to open canvas");
            canvasToOpen.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        if (canvasToClose != null) canvasToClose.SetActive(true);
    }

    public void ShowMissingItemPopup()
    {
        if (missingItemPopup == null) return;

        if (missingItemText != null)
        {
            missingItemText.text = "You need a " + requiredItem + "!";
        }

        missingItemPopup.SetActive(true);
        CancelInvoke(nameof(HideMissingItemPopup));
        Invoke(nameof(HideMissingItemPopup), popupDuration);
    }

    void HideMissingItemPopup()
    {
        missingItemPopup.SetActive(false);
    }
}
