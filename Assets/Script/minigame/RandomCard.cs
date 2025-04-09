
using UnityEngine;

public class RandomCard : MonoBehaviour
{
    [SerializeField] private GameObject[] cardsTop;
    [SerializeField] private GameObject[] cardsBottom;

    private void Start()
    {
        ShuffleSprites(cardsTop);
        ShuffleSprites(cardsBottom);
    }

    void ShuffleSprites(GameObject[] cards)
    {
        int numCards = cards.Length;
        Sprite[] sprites = new Sprite[numCards];

      
        for (int i = 0; i < numCards; i++)
        {
            sprites[i] = cards[i].GetComponent<SpriteRenderer>().sprite;
        }

        for (int i = numCards - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Sprite temp = sprites[i];
            sprites[i] = sprites[j];
            sprites[j] = temp;
        }

        for (int i = 0; i < numCards; i++)
        {
            cards[i].GetComponent<SpriteRenderer>().sprite = sprites[i];
        }
    }
}