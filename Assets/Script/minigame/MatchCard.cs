using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class MatchCard : MonoBehaviour , IPointerDownHandler, IDragHandler, IDropHandler
{
    public Image outline;
    public string GroupName => transform.parent.name;
    public void OnDrag(PointerEventData eventData)
    {
        MatchManager.Instance.Dragging();
    }

    public void OnDrop(PointerEventData eventData) 
    {
        MatchManager.Instance.DroppedOnCard(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MatchManager.Instance.CardPicked(this);
    }
    public void Hightlight(bool highlight)
    {
        outline.color = highlight ? Color.green : Color.grey;
    }
}


