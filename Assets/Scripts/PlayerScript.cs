using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerScript : MonoBehaviour
{
    private float movement;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 10;
    private Rigidbody2D rb;
    private Collider2D coll;
    private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
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
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
      //  UpdateFacingDirection();
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