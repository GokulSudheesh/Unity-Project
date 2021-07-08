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
    GameObject trap;
    int trapCount = 10;
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
        trap = GameObject.Find("Trap");
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

            if (go_to(player.transform.position, "Chase"))
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
            if (go_to(lastknownLoc, "Investigate"))
            {
                // Cooldown
                cooldown();
                if (!inCooldown)
                {
                    if (trapCount > 0)
                    {
                        // Spawn a trap game object
                        spawnTrap();
                    }
                    investigate = false;
                }
            }
        }
        else
        {
            roam();
        }
    }

    private bool go_to(Vector3 dest, String state)
    {
        // Returns true when the enemy gets stuck or completes the path
        destination = dest;
        agent.SetDestination(destination);
        //pathCompleted = Vector3.Distance(transform.position, destination);
        pathCompleted = agent.remainingDistance;
        Debug.Log(state + " " + pathCompleted);
        if (pathCompleted <= 1 || prePos == transform.position)
        {
            return true;
        }
        prePos = transform.position;
        return false;
    }

    private void spawnTrap()
    {
        if(Random.Range(0, 100) > 50) // 50 - 50 probability to spawn the trap
        {
            trapCount--;
            Instantiate(trap, new Vector3(transform.position.x, transform.position.y - 1.166667f, transform.position.z), Quaternion.identity);
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
        if (go_to(destinations_path[dest_i], "Roam"))
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
            if (go_to(destination, "Roam"))
            {
                cooldown();
                if (!inCooldown)
                {
                    isRoam = false;
                }
            }
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