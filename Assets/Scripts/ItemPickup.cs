using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour {
    public Item item;
    public float pickupDistance = 2;
    Transform player;

    bool shouldShowMessage;
    bool isShowingMessage;

    GameObject interactField;
    Text interactText;
    RawImage interactIcon;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        interactField = GameObject.Find("Canvas").transform.Find("InteractPanel").gameObject;
        interactIcon = interactField.transform.Find("Icon").GetComponent<RawImage>();
        interactText = interactField.transform.Find("Label").GetComponent<Text>();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 2 && Vector3.Dot((player.position - transform.position).normalized, player.Find("Camera").forward) < -0.96f)
            shouldShowMessage = true;
        else
            shouldShowMessage = false;

        if (shouldShowMessage && !isShowingMessage)
        {
            //Show the interact message
            interactText.text = "Press E to pick up " + item.DisplayName;
            interactIcon.texture = item.displayImage;
            interactField.SetActive(true);
            isShowingMessage = true;
        }
        else if (isShowingMessage && !shouldShowMessage)
        {
            //Hide the interact message
            interactField.SetActive(false);
            isShowingMessage = false;
            //if (hasPickedUp) Destroy(gameObject);
        }

        if (shouldShowMessage && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Picking up " + item.itemname);
            player.GetComponent<Inventory>().AddItem(gameObject);
            shouldShowMessage = false;
            interactField.SetActive(false);
        }
    }
}
