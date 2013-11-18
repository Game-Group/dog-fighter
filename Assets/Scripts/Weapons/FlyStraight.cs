using UnityEngine;
using System.Collections;

public class FlyStraight : MonoBehaviour 
{
	public Transform Transform;
	public Vector3 VelocityDirection;
	public float TimeBeforeFullSpeed = 0;
	public float DesiredSpeed;
	public float InitialSpeed = 0;

	private float currentSpeed;
	private float speedIncrementPerSecond;

	void Start()
	{
		currentSpeed = InitialSpeed;

		if (TimeBeforeFullSpeed <= 0)
			speedIncrementPerSecond = -1f;
		else
			speedIncrementPerSecond = 1f / TimeBeforeFullSpeed;

		VelocityDirection.Normalize();
	}

	void LateUpdate () 
	{
		if (currentSpeed != DesiredSpeed)
		{
			if (speedIncrementPerSecond == -1f)
				currentSpeed = DesiredSpeed;
			else
			{
				currentSpeed += speedIncrementPerSecond * (DesiredSpeed - InitialSpeed) * Time.deltaTime;
				currentSpeed = Mathf.Min(currentSpeed, DesiredSpeed);
			}
		}

		Transform.Translate(VelocityDirection * currentSpeed * Time.deltaTime, Space.World);
	}
}
