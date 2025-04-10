using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalRequiredItems = 3;
    private int currentItemCount = 0;

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

    public void CollectItem()
    {
        currentItemCount++;
        Debug.Log("Collected: " + currentItemCount + "/" + totalRequiredItems);
    }

    public bool HasCollectedAllItems()
    {
        return currentItemCount >= totalRequiredItems;
    }
}

