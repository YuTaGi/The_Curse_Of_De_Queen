using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;

    public string requiredItem = "Pot";
    public int totalRequiredItems = 8;
    public int currentItemCount = 0;

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
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < totalRequiredItems; i++)
            {
                CollectItem(requiredItem);
            }

            Debug.Log("Debug: Added " + totalRequiredItems + " items (" + requiredItem + ")");
        }
    }
    public void CollectItem(string id)
    {
        if (id == requiredItem)
        {
            currentItemCount++;

            Debug.Log("Collected item count: " + currentItemCount);

            if (currentItemCount >= totalRequiredItems)
            {
                Debug.Log("✅ All items collected! Transition to next scene...");
                // เรียกฟังก์ชันเปลี่ยนฉากหรือลูปจบตรงนี้
            }
        }
        else
        {
            Debug.Log("Collected non-required item: " + id);
        }
    }

    public bool HasCollectedAllItems()
    {
        Debug.Log("Check collected items: " + currentItemCount + "/" + totalRequiredItems);
        return currentItemCount >= totalRequiredItems;
    }
}

