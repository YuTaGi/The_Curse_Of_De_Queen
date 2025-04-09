using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Roommanager : MonoBehaviour
{
    public string scene;
    public string requiredItem = "Key";
    private bool isPlayerInRange = false;

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
                SavePlayerPosition();
                Debug.Log("Player has " + requiredItem);
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log("Player needs " + requiredItem);
            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {

            player.transform.position = new Vector2(-7, 0);
        }
    }

}
