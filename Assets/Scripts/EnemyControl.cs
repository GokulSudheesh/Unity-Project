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
    [SerializeField] float maxRoamDist = 50f;
    [SerializeField] float coolDown = 5f;
    [SerializeField] float viewRadius = 15f;
    float proximity;
    bool gameOver = false;
    bool isRoam = false;
    bool isChase = false;
    bool inProximity = false;
    bool inRange = false;
    float coolDownTimer;
    bool inCooldown = false;
    Vector3 destination;
    Vector3 prePos;
    Vector3[] destinations_path;
    int dest_i = 0;
    float pathCompleted;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        prePos = transform.position;
        coolDownTimer = coolDown;
        destinations_path = new[] { new Vector3(36f, 1f, 18.7f), new Vector3(4.1f, 1f, 40.9f), new Vector3(20f, 1f, 4.3f)};
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //isChase = lineOfSight() && checkProximity();
        isChase = lineOfSightAngle();
        if (isChase)
        {
            // Add a search algorithm UnityEngine.AI :)
            /*if (inProximity) 
            {
                transform.LookAt(player.transform);
            }*/
            destination = player.transform.position;
            agent.SetDestination(destination);
            //pathCompleted = Vector3.Distance(transform.position, destination);
            pathCompleted = agent.remainingDistance;
            Debug.Log("Chase "+pathCompleted);
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
    }

    private void roam_path()
    {
        destination = destinations_path[dest_i];
        agent.SetDestination(destination);
        //pathCompleted = Vector3.Distance(transform.position, destination);
        pathCompleted = agent.remainingDistance;
        Debug.Log(pathCompleted);
        if (pathCompleted < 1)
        {
            // Cooldown
            cooldown();
            if (!inCooldown)
            {
                dest_i = (dest_i + 1) % destinations_path.Length;
            }
        }
    }

    private void roam()
    {
        if (!isRoam)
        {
            // Calculate a new random posiiton when the enemy is not roaming
            destination = new Vector3(Random.Range(0, maxRoamDist), transform.position.y, Random.Range(0, maxRoamDist));
            isRoam = true;
        }
        if (isRoam)
        {
            // Set the destination to that random destination
            agent.SetDestination(destination);
            //pathCompleted = Vector3.Distance(transform.position, destination);
            pathCompleted = agent.remainingDistance;
            Debug.Log(pathCompleted);
            if (pathCompleted < 1 || prePos == transform.position)
            {
                // prePos == currentPos -> just to check if the enemy is stuck.
                // When the random destination is outside the walls the enemy might get stuck. DO: (simple fix but change this later!)
                // pathCompleted == 0 -> If the enemy reached the destination compute a new random position 
                // Cooldown
                cooldown();
                if (!inCooldown)
                {
                    isRoam = false;
                }
            }
            prePos = transform.position;
        }
    }

    private void cooldown()
    {
        inCooldown = true;
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }
        else
        {
            coolDownTimer = coolDown;
            inCooldown = false;
        }
    }

    private bool checkProximity()
    {
        proximity = Vector3.Distance(transform.position, player.transform.position);
        if (proximity < viewRadius) { 
            inProximity = true;
            return true;
        }
        else
        {
            inProximity = false;
            return false;
        }
    }

    private bool lineOfSight()
    {
        NavMeshHit hit;
        if (!agent.Raycast(player.transform.position, out hit))
        {
            //print("Visible");
            return true;
        }
        else
        {
            //print("Not Visible");
            return false;
        }
    }

    private bool lineOfSightAngle()
    {
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit hit;
        inRange = Vector3.Angle(direction, transform.forward) <= 80f;

        if (Physics.Raycast(transform.position, direction, out hit) && hit.transform.gameObject.name == "Player" && inRange)
        {
            //print("Visible");
            return true;
        }
        else
        {
            //print("Not Visible");
            return false;
        }            
    }
}
