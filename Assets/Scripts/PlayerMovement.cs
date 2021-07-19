using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //*****variables*******

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    [SerializeField] private float jumpSpeed; //Calculate the jump speed
    private bool onGround;

    CameraControl camera;

    private float coolDown = 6;
    [SerializeField] private float coolDownTimer;
    [SerializeField] private float sprintTime;
    private float sprintingTimer;
    int acceleration; // speed * acceleration while sprinting
    public Text playerStat;
    private float countDown = 5;

    Vector3 movementDirection;
    Vector3 movementDirectionH;

    //**********************


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //get the rigidbody component of the current object the script is attached to
        onGround = true;
        // find the current instance of the camera control script:
        GameObject camObject = GameObject.Find("Main Camera");
        camera = camObject.GetComponent<CameraControl>();
        // Set the sprint cool down
        sprintingTimer = sprintTime;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");//-1 or 1 value is inputted to the horizontal axis. By default left and right arrow keys.
        float verticalInput = Input.GetAxis("Vertical"); //Mapped to the up and down arrow key
        var space = Space.Self;
        //Checking if the object is on the ground to make it jump on the ground and not in the air
        if (onGround)
        {

            if (Input.GetButtonDown("Jump"))
            {
                //Jump is the spacebar
                //Add force to a Rigidbody along the direction of the force vector
                rb.AddForce(Vector3.up * jumpSpeed); //This force is added to each frame

                //After jumping, make onGround false so that hitting space for the second time will not chnage anything
                onGround = false;
            }
        }

        //****First person camera*****
        if (camera.fpCam)
        {
            space = Space.Self;
            // First person player movement
            movementDirection = Vector3.forward * verticalInput;
            movementDirectionH = Vector3.right * horizontalInput;
        }
        //****************************


        else
        {
            space = Space.World;
            // Third person player movement
            movementDirection = new Vector3(verticalInput, 0, -horizontalInput);//Three dimensional vector required for the object to move
            movementDirection.Normalize();//Normalizes the vector and sets magnitude to one
            movementDirectionH = new Vector3(0, 0, 0);

            if (movementDirection != Vector3.zero)
            {
                //Quaternion is a representation of a vector and a rotation around that vector

                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);//Creates a rotation looking in the desired direction. (x, y)

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);//Rotate the character to the quaternion variable
            }
        }
        
        //Simple code to make the character sprint
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
        rb.AddForce(movementDirection * speed);
        transform.Translate(movementDirection * speed * acceleration * Time.deltaTime, space); // +1 forward -1 backward
        transform.Translate(movementDirectionH * (speed - 1) * acceleration * Time.deltaTime); // +1 right -1 left
        playerStat.text = sprintingTimer.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Notice you have to create a tag for this method. You can name it the way you want.
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

}