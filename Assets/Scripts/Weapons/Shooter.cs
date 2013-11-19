using UnityEngine;
using System.Collections;

/// <summary>
/// Shoots a certain prefab RigidBody from one or more positions with a certain velocity.
/// </summary>
public class Shooter : MonoBehaviour 
{
	public Rigidbody Projectile;
	public Transform[] ShotPositions;
	public Collider[] IgnoredCollisions;
	public float ReloadDelay;

	private float reloadTimer;

	void Start()
	{
		reloadTimer = 0;
	}

	void Update () 
	{		
		if (reloadTimer > 0)
			reloadTimer -= Time.deltaTime;

		// Fire projectiles
		if (reloadTimer <= 0)
		{
			if (Input.GetButton("Fire1"))
			{
				// Spawn a new projectile at each position.
				foreach (Transform t in ShotPositions)
				{
					SpawnProjectile(t);
				}

				reloadTimer = ReloadDelay;
			}
		}
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

		// Ignore collisions with given objects (usually the player and his guns, if they have collision models)
		for (int i = 0; i < IgnoredCollisions.Length; i++)
			Physics.IgnoreCollision(IgnoredCollisions[i], shot.collider);
	}
}
