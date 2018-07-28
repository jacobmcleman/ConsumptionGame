using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
    public float damage;
    bool stuck;

    void Start()
    {
        stuck = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (stuck) return;
        if (other.GetComponent<Health>())
        {
            other.GetComponent<Health>().ApplyDamage(damage, 2);
        }
        transform.SetParent(other.transform);
        GetComponent<Rigidbody>().isKinematic = true;
        stuck = true;
    }

    void Update()
    {
        if (!stuck && !GetComponent<Rigidbody>().isKinematic)
        {
            transform.LookAt(transform.position + GetComponent<Rigidbody>().velocity);
        }

    }
}
