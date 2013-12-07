using UnityEngine;
using System.Collections;

public class ShipOrientation : MonoBehaviour {
	
	private Quaternion initialRotation;

    float mouseFollowSpeed;
    ShipControl script;

	void Start () 
    {
        mouseFollowSpeed = 0.3f;
        script = transform.parent.GetComponent<ShipControl>();
		this.initialRotation = this.transform.localRotation;
	}
	
	void Update () 

	{        
		if (Network.peerType != NetworkPeerType.Disconnected)
		{
			this.transform.localRotation = this.initialRotation;
		}
		else
		{
            //Rotate spaceship 
            float rotationx = script.rotationx * mouseFollowSpeed;
            float rotationy = script.rotationy * mouseFollowSpeed;
            //float rotationy = 0;
            
            transform.localRotation =
                Quaternion.AngleAxis(rotationx, Vector3.up) * Quaternion.AngleAxis(rotationy, Vector3.left);

		}
	}
}
