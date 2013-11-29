using UnityEngine;
using System.Collections;

public class PlayerOnCollideHandling : MonoBehaviour 
{
	public HealthControl HealthControl;

	void OnCollisionEnter(Collision info)
	{
		switch(info.collider.gameObject.tag)
		{
			case "Projectile":
				HealthControl.TakeDamage(info.gameObject.GetComponent<ProjectileController>().Damage);
				Destroy(info.collider);
				break;

			case "Mothership":
				HealthControl.TakeDamage(HealthControl.MaxHealth, HealthControl.MaxShields);
				break;
		}

		Debug.Log("layer is " + LayerMask.LayerToName(info.collider.gameObject.layer));
		switch(LayerMask.LayerToName(info.collider.gameObject.layer))
		{
			case "Obstacles": 
				HealthControl.TakeDamage(HealthControl.MaxHealth, HealthControl.MaxShields);
				break;
		}
	}
}
