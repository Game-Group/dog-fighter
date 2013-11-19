using UnityEngine;
using System.Collections;

public class turretBehaviour : MonoBehaviour {


    Vector3 turretPos;
    public Transform top;
    public Transform gunLeft;
    public Transform gunRight;
	// Use this for initialization
    float angle = 0f;
	void Start () {
        turretPos = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider Object)
    {

        
        // TODO check if object is spaceship
        Vector3 objPos = Object.transform.position;
        Vector3 diff = turretPos - objPos;

        diff.Normalize();

        Vector2 u;
        u.x = diff.x;
        u.y = diff.z;
        Debug.Log(u);
        /*
        u.Normalize();


        Vector2 v;
        v.x = Mathf.Cos(angle);
        v.y = Mathf.Sin(angle);

        v.Normalize();

        angle = Mathf.Acos(Vector2.Dot(v, u));

        Debug.Log("Angle in degrees: ");
        top.eulerAngles = new Vector3(0, Mathf.Rad2Deg * angle, 0); 
          */
        Vector3 lookPos = Object.transform.position - top.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        top.rotation = Quaternion.Slerp(top.transform.rotation, rotation, Time.deltaTime * 5) ;
    }
}
