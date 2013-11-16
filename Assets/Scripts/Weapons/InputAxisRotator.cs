using UnityEngine;
using System.Collections;

public class InputAxisRotator : MonoBehaviour 
{
	public Transform RotatingObject;
	public Vector3 Axis;
	public string AxisInput;
	
	public float RotationSpeed;
	
	void Update () 
	{
		float angle = Input.GetAxis(AxisInput) * RotationSpeed * Time.deltaTime;
		
		RotatingObject.Rotate(Axis, angle, Space.Self);
	}
}
