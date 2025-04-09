using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Roommanager : MonoBehaviour
{
    public string scene;
    public string requiredItem = "Key";
    private bool isPlayerInRange = false;
    public GameObject missingItemPopup;
    public float popupDuration = 2f;
    public TextMeshProUGUI missingItemText;

    private void Start()
    {
        PlayerPrefs.DeleteAll(); 
    }
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
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        InventorySystem playerInventory = player.GetComponent<InventorySystem>();

        if (playerInventory != null)
        {

            if (playerInventory.HasItem(requiredItem))
            {
                playerInventory.RemoveItem(requiredItem);
                SavePlayerPosition();
                Debug.Log("Used item: " + requiredItem);
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log("Player needs " + requiredItem);
                ShowMissingItemPopup();
            }
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
