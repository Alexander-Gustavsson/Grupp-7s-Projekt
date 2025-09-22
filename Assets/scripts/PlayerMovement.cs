using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform LeftFoot, RightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject appleParticals, dustParticles;
    [SerializeField] private GameObject orangeParticals;
    [SerializeField] private AudioClip HitSounds;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillColor;
    [SerializeField] private Color greenHealth, redHealth;
    [SerializeField] private TMP_Text appleText;

    private float horizontalInput;
    private float rayDistance = 0.25f;
    private bool isGrounded;
    private bool canMove;
    private int startingHealth = 5;
    private int currentHealth = 0;
    public int applesCollected = 0;
    public Vector3 spawnPosition;


    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = true;
        currentHealth = startingHealth;
        appleText.text = "" + applesCollected;
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIfGrounded();
        horizontalInput = Input.GetAxis("Horizontal");

        if(horizontalInput < 0)
        {
            FlipSprite(true);
        }

        if (horizontalInput > 0)
        {
            FlipSprite(false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            Jump();
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }

        rgbd.linearVelocity = new Vector2(horizontalInput * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            applesCollected++;
            appleText.text = "" + applesCollected;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(pickupSound, 0.5f);
            Instantiate(appleParticals, other.transform.position, Quaternion.identity);
        }

        if(other.CompareTag("Health"))
        {
            RestoreHealth(other.gameObject);
        }
    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));
        int randomValue = Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[randomValue], 0.25f);
        Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar();


        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void TakeKnockBack(float knockbackForce, float upwards)
    {
        anim.SetTrigger("hit");
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(HitSounds, 0.5f);
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwards));
        Invoke("CanMoveAgain", 0.25f);
    }

    public void Respawn()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();
        transform.position = spawnPosition;
        rgbd.linearVelocity = Vector2.zero;
    }

    private void CanMoveAgain()
    {
        anim.SetTrigger("hitdone");
        canMove = true;
    }

    private void RestoreHealth(GameObject healthPickup)
    {
        if(currentHealth >= startingHealth)
        {
            return;
        }
        else
        {
            int healthToRestore = healthPickup.GetComponent<HealthPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHealthBar();
            audioSource.pitch = Random.Range(0.4f, 0.7f);
            audioSource.PlayOneShot(pickupSound, 0.5f);
            Destroy(healthPickup);
            Instantiate(orangeParticals, healthPickup.transform.position, Quaternion.identity);

            if (currentHealth >= startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;

        if (currentHealth >= 2)
        {

            fillColor.color = greenHealth;
        }
        else
        {
            fillColor.color = redHealth;
        }
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(LeftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(RightFoot.position, Vector2.down, rayDistance, whatIsGround);

        //Debug.DrawRay(LeftFoot.position, Vector2.down * rayDistance, Color.blue, 0.25f);
        //Debug.DrawRay(RightFoot.position, Vector2.down * rayDistance, Color.red, 0.25f);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
