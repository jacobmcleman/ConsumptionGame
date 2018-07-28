using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

    public float globalBaseTemp = 21;
    float treeChange = 10;
    float bushChange = 5;

    public GameObject trees;
    public GameObject bushes;

    public float Temp;
    

    public GameObject level;
    public Material desertMat;
    public Material ashMat;

    bool desertMatChanged;
    bool ashMatChanged;

    public GameObject water;
    float dryWaterLevel = -2;
    float emptyWaterLevel = -3;

    FPPlayer player;

    void Start()
    {
        desertMatChanged = false;
        ashMatChanged = false;

        player = GameObject.Find("Player").GetComponent<FPPlayer>();
    }

    void Update()
    {
        if (player.paused) return;
        
        Temp =  globalBaseTemp + (10 - treeChange) + (5 - bushChange) + Mathf.Sqrt(Campfire.NumCampfires());

        Rabbit.MAXBUNNIES = 20 * (int)((Tree.TreeRatio() + Bush.BushRatio()) / 2);
        Herd.MaxHerdSize = 15 * (int)((Tree.TreeRatio() + Bush.BushRatio()) / 2);

        if (Temp > 27)
        {
            if (!desertMatChanged)
            {
                Debug.Log("Global temp at: " + Temp + " desertification beginning");
                level.GetComponent<Renderer>().material = desertMat;
                desertMatChanged = true;
                water.transform.position = new Vector3(water.transform.position.x, dryWaterLevel, water.transform.position.z);
            }

            if (!ashMatChanged && Temp > 35)
            {
                Debug.Log("Global temp at: " + Temp + " desertification severe");
                level.GetComponent<Renderer>().material = ashMat;
                ashMatChanged = true;
                water.transform.position = new Vector3(water.transform.position.x, emptyWaterLevel, water.transform.position.z);
            }

            if (Random.Range(0f, 1f) > 0.5f)
            {
                if (trees.transform.childCount > 0) trees.transform.GetChild((int)Random.Range(0, trees.transform.childCount)).GetComponent<Health>().ApplyDamage(Random.Range(0, (Temp - 27) / 10));
            }
            else
            {
                if (bushes.transform.childCount > 0) bushes.transform.GetChild((int)Random.Range(0, bushes.transform.childCount)).GetChild(0).GetComponent<Health>().ApplyDamage(Random.Range(0, (Temp - 27) / 10));
            }
        }

        treeChange = 10 * Tree.TreeRatio();
        bushChange = 5 * Bush.BushRatio();    
    }
}
