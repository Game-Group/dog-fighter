using UnityEngine;
using System.Collections;

/// <summary>
/// Draws a crosshair on a camera, given a transform to fire a collision detection ray from.
/// </summary>
public class ThirdPersonCrosshair : MonoBehaviour 
{
    public Camera Camera;

	public Transform RayTransform;
	public float MaxDistance; 

	public Texture2D CrosshairTexture;

    private Vector2 crosshairPosition;
	private int layerMask;

	void Start()
	{       
        int teamNumber = TeamHelper.GetTeamNumber(gameObject.layer);

        int teamXActorMask;
        if (teamNumber == 1)
            teamXActorMask = 1 << 8;
        else teamXActorMask = 1 << 11;

        int projectileMask = (1 << 10) | (1 << 13);

        layerMask = ~(teamXActorMask | projectileMask);

        crosshairPosition = new Vector2(0, 0);
	}
	
	void Update () 
	{
		RaycastHit hitInfo;
		Vector3 newCrosshairPosition;

		// Fire a ray, and check for collisions.
		// Use the collision point if it exists, and the max distance if it doesnt.
        if (Physics.Raycast(RayTransform.position, RayTransform.forward, out hitInfo, MaxDistance, layerMask))
        {
            //Debug.DrawRay(RayTransform.position, RayTransform.forward * MaxDistance, Color.red);
            newCrosshairPosition = hitInfo.point;
        }
        else
        {
            newCrosshairPosition = RayTransform.position + RayTransform.forward.normalized * MaxDistance;
            //Debug.DrawRay(RayTransform.position, RayTransform.forward * MaxDistance, Color.green);
        }

		// Obtain the position of the 3D crosshair in 2D.
        Vector3 screenPosition = Camera.WorldToScreenPoint(newCrosshairPosition);

        crosshairPosition.x = screenPosition.x;
        crosshairPosition.y = Screen.height - screenPosition.y;
	}

    void OnGUI()
    {
        GUI.Label(new Rect(crosshairPosition.x - CrosshairTexture.width / 2f,
                           crosshairPosition.y - CrosshairTexture.height / 2f,
                           CrosshairTexture.width, CrosshairTexture.height), 
                  new GUIContent(CrosshairTexture));
    }
}
