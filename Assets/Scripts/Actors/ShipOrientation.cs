using UnityEngine;
using System.Collections;

public class ShipOrientation : MonoBehaviour {
	
	private Quaternion initialRotation;

    float mouseFollowSpeed;
    float maxRadius = 300;
    float rollSpeed = 200;
    ShipControl script;

	void Start () 
    {
        mouseFollowSpeed = 0.1f;
        script = transform.parent.GetComponent<ShipControl>();
		this.initialRotation = this.transform.localRotation;
	}
	
	void Update () 

	{        
		if (!this.transform.parent.GetComponent<NetworkView>().isMine
		    && Network.peerType != NetworkPeerType.Disconnected)
		{
			this.transform.localRotation = this.initialRotation;
		}
		else
		{
            // Call ShipControls mouse position calculator
            float[] p = script.CalculateMousePosition();
            
            // Rotate the rotation
            float rotationx = p[0] * mouseFollowSpeed;
            float rotationy = p[1] * mouseFollowSpeed;
            transform.localRotation =
                Quaternion.AngleAxis(rotationx, Vector3.up) * Quaternion.AngleAxis(rotationy, Vector3.left);

            bool rollLeft = transform.parent.GetComponent<ShipControl>().rollLeft;
            bool rollRight = transform.parent.GetComponent<ShipControl>().rollLeft;

            if (rollLeft)
            {

            }
            
		}
	}
}
