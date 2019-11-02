using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float distance = 0;
    [SerializeField] private float range = 1.6f;
    private Animator animator;
    private GameObject player;
    private AudioSource mAudio;
    private ZController2D zController;
    private bool alive = true;

    public int dropID = -1;
    public GameObject deathEffect;
    public bool isAttacking = false;
    public bool hasWeapon = true;
    public int zombieHealth = 10;
    public float zombieKnockback = 5f;
    public float zombieKnockbackY = 8f;
    public int zombieDamage = 1;
    public DroppedItem dropPrefab;
 


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        mAudio = GetComponent<AudioSource>();
        zController = GetComponent<ZController2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(zombieHealth < 1 && alive)
        {
            alive = false;
            Death();
        }


        Vector2 playerPos = player.transform.position;
        Vector2 pos = transform.position;

        distance = Vector2.Distance(pos, playerPos);
        if (distance < range && !zController.isStunned)
        {
            Attack();
        }
    }

    void Death()
    {

        if(dropID >= 0)
        {
            DroppedItem droppedWeapon = Instantiate(dropPrefab, transform.position, transform.rotation);
            droppedWeapon.itemID = dropID;
            droppedWeapon.SpriteFromID();
        }


        player.GetComponent<PlayerAttack>().score++;
        Vector3 rot = new Vector3();
        if (zController.facingRight)
        {
            rot.y = -90;
        } else
        {
            rot.y = 90;
        }
        Instantiate(deathEffect, transform.position, Quaternion.Euler(rot));
        GameObject.Destroy(gameObject);
    }



    void Attack()
    {
        animator.SetTrigger("Attack");
    }
    void EndOfAttack()
    {
        animator.SetTrigger("CancelAttack");
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
        other.attachedRigidbody.velocity = knockback;
        otherAnim.SetTrigger("Injured");


        // let the player do the rest of the calculations
        PlayerAttack playerAtk = other.GetComponent<PlayerAttack>();
        playerAtk.ReceiveDamage(zombieDamage);

    }

    void InjuredSound()
    {
        mAudio.Play();
    }

    public void ReceiveDamage(int damage, int stunDuration)
    {
        zombieHealth -= damage;
        zController.targetStunDuration = stunDuration;
        zController.isStunned = true;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isAttacking && collider.tag == "Player") 
        {
            HitPlayer(collider);
            isAttacking = false;
        }
    }

}
