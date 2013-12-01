using UnityEngine;
using System.Collections;

public class PlayerOnCollideHandling : MonoBehaviour 
{
	public HealthControl HealthControl;

	void OnCollisionEnter(Collision info)
	{
		switch(info.collider.gameObject.tag)
		{
			case "Team1Projectile":
				HandleProjectileCollision(info);
				break;
			case "Team2Projectile":
				HandleProjectileCollision(info);
				break;


			case "Mothership":
				HealthControl.TakeDamage(HealthControl.MaxHealth, HealthControl.MaxShields);
				break;

			case "Obstacle": 
				HealthControl.TakeDamage(HealthControl.MaxHealth, HealthControl.MaxShields);
				break;
		}
	}

	private void HandleProjectileCollision(Collision info)
	{
		HealthControl.TakeDamage(info.gameObject.GetComponent<ProjectileController>().Damage);

		Destroy(info.gameObject);
	}
}
