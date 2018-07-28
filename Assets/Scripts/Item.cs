using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    public string itemname;
    public string displayname;
    public string description = "";

    public int maxStack;
    public int count;

    public GameObject DropItem;

    public Texture2D displayImage;

    public bool isTool = false;

    public bool droppable = true;
    public bool edible = false;
    public bool placeable = false;

    public float foodAmount = 0;

    public string DisplayName
    {
        get {
            if (count == 1) return displayname;
            else return displayname + " (" + count + ")";
        }
    }

    public bool Equals(Item other)
    {
        return other.itemname == this.itemname;
    }

    public Item TryMergeStack(Item other)
    {
        //Debug.Log("Attempting stack merge...");
        if (!Equals(other)) return other;
        else if (other.count + count <= maxStack)
        {
            count += other.count;
            //Debug.Log("Compatible stack with sufficent room for all incoming items, stack merge complete");
            return null;
        }
        else
        {
            other.count -= maxStack - count;
            count = maxStack;
            //Debug.Log("Compatible stack with insufficent room for all incoming items, returning leftovers");
            return other;
        }
    }
}
