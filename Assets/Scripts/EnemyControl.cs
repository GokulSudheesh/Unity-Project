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
    [SerializeField] float speed = 3.5f;
    [SerializeField] float sprintSpeed = 5.0f;
    [SerializeField] float maxRoamDist = 50f;
    [SerializeField] float coolDown = 5f;
    [SerializeField] float viewRadius = 15f;
    float sprintCoolDown = 10f;
    float chaseSpeed;
    float proximity;
    bool gameOver = false;
    /* Enemy Sates */
    bool isRoam = false;
    bool isChase = false;
    bool investigate = false;
    /* Player Detection */
    bool inProximity = false;
    bool inRange = false;
    /* Cooldown to pause while roaming */
    float coolDownTimer;
    bool inCooldown = false;
    Vector3 destination;
    Vector3 lastknownLoc;
    Vector3 prePos;
    Vector3[] destinations_path;
    int dest_i = 0;
    float pathCompleted;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        chaseSpeed = sprintSpeed;
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
        isChase = lineOfSightAngle() || checkProximity();
        if (isChase)
        {
            agent.speed = chaseSpeed;
            sprint();
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
            investigate = true;
            lastknownLoc = player.transform.position;
        }
        else if (investigate)
        {
            agent.speed = speed;
            chaseSpeed = sprintSpeed;
            sprintCoolDown = 10f;
            agent.SetDestination(lastknownLoc);
            pathCompleted = agent.remainingDistance;
            Debug.Log("Investigate " + pathCompleted);
            if (pathCompleted < 1)
            {
                // Cooldown
                cooldown();
                if (!inCooldown)
                {
                    investigate = false;
                }
            }
        }
        else
        {
            roam();
        }
    }

    private void sprint()
    {
        if (sprintCoolDown > 0)
        {
            sprintCoolDown -= Time.deltaTime;
        }
        else
        {
            if (chaseSpeed == sprintSpeed)
            {
                chaseSpeed = speed;
                sprintCoolDown = 2f; // Catch breath for 2 secs (enemy will walk)
            }
            else
            {
                chaseSpeed = sprintSpeed;
                sprintCoolDown = 10f; // Run for 10 secs (enemy will run)
            }
        }
    }

    private void roam_path()
    {
        destination = destinations_path[dest_i];
        agent.SetDestination(destination);
        //pathCompleted = Vector3.Distance(transform.position, destination);
        pathCompleted = agent.remainingDistance;
        Debug.Log("Roam "+pathCompleted);
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
            Debug.Log("Roam "+pathCompleted);
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
        if (proximity < viewRadius)
        {
            inProximity = hidden();
            return inProximity;
        }
        else
        {
            inProximity = false;
            return inProximity;
        }
    }

    private bool hidden()
    {
        // Blocked by enemy's vision (walls / other obstacles)
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
