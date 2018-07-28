using UnityEngine;
using System.Collections;

public class Bow : Weapon {
    public float maxDraw = 0.2f;
    public float drawTime = 2f;
    public float maxPower = 50;

    public Transform knockPoint;
    public GameObject arrowPrefab;

    public void Use()
    {
        if (!isSwinging)
        {
            StartCoroutine(DrawAndFire());
        }
    }

    IEnumerator DrawAndFire()
    {
        bool back = true;
        float pull = 0;

        GameObject arrow = Instantiate(arrowPrefab);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        arrow.transform.parent = knockPoint;
        isSwinging = true;

        rb.isKinematic = true;

        while (Input.GetMouseButton(0))
        {
            if (back)
            {
                pull += Time.deltaTime * (maxDraw / drawTime);
                if (pull > maxDraw) back = false;
            }

            arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            arrow.transform.localPosition = new Vector3(0, 0, -pull);
            yield return null;
        }

        rb.isKinematic = false;
        arrow.transform.SetParent(null);
        rb.AddForce(arrow.transform.forward * maxPower * (pull / maxDraw));

        isSwinging = false;
    }
}
