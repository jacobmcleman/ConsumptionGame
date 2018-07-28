using UnityEngine;
using System.Collections;

public class HerdMover : MonoBehaviour {

    public Transform[] nodes;
    public float speed;

    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            int i = 1;
            float swingTime = 0;
            while (i < nodes.Length)
            {
                swingTime = 0;
                while (swingTime < speed / nodes.Length)
                {
                    swingTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(nodes[i - 1].position, nodes[i].position, swingTime / (speed / nodes.Length));
                    yield return null;
                }
                i++;
            }
            swingTime = 0;
            i -= 1;
            while (i > 0)
            {
                swingTime = 0;
                while (swingTime < speed / nodes.Length)
                {
                    swingTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(nodes[i].position, nodes[i - 1].position, swingTime / (speed / nodes.Length));
                    yield return null;
                }
                i--;
            }
            yield return null;
        }
    }
}
