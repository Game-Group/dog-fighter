using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	
	bool forward;
	// Use this for initialization
	void Start () {
	
		forward = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		rotate();
		if(Input.GetKeyDown(KeyCode.W))
		{
			forward = true;
		}
		handle_input();
	}
	void rotate()
	{
		float rotationx = (Input.mousePosition.x - Screen.width/2)/80;
		float rotationy = (Input.mousePosition.y - Screen.height/2)/80;
		transform.Rotate(0, rotationx, 0, Space.Self);
		transform.Rotate (-rotationy, 0, 0, Space.Self);
	}
	void handle_input()
	{
		if(forward)
		{
			transform.Translate(Vector3.forward);
		}
		
	}
}
