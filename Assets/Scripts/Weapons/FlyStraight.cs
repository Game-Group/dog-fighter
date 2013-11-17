using UnityEngine;
using System.Collections;

public class FlyStraight : MonoBehaviour 
{
	public Transform Transform;
	public Vector3 Velocity;
	
	void LateUpdate () 
	{
		Transform.Translate(Velocity * Time.deltaTime, Space.World);
	}
}
