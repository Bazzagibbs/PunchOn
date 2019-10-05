using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]

public class Controller2D : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private LayerMask groundMask = -1;
    [SerializeField] private float smoothing = 0.05f;
    [SerializeField] private Rect groundCheckCollider = new Rect(0f, -1f, .5f, .2f);

    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool onGround = true;
    private bool moving = false;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    
    void Update()
    {
        
    }
}
