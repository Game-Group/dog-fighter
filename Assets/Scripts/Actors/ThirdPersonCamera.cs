using UnityEngine;
using System.Collections;


public class ThirdPersonCamera : MonoBehaviour
{
	/*
	This camera smoothes out rotation around the y-axis and height.
	Horizontal Distance to the target is always fixed.
	 
	There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.
	 
	For every of those smoothed values we calculate the wanted value and the current value.
	Then we smooth it using the Lerp function.
	Then we apply the smoothed values to the transform's position.
	*/
	 
	// The target we are following
	public GameObject target;
	// The distance in the x-z plane to the target
	float distance = 50.0f;
	// the height we want the camera to be above the target
	float height = 25.0f;
	// How much we 
	float heightDamping = 2.0f;
	float rotationDamping = 2.0f;
    float rotationz = 0;

    ShipControl s; 
	 
    void Start()
    {
    }
	// Place the script in the Camera-Control group in the component menu
	//@script AddComponentMenu("Camera-Control/Smooth Follow")
	 
	 
	void LateUpdate () 
	{
		
	    // Early out if we don't have a target
	    if (!target)
	        return;

	    // Calculate the current rotation angles
	    float wantedRotationAngle = target.transform.eulerAngles.x;
	    float wantedRotationAngley = target.transform.eulerAngles.y;
	    float wantedHeight = target.transform.position.y + height;
	 
	    float currentRotationAngle = transform.eulerAngles.x;
	    float currentRotationAngley = transform.eulerAngles.y;
	    float currentHeight = transform.position.y;
	 
	    // Damp the rotation around the y-axis
	    currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
	    currentRotationAngley = Mathf.LerpAngle (currentRotationAngley, wantedRotationAngley, rotationDamping * Time.deltaTime);
	    // Damp the height
	    currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        s = target.GetComponent<ShipControl>();

        Quaternion currentRotation;

        currentRotation = Quaternion.Euler(currentRotationAngle, currentRotationAngley, 0);
    
	    // Set the position of the camera on the x-z plane to:
	    // distance meters behind the target
	    transform.position = target.transform.position;
	    transform.position -= currentRotation * Vector3.forward * distance;
	    // Set the height of the camera
	    transform.position.Set(transform.position.x, currentHeight, transform.position.z);
	 
	    // Always look at the target
	    transform.LookAt(target.transform);
        try
            {
                rotationz += s.currentRotation.z;
                this.transform.Rotate(0, 0, rotationz * Time.deltaTime, Space.Self);
                // Convert the angle into a rotation
            }
            catch(System.Exception e)
            {
            }
	}
}