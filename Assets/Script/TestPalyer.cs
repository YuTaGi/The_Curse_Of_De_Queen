using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPalyer : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public GameObject InteractIcon;
    private Vector2 boxsize = new Vector2(0.1f, 1f);
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteraction();
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized * moveSpeed;

    }

    void FixedUpdate()
    {
        rb.velocity = moveInput;
    }

    public void OpenInteractableIcon()
    {
       
        InteractIcon.SetActive(true);
    }
    public void CloseInteractableIcon()
    {
        
        InteractIcon.SetActive(false);
    }
    private void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position,boxsize,0, Vector2.zero);
        if(hits.Length > 0 )
        {
            foreach(RaycastHit2D rc in hits)
            {
                if(rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }
}