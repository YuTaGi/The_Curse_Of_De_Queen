using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public float attackRange = 1f; 
    public LayerMask enemyLayer;
    public GameObject[] InteractIcon;

    public AudioClip attackSound; // เสียงตอนโจมตี
    private AudioSource audioSource;

    private Vector2 boxsize = new Vector2(0.1f, 1f);
    private Vector2 movement;
    private bool isAttacking = false;
    public bool isDying = false;

    void Start()
    {
        // เพิ่ม AudioSource ถ้ายังไม่มี
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
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

        // เล่นเสียงโจมตี
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        yield return new WaitForSeconds(1.5f);

        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 attackCenter = (Vector2)transform.position + direction * 0.7f;

        Debug.DrawLine(transform.position, attackCenter, Color.yellow, 0.5f);
        Debug.DrawRay(attackCenter, Vector2.up * 0.01f, Color.red, 0.3f);
        Debug.DrawRay(attackCenter, Vector2.down * 0.01f, Color.red, 0.3f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.HideEnemyTemporarily();
            }
        }

        isAttacking = false;
    }
    public void OpenInteractableIcon()
    {
        foreach (GameObject icon in InteractIcon)
        {
            icon.SetActive(true); 
        }
    }

    public void CloseInteractableIcon()
    {
        foreach (GameObject icon in InteractIcon)
        {
            icon.SetActive(false);
        }
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
    // เอาไว้ดูวงโจมตีใน Scene View
    void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null) return;

        Vector2 direction = Vector2.left;
        Vector2 attackCenter = (Vector2)transform.position + direction * 1.0f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCenter, attackRange);
    }
}