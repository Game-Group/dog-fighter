using UnityEngine;
using System.Collections;

public class droneBehaviour : MonoBehaviour
{
    public Transform target;
    private Transform prevTarget;

    public float speed;
    public float followRadius;
    public float shootRadius;
    public Transform[] gun;
    Shooter gunL;
    private Shooter[] gunScripts;
    
    enum Behaviours{Patrol, Chase, Defend}
    Behaviours currentState;
    Behaviours prevState;


    // Ugly bool to see if we already have set the Shooter array
    bool first;

    void Start()
    {
        // Radius in which drone follows the player for attacking
        SphereCollider c = this.gameObject.AddComponent<SphereCollider>();
        c.radius = 10;//followRadius;
        c.isTrigger = true;


        // initialize gunScripts array
        gunScripts = new Shooter[gun.Length];
        first = true;
        
    }

    void Update()
    {
        // Retrieve gun scripts for shooting purposes
        // TODO: replace to main when possible
        MoveDrone();
    }
    
    
    void GetGunScript()
    {
        for(int i = 0; i < gun.Length; i ++)
        {
            foreach (Transform child in gun[i])
            {
                gunScripts[i] = child.GetComponent<Shooter>();
            }
        }

    }


    // Does the actual moving of the drone
    void MoveDrone()
    {
        // Recalculates the path to get to the target
        Vector3 direction = RecalculatePath();
        
        // Rotation needed to look at direction
        Quaternion rot = Quaternion.LookRotation(direction);

        // Apply rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);

        // Move the drone to the viewed direction
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    // Go back to original target
    void OnTriggerLeave(Collider Object)
    {

        target = prevTarget;
    }


    void OnTriggerStay(Collider Object)
    {

        if ((transform.position - Object.transform.position).magnitude < shootRadius)
        {
              if (first)
              { 
                  GetGunScript();
                  first = false;
              }
              foreach (Shooter s in gunScripts)
              {
                  // Do not yet shoot
                  //s.Shoot();
              }
        }
        // In case in shooting range, shoot shoot shoot
        
    }

    // 
    void OnTriggerEnter(Collider Object)
    {

        //  TODO change in some important condition like:
        //  - enough health
        //  - not state defending
        //  - is this object enemy team
        //  - some other?
        if(Object.tag == "Player")
        {
            prevTarget = target;
            target = Object.transform;
        }
    }

    // Sets a new target
    public void SetTarget(Transform newTarget)
    {
        prevTarget = target;
        target = newTarget;
    }

    // Prevents collision by finding new heading direction
    // FIXME: This should probably have its own class?
    Vector3 RecalculatePath()
    {
        // Direction to target
        Vector3 dir = (target.position - transform.position).normalized;

        // Position of drone
        Vector3 origin = transform.position;

        // direction drone is flying
        Vector3 direction = transform.forward;

        RaycastHit hitInfo;

        // TODO Finetune loength of ray
        float distance = 15;


        // Construct a temporary vector for different orientation
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
        // Shoot ray to the right
        temp.transform.localEulerAngles = new Vector3(0, 45 + transform.localEulerAngles.y, 0);

        Debug.DrawLine(origin, temp.transform.forward + origin, Color.green);
        if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
        {
            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }

        // Shoot ray to the left
        temp.transform.localEulerAngles = new Vector3(0, -45 + transform.localEulerAngles.y, 0);

        Debug.DrawLine(origin, temp.transform.forward + origin, Color.green);
        if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
        {

            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }

        // Shoot ray up
        temp.transform.localEulerAngles = new Vector3(40 + transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        Debug.DrawLine(origin, temp.transform.forward + origin, Color.green);
        if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
        {
            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }

        // Shoot ray down
        temp.transform.localEulerAngles = new Vector3(-40 + transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        Debug.DrawLine(origin, temp.transform.forward + origin, Color.green);
        if (Physics.Raycast(origin, temp.transform.forward, out hitInfo, distance))
        {

            if (hitInfo.transform != transform)
            {

                Debug.DrawLine(origin, hitInfo.point, Color.red);
                Debug.DrawLine(hitInfo.point, hitInfo.normal, Color.blue);
                dir += hitInfo.normal * 20;
            }
        }
        Destroy(temp);
        // Return final new direction
        return dir;
    }

}
