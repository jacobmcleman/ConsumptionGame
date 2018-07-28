using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public int INVSIZE = 4;
    public int toolBeltSize = 3;

    public GameObject[] inventory;
    public GameObject[] toolBelt;

    public Wielder wielder;
    public PlayerNeeds needs;

    bool shouldShowInventory;
    bool isShowingInventory;

    GameObject invMenu;
    GameObject inv1, inv2, inv3, inv4;

    GameObject invInteractMenu;
    GameObject recipesPanel;

    int curInvInteractSlot;

    public Recipe[] recipes;
    public GameObject recipeButton;

    bool placingItem;
    int activeSlot;

    Transform cam;

    GameObject toolbar;
    GameObject tool1, tool2, tool3;
    bool tool1Shown, tool2Shown, tool3Shown;

    int curTool;
    int shownTool;

    void Start()
    {
        inventory = new GameObject[INVSIZE];
        toolBelt = new GameObject[toolBeltSize];

        shouldShowInventory = false;
        isShowingInventory = false;

        invMenu = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        inv1 = invMenu.transform.Find("Inv1").gameObject;
        inv2 = invMenu.transform.Find("Inv2").gameObject;
        inv3 = invMenu.transform.Find("Inv3").gameObject;
        inv4 = invMenu.transform.Find("Inv4").gameObject;

        invInteractMenu = invMenu.transform.Find("InvInteract").gameObject;
        recipesPanel = invInteractMenu.transform.Find("Crafting").Find("RecipesScrollPanel").Find("RecipesPanel").gameObject;

        toolbar = GameObject.Find("Canvas").transform.Find("ToolBelt").gameObject;
        tool1 = toolbar.transform.Find("Tool1").gameObject;
        tool1Shown = false;
        tool1.SetActive(false);
        tool2 = toolbar.transform.Find("Tool2").gameObject;
        tool2Shown = false;
        tool2.SetActive(false);
        tool3 = toolbar.transform.Find("Tool3").gameObject;
        tool3Shown = false;
        tool3.SetActive(false);

        invMenu.SetActive(false);
        invInteractMenu.SetActive(false);

        placingItem = false;
        activeSlot = -1;
        curInvInteractSlot = -1;
        curTool = -1;
        shownTool = -1;

        cam = transform.Find("Camera");
    }

    public void AddItem(GameObject item)
    {
        Item remItem = item.GetComponent<Item>();
        item.SetActive(false);
        if (item.GetComponent<Rigidbody>() != null) item.GetComponent<Rigidbody>().isKinematic = true;

        if (remItem.isTool)
        {
            if (remItem.itemname == "axe")
            {
                toolBelt[0] = item;
                curTool = 0;
                Destroy(item.GetComponent<ItemPickup>());
            }
            if (remItem.itemname == "knife")
            {
                toolBelt[1] = item;
                curTool = 1;
                Destroy(item.GetComponent<ItemPickup>());
            }
            if (remItem.itemname == "bow")
            {
                toolBelt[2] = item;
                curTool = 2;
                Destroy(item.GetComponent<ItemPickup>());
            }
            return;
        }

        for (int i = 0; i < INVSIZE && remItem != null; i++)
        {
            if (inventory[i] == null || inventory[i].GetComponent<Item>().count == 0)
            {
                //Debug.Log("Empty slot found, item picked up");
                inventory[i] = item;
                remItem = null;
            }
            else
            {
                remItem = inventory[i].GetComponent<Item>().TryMergeStack(remItem);
            }
        }

        if (remItem != null)
        {
            //Debug.Log("Not all items fit in inventory, dropping remainder");
            DropItem(item);
        }
    }

    void DropItem(GameObject item)
    {
        item.transform.position = transform.position;
        if (item.GetComponent<Rigidbody>() != null) item.GetComponent<Rigidbody>().isKinematic = false;
        item.SetActive(true);
    }

    void DropItem(int slot)
    {
        DropItem(inventory[slot]);
        inventory[slot] = null;
    }

    public void DropSelectedItem()
    {
        DropItem(curInvInteractSlot);
        HideInvInteract(curInvInteractSlot);
    }

    public void PlaceSelectedItem()
    {
        placingItem = true;
        activeSlot = curInvInteractSlot;
        shouldShowInventory = false;
        HideInvInteract(curInvInteractSlot);
    }

    public void EatSelectedItem()
    {
        needs.changeHunger(inventory[curInvInteractSlot].GetComponent<Item>().foodAmount);
        inventory[curInvInteractSlot].GetComponent<Item>().count--;
        if (inventory[curInvInteractSlot].GetComponent<Item>().count <= 0)
        {
            HideInvInteract(curInvInteractSlot);
        }
    }

    public void ShowInvInteract(int slot)
    {
        //Show interaction menu
        if (curInvInteractSlot == slot)
        {
            HideInvInteract(slot);
            return;
        }
        else
        {
            HideInvInteract(slot);
        }

        Text title = invInteractMenu.transform.Find("Title").GetComponent<Text>();

        Item item = inventory[slot].GetComponent<Item>();
        title.text = item.DisplayName;

        curInvInteractSlot = slot;

        //Enable/disable options depending on item
        GameObject dropButton = invInteractMenu.transform.Find("Drop").gameObject;
        GameObject placeButton = invInteractMenu.transform.Find("Place").gameObject;
        GameObject eatButton = invInteractMenu.transform.Find("Eat").gameObject;

        dropButton.SetActive(inventory[slot].GetComponent<Item>().droppable);
        placeButton.SetActive(inventory[slot].GetComponent<Item>().placeable);
        eatButton.SetActive(inventory[slot].GetComponent<Item>().edible);

        //Show relevant recipes
        int applicableRecipes = 0;
        foreach (Recipe recipe in recipes)
        {
            if (!((recipe.results[0].itemname == "axe" && toolBelt[0] != null)
                || (recipe.results[0].itemname == "knife" && toolBelt[1] != null)
                || (recipe.results[0].itemname == "bow" && toolBelt[2] != null)))
            {

                foreach (Item ingredient in recipe.components)
                {
                    if (ingredient.itemname == item.itemname)
                    {
                        applicableRecipes++;
                        GameObject recButt = Instantiate(recipeButton);
                        recButt.transform.SetParent(recipesPanel.transform);

                        Recipe rec = recButt.GetComponent<Recipe>();
                        rec.components = recipe.components;
                        rec.results = recipe.results;

                        int numIngredients = recipe.components.Length;
                        int numResults = recipe.results.Length;

                        for (int i = 1; i <= 6; i++)
                        {
                            if (i > numIngredients) recButt.transform.Find("Component" + i).gameObject.SetActive(false);
                            else
                            {
                                GameObject com = recButt.transform.Find("Component" + i).gameObject;
                                com.GetComponent<Text>().text = recipe.components[i - 1].DisplayName;
                                com.transform.Find("Icon").gameObject.GetComponent<RawImage>().texture = recipe.components[i - 1].displayImage;
                                com.SetActive(true);
                            }

                            if (i > numResults) recButt.transform.Find("Result" + i).gameObject.SetActive(false);
                            else
                            {
                                GameObject res = recButt.transform.Find("Result" + i).gameObject;
                                res.GetComponent<Text>().text = recipe.results[i - 1].DisplayName;
                                res.transform.Find("Icon").gameObject.GetComponent<RawImage>().texture = recipe.results[i - 1].displayImage;
                                res.SetActive(true);
                            }
                        }

                        break;
                    }
                }
            }
        }

        invInteractMenu.SetActive(true);
    }

    public bool RemoveSingleItem(string name)
    {
        for (int i = 0; i < INVSIZE; i++)
        {
            if (inventory[i] != null && inventory[i].GetComponent<Item>().itemname == name && inventory[i].GetComponent<Item>().count > 0)
            {
                inventory[i].GetComponent<Item>().count--;
                return true;
            }
        }
        return false;
    }

    public bool HasItem(string name)
    {
        for (int i = 0; i < INVSIZE; i++)
        {
            if (inventory[i] != null && inventory[i].GetComponent<Item>().itemname == name && inventory[i].GetComponent<Item>().count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void HideInvInteract(int slot)
    {
        foreach (Transform child in recipesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        curInvInteractSlot = -1;
        invInteractMenu.SetActive(false);
    }

    void Update()
    {  
        //Place items
        if (placingItem)
        {
            GameObject item = inventory[activeSlot];

            int layerMask = 1 << 9;

            RaycastHit hit;
            if (Physics.Raycast(cam.position + cam.forward, cam.forward, out hit, 5, layerMask) && hit.point.y > -0.3f && Vector3.Dot(hit.normal, Vector3.up) > 0.94f)
            {
                Renderer[] renderers = item.GetComponentsInChildren<Renderer>();
                
                //Place the thing
                if (Input.GetMouseButtonDown(0))
                {
                    placingItem = false;
                    activeSlot = -1;
                    if (item.GetComponent<Rigidbody>() != null) item.GetComponent<Rigidbody>().isKinematic = false;
                    GameObject newItem = Instantiate(item, item.transform.position, item.transform.rotation) as GameObject;
                    item.GetComponent<Item>().count--;
                    newItem.GetComponent<Item>().count = 1;
                    item.SetActive(false); 

                }
                else //Display the ghost of the thing
                {
                    if (item.GetComponent<Rigidbody>() != null) item.GetComponent<Rigidbody>().isKinematic = true;
                    item.transform.position = hit.point;
                    item.transform.LookAt(new Vector3(transform.position.x, item.transform.position.y, transform.position.z));
                    item.SetActive(true);
                }

                //item.transform.rotation = Quaternion.LookRotation(transform.position - item.transform.position, hit.normal);
                
            }
            else
            {
                item.SetActive(false);
            }
        }

        if (curTool != shownTool)
        {
            wielder.UnWield();
            if (curTool >= 0)
            {
                wielder.Wield(toolBelt[curTool]);
            }

            shownTool = curTool;
        }

        if (!tool1Shown && !tool2Shown && !tool3Shown)
        {
            toolbar.SetActive(false);
        }
        if (!tool1Shown && toolBelt[0] != null)
        {
            tool1.SetActive(true);
            tool1Shown = true;
            toolbar.SetActive(true);
        }
        if (!tool2Shown && toolBelt[1] != null)
        {
            tool2.SetActive(true);
            tool2Shown = true;
            toolbar.SetActive(true);
        }
        if (!tool3Shown && toolBelt[2] != null)
        {
            tool3.SetActive(true);
            tool3Shown = true;
            toolbar.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) curTool = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) curTool = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) curTool = 2;

        //Show/hide
        if (shouldShowInventory && !isShowingInventory)
        {
            //Show the inventory
            invMenu.SetActive(true);
            isShowingInventory = true;

            //Release and show the cursor, and lock movement
            GetComponent<FPPlayer>().locked = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (isShowingInventory && !shouldShowInventory)
        {
            //Hide the inventory
            invInteractMenu.SetActive(false);
            invMenu.SetActive(false);
            isShowingInventory = false;

            //Release and player and hide the cursor
            Cursor.visible = false;
            GetComponent<FPPlayer>().locked = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Do the showing stuff
        if (isShowingInventory)
        {
            //Show/hide each slot depending on if they are filled
            Item item;
            if (inventory[0] == null || (item = inventory[0].GetComponent<Item>()).count == 0) inv1.SetActive(false);
            else
            {
                inv1.transform.Find("Text").GetComponent<Text>().text = item.DisplayName;
                inv1.transform.Find("Icon").GetComponent<RawImage>().texture = item.displayImage;
                inv1.transform.Find("Description").GetComponent<Text>().text = item.description;
                inv1.SetActive(true);
            }

            if (inventory[1] == null || (item = inventory[1].GetComponent<Item>()).count == 0) inv2.SetActive(false);
            else
            {
                inv2.transform.Find("Text").GetComponent<Text>().text = item.DisplayName;
                inv2.transform.Find("Icon").GetComponent<RawImage>().texture = item.displayImage;
                inv2.transform.Find("Description").GetComponent<Text>().text = item.description;
                inv2.SetActive(true);
            }

            if (inventory[2] == null || (item = inventory[2].GetComponent<Item>()).count == 0) inv3.SetActive(false);
            else
            {
                inv3.transform.Find("Text").GetComponent<Text>().text = item.DisplayName;
                inv3.transform.Find("Icon").GetComponent<RawImage>().texture = item.displayImage;
                inv3.transform.Find("Description").GetComponent<Text>().text = item.description;
                inv3.SetActive(true);
            }


            if (inventory[3] == null || (item = inventory[3].GetComponent<Item>()).count == 0) inv4.SetActive(false);
            else
            {
                inv4.transform.Find("Text").GetComponent<Text>().text = item.DisplayName;
                inv4.transform.Find("Icon").GetComponent<RawImage>().texture = item.displayImage;
                inv4.transform.Find("Description").GetComponent<Text>().text = item.description;
                inv4.SetActive(true);
            }
        }

        //Toggle Button
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shouldShowInventory = !shouldShowInventory;
            placingItem = false;
            if (activeSlot >= 0)
            {
                GameObject item = inventory[activeSlot];
                activeSlot = -1;
            }
        }
    }
}
