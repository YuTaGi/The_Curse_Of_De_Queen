using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [Header("Reward Settings")]
    public List<GameObject> successRewards;

    [Header("UI")]
    public TextMeshProUGUI rewardText;
    public Image RewardImage;
    public TextMeshProUGUI RewardName;

    void Start()
    {
        
        if (PlayerPrefs.HasKey("LastReward"))
        {
            string rewardName = PlayerPrefs.GetString("LastReward");
            if (rewardText != null)
                rewardText.text = "YouGot: " + rewardName;
            PlayerPrefs.DeleteKey("LastReward"); 
        }
        else
        {
            if (rewardText != null)
                rewardText.text = ""; 
        }
    }
    public void GiveReward(bool isSuccess)
    {
        GameObject selectedReward = null;
        if (isSuccess)
        {
            if (successRewards != null && successRewards.Count > 0)
            {
                int index = Random.Range(0, successRewards.Count);
                selectedReward = successRewards[index];
            }
        }
       
        if (selectedReward != null)
        {

            InventorySystem.instance.PickUp(selectedReward);
            PlayerPrefs.SetString("LastReward", selectedReward.name); 
            PlayerPrefs.Save();
            Debug.Log("Reward given: " + selectedReward.name);
        }
        else
        {
            Debug.LogWarning("No reward found for isSuccess: " + isSuccess);
        }
    }
}
