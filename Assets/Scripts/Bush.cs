using UnityEngine;
using System.Collections;

public class Bush : MonoBehaviour, DeathNotified {
    static int startingBushes;
    static int currentBushes;

    public GameObject bunnyPrefab;

    void Start()
    {
        startingBushes++;
        currentBushes++;

        StartCoroutine(SpawnBunnies());
    }

    IEnumerator SpawnBunnies()
    {
        while (true)
        {
            float waitTime = Random.Range(10, 180);
            yield return new WaitForSeconds(waitTime);
            if (Rabbit.curBunnies < Rabbit.MAXBUNNIES)
            {
                GameObject bun = Instantiate(bunnyPrefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            }
        }
    }

    public static float BushRatio()
    {
        return (currentBushes / (startingBushes * 1.0f));
    }

    public void OnDeath()
    {
        currentBushes--;
        StopCoroutine(SpawnBunnies());
        Destroy(transform.parent.gameObject);
    }
}
