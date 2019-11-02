using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Animator))]


public class PlayerAttack : MonoBehaviour
{

    private AudioSource mAudio;
    private Animator anim;
    private Controller2D controller2D;
    private SpriteRenderer sprite;
    public Text healthText;
    public Text scoreText;
    public GameObject resetButton;
    public GameObject menuButton;
    public PlayerWeapon playerWeapon;

    public int score = 0;
    public bool alive = true;
    public int health = 10;
    public bool isAttacking = false;
    public bool hasWeapon = true;

    public float playerKnockback;
    public float basePlayerKnockback = 100f;

    public float playerKnockbackY = 400f;

    public int playerDamage;
    public int basePlayerDamage = 1;

    public int playerStunDuration;
    public int basePlayerStunDuration = 60;

    public GameObject deathEffect;

    void Awake()
    {
        anim = GetComponent<Animator>();
        mAudio = GetComponent<AudioSource>();
        controller2D = GetComponent<Controller2D>();
        sprite = GetComponent<SpriteRenderer>();
        resetButton = GameObject.FindGameObjectWithTag("RestartButton");
        resetButton.SetActive(false);
        menuButton = GameObject.FindGameObjectWithTag("MenuButton");
        menuButton.SetActive(false);

        playerKnockback = basePlayerKnockback;
        playerDamage = basePlayerDamage;
        playerStunDuration = basePlayerStunDuration;

    }

    // Update is called once per frame
    void Update()
    {

        int colons = health / 2;
        int periods = health % 2;
        string healthTextColons = new string(':', colons);
        string healthTextPeriods = new string('.', periods);

        healthText.text = healthTextColons + healthTextPeriods;


        scoreText.text = "SCORE\n" + score;



        if (Input.GetButtonDown("Fire1") && alive)
        {
            anim.SetBool("HasWeapon", hasWeapon);
            anim.SetTrigger("Attack");
        }


        if (health < 1 && alive)
        {
            alive = false;
            resetButton.SetActive(true);
            menuButton.SetActive(true);
            Death();
        }
        

        if(!alive)
        {
            sprite.enabled = false;
            controller2D.DisableMovement();
        }
    }

    void Death()
    {
        foreach(Item i in playerWeapon.parts)
        {
            i.enabled = false;
            i.GetComponent<SpriteRenderer>().enabled = false;
        }

        Vector3 rot = new Vector3();
        if (controller2D.facingRight)
        {
            rot.z = 90;
        }
        else
        {
            rot.z = 270;
        }
        Instantiate(deathEffect, transform.position, Quaternion.Euler(rot));
        
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
        ZombieAttack otherAttack = other.GetComponent<ZombieAttack>();
        Animator otherAnim = other.GetComponent<Animator>();
        otherController.isStunned = true;
        Vector2 knockback = new Vector2();
        knockback.x = direction * playerKnockback;
        knockback.y = playerKnockbackY;
        other.attachedRigidbody.velocity = knockback;
        otherAnim.SetTrigger("Injured");


        // perform other calculations on Zombie side

        otherAttack.ReceiveDamage(playerDamage, playerStunDuration);

    }


    void Pickup() {

    }

    void InjuredSound()
    {
        mAudio.Play();
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isAttacking && collider.tag == "Enemy")
        {
            
            Attack(collider);
            isAttacking = false;
        }
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
