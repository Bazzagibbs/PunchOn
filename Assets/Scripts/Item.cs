using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID = -1;
    public bool isEnabled = false;
    public bool isBaseWeapon = false;

    public int damageModifier = 0;
    public int knockbackModifier = 0;
    public int stunModifier = 0;
    public DroppedItem dropPrefab;

    public void DropWeapon()
    {
        isEnabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        DroppedItem droppedWeapon = Instantiate(dropPrefab, transform.position, transform.rotation);
        droppedWeapon.itemID = itemID;
        droppedWeapon.SpriteFromID();
    }

}
