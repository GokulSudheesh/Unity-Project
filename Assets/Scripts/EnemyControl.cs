using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    public float speed = 2f;
    float proximity;
    bool isChase = false;
    bool inProximity = false;
    //Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();

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
            agent.SetDestination(player.transform.position);
            //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
        //Debug.Log(proximity);
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
