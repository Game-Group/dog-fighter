using UnityEngine;
using System.Collections;

//TODO:	Add max rotation value
//		Lose the magic number 80. Try to think of a more intuitive rotation computation
//		Use roll rotation
// 		Create control key file.
//		Remove all bools and create array instead.
//		Change velocity instead of translating when moving forward
public class ShipControl : MonoBehaviour {
	
    // Flying speed
    float speed;

    // Speed of roll rotation
    float rollSpeed;
    
    // variables to keep track of the
    // pressed keys
	bool forward;
	bool rollLeft;
	bool rollRight;

	// Radius from circel around center point in which the 
	// ship will not change position
	float minRadius;

	void Start () {

        // Init speeds
        speed = 30;
        rollSpeed = 10;

        // Init key press variables
		forward = false;
		rollLeft = false;
		rollRight = false;

        // Init radii
		// TODO: Make radius a ratio of screensize?
		minRadius = 10;
	}

	void Update () {

		//if (!this.networkView.isMine)
		//	return;
		
		GameObject obj = GameObject.Find("Global");

		if (obj != null)
		{
			if (!obj.GetComponent<GlobalSettings>().HasFocus)
				return;		
		}

		HandleMouse();
        HandleKeypress();
		HandleMotion();
	}
	
	// Rotates the spacecraft depending on the position of the mouse.
	void HandleMouse()
	{
		float mousex = Input.mousePosition.x;
		float mousey = Input.mousePosition.y;
        
        // Determine angle to be rotated byt the plane
		float rotationx = (mousex - Screen.width/2)/80;
		float rotationy = (mousey - Screen.height/2)/80;

		// Calculate the Euclidian distance between mid of screen to mouse position
		float dx = mousex - (Screen.width/2);
		float dy = mousey - (Screen.height/2);
		float length = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
		
		// Only change the position  in case mouse is far away from center
		if(length > minRadius)
		{
			transform.Rotate(0, rotationx, 0, Space.Self);
			transform.Rotate (-rotationy, 0, 0, Space.Self);
		}

	}

    // Checks which keys have been pressed and set
    // the appropriate boolean value
    void HandleKeypress()
    {
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
			rollLeft = true;
		}
		if(Input.GetKeyUp (KeyCode.A))
		{
			rollLeft = false;
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			rollRight = true;
		}
		if(Input.GetKeyUp (KeyCode.D))
		{
			rollRight = false;
		}
    }

    // Handle motions
	void HandleMotion()
	{
        // Make the spaceship move forward
		if(forward)
		{
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
		if(rollLeft)
		{
			transform.Rotate (0,0, rollSpeed * Time.deltaTime, Space.Self);
		}
		if(rollRight)
		{
			transform.Rotate (0,0, -rollSpeed * Time.deltaTime, Space.Self);
		}
	
	}
}
