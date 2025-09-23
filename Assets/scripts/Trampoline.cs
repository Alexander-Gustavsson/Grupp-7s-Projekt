using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hoppsan hejsan

public class Trampoline : MonoBehaviour
{

    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private AudioClip trampolineSound;


    private AudioSource audioSource;
  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) //&& other.CompareTag("Enemy"))
        {
            audioSource = GetComponent<AudioSource>();
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, 0);
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            GetComponent<Animator>().SetTrigger("Jump");
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(trampolineSound, 0.5f);
        }
    }
}
