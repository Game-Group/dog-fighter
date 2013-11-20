using UnityEngine;
using System.Collections;

/// <summary>
/// Shoots a certain prefab RigidBody from one or more positions with a certain velocity.
/// </summary>
public class Shooter : MonoBehaviour 
{
	public Rigidbody Projectile;
	public Transform[] ShotPositions;
	public float ReloadDelay;
	public bool HumanControlled;

	private float reloadTimer;

	void Start()
	{
		reloadTimer = 0;
	}

	public void Shoot()
	{
		// Fire projectiles
		if (reloadTimer <= 0)
		{
			// Spawn a new projectile at each position.
			foreach (Transform t in ShotPositions)
			{
				SpawnProjectile(t);
			}
				
			reloadTimer = ReloadDelay;
		}
	}

	void Update () 
	{		
		if (reloadTimer > 0)
			reloadTimer -= Time.deltaTime;

		if (HumanControlled)
			if (Input.GetButton("Fire1"))
				Shoot();
	}

	private void SpawnProjectile(Transform shotPosition)
	{
		// Create an instance of the prefab projectile.
		Rigidbody shot = Instantiate(Projectile) as Rigidbody;

		// Transform the new projectile to align it properly.
		shot.transform.Rotate(shotPosition.eulerAngles, Space.World);
		shot.transform.position = shotPosition.position;
		shot.gameObject.layer = gameObject.layer;

		// Fire it away by giving it a velocity.
		ProjectileController pController = shot.GetComponent<ProjectileController>();
		pController.SetVelocityDirection(shotPosition.forward);
	}
}
