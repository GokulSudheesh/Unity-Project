using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    [SerializeField] private float jumpSpeed; //Calculate the jump speed
    private bool onGround;
    
    
    void Start() {
    	 rb = GetComponent<Rigidbody> (); //get the rigidbody component of the current object the script is attached to
    	 onGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");//-1 or 1 value is inputted to the horizontal axis. By default left and right arrow keys.
        float verticalInput = Input.GetAxis("Vertical"); //Mapped to the up and down arrow key
        
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);//Three dimensional vector required for the object to move
        movementDirection.Normalize();//Normalizes the vector and sets magnitude to one
        
        rb.AddForce (movementDirection * speed);
        
        //Simple code to make the character sprint
        
        
        if (Input.GetKey (KeyCode.Z))
        	
        		transform.Translate(movementDirection * (speed*2) * Time.deltaTime, Space.World);
        		
        		
        		
        
        if (onGround){
        
        	if (Input.GetButtonDown("Jump"))
        		{
        		//Jump is the spacebar
        		//Add force to a Rigidbody along the direction of the force vector
        		rb.AddForce(Vector3.up * jumpSpeed); //This force is added to each frame
        		
        		//After jumping, make onGround false so that hitting space for the second time will not chnage anything
        		onGround = false;
        	}
        }
        		
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);//Changing position of the object based on the movement direction. The transalte method helps move it in the direction that we want
        //Time.deltaTime is the amount of seconds that have passed since the last frame
        
        if (movementDirection != Vector3.zero)
        {
               //Quaternion is a representation of a vector and a rotation around that vector
               
        	Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);//Creates a rotation looking in the desired direction. (x, y)
        	
        	transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);//Rotate the character to the quaternion variable
        	
        	
        }
    }
    
    private void OnCollisionEnter(Collision collision){
     	//Notice you have to create a tag for this method. You can name it the way you want.
     	if (collision.gameObject.CompareTag("Ground")){
         	onGround = true;
     	}
    }	
}
