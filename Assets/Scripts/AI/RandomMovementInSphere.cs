using UnityEngine;
using System.Collections;
//using Random = System.Random;

public class RandomMovementInSphere : MonoBehaviour 
{
	public Transform Character;
	public Transform SphereCenter;
	public int SphereRadius;
	public float MovementSpeed;

	private Vector3 CurrentTarget;

	void Start () 
	{
		PickNewTarget();
	}
	
	void Update () 
	{
		Vector3 direction = CurrentTarget - Character.position;

		float speed = MovementSpeed * Time.deltaTime;

		float directionLength = direction.magnitude;
		if (directionLength <= MovementSpeed || directionLength == 0)
		{
			PickNewTarget();
		}

		Character.Translate(direction.normalized * speed);
	}

	private void PickNewTarget()
	{
		Vector3 directionFromCenter = Random.insideUnitSphere;

		float randomRadius = Random.Range(0, SphereRadius);

		CurrentTarget = SphereCenter.position + randomRadius * directionFromCenter.normalized;
	}
}
