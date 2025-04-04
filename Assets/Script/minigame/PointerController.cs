using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public RectTransform Safexone;
    public float moveSpeed = 100f;
    public string sceneName;

    private int successCount = 0; 
    private int requiredSuccesses = 3;
    private float direction = 1f;
    private RectTransform pointerTranform;
    private Vector3 targetposition;
    void Start()
    {
        pointerTranform = GetComponent<RectTransform>();
        targetposition = PointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        pointerTranform.position = Vector3.MoveTowards(pointerTranform.position, targetposition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTranform.position, PointA.position) < 0.1f)
        {
            targetposition = PointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTranform.position, PointB.position) < 0.1f)
        {
            targetposition = PointA.position;
            direction = -1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(Safexone, pointerTranform.position, null))
        {
            successCount++; 
            Debug.Log("Success " + successCount + "/" + requiredSuccesses);

            if (successCount >= requiredSuccesses)
            {
                SceneManager.LoadScene(sceneName); 
            }
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
