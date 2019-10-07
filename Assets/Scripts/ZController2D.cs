using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class ZController2D : MonoBehaviour
{
    public bool isStunned = false;
    private int stunDuration = 0;
    public int targetStunDuration = 2;

    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private LayerMask groundMask = -1;
    [SerializeField] private Rect groundCheckCollider = new Rect(0f, -1f, .5f, .2f);

    private bool onGround = true;
    private Rigidbody2D rb;
    public bool facingRight = true;
    private Animator animator;
    private GameObject player;



    void Awake()
    {
        //atkCollider = GetComponentInChildren<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");        
    }


    void Update()
    {
        CheckGround();

        Vector2 v = rb.velocity;
        Vector2 playerPos = player.transform.position;
        Vector2 pos = transform.position;
        int walkDir = 1;


        // Movement

        if (pos.x < playerPos.x)
        {
            walkDir = 1;
        }

        if (pos.x > playerPos.x)
        {
            walkDir = -1;
        }


        if (!isStunned)
        {
            

            if (Mathf.Abs(pos.x - playerPos.x) < minDistance)
            {
                v.x = 0;
                animator.SetBool("Moving", false);

            }
            else
            {
                v.x = walkSpeed * walkDir;
                animator.SetBool("Moving", true);
            }
        

       

            if (walkDir == -1 && facingRight || walkDir == 1 && !facingRight)
            {
                facingRight = !facingRight;
                Vector2 s = transform.localScale;
                s.x *= -1;
                transform.localScale = s;
            }
        }
        else stunDuration++;
        rb.velocity = v;

    }


    void CheckGround()
    {
        Vector2 min = new Vector2(transform.position.x, transform.position.y) + groundCheckCollider.min;
        Vector2 max = new Vector2(transform.position.x, transform.position.y) + groundCheckCollider.max;

        Collider2D collider = Physics2D.OverlapArea(min, max, groundMask);

        onGround = (collider != null);
        animator.SetBool("OnGround", collider != null);

        if (onGround && stunDuration > targetStunDuration)
        {
            isStunned = false;
            stunDuration = 0;
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

