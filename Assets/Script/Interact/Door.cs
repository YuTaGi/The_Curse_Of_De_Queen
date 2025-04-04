
using UnityEngine;

public class Door : Interactable
{
    public GameObject target;

    public override void Interact()
    {
        InSceneControl.TransitionPlayer(target.transform.position);
    }
}
