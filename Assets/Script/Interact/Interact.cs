using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IInteract : MonoBehaviour
{
    private bool isPlayerNearby = false;
    public string sceneName;
    public string objectID;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt(objectID, 0) == 1)
        {
            Destroy(gameObject); 
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {

           interact();
        }
    }

    public void interact()
    {
        PlayerPrefs.SetInt(objectID, 1); 
        PlayerPrefs.Save(); 
        SceneManager.LoadScene(sceneName);
        Debug.Log("Interacted with the object!");
        Destroy(gameObject);
    }
}
