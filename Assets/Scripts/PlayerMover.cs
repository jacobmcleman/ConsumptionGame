using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour {
    public Transform planet;
    bool isGrounded;

    public bool Grounded
    {
        get { return isGrounded; }
    }

    void Update()
    {
        if(planet == null) return;
        transform.rotation = Quaternion.LookRotation(transform.position - planet.position);
    }

    void OnTriggerEnter()
    {
        isGrounded = true;
    }

    void OnTriggerExit()
    {
        isGrounded = false;
    }

}
