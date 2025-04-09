using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{

    [Header("Scene Settings")]
    public string sceneName;
    private bool canChangeScene = false;

    [Header("UI")]
    public GameObject interactIcon;

    void Start()
    {
        if (interactIcon != null)
            interactIcon.SetActive(false);
    }

    void Update()
    {
        if (canChangeScene && Input.GetKeyDown(KeyCode.E))
        {
            ChangeScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canChangeScene = true;
            if (interactIcon != null)
                interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canChangeScene = false;
            if (interactIcon != null)
                interactIcon.SetActive(false);
        }
    }

    private void ChangeScene()
    {
       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SavePlayerPosition();
            }
        }

        SceneManager.LoadScene(sceneName);
    }
}

