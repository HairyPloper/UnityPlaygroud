using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] float movementSpped = 10f;
    [SerializeField] float jumpForce = 10f;
    public AudioSource playerCollectItem;
    bool facingRight = true;
    int extraJumps;
    [SerializeField] int extraJumpValue;

    private bool IsGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGroud;
    Animator animator;

    public SpriteRenderer Joint;
    public ItemCollector script;


    Rigidbody2D rb;
    BoxCollider2D bc;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        extraJumps = extraJumpValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded)
        {
            extraJumps = extraJumpValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            Jump();
            //playerJumpSound.Play();
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && IsGrounded)
        {
            Jump();
        }
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void Jump()
    {
        //Debug.Log(extraJumps);
        // Perform jump mechanic
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        animator.SetTrigger("death");
        Joint.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGroud);
        animator.SetBool("isJumping", !IsGrounded);

        rb.velocity = new Vector2(horizontalInput * movementSpped, rb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);


        if (facingRight && horizontalInput < 0)
        {
            Flip();
        }
        else if (!facingRight && horizontalInput > 0)
        {
            Flip();
        }

    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 vector = transform.localScale;
        vector.x *= -1;
        transform.localScale = vector;
    }

    public IEnumerator ShowJointForSecond(float seconds)
    {
        Joint.enabled = true;           //show
        yield return new WaitForSeconds(seconds);  //wait
        Joint.enabled = false;          //hide
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
