using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    GameObject player;
    GameObject vitualCam;

    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float fpCamHeight = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public bool fpCam;
    CinemachineBrain tpVcam;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = GameObject.Find("Player");
        fpCam = true;
        // Get Cinemachine instance
        tpVcam = GetComponent<CinemachineBrain>();
        tpVcam.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            fpCam = !fpCam;
            tpVcam.enabled = !tpVcam.enabled;        
        }
    }
    void LateUpdate()
    {        
        if (fpCam)
        {
            // Camera Rotation: https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            player.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + fpCamHeight, player.transform.position.z);
        } 
    }
}