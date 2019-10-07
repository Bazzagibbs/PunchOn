using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{

    [SerializeField] private float range = 1.6f;
    private Animator animator;
    private GameObject player;
    public bool isAttacking = false;
    public bool hasWeapon = true;
    public float zombieKnockback = 200f;
    public float zombieKnockbackY = 400f;
    public float zombieDamage = 5f;
 


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 playerPos = player.transform.position;
        Vector2 pos = transform.position;


        if (Vector2.Distance(pos, playerPos) < range)
        {
            Attack();
        }
    }


    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    void HitPlayer(Collider2D other)
    {
        Debug.Log(name + " hit " + other.name);

        // get direction to knockback
        float directionRaw = transform.position.x - other.transform.position.x;
        int direction = 0;
        if(directionRaw < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        // perform knockback

        Controller2D otherController = other.GetComponent<Controller2D>();
        Animator otherAnim = other.GetComponent<Animator>();
        otherController.isStunned = true;
        Vector2 knockback = new Vector2();
        knockback.x = direction * zombieKnockback;
        knockback.y = zombieKnockbackY;
        other.attachedRigidbody.AddForce(knockback);
        otherAnim.SetTrigger("Injured");


    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isAttacking)
        {
            HitPlayer(collider);
            isAttacking = false;
        }
    }

}
