using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]


public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public int health = 10;
    public bool isAttacking = false;
    public bool hasWeapon = true;
    public float playerKnockback = 200f;
    public float playerKnockbackY = 400f;
    public float playerDamage = 5f;
    public float playerAttackSpeed = 1f;
    public float playerStunDuration = 0.3f;
    public float playerParryStun = 0.5f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
        }
    }


    void Attack(Collider2D other)
    {
        Debug.Log(name + " hit " + other.name);

        // get direction to knockback
        float directionRaw = transform.position.x - other.transform.position.x;
        int direction = 0;
        if (directionRaw < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        // perform knockback

        ZController2D otherController = other.GetComponent<ZController2D>();
        Animator otherAnim = other.GetComponent<Animator>();
        otherController.isStunned = true;
        Vector2 knockback = new Vector2();
        knockback.x = direction * playerKnockback;
        knockback.y = playerKnockbackY;
        other.attachedRigidbody.AddForce(knockback);
        otherAnim.SetTrigger("Injured");
    }


    void Pickup() {

    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isAttacking)
        {
            Attack(collider);
            isAttacking = false;
        }
    }
}
