using UnityEngine;
using System.Collections;

public class ShipOrientation : MonoBehaviour {
	
	private Quaternion initialRotation;
	
	// Use this for initialization
	void Start () {
		this.initialRotation = this.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!this.transform.parent.GetComponent<NetworkView>().isMine)
			this.transform.localRotation = this.initialRotation;
		else
		{		
			float rotationx = (Input.mousePosition.x - Screen.width/2)/4;
			float rotationy = (Input.mousePosition.y - Screen.height/2)/4;
			transform.localRotation = 
				Quaternion.AngleAxis (rotationx, Vector3.up) *  Quaternion.AngleAxis (rotationy, Vector3.left);  	
		}
	}
}
