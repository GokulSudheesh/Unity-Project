using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Bed : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera hidingCamera;
    
    private bool isHiding = false;
    private bool guiShow = false;
    
    private int rayLength = 10;
    
    [SerializeField] GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera.GetComponent<Camera>().enabled = true;
        hidingCamera.GetComponent<Camera>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 fwd;
        fwd = transform.TransformDirection(Vector3.forward);
        
        if(Physics.Raycast(transform.position, fwd, out hit, rayLength))
        {
        	if(hit.collider.gameObject.tag == "Hide_Bed" && isHiding == false)
        	{
        		guiShow = true;
        		
        		if(Input.GetKeyDown("x"))
        		{
        			//Disable Player
        			player.SetActive(false);
        			
        		
        			//Change Cameras
        			mainCamera.GetComponent<Camera>().enabled = false;
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
        	if(Input.GetKeyDown("x"))
        	{
        		//Disable Player
        			player.SetActive(true);
        			
        			//Change Cameras
        			mainCamera.GetComponent<Camera>().enabled = true;
        			hidingCamera.GetComponent<Camera>().enabled = false;
        			
        			isHiding = false;
        	}
        }
    }
    
    IEnumerator Wait()
    {
    	yield return new WaitForSeconds(0.5f);
    	isHiding = true;
    	guiShow = false;
    }
    
    void onGUI()
    {
    	if(guiShow == true)
    	{
    		GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 100, 25), "Hide Inside?");
    	}
    }
}
