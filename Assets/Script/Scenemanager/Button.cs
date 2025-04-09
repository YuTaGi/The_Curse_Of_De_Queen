using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public string SceneName;
    public void OnClickNewGameButton()
    {
       
        PlayerPrefs.DeleteAll();

    
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneName);
    }
}
