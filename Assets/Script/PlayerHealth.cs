using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
  
        public float health;
        public float MaxHealth;
        public Image HealthBar;
        public PlayerController playerController;

        [Header("Sound Effects")]
        public AudioClip hitSound; // เสียงตอนโดนตี
        private AudioSource audioSource;

        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        void Start()
        {
            MaxHealth = health;

            // ตรวจสอบและเพิ่ม AudioSource ถ้าไม่มี
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // ดึง SpriteRenderer และจำสีเดิมไว้
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        void Update()
        {
            HealthBar.fillAmount = Mathf.Clamp(health / MaxHealth, 0, 1);

            if (health <= 0 && !playerController.isDying)
            {
                playerController.Die();
            }
        }

        // ฟังก์ชันรับความเสียหาย
        public void TakeDamage(float damage)
        {
            health -= damage;

            // เล่นเสียงโดนตี
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // เอฟเฟกต์กระพริบสีแดง
            if (spriteRenderer != null)
            {
                StartCoroutine(FlashRed());
            }
        }

        IEnumerator FlashRed()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = originalColor;
        }
    
}
