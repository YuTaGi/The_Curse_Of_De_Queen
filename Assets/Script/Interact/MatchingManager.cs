using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{
    [SerializeField] private GameObject gameToActivate;

    public string requiredItem = "Key";
    private bool isPlayerInRange = false;
    public GameObject missingItemPopup;
    public float popupDuration = 2f;
    public TextMeshProUGUI missingItemText;
    private GameObject player;

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
            CheckPlayerInventory();
        }
    }

    private void CheckPlayerInventory()
    {
        if (player == null) return;

        InventorySystem playerInventory = player.GetComponent<InventorySystem>();

        if (playerInventory.HasItem(requiredItem))
        {

            if (gameToActivate != null)
            {
                bool isActive = gameToActivate.activeSelf;
                gameToActivate.SetActive(!isActive);


                if (gameToActivate != null && gameToActivate.activeSelf)
                {
                    playerInventory.RemoveItem(requiredItem);
                    InventorySystem.instance.SaveInventory();
                }
            }
        }
        else
        {
            ShowMissingItemPopup();
        }
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
