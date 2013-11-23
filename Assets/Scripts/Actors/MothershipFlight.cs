using UnityEngine;
using System.Collections;

/*
 * This script is used to make the motherships fly in circles
 */
public class MothershipFlight : MonoBehaviour {
	public float speed = 50;
	public float radius = 2000;
	//angularVelocity is the speed with which the object
	// should change its rotation (around the y axis)
	private float angularVelocity;

	// Use this for initialization
	void Start () {
		float circumference = radius * Mathf.PI * 2;
		angularVelocity = (speed / circumference) * 360;
		Debug.Log("Circumference: " + circumference);
		Debug.Log("Speed: " + speed);
		Debug.Log("Angular velocity: " + angularVelocity);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(/*transform.forward*/Vector3.forward * speed * Time.deltaTime);
		transform.Rotate(new Vector3(0, angularVelocity * Time.deltaTime, 0));
	}
}
//""