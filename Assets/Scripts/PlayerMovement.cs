using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    CameraControl camera;
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

        if (camera.fpCam)
        {
            // First person player movement
            transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime); // +1 forward -1 backward
            transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime); // +1 right -1 left
        }
        else
        {
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);//Three dimensional vector required for the object to move
            movementDirection.Normalize();//Normalizes the vector and sets magnitude to one

            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);//Changing position of the object based on the movement direction. The transalte method helps move it in the direction that we want

            if (movementDirection != Vector3.zero)
            {
                //Quaternion is a representation of a vector and a rotation around that vector

                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);//Creates a rotation looking in the desired direction. (x, y)

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);//Rotate the character to the quaternion variable
            }
        }
    }
}
