using UnityEngine;
using System.Collections;

public class Controls2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float rotationx = (Input.mousePosition.x - Screen.width/2)/4;
		float rotationy = (Input.mousePosition.y - Screen.height/2)/4;
		transform.localRotation = Quaternion.AngleAxis (rotationx, Vector3.up) *  Quaternion.AngleAxis (rotationy + 90, Vector3.left);  
	}
}
