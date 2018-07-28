using UnityEngine;
using System.Collections;

public class Recipe : MonoBehaviour {
    public Item[] components;
    public Item[] results;

    Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Player").GetComponent<Inventory>();
    }

    public void Craft()
    {
        Debug.Log("Attempting to craft " + results[0].DisplayName);
        //Check that there is enough
        int[] needed = new int[components.Length];
        for (int i = 0; i < needed.Length; i++)
        {
            needed[i] = components[i].count;
        }

        for (int j = 0; j < needed.Length; j++)
        {
            Item comp = components[j];
            for (int i = 0; i < inv.inventory.Length && needed[j] > 0; i++)
            {
                Item item = null;
                if (inv.inventory[i] != null)
                {
                    item = inv.inventory[i].GetComponent<Item>();
                    //Debug.Log("Item component found");
                }
                if (item != null && item.itemname == comp.itemname)
                {
                    needed[j] -= item.count;
                    Debug.Log("Found " + item.DisplayName + ", still need " + needed[j]);
                }
            }
        }

        for (int i = 0; i < needed.Length; i++)
        {
           
            if (needed[i] > 0)
            {
                 //Insufficient components, go home
                Debug.Log("Crafting failed - insufficient supplies");
                Debug.Log("Need additional " + components[i].DisplayName);
                return;
            }
        }

        //Take the components since there is enough
        for (int i = 0; i < needed.Length; i++)
        {
            needed[i] = components[i].count;
        }

        for (int j = 0; j < needed.Length; j++)
        {
            Item comp = components[j];
            int i = 0;
            while (i < inv.inventory.Length && needed[j] > 0)
            {
                Item item = null;
                if(inv.inventory[i] != null) item = inv.inventory[i].GetComponent<Item>();
                if (item != null && item.itemname == comp.itemname)
                {
                    while(item.count > 0 && needed[j] > 0){
                        item.count--;
                        needed[j]--;
                    }
                }
                i++;
            }
        }

        foreach (Item result in results)
        {
            GameObject thing = Instantiate(result.DropItem);
            thing.GetComponent<Item>().count = result.count;
            inv.AddItem(thing);
        }

        Debug.Log("Crafting Success!");
    }
}
