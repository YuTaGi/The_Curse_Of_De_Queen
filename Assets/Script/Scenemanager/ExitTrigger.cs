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
            if (GameManager.Instance.HasCollectedAllItems())
            {
                SceneManager.LoadScene(endSceneName);
            }
            else
            {
                Debug.Log("Item not enough!");
            }
        }
    }
}
