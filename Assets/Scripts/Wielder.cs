using UnityEngine;
using System.Collections;

public class Wielder : MonoBehaviour {
    public Inventory myInv;
    public Transform hand;

    GameObject curWeapon;
    Weapon wpn;

    public FPPlayer player;

    public void Wield(GameObject weapon)
    {
        UnWield();
        weapon.SetActive(true);
        curWeapon = weapon;
        wpn = weapon.GetComponent<Weapon>();
        weapon.transform.SetParent(hand, true);
        weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        weapon.transform.localPosition = Vector3.zero;

        wpn.wielder = transform.parent.gameObject;

        //Use();
    }

    public void UnWield()
    {
        if (curWeapon != null)
        {
            curWeapon.SetActive(false);
            curWeapon.transform.SetParent(transform.parent, true);
            curWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            curWeapon.transform.localPosition = new Vector3(0.4f, 0, 0);
            wpn = null;
            curWeapon = null;
        }
    }

    public void Use()
    {
        //Debug.Log("Attack");
        if (wpn != null && !player.locked)
        {
            if (wpn.type == 2 && !wpn.isSwinging && player.GetComponent<Inventory>().HasItem("arrow"))
            {
                ((Bow)wpn).Use();
                player.GetComponent<Inventory>().RemoveSingleItem("arrow");
            }
            else wpn.Use();
        }
    }

    void Update()
    {
        if (Input.GetAxis("Fire1") != 0) Use();
    }
}
