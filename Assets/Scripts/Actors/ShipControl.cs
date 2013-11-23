using UnityEngine;
using System.Collections;
//TODO:	Add max rotation value
//		Lose the magic number 80. Try to think of a more intuitive rotation computation
// 		Create control key file.
//		Remove all bools and create array instead.
//		Change velocity instead of translating when moving forward
public class ShipControl : MonoBehaviour {
	
    // speeds
    public float speed;
    public float currentSpeed;
    float incrSpeed;

    float rollSpeed;
    float mouseFollowSpeed;
    
    // variables to keep track of the
    // pressed keys
	bool forward;
	public bool rollLeft;
	public bool rollRight;

	// Radius from circel around center point in which the 
	// ship will not change position
	float minRadius;
    public float maxRadius;

	void Start () 
    {

        // Init speeds
        speed = 100;
        currentSpeed = 0;
        incrSpeed = 0.005f;
        rollSpeed = 50;
        mouseFollowSpeed = 0.5f;

        // Init key press variables
		forward = false;
		rollLeft = false;
		rollRight = false;

        // Init radii
		// TODO: Make radius a ratio of screensize?
		minRadius = 10;
        maxRadius = 400;
	}

	void Update () 
    {

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

        float[] p = CalculateMousePosition();
        
        float rotationx = p[0] * mouseFollowSpeed * Time.deltaTime;
        float rotationy = p[1] * mouseFollowSpeed * Time.deltaTime;

        transform.Rotate(0, rotationx, 0, Space.Self);
        transform.Rotate (-rotationy, 0, 0, Space.Self);
	}

    // Returns the position of the mouse, bounded my minRadius and maxRadius
    public float[] CalculateMousePosition()
    {
        float mousex = Input.mousePosition.x;
		float mousey = Input.mousePosition.y;
        
		// Calculate the Euclidian distance between mid of screen to mouse position
		float dx = mousex - (Screen.width/2);
		float dy = mousey - (Screen.height/2);
		float length = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));

        float[] p = new float[2];
        
        // In case the mouse position falls into min and max rad,
        // keep the position with respects to screen center
		if(length > minRadius && length < maxRadius)
		{
            p[0] = dx;
            p[1] = dy;
		}
        
        // In case the mouse position falls beyond
        // max radius, find the closes point on the circle with
        // maxradius
        if (length > maxRadius)
        {
           p[0] = maxRadius * (dx / length);
           p[1] = maxRadius * (dy / length);
        }

        return p;

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
        if (forward)
        {
            if (currentSpeed < speed)
            {
                currentSpeed += (speed * incrSpeed);
            }

            //this.gameObject.rigidbody.velocity = this.gameObject.transform.forward * currentSpeed;
            // this.gameObject.rigidbody.AddForce(this.gameObject.transform.forward * speed);
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
        else
        {
            if (currentSpeed > 0)
            {

                currentSpeed -= (speed * incrSpeed);
                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            }
            else
            {
                // Make sure it is not lower than 0
                currentSpeed = 0;
                transform.Translate(Vector3.forward * 0 * Time.deltaTime);
            }

            //this.gameObject.rigidbody.velocity = this.gameObject.transform.forward * currentSpeed;

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
