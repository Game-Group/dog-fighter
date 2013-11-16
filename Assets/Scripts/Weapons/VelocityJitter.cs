using UnityEngine;
using System.Collections;
using System;
using Random = System.Random;

public class VelocityJitter : MonoBehaviour 
{
	public Rigidbody JitteringObject;
	public float Magnitude;

	private Random r;

	void Start () 
	{
		r = new Random ();
	}
	
	void Update () 
	{
		int mag = (int)(Magnitude * Time.deltaTime);
		float angle1 =  r.Next (mag) - r.Next (mag);
		float angle2 = r.Next (mag) - r.Next (mag);
		float angle3 = r.Next (mag) - r.Next (mag);

		JitteringObject.velocity = Quaternion.Inverse(JitteringObject.transform.rotation) * JitteringObject.velocity;

		JitteringObject.transform.Rotate (angle1, angle2, angle3, Space.Self);
		JitteringObject.velocity = JitteringObject.transform.rotation * JitteringObject.velocity;
	}
}
