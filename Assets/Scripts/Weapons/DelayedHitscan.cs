using UnityEngine;
using System.Collections;

public class DelayedHitscan : MonoBehaviour 
{
	public ProjectileController ProjectileController;
	public float MaxTimeToLive;

	private float timeToLive;
	private GameObject raycastHit;

	void Start () 
	{
		RaycastHit hit;
		Physics.Raycast(transform.position, ProjectileController.FlyControl.VelocityDirection, out hit);
		if (hit.collider != null)
		{
			timeToLive = hit.distance / ProjectileController.FlyControl.DesiredSpeed;
			raycastHit = hit.collider.gameObject;
		}
		else
			timeToLive = MaxTimeToLive;
		}

	void Update () 
	{
		timeToLive -= Time.deltaTime;

		if (timeToLive <= 0)
		{
			if (raycastHit != null)
				if (!TeamHelper.IsSameTeam(gameObject.layer, raycastHit.layer))
					switch (raycastHit.tag)
					{
						case "Player":
							raycastHit.GetComponent<HealthControl>().TakeDamage(ProjectileController.Damage);
							break;
						case "Npc":
							// TODO
							break;
						case "Mothership":
							// TODO
							break;
					}

			Destroy (gameObject);
		}
	}
}
