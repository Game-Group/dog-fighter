using UnityEngine;
using System.Collections;
//TODO:	Add max rotation value
//		Lose the magic number 80. Try to think of a more intuitive rotation computation
// 		Create control key file.
//		Remove all bools and create array instead.
//		Change velocity instead of translating when moving forward
public class ShipControl : MonoBehaviour {
	
    // speeds
    public float maxSpeed;
    public float[] speedStages;
    int currentSpeedStage;
    [HideInInspector]
    public float currentSpeed;
    float incrSpeed;

    // Rotation of space ship
    [HideInInspector]
    public float mousex;
    [HideInInspector]
    public float mousey;
    
	public Vector2 MouseRotation { get; private set; }

	private ObjectTransformer objectTransform;

    float rollSpeed;
    float mouseFollowSpeed;
    
    // variables to keep track of the
    // pressed keys
	bool forward;
	bool backward;
	public bool rollLeft;
	public bool rollRight;

	// Radius from circel around center point in which the 
	// ship will not change position
	float minRadius;
    public float maxRadius;

	void Start () 
    {
		this.objectTransform = this.GetComponent<ObjectTransformer>();
        // Init speeds in case no manual initailisation
        if (maxSpeed == 0)
        {
            maxSpeed = 100;
        }
        // just always override currentspeed to be 0
        currentSpeed = 0;
        
        // initialise speed staged and fill with speeds
        speedStages = new float[6];
        for (int i = 0; i < speedStages.Length; i++)
        {
            speedStages[i] = (maxSpeed / 5) * i;
        }

        maxSpeed = 0;
        // initialisation of private variables
        incrSpeed = 0.005f;
        rollSpeed = 50;
        mouseFollowSpeed = 0.5f;

        // Init key press variables
		forward = false;
		backward = false;
		rollLeft = false;
		rollRight = false;

        // Init radii
		// TODO: Make radius a ratio of screensize?
		minRadius = 10;
        maxRadius = 400;


        mousex = 0;
        mousey = 0;
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

        mousex = p[0];
        mousey = p[1];
        float rotationx = mousex * mouseFollowSpeed * Time.deltaTime;
        float rotationy = mousey * mouseFollowSpeed * Time.deltaTime;

		this.MouseRotation = new Vector2(rotationx, rotationy);

        //transform.Rotate(0, rotationx, 0, Space.Self);
        //transform.Rotate (-rotationy, 0, 0, Space.Self);

		Vector3 currentRotation = this.objectTransform.Rotation;
        
		this.objectTransform.Rotation = new Vector3(rotationx, -rotationy, currentRotation.z);
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
		if(Input.GetKeyDown(KeyCode.S))
		{
			backward = true;
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

        
        if (currentSpeed < maxSpeed)
        {
            // Acceleration depends on the current speed of the ship
            currentSpeed += ((maxSpeed - currentSpeed)* incrSpeed);
        }
        if (currentSpeed > maxSpeed)
        {
            currentSpeed -= ((currentSpeed - maxSpeed) * incrSpeed);
            // Check if current speed does not get negative
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }
        }

		this.objectTransform.TranslationDirection = Vector3.forward;
		this.objectTransform.TranslationSpeed = this.currentSpeed;
        //transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);


        // Make the spaceship move forward
        if (forward)
        {
            if (currentSpeedStage < speedStages.Length - 1)
            {
                currentSpeedStage++;
                maxSpeed = speedStages[currentSpeedStage];
            }
            forward = false;
        }
        if(backward)
        {

            if (currentSpeedStage > 0)
            {
                currentSpeedStage--;
                maxSpeed = speedStages[currentSpeedStage];
            }
            backward = false;
        }

		Vector3 currentRotation = this.objectTransform.Rotation;
		

		if(rollLeft)
		{
			//transform.Rotate (0,0, rollSpeed * Time.deltaTime, Space.Self);
			currentRotation.z = rollSpeed * Time.deltaTime;
		}
		else if(rollRight)
		{
			//transform.Rotate (0,0, -rollSpeed * Time.deltaTime, Space.Self);
			currentRotation.z = -rollSpeed * Time.deltaTime;			
		}
		else
			currentRotation.z = 0;

		this.objectTransform.Rotation = currentRotation;
		
	
	}
}
