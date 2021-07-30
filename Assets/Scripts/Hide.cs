using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Hide : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    
    Camera hidingCamera;
    [SerializeField] CinemachineFreeLook freeLookCam;

    public bool isHiding = false;
    private bool guiShow = false;
    
    private float rayLength = 3f;
    private string hit_tag;
    
    [SerializeField] GameObject player;

    GameObject hide_prompt;
    
    // Start is called before the first frame update
    void Awake()
    {
        mainCamera.GetComponent<Camera>().enabled = true;
        freeLookCam.GetComponent<CinemachineFreeLook>().enabled = true;
        hide_prompt = GameObject.Find("Hide-Prompt");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 fwd;
        fwd = player.transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(player.transform.position, fwd, out hit, rayLength))
        {
            hit_tag = hit.collider.gameObject.name; // Get the name of the object

            if ((hit_tag.Split('-')[0] == "Hide") && isHiding == false)
        	{                
                guiShow = true;
        		
        		if(Input.GetKeyDown("x"))
        		{
                    guiShow = false;
                    // Get the camera
                    hidingCamera = getCam(hit_tag.Split('-')[1]);
                    //Disable Player
                    player.SetActive(false);
                    
                    //Change Cameras
                    mainCamera.GetComponent<Camera>().enabled = false;
                    freeLookCam.GetComponent<CinemachineFreeLook>().enabled = false;
                    hidingCamera.GetComponent<Camera>().enabled = true;
                    
                    StartCoroutine(Wait());
        		}
        	}
        }
        
        else
        {
        	guiShow = false;
        }
        
        if (isHiding == true)
        {
            // Get out prompt
            hide_prompt.GetComponent<Text>().text = "X: Get out";
            guiShow = true;
        	if(Input.GetKeyDown("x"))
        	{
                get_out();
                guiShow = false;
            }
        }
        else
        {
            // Hiding prompt
            hide_prompt.GetComponent<Text>().text = "X: Hide";
        }
        onGUI();
    }

    private Camera getCam(string obj_name)
    {
        GameObject cam_object = GameObject.Find(obj_name);
        return cam_object.GetComponent<Camera>();
    }

    public void get_out()
    {
        //Disable Player
        player.SetActive(true);

        //Change Cameras
        mainCamera.GetComponent<Camera>().enabled = true;
        freeLookCam.GetComponent<CinemachineFreeLook>().enabled = true;
        hidingCamera.GetComponent<Camera>().enabled = false;

        isHiding = false;
    }

    IEnumerator Wait()
    {
    	yield return new WaitForSeconds(0.5f);
    	isHiding = true;
    }
    
    void onGUI()
    {
        hide_prompt.SetActive(guiShow);
    }
}
