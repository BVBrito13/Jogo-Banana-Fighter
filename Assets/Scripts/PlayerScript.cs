using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerScript : MonoBehaviour
{
    private float movement;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 10;
    private enum AnimationState { Idle, Walking, Jumping, Falling, Kicking, Punching };
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private LayerMask groundLayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
    }
    private void OnMovement(InputValue value)
    {
        movement = value.Get<float>();
    }
    private void OnJump(InputValue value)
    {
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    private void OnJab(InputValue value)
    {
        anim.SetTrigger("jab");
    }
    private void OnKick(InputValue value)
    {
        anim.SetTrigger("kick");
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
        UpdateFacingDirection();
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        if (rb.linearVelocityY != 0 && !IsGrounded())
            anim.SetInteger("state", (int)AnimationState.Jumping);
        else if (rb.linearVelocityX == 0)
        {
            anim.SetInteger("state", (int)AnimationState.Idle);
        }
        else
        {
            anim.SetInteger("state", (int)AnimationState.Walking);
        }
    }
    private void UpdateFacingDirection()
    {
        if (movement != 0 && transform.localScale.x != movement)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
    }
}