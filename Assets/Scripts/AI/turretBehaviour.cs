using UnityEngine;
using System.Collections;

public class turretBehaviour : MonoBehaviour {


    public Transform top;
    public Transform gunLeft;
    public Transform gunRight;
    public Transform gunRight1;
    public Transform gunLeft1;
    Shooter shootL;
    Shooter shootR;
    ShipControl shipData;
	// Use this for initialization


	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        foreach (Transform child in gunLeft1)
        {
            shootL = child.GetComponent<Shooter>();
        }

        foreach (Transform child in gunRight1)
        {
            shootR = child.GetComponent<Shooter>();
        }
        
       shootL.Shoot();
       shootR.Shoot();
        

        //gunRight1.GetComponent<Shooter>().Shoot();
	}

    void OnTriggerEnter(Collider Object)
    {

        // Get ShipControl script
       
    }

    void OnTriggerStay(Collider Object)
    {

        shipData = Object.GetComponent<ShipControl>();

        // TODO: only target current team
        if (Object.tag == "Player")
        {

            // Check the predicted position given the current flying velocity 
            /*
            Vector3 targetPosM = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Vector3.forward * Time.deltaTime,
                                    top.position, shootL.ProjectileSpeed);

            Debug.Log(shipData.currentSpeed * Vector3.forward * Time.deltaTime);
            */
            Vector3 targetPosM = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Object.transform.forward * Time.deltaTime,
                                    top.position,
                                    shootL.ProjectileSpeed);

            Vector3 targetPosL = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Object.transform.forward * Time.deltaTime,
                                    gunLeft.position,
                                    shootL.ProjectileSpeed);
            
            Vector3 targetPosR = PredictPosition.Predict(Object.transform.position,
                                    shipData.currentSpeed * Object.transform.forward * Time.deltaTime,
                                    gunRight.position,
                                    shootR.ProjectileSpeed);
            
           
            Vector3 lookPos = targetPosM - top.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            top.localRotation = Quaternion.Slerp(top.transform.rotation, rotation, Time.deltaTime * 100);

            Vector3 objPos = Object.transform.position;

            float angle = getXrotation(gunLeft.position, targetPosL);

            gunLeft.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);

            angle = getXrotation(gunRight.position, targetPosR);
  
            gunRight.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);

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
