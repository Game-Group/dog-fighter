using UnityEngine;
using System.Collections;

public class ThirdPersonCrosshair : MonoBehaviour 
{
	public Transform RayTransform;
	public float MaxDistance; 
	
	public Camera _camera;
	public GUITexture Crosshair;
	
	private int layerMask = ~(1 << 11);
	
	void Update () 
	{
		RaycastHit hitInfo;
		Vector3 newCrosshairPosition;
		
		if (Physics.Raycast(RayTransform.position, RayTransform.forward, out hitInfo, MaxDistance, layerMask))
			newCrosshairPosition = hitInfo.point;
		else newCrosshairPosition = RayTransform.position + RayTransform.forward.normalized * MaxDistance;
		
		Vector3 screenPosition = _camera.WorldToScreenPoint(newCrosshairPosition);
		
		screenPosition.x = Mathf.Clamp(screenPosition.x / Screen.width, 0, 1);
		screenPosition.y = Mathf.Clamp(screenPosition.y / Screen.height, 0, 1);
		screenPosition.z = 0;		
		
		Crosshair.transform.position = screenPosition;
	}
}
