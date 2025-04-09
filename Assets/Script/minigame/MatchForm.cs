using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MatchForm : MonoBehaviour
{
    [SerializeField] private int matchId;

    public int Get_Id()
    {
        return matchId;
    }
}
