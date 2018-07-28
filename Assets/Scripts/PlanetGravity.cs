using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {
    public Rigidbody rb;
    public Transform planet;
    public float gravity = 9.81f;

    void Update()
    {
        if (rb != null)
        {
            rb.AddForce(gravity * Time.deltaTime * (planet.position - transform.position));
        }
    }
}
