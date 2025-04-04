using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    public Sprite hiddenIconSprite;
    public Sprite IconSprite;

    public bool isSelected;

    public CardController Cardcontroller;

    public void OnCardClick()
    {
        Cardcontroller.SetSelected(this);
    }

    public void SetIconSprite(Sprite sp)
    {
        IconSprite = sp;
    }
    public void Show()
    {
        iconImage.sprite = IconSprite;
        isSelected = true;
    }
    public void Hide()
    {
        iconImage.sprite = hiddenIconSprite;
        isSelected = false;
    }
}
