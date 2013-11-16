using UnityEngine;
using System.Collections;

public class ShipOrientation : MonoBehaviour {
	
	private Quaternion initialRotation;

    float mouseFollowSpeed;
	
	void Start () {
        mouseFollowSpeed = 0.1f;

		this.initialRotation = this.transform.localRotation;
	}
	
	void Update () 
	{
        
	/*	if (!this.transform.parent.GetComponent<NetworkView>().isMine)
			this.transform.localRotation = this.initialRotation;
		else
		{*/		
            // Quaternion plane rotation
        float rotationx = (Input.mousePosition.x - Screen.width / 2) * mouseFollowSpeed;
        float rotationy = (Input.mousePosition.y - Screen.height / 2) * mouseFollowSpeed;
			transform.localRotation = 
				Quaternion.AngleAxis (rotationx, Vector3.up) *  Quaternion.AngleAxis (rotationy, Vector3.left);  	
		//}
	}
}
