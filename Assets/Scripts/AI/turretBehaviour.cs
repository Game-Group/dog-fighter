using UnityEngine;
using System.Collections;

// TODO: make number of guns adaptive?
// TODO: set collision radius here?

public class turretBehaviour : MonoBehaviour {
    public Transform top;
    public Transform muzzleLeft;
    public Transform muzzleRight;
    public Transform gunRight;
    public Transform gunLeft;
    // The radius
    public float shootRadius;

    Shooter shootL;
    Shooter shootR;
    ShipControl shipData;

	void Start () {
        // If shootradius is bigger than collision radius
        // set it to the same size
        SphereCollider collider = this.gameObject.GetComponent<SphereCollider>();
        Debug.Log(collider.radius);

        if (collider.radius < shootRadius)
        {
            shootRadius = collider.radius;
        }
	}
	
	void Update () {
        // TODO: put this in Start
        foreach (Transform child in gunLeft)
        {
            shootL = child.GetComponent<Shooter>();
        }

        foreach (Transform child in gunRight)
        {
            shootR = child.GetComponent<Shooter>();
        }
        
	}

    void OnTriggerEnter(Collider Object)
    {

       
    }

    void OnTriggerStay(Collider Object)
    {

        shipData = Object.GetComponent<ShipControl>();

        // TODO: only target current team
        if (Object.tag == "Player")
        {

            // Check the predicted position given the current flying velocity 
            Vector3 targetPosM = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Object.transform.forward,
                                    top.position,
                                    shootL.ProjectileSpeed);

            Vector3 targetPosL = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Object.transform.forward,
                                    muzzleLeft.position,
                                    shootL.ProjectileSpeed);
            
            Vector3 targetPosR = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Object.transform.forward,
                                    muzzleRight.position,
                                    shootR.ProjectileSpeed);
           
            Vector3 lookPos = targetPosM - top.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            top.localRotation = Quaternion.Slerp(top.transform.rotation, rotation, Time.deltaTime * 100);

            Vector3 objPos = Object.transform.position;

            float angle = getXrotation(muzzleLeft.position, targetPosL);

            muzzleLeft.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);

            angle = getXrotation(muzzleRight.position, targetPosR);
  
            muzzleRight.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);

            // Only shoot in case of in shoot radius
            if((Object.transform.position - top.position).magnitude < shootRadius)
            {
                
                   shootL.Shoot();
                   shootR.Shoot();

            }

        }
    }
    float getXrotation(Vector3 startPos, Vector3 targetPos)
    {

        Vector3 diff = startPos - targetPos;

        // The angle is arccos(hypothenuse/adjacent) 
        float hypothenuse = diff.magnitude;

        Vector2 diffxy;
        diffxy.x = diff.x;
        diffxy.y = diff.z;

        float adjacent = diffxy.magnitude;

        float angle = Mathf.Acos(adjacent / hypothenuse);

        return angle;

    }

}
