using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float damage = 10f;
    private Vector2 currentTarget;
    private bool isHidden = false;

    [Header("Sound Effects")]
    public AudioClip attackSound;   // เสียงตอนโจมตีผู้เล่น
    public AudioClip hurtSound;     // เสียงตอนโดนตี
    private AudioSource audioSource;

    void Start()
    {
        currentTarget = pointB.position;

        // ตรวจสอบหรือเพิ่ม AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isHidden) return;

        Patrol();
        FlipSprite();
    }

    void Patrol()
    {
        Vector2 pos = transform.position;
        MoveTo(currentTarget);

        if ((pos - currentTarget).sqrMagnitude < 0.01f)
        {
            currentTarget = (currentTarget == (Vector2)pointA.position) ? (Vector2)pointB.position : (Vector2)pointA.position;
        }
    }

    void MoveTo(Vector2 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;

        if (currentTarget.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else if (currentTarget.x < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    // ฟังก์ชันทำให้ศัตรูหายไปชั่วคราว
    public void HideEnemyTemporarily()
    {
        isHidden = true;

        // เล่นเสียงโดนตี
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        gameObject.SetActive(false);
        Invoke("ShowEnemy", 2f);
    }

    private void ShowEnemy()
    {
        isHidden = false;
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // เล่นเสียงโจมตี
                if (attackSound != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }

                playerHealth.TakeDamage(damage);
            }
        }
    }
}