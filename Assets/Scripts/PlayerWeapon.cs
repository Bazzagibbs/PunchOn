using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    public bool hasBaseWeapon = false;
    public int currentBaseWeaponIndex = -1;
    public int damageTotal = 0;
    public int knockbackTotal = 0;
    public int stunTotal = 0;
    public PlayerAttack pa;
    public Item[] parts;

    void Awake()
    {
        parts = GetComponentsInChildren<Item>();
        RenderItems();
    }

    void Update()
    {

    }


    void RenderItems()
    {
        damageTotal = 0;
        knockbackTotal = 0;
        stunTotal = 0;


        CheckCurrentBaseWeapon();



        foreach(Item i in parts)
        {
            if (i.isEnabled && hasBaseWeapon)
            {
                i.gameObject.SetActive(true);
                damageTotal += i.damageModifier;
                knockbackTotal += i.knockbackModifier;
                stunTotal += i.stunModifier;
            }
            else
            {
                i.gameObject.SetActive(false);
            }
        }

        pa.playerDamage = pa.basePlayerDamage + damageTotal;
        pa.playerKnockback = pa.basePlayerKnockback + knockbackTotal;
        pa.playerStunDuration = pa.basePlayerStunDuration + stunTotal;

    }

    void CheckCurrentBaseWeapon()
    {
        hasBaseWeapon = false;
        pa.hasWeapon = false;
        for (int i = 0; i < parts.Length; i++) // check if there is a base weapon
        {
            if (parts[i].isBaseWeapon && parts[i].isEnabled)
            {
                hasBaseWeapon = true;
                pa.hasWeapon = true;
                currentBaseWeaponIndex = i;
            }
        }
    }



    public void PickupItem(DroppedItem other)
    {
        CheckCurrentBaseWeapon();
        Debug.Log("Picked up " + other.name);
        if (parts[other.itemID].isBaseWeapon && hasBaseWeapon)
        {
            foreach(Item i in parts)
            {
                if (i.isEnabled) i.DropWeapon();
            }
        }


        if (parts[other.itemID].isEnabled) // if the player already has the item
        {
            other.DestroyItem();
        }
        else
        {
            parts[other.itemID].isEnabled = true;
            parts[other.itemID].GetComponent<SpriteRenderer>().enabled = true;
            other.DestroyItem();
        }


        RenderItems();
    }
}
