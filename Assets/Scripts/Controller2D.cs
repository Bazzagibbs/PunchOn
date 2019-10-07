using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]

public class Controller2D : MonoBehaviour
{

    public bool isStunned = false;
    private float stunDuration = 0f;


    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private LayerMask groundMask = -1;
    [SerializeField] private Rect groundCheckCollider = new Rect(0f, -1f, .5f, .2f);


    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool onGround = true;
    private Collider2D coll;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

    }

    
    void Update()
    {
        CheckGround();

        Vector2 v = rb.velocity;

        if (!isStunned)
        {
            if (!coll.IsTouchingLayers(groundMask) || onGround)
            {
                v.x = walkSpeed * Input.GetAxis("Horizontal");
                animator.SetBool("Moving", v.x != 0);
            }
            else
            {
                v.x = 0;
            }

            if (onGround && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(new Vector2(0, jumpSpeed));
            }

            
        



            if(v.x < 0 && facingRight || v.x > 0 && !facingRight)
            {
                facingRight = !facingRight;
                Vector2 s = transform.localScale;
                s.x *= -1;
                transform.localScale = s;
                groundCheckCollider.x = -groundCheckCollider.max.x;
            }
        }
        else stunDuration += 0.01f;
        rb.velocity = v;

    }

    void Move(Vector2 velocity)
    {
        if (!coll.IsTouchingLayers(groundMask) || onGround)
        {
            velocity.x = walkSpeed * Input.GetAxis("Horizontal");
            animator.SetBool("Moving", velocity.x != 0);
        }
        else
        {
            velocity.x = 0;
        }

        if (onGround && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpSpeed));
        }

        rb.velocity = velocity;
    }


    void CheckGround() {
        Vector2 min = new Vector2(transform.position.x, transform.position.y) + groundCheckCollider.min;
        Vector2 max = new Vector2(transform.position.x, transform.position.y) + groundCheckCollider.max;

        Collider2D collider = Physics2D.OverlapArea(min, max, groundMask);

        onGround = (collider != null);
        animator.SetBool("OnGround",collider != null);
        if (onGround && stunDuration > 0.2)
        {
            isStunned = false;
            stunDuration = 0f;
        }
    }

    void OnDrawGizmos()
    {
        // draw a box showing the groundCheckCollider. It is red if the avatar is on the ground and white otherwise.

        Vector3 centre = transform.position;
        centre.x += groundCheckCollider.center.x;
        centre.y += groundCheckCollider.center.y;

        Vector3 size = Vector3.zero;
        size.x += groundCheckCollider.width;
        size.y += groundCheckCollider.height;

        if (onGround)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawWireCube(centre, size);

    }


}
