using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	public Rigidbody Projectile;
	public Transform[] ShotPositions;
	public float ShotVelocity;
	
	void Update () 
	{		
		// Fire projectiles
		if (Input.GetButtonDown("Fire1"))
		{
			foreach (Transform t in ShotPositions)
			{
				Rigidbody shot = Instantiate(Projectile) as Rigidbody;
				shot.transform.Rotate(t.eulerAngles, Space.World);
				shot.transform.position = t.position;
				shot.velocity = t.forward * ShotVelocity;
			}
		}
	}
}
