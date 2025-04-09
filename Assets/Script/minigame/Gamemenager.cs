using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemenager : MonoBehaviour
{
    public string sceneName;

    public static Gamemenager Instance;

    [SerializeField] private int totalMatches;
    private int currentMatches = 0;
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterCorrectMatch()
    {
        currentMatches++;
        score++;

        
    }
}
