using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    CameraControl camera;

    Vector3 movementDirection;
    Vector3 movementDirectionH;
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        GameObject camObject = GameObject.Find("Main Camera");
        camera = camObject.GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");//-1 or 1 value is inputted to the horizontal axis. By default left and right arrow keys.
        float verticalInput = Input.GetAxis("Vertical"); //Mapped to the up and down arrow key
        var space = Space.Self;
        if (camera.fpCam)
        {
            space = Space.Self;
            // First person player movement
            movementDirection = Vector3.forward * verticalInput;
            movementDirectionH = Vector3.right * horizontalInput;
        }
        else
        {
            space = Space.World;
            // Third person player movement
            movementDirection = new Vector3(horizontalInput, 0, verticalInput);//Three dimensional vector required for the object to move
            movementDirection.Normalize();//Normalizes the vector and sets magnitude to one
            movementDirectionH = new Vector3(0, 0, 0);

            if (movementDirection != Vector3.zero)
            {
                //Quaternion is a representation of a vector and a rotation around that vector

                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);//Creates a rotation looking in the desired direction. (x, y)

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);//Rotate the character to the quaternion variable
            }
        }
        transform.Translate(movementDirection * speed * Time.deltaTime, space); // +1 forward -1 backward
        transform.Translate(movementDirectionH * (speed - 1) * Time.deltaTime); // +1 right -1 left
    }
}
