using UnityEngine;
using System.Collections;

public class droneBehaviour : MonoBehaviour {
public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        
        Vector3 direction = recalculatePath();
        // Mooove
        Quaternion rot = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);

        transform.position += transform.forward * 5f * Time.deltaTime;
    }

    // Prevents collision by finding new heading direction
    Vector3 recalculatePath()
    {
         // Direction to target
        Vector3 dir = (target.position - transform.position).normalized;
        
        // Position of drone
        Vector3 origin = transform.position;
        // direction it is flying
        Vector3 direction = transform.forward;

        RaycastHit hitInfo;
        float distance = 10;
      
        GameObject temp = new GameObject();
        Vector3 t = origin;
       
        temp.transform.position = t;
        temp.transform.localEulerAngles = transform.localEulerAngles;


        // Shoot 3 different rays
        // One from the center of the drone to the fron
        // two from the sides
        // In case the ray hits a rigid body, change the course of the drone
        if (Physics.Raycast(origin, direction, out hitInfo, distance))
        {
            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }

        temp.transform.localEulerAngles = new Vector3(0, 40 + transform.localEulerAngles.y, 0);
        if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
        {
            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }

        temp.transform.localEulerAngles = new Vector3(0, -40 + transform.localEulerAngles.y, 0);
        if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
        {

            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }


          temp.transform.localEulerAngles = new Vector3(40 + transform.localEulerAngles.x, 0, 0);
          if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
                {
                    if (hitInfo.transform != transform)
                    {

                        Debug.DrawLine(origin, hitInfo.point, Color.red);
                        Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                        dir += hitInfo.normal * 20;
                    }
                }

          temp.transform.localEulerAngles = new Vector3(-40 + transform.localEulerAngles.x, 0, 0);
            if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
            {

                if (hitInfo.transform != transform)
                {

                    Debug.DrawLine(origin, hitInfo.point, Color.red);
                    Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                    dir += hitInfo.normal * 20;
                }
            }



        return dir;
    }

}
