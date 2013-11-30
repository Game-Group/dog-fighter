using UnityEngine;
using System.Collections;

public class ShipOrientation : MonoBehaviour {
	
	private Quaternion initialRotation;

    float mouseFollowSpeed;
    ShipControl script;

	void Start () 
    {
        mouseFollowSpeed = 0.1f;
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
            float rotationx = script.mousex * mouseFollowSpeed;
            float rotationy = script.mousey * mouseFollowSpeed;
            
            transform.localRotation =
                Quaternion.AngleAxis(rotationx, Vector3.up) * Quaternion.AngleAxis(rotationy, Vector3.left);

            bool rollLeft = transform.parent.GetComponent<ShipControl>().rollLeft;


            if (rollLeft)
            {

            }
            
		}
	}
}
