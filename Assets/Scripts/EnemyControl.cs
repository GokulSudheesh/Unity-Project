using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyControl : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    //public float speed = 2f;
    public float maxRoamDist = 50f;
    float proximity;
    bool gameOver = false;
    bool isRoam = false;
    bool isChase = false;
    bool inProximity = false;
    Vector3 destination;
    Vector3 prePos;
    float pathCompleted;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        prePos = transform.position;

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        checkProximity();
        if (isChase)
        {
            // Add a search algorithm UnityEngine.AI :)
            if (inProximity) 
            {
                transform.LookAt(player.transform);
            }
            destination = player.transform.position;
            agent.SetDestination(destination);
            pathCompleted = Vector3.Distance(transform.position, destination);
            Debug.Log(pathCompleted);
            if (pathCompleted <= 1)
            {
                Debug.Log("Game Over!");
                gameOver = true;
            }
        }
        else
        {
            roam();
        }
        
        //Debug.Log(proximity);
    }

    private void roam()
    {
        if (!isRoam)
        {
            // Calculate a new random posiiton when the enemy is not roaming
            destination = new Vector3(Random.Range(0, maxRoamDist), transform.position.y, Random.Range(0, maxRoamDist));
            //destination = new Vector3(21.02f, 0.3015547f, -4.84f);
            isRoam = true;
            // DO :(Add a delay ova here lateeer)
        }
        if (isRoam)
        {
            // Set the destination to that random destination
            agent.SetDestination(destination);
            pathCompleted = Vector3.Distance(transform.position, destination);
            Debug.Log(pathCompleted);
            if (pathCompleted == 0 || prePos == transform.position)
            {
                // prePos == currentPos -> just to check if the enemy is stuck.
                // When the random destination is outside the walls the enemy might get stuck. DO: (simple fix but change this later!)
                // pathCompleted == 0 -> If the enemy reached the destination compute a new random position 
                isRoam = false;
            }
            prePos = transform.position;
        }
    }

    private void checkProximity()
    {
        proximity = Vector3.Distance(transform.position, player.transform.position);
        if (proximity < 15f) {            
            isChase = true;
            inProximity = true;
        }
        else
        {
            isChase = false;
            inProximity = false;
        }
    }
}
