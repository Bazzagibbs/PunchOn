using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private int timer = 0;
    public int lifetime = 300;
    public int itemID = -1;
    public GameObject particleEffect;
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteFromID();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer++;
        if (timer > lifetime) DestroyItem();
    }


    public void SpriteFromID()
    {
        spriteRenderer.sprite = sprites[itemID];
    }


    public void DestroyItem()
    {
        Instantiate(particleEffect, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
