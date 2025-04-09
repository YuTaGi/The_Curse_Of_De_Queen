using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopUp : MonoBehaviour
{
    public static ItemPopUp Instance;
    [Header("UI")]
    public GameObject rewardPopupUI;
    public Text rewardText;
    public Image rewardImage;

    [Header("Reward Database")]
    public List<RewardData> allRewards;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        CheckRewardFlag();
    }

    public void GiveReward(string rewardName)
    {
       
        PlayerPrefs.SetString("RewardItem", rewardName);
        PlayerPrefs.SetInt("ShowRewardAfterLoad", 1);
        PlayerPrefs.Save();
    }

    private void CheckRewardFlag()
    {
        if (PlayerPrefs.GetInt("ShowRewardAfterLoad", 0) == 1)
        {
            string rewardName = PlayerPrefs.GetString("RewardItem", "");
            ShowPopup(rewardName);

            PlayerPrefs.DeleteKey("ShowRewardAfterLoad");
            PlayerPrefs.DeleteKey("RewardItem");
        }
    }

    public void ShowPopup(string rewardName)
    {
        rewardPopupUI.SetActive(true);
        rewardText.text = $"Got {rewardName}";

        ItemPopUp data = allRewards.Find(r => r.rewardName == rewardName);
        if (data != null)
        {
            rewardImage.sprite = data.rewardIcon;
        }
        else
        {
            Debug.LogWarning("not found " + rewardName);
        }
    }

    public void ClosePopup()
    {
        rewardPopupUI.SetActive(false);
    }
}
