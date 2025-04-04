using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCard : MonoBehaviour
{
    [SerializeField] private int matchID;

    public int GetId()
    {
        return matchID;
    }
}
