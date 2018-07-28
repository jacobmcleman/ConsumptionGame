using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class DropOnDeath : MonoBehaviour, DeathNotified {
    public GameObject[] drops;

    public void OnDeath()
    {
        foreach (GameObject go in drops)
        {
            Instantiate(go, transform.position, Quaternion.Euler(Vector3.zero));
        }
        Destroy(gameObject);
    }
}
