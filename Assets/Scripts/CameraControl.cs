using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public enum RotationAxis
    {
        MouseX = 1,
        MouseY = 2,

    }

    public RotationAxis axes = RotationAxis.MouseX;

    public float senseHorizontal = 10.0f;
    public float senseVertical = 10.0f;

    public float _rotationX = 0;
	
	// Update is called once per frame
	void Update ()
    {
        if (axes == RotationAxis.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * senseHorizontal, 0);
        }
        else if (axes == RotationAxis.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * senseVertical;
            _rotationX = Mathf.Clamp(_rotationX, -90, 90);

            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
        
	}
}
