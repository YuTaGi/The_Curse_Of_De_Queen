using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectItem : MonoBehaviour
{
    public static CollectItem instance;
    private HashSet<string> collectedItems = new HashSet<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsCollected(string id)
    {
        return collectedItems.Contains(id);
    }

    public void MarkCollected(string id)
    {
        collectedItems.Add(id);
    }
}