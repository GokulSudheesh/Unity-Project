using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 offsetPos;
    GameObject player;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = GameObject.Find("Player");
        offsetPos = new Vector3(transform.position.x - player.transform.position.x,
            transform.position.y - player.transform.position.y, transform.position.z - player.transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Camera Rotation: https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        transform.position = player.transform.position + offsetPos;
    }
}