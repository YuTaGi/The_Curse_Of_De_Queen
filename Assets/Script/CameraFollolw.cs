using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollolw : MonoBehaviour
{
    public Transform target;       
    public Vector3 offset = new Vector3(0f, 1f, -10f); 
    public float smoothSpeed = 0.125f;  

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
