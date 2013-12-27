using UnityEngine;
using System.Collections;

public class AimAtTarget : MonoBehaviour 
{
    [HideInInspector]
    public ThirdPersonCrosshair Crosshair;

    public float MaxTurnAngle;

    private Quaternion initialRotation;

	void Start () 
    {
        initialRotation = transform.localRotation;
	}
	
	void Update () 
    {
        if (Crosshair == null)
            return;

        transform.LookAt(Crosshair.ThreeDimensionalCrosshair);

        float angle = Quaternion.Angle(initialRotation, transform.localRotation);

        if (angle > MaxTurnAngle)
        {
            transform.localRotation = Quaternion.Slerp(initialRotation, transform.localRotation, MaxTurnAngle / angle);
        }
	}
}
