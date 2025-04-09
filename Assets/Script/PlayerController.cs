using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject InteractIcon;
    private Vector2 boxsize = new Vector2(0.1f, 1f);
    public Vector2 startPosition = new Vector2(-33.2f, -3.8f);
    private Vector2 movement;
    private bool isAttacking = false;
    private bool isDying = false;

    public void Start()
    {
        int isNewGame = PlayerPrefs.GetInt("IsNewGame", 1);

        if (isNewGame == 1)
        {
           
            transform.position = startPosition;
            
        }
        else
        {
            
            LoadPlayerPosition();
        }

      
        PlayerPrefs.SetInt("IsNewGame", 0);
        PlayerPrefs.Save();
    }
    void Update()
    {
        if (isDying) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(moveInput, 0f);

        animator.SetBool("IsMoving", moveInput != 0);

       
        if (moveInput != 0)
        {
            spriteRenderer.flipX = moveInput > 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteraction();
        }
        

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackOnce());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        if (!isAttacking && !isDying)
        {
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    System.Collections.IEnumerator AttackOnce()
    {
        isAttacking = true;
        animator.SetTrigger("IsAttacking"); 

       
        rb.velocity = Vector2.zero;

      
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
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
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxsize, 0, Vector2.zero);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }
   
    public void Die()
    {
        if (!isDying)
        {
            isDying = true;
            animator.SetBool("IsDying", true);

           
            rb.velocity = Vector2.zero;

           
            Destroy(gameObject, 2f); 
        }
    }
    public void LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerPosX", transform.position.x);
        float y = PlayerPrefs.GetFloat("PlayerPosY", transform.position.y);
        transform.position = new Vector3(x, y, 0f);
    }
    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.Save();
    }

   
    void Awake()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}