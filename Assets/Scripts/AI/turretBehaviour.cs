using UnityEngine;
using System.Collections;

public class turretBehaviour : MonoBehaviour {


    Vector3 turretPos;
    public Transform top;
    public Transform gunLeft;
    public Transform gunRight;
	// Use this for initialization

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

        Vector3 lookPos = Object.transform.position - top.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        top.localRotation = Quaternion.Slerp(top.transform.rotation, rotation, Time.deltaTime * 5) ;



    }
}
