using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float gravity = -9.8f;

    private CharacterController _charController;

	// Use this for initialization
	void Start ()
    {
        _charController = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaY = 0;// Camera.main.transform.forward * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, deltaY, deltaZ);

        //movement = Camera.main.transform.forward * deltaZ;

        movement = Vector3.ClampMagnitude(movement, speed);

        if     (Input.GetKey(KeyCode.Space))        movement.y += speed;
        else if(Input.GetKey(KeyCode.RightControl)) movement.y -= speed;

        //movement.y = gravity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);
	}
}
