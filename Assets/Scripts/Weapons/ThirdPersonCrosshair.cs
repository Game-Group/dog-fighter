using UnityEngine;
using System.Collections;

/// <summary>
/// Draws a crosshair on a camera, given a transform to fire a collision detection ray from.
/// </summary>
public class ThirdPersonCrosshair : MonoBehaviour 
{
	public Transform RayTransform;
	public float MaxDistance; 
	
	public Camera Camera;
	public GUITexture Crosshair;
	
	private int layerMask = ~(1 << 11);
	
	void Update () 
	{
		RaycastHit hitInfo;
		Vector3 newCrosshairPosition;

		// Fire a ray, and check for collisions.
		// Use the collision point if it exists, and the max distance if it doesnt.
		if (Physics.Raycast(RayTransform.position, RayTransform.forward, out hitInfo, MaxDistance, layerMask))
			newCrosshairPosition = hitInfo.point;
		else newCrosshairPosition = RayTransform.position + RayTransform.forward.normalized * MaxDistance;

		// Obtain the position of the 3D crosshair in 2D.
		Vector3 screenPosition = Camera.WorldToScreenPoint(newCrosshairPosition);

		// Clamp the position so the crosshair is always on-screen.
		screenPosition.x = Mathf.Clamp(screenPosition.x / Screen.width, 0, 1);
		screenPosition.y = Mathf.Clamp(screenPosition.y / Screen.height, 0, 1);
		screenPosition.z = 0;		

		// Move the 2D texture.
		Crosshair.transform.position = screenPosition;
	}
}
