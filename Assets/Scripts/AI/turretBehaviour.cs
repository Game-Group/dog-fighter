using UnityEngine;
using System.Collections;

// TODO: make number of guns adaptive?
// TODO: set collision radius here?

public class TurretBehaviour : MonoBehaviour {
    public Transform top;
    public Transform muzzleLeft;
    public Transform muzzleRight;
    public Transform gunRight;
    public Transform gunLeft;
    // The radius
    public float shootRadius;
    public float followRadius;

    Shooter shootL;
    Shooter shootR;
    ShipControl shipData;

	void Start () {
        // If shootradius is bigger than collision radius
        // set it to the same size
        SphereCollider collider = this.gameObject.GetComponent<SphereCollider>();
        collider.radius = followRadius;

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

        // Checking for the player tag is only for no crashing
        if (Object.tag == "Player" && Object.gameObject.layer != this.gameObject.layer)
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

            // TODO: Only get random target sometimes
            targetPosM = GetRandomTarget(targetPosM, 1);
            //targetPosL = GetRandomTarget(targetPosL, 1);
            //targetPosR = GetRandomTarget(targetPosR, 1);
           
            Vector3 lookPos = targetPosM - top.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            //Quaternion b = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
            Vector3 eul = rotation.eulerAngles;
            eul.y -= this.transform.eulerAngles.y;
            Quaternion rotation2 = Quaternion.Euler(eul.x, eul.y, eul.z);
            
            top.localRotation = Quaternion.Slerp(top.transform.rotation, rotation2, Time.deltaTime * 100);

            Vector3 objPos = Object.transform.position;

            float angle = getXrotation(muzzleLeft.position, targetPosL);

            muzzleLeft.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);

            angle = getXrotation(muzzleRight.position, targetPosR);
  
            muzzleRight.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);
            
            float diff = (Object.transform.position - top.position).magnitude;
            // Only shoot in case of in shoot radius
            if(diff < shootRadius)
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
    // TODO: make this own class? add to RandomMomvementInSphere
    public Vector3 GetRandomTarget(Vector3 targetPos, float radius)
    {

        // Pick a random direction from the center of the sphere.
        Vector3 directionFromCenter = Random.insideUnitSphere;

        // Pick a random distance from the center of the sphere.
        float randomRadius = Random.Range(0, radius);

        // Create a new target.
        return targetPos + randomRadius * directionFromCenter.normalized;

    }
    
}
