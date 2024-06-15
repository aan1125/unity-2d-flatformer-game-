using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exam08_player : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    Vector2 velocity;
    public float speed = 1.0f;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;
    bool isGrounded;
    bool isJumping;
    [SerializeField] GameObject bodyObject;
    [SerializeField] Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = bodyObject.GetComponent<Animator>();
    }

    void Update()
    {
        float _hozInput = Input.GetAxisRaw("Horizontal");

        velocity = new Vector2(_hozInput, 0).normalized * speed;

        animator.SetBool("isWalk", velocity.x != 0);

        if (_hozInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_hozInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !isJumping)
        {
            animator.SetBool("isGrounded", true);
            if (!wasGrounded)
            {
                animator.SetTrigger("Land");
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
            if (wasGrounded)
            {
                animator.SetTrigger("Fall");
            }
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            isJumping = true;
        }

        // 애니메이터 상태 확인
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Jump") && stateInfo.normalizedTime >= 1.0f)
        {
            OnJumpAnimationEnd();
        }

        Debug.Log($"Update - isGrounded: {isGrounded}, isJumping: {isJumping}");
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(velocity.x, rigidbody.velocity.y);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void OnJumpAnimationEnd()
    {
        Debug.Log("Jump animation ended.");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isJumping = false;

        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            animator.SetTrigger("Land");
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        Debug.Log($"OnJumpAnimationEnd - isGrounded: {isGrounded}, isJumping: {isJumping}");
    }
}
