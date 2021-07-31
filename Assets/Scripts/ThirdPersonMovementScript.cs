using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovementScript : MonoBehaviour
{
    //*****variables*******

    public CharacterController controller;
    public Transform cam;

    [SerializeField] private float speed = 6f;
    // Jumping
    private bool onGround;
    [SerializeField] private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    // Sprinting
    [SerializeField] private float sprintTime;
    private float sprintingTimer;
    int acceleration; // speed * acceleration while sprinting
    public Text playerStat;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity; 
    //**********************
    
    void Awake()
    {
        onGround = true;
        // Set the sprint cool down
        sprintingTimer = sprintTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Jump https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        onGround = controller.isGrounded;
        if (onGround && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && onGround)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        //Code for character movement, rotation and camera-direction-oriented movement

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintingTimer -= (sprintingTimer <= 0) ? 0 : Time.deltaTime;
            if (sprintingTimer > 0)
            {
                acceleration = 2;
            }
            else
            {
                acceleration = 1;
            }
        }
        else
        {
            acceleration = 1;
            sprintingTimer += (sprintingTimer >= sprintTime) ? 0 : (2 * Time.deltaTime);
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if(direction.magnitude >= 0.1f)
        {
        	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        	transform.rotation = Quaternion.Euler(0f, angle, 0f); 
        	
        	Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        	controller.Move(moveDirection.normalized * acceleration * speed * Time.deltaTime);
        
        
        //End of code
        }
        playerStat.text = sprintingTimer.ToString();
    }
}
