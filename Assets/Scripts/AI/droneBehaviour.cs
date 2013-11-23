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
        var rot = Quaternion.LookRotation(direction);
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
        float distance = 20;
      
        Vector3 leftR = origin;
        Vector3 rightR = origin;
        leftR.x -= 2;
        rightR.x += 2;

        // Shoot 3 different rays
        // One from the center of the drone to the fron
        // two from the sides
        // In case the ray hits a rigid body, change the course of the drone
        if (Physics.Raycast(origin, direction, out hitInfo, distance))
        {
            if (hitInfo.transform != transform)
            {
                dir += hitInfo.normal * 20;
            }
        }

        if (Physics.Raycast(leftR, direction, out hitInfo, distance))
        {
            if (hitInfo.transform != transform)
            {
                dir += hitInfo.normal * 20;
            }
        }

        if (Physics.Raycast(rightR, direction, out hitInfo, distance))
        {

            if (hitInfo.transform != transform)
            {
                dir += hitInfo.normal * 20;
            }
        }

        return dir;
    }

}
