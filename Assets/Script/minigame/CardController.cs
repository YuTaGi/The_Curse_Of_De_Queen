using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardController : MonoBehaviour
{
    [SerializeField] Transform gridTransform;
    [SerializeField] Card cardPrefab;
    [SerializeField] Sprite[] sprites;

    private List<Sprite> spritePairs;
    private Card firstSelected;
    private Card secondSelected;
    private int matchedPairs = 0;
    public string sceneName;
   

    private void Start()
    {
        PrepareSprite();
        CreatCards();
    }

    private void PrepareSprite()
    {
        spritePairs = new List<Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }
        ShuffleSprites(spritePairs);
    }

    void CreatCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.Cardcontroller = this;
        }
    }

    public void SetSelected(Card card)
    {
        if (!card.isSelected)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }
            if (secondSelected == null)
            {
                secondSelected = card;
                StartCoroutine(CheckMatching(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }
        }
    }

    IEnumerator CheckMatching(Card a, Card b)
    {
        yield return new WaitForSeconds(0.3f);

        if (a.IconSprite == b.IconSprite)
        {
            matchedPairs++; 
            if (matchedPairs == sprites.Length) 
            {
                yield return new WaitForSeconds(0.5f); 
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            a.Hide();
            b.Hide();
        }
    }

    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}
