using UnityEngine;
using System.Collections;

public class Deer : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent agent;
    Transform player;

    public float sightDistance;
    public float sightConeDot;
    public float hearDistance;

    public float runSpeed;
    public float walkSpeed;

    public float minRunTime = 10;
    public float maxRunTime = 30;

    public float minDist = 5;
    public float maxDist = 40;

    public Transform herd;

    bool isRunningAway;
    float runAway;
    float runAwayTimer;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (isRunningAway)
        {
            runAwayTimer += Time.deltaTime;
            
            if (Vector3.Distance(transform.position, agent.destination) < 1)
            {
                if (runAwayTimer >= runAway)
                {
                    isRunningAway = false;
                    agent.speed = walkSpeed;
                }
                else
                {
                    agent.destination = transform.position + (Random.Range(minDist, maxDist) * (Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * transform.forward)) ;
                }
            }
        }
        else
        {
            if (CanSeePlayer())
            {
                isRunningAway = true;
                agent.speed = runSpeed;
                runAway = Random.Range(minRunTime, maxRunTime);
                agent.destination = transform.position + (Random.Range(minDist, maxDist) * (transform.position - player.position));
                runAwayTimer = 0;
            }
            else
            {
                if (Vector3.Distance(transform.position, agent.destination) < 1)
                {
                    agent.destination = herd.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                }
            }
        }
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(player.position, transform.position) < hearDistance) return true;
        
        bool canSee = Vector3.Distance(player.position, transform.position) < sightDistance;
        if (!canSee) return canSee;

        RaycastHit hit;
        return (Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, sightDistance) && hit.transform.tag == "Player");
    }
}
