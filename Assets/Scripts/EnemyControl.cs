using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    GameObject player;
    public float speed = 2f;
    float proximity;
    bool isChase = false;
    bool inProximity = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        checkProximity();
        if (isChase)
        {
            // Add a search algorithm
            if (inProximity) 
            {
                transform.LookAt(player.transform);
            }            
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
