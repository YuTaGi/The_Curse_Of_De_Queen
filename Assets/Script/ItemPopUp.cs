using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopUp : MonoBehaviour
{
    public GameObject rewardPopupUI;
    public TextMeshProUGUI rewardText; 
    public Image itemImage;

    void Start()
    {
        if (PlayerPrefs.HasKey("RewardItem"))
        {
            string itemName = PlayerPrefs.GetString("RewardItem");
            ShowPopup(itemName);
            PlayerPrefs.DeleteKey("RewardItem");
        }
    }

    void ShowPopup(string itemName)
    {
        rewardPopupUI.SetActive(true);
        rewardText.text = $"You got {itemName}!";

    }

    public void ClosePopup()
    {
        rewardPopupUI.SetActive(false);
    }
}

