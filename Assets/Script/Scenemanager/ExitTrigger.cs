using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public string endSceneName = "EndingCutscene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the exit trigger.");

            if (Gamemanager.Instance.HasCollectedAllItems())
            {
                Debug.Log(" Enough items, loading scene...");
                SceneManager.LoadScene(endSceneName);
            }
            else
            {
                Debug.Log(" Not enough items!");
                Debug.Log("Current Count: " + Gamemanager.Instance.currentItemCount);
                Debug.Log("Required: " + Gamemanager.Instance.totalRequiredItems);
            }
        }
    }
}
