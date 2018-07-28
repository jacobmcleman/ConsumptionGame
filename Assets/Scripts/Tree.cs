using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tree : MonoBehaviour, DeathNotified {
    public float pickupDistance = 2;
    Transform player;

    bool shouldShowMessage;
    bool isShowingMessage;
    bool felled;

    GameObject interactField;
    Text interactText;
    RawImage interactIcon;

    Rigidbody rb;

    public GameObject logsPrefab;

    static int startingTrees;
    static int currentTrees;

    Health health;

    void Start()
    {
        //if (Random.Range(0f, 10f) < 1) Destroy(gameObject);
        
        player = GameObject.Find("Player").transform;
        interactField = GameObject.Find("Canvas").transform.Find("InteractPanel").gameObject;
        interactIcon = interactField.transform.Find("Icon").GetComponent<RawImage>();
        interactText = interactField.transform.Find("Label").GetComponent<Text>();
        rb = GetComponent<Rigidbody>();

        startingTrees++;
        currentTrees++;

        rb.isKinematic = true;
        felled = false;

        health = GetComponent<Health>();
    }

    public static float TreeRatio()
    {
        //Debug.Log("Current Tree Ratio " + currentTrees + "/" + startingTrees);
        return (currentTrees / (startingTrees * 1.0f));
    }

    public void OnDeath()
    {
        if (!felled)
        {
            felled = true;
            rb.isKinematic = false;
            rb.AddForceAtPosition(100 * (transform.position - player.position), transform.position + (10 * Vector3.up));
            health.curHealth = health.maxHealth;
            currentTrees--;
        }
        else if (felled)
        {
            Instantiate(logsPrefab, transform.position, transform.rotation);
            
            //TreeRatio();
            Destroy(gameObject);
        }
    }
}
