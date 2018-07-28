using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Campfire : MonoBehaviour {
    bool isBurning;

    public float interactDistance = 2;
    public Texture interactImage;
    Transform player;

    bool shouldShowMessage;
    bool isShowingMessage;

    GameObject interactField;
    Text interactText;
    RawImage interactIcon;

    public float warmthRegenRate = 5;

    public GameObject cookedMeatPrefab;


    static int curCampfires;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        interactField = GameObject.Find("Canvas").transform.Find("InteractPanel").gameObject;
        interactIcon = interactField.transform.Find("Icon").GetComponent<RawImage>();
        interactText = interactField.transform.Find("Label").GetComponent<Text>();

        curCampfires++;
    }

    public static int NumCampfires()
    {
        return curCampfires;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerNeeds>())
        {
            other.GetComponent<PlayerNeeds>().changeWarmth(warmthRegenRate * Time.deltaTime);
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < interactDistance && 
            Vector3.Dot((player.position - transform.position).normalized, player.Find("Camera").forward) < -0.96f &&
            player.GetComponent<Inventory>().HasItem("rawmeat"))
        {
            shouldShowMessage = true;
        }
                
        else
            shouldShowMessage = false;

        if (shouldShowMessage && !isShowingMessage)
        {
            //Show the interact message
            interactText.text = "Press E to cook one meat";
            interactIcon.texture = interactImage;
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
            //shouldShowMessage = false;
            //interactField.SetActive(false);
            if (player.GetComponent<Inventory>().RemoveSingleItem("rawmeat"))
            {
                player.GetComponent<Inventory>().AddItem(Instantiate(cookedMeatPrefab));
            }
        }
    }
}
