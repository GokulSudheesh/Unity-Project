using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 offsetPos;
    GameObject enemy;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        enemy = GameObject.Find("Enemy");
        offsetPos = new Vector3(transform.position.x - enemy.transform.position.x, 
            transform.position.y - enemy.transform.position.y, transform.position.z - enemy.transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = enemy.transform.rotation;
        transform.position = enemy.transform.position + offsetPos;
    }
}
