using UnityEngine;
using System.Collections;

public class turretBehaviour : MonoBehaviour {


    Vector3 gunLPos;
    Vector3 gunRPos;
    public Transform top;
    public Transform gunLeft;
    public Transform gunRight;
    public Transform gunRight1;
    public Transform gunLeft1;
    Shooter shootL;
    Shooter shootR;

	// Use this for initialization


	void Start () {
        gunLPos = gunLeft.position;
        gunRPos = gunRight.position;

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

    void OnTriggerStay(Collider Object)
    {

        if (Object.tag == "Player")
        {
            Vector3 lookPos = Object.transform.position - top.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            top.localRotation = Quaternion.Slerp(top.transform.rotation, rotation, Time.deltaTime * 5);

            Vector3 objPos = Object.transform.position;
            Vector3 diff = gunLPos - objPos;

            // The angle is arccos(hypothenuse/adjacent) 
            float hypothenuse = diff.magnitude;

            //Debug.Log("length from gun to ship");
            //Debug.Log(hypothenuse);

            Vector2 diffxy;

            //Debug.Log("difference x and y");
            //Debug.Log(diff.x);
            //Debug.Log(diff.z);
            diffxy.x = diff.x;
            diffxy.y = diff.z;

            float adjacent = diffxy.magnitude;

            //Debug.Log("length from gun xy");
            //Debug.Log(adjacent);

            float angle = Mathf.Acos(adjacent / hypothenuse);
            //Debug.Log(angle*Mathf.Rad2Deg);

            gunLeft.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);
            //gunLeft.Rotate(angle - prevAngle, 0, 0, Space.Self);
            //prevAngle = angle;

            // Now for right gun:
            diff = gunRPos - objPos;

            // The angle is arccos(hypothenuse/adjacent) 
            hypothenuse = diff.magnitude;

            //Debug.Log("length from gun to ship");
            //Debug.Log(hypothenuse);

            //Debug.Log("difference x and y");
            //Debug.Log(diff.x);
            //Debug.Log(diff.z);
            diffxy.x = diff.x;
            diffxy.y = diff.z;

            adjacent = diffxy.magnitude;

            //Debug.Log("length from gun xy");
            //Debug.Log(adjacent);

            angle = Mathf.Acos(adjacent / hypothenuse);
            //Debug.Log(angle*Mathf.Rad2Deg);

            gunRight.localEulerAngles = new Vector3(-angle * Mathf.Rad2Deg, -90, 0);
            //gunLeft.Rotate(angle - prevAngle, 0, 0, Space.Self);
            //prevAngle = angle;
            
            Component shootL;
            Component shootR;

        }



    }
}
