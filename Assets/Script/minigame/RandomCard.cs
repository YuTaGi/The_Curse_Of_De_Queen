
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
        Vector3[] positions = new Vector3[numCards];

        for (int i = 0; i < numCards; i++)
        {
            positions[i] = cards[i].transform.position;
        }

      
        for (int i = numCards - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[j];
            positions[j] = temp;
        }

        for (int i = 0; i < numCards; i++)
        {
            cards[i].transform.position = positions[i];
        }
    }
}