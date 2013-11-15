using UnityEngine;
using System.Collections;

//TODO:	Add max rotation value
//		Lose the magic number 80. Try to think of a more intuitive rotation computation
//		Use roll rotation
// 		Create control key file.
//		Remove all bools and create array instead.
//		Change velocity instead of translating when moving forward
public class ShipControl : MonoBehaviour {
	
	bool forward;
	bool roll_left;
	bool roll_right;

	// Radius from circel around center point in which the 
	// ship will not change position
	float radius;


	void Start () {
	
		forward = false;
		roll_left = false;
		roll_right = false;

		// TODO: Make radius a ratio of screensize?
		radius = 10;
	}

	void Update () {

		if (!this.networkView.isMine)
			return;
		
		GameObject obj = GameObject.Find("Global");

		if (obj != null)
		{
			if (!obj.GetComponent<GlobalSettings>().HasFocus)
				return;		
		}
		
		rotate();
		if(Input.GetKeyDown(KeyCode.W))
		{
			forward = true;
		}
		if(Input.GetKeyUp (KeyCode.W))
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
		float mousex = Input.mousePosition.x;
		float mousey = Input.mousePosition.y;

		float rotationx = (mousex - Screen.width/2)/80;
		float rotationy = (mousey - Screen.height/2)/80;

		// Calculate the Euclidian distance between mid of screen to mouse position
		float dx = mousex - (Screen.width/2);
		float dy = mousey - (Screen.height/2);
		float length = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
		
//		Debug.Log(rotationy);

		// Only change the position 
		if(length > radius)
		{
			transform.Rotate(0, rotationx, 0, Space.Self);
			transform.Rotate (-rotationy, 0, 0, Space.Self);
		}

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
