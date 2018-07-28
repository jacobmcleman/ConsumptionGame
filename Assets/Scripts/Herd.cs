using UnityEngine;
using System.Collections;

public class Herd : MonoBehaviour {
    public static int MaxHerdSize = 10;
    public GameObject deerPrefab;
    public Transform herdGuide;
    public int curHerdSize;

    void Start()
    {
        CountHerd();

        StartCoroutine(SpawnDeer());
    }

    int CountHerd()
    {
        curHerdSize = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.StartsWith("Deer")) curHerdSize++;
        }

        return curHerdSize;
    }

    IEnumerator SpawnDeer()
    {
        while (curHerdSize > 1)
        {
            CountHerd();
            float waitTime = Random.Range(256f / curHerdSize, 2048f / curHerdSize);
            yield return new WaitForSeconds(waitTime);
            if (curHerdSize < MaxHerdSize)
            {
                GameObject deer = Instantiate(deerPrefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                deer.GetComponent<Deer>().herd = herdGuide;
                deer.transform.parent = transform;
            }
        }
    }
}
