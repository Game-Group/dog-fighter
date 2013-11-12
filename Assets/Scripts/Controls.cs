using UnityEngine;
using System.Collections;

//TODO:	Add max rotation value
//		Lose the magic number 80. Try to think of a more intuitive rotation computation
//		Use roll rotation
// 		Create control key file.
//		Remove all bools and create array instead.
//		Change velocity instead of translating when moving forward
public class Controls : MonoBehaviour {
	
	bool forward;
	bool roll_left;
	bool roll_right;
	
	void Start () {
	
		forward = false;
		roll_left = false;
		roll_right = false;
	}
	

	void Update () {
		rotate();
		if(Input.GetKeyDown(KeyCode.W))
		{
			forward = true;
		}
		if(Input.GetKeyUp (KeyCode.S))
		{
			forward = false;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			roll_left = true;
		}
		if(Input.GetKeyUp (KeyCode.A))
		{
			roll_left = false;
		}
				if(Input.GetKeyDown(KeyCode.D))
		{
			roll_right = true;
		}
		if(Input.GetKeyUp (KeyCode.D))
		{
			roll_right = false;
		}
		handle_input();
	}
	
	// Rotates the spacecraft depending on the position of the mouse
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
		if(roll_left)
		{
			transform.Rotate (0,0, 1, Space.Self);
		}
		if(roll_right)
		{
			transform.Rotate (0,0, -1, Space.Self);
		}
	
		
	}
}
