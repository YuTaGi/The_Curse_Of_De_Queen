
using UnityEngine;

public class Door : Interactable
{
    public GameObject target;
    public AudioClip doorOpenSound; // เสียงเปิดประตู
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // เพิ่ม AudioSource ถ้ายังไม่มี
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void Interact()
    {
        Debug.Log("Find Door");

        // เล่นเสียงเปิดประตู
        if (doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        // ย้ายผู้เล่น
        InSceneControl.TransitionPlayer(target.transform.position);
    }
}
