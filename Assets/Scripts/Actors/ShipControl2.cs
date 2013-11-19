using UnityEngine;
using System.Collections;

public class ShipControl2 : MonoBehaviour {


	bool forward;
	bool roll_left;
	bool roll_right;

	// Radius from circel around center point in which the 
	// ship will not change position
	float radius;

    GameObject ship;

	void Start () {
	
		forward = false;
		roll_left = false;
		roll_right = false;
        ship = GameObject.Find("PlayerSpaceShip");
		// TODO: Make radius a ratio of screensize?
		radius = 10;
	}
	

	void Update () {
		
		if (Network.isClient)
			return;
		
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
		
		Debug.Log(rotationy);

		// Only change the position 
		if(length > radius)
		{
            transform.RotateAround(ship.transform.position, ship.transform.up, rotationx);
            transform.RotateAround(ship.transform.position, ship.transform.right, -rotationy);
            
			//transform.RotateAround(0, rotationx, 0, Space.Self);
			//transform.RotateAround (-rotationy, 0, 0, Space.Self);
		}

	}
	void handle_input()
	{
		if(forward) 
		{
            Debug.Log(Vector3.forward);
			//transform.Translate(Vector3.forward);
            float x = ship.transform.position.x;
            float y = ship.transform.position.y;
            float z = ship.transform.position.z;
            z -= 2;

            Vector3 v;
            v.x = x;
            v.y = y;
            v.z = z;

            transform.position = v;
            
        }
		if(roll_left)
		{
			transform.RotateAround (ship.transform.position, ship.transform.forward, 1);
		}
		if(roll_right)
		{
			transform.RotateAround(ship.transform.position, ship.transform.forward, -1);
		}
	
		
	}
}
