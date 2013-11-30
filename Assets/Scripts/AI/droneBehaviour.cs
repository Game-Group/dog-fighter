using UnityEngine;
using System.Collections;

public class DroneBehaviour : MonoBehaviour
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

    PreventCollision pc;

    // Ugly bool to see if we already have set the Shooter array
    bool first;

    void Start()
    {
        // Radius in which drone follows the player for attacking
        SphereCollider c = this.gameObject.AddComponent<SphereCollider>();
        //followRadius;
        c.radius = 10;
        c.isTrigger = true;


        // initialize gunScripts array
        gunScripts = new Shooter[gun.Length];
        first = true;
        
        // Add the prevent collision code 
        pc = this.gameObject.AddComponent<PreventCollision>();
        pc.setActor(this.transform);
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

    Transform temp;
    // Does the actual moving of the drone
    void MoveDrone()
    {



        Quaternion rot;
        Vector3 direction;

 

        // In case we are in shooting range keep looking at the opponent

        if (InShootingRange())
        {
            rot = Quaternion.LookRotation(target.position - transform.position);
            // Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 8);
            
            if ((target.position - transform.position).magnitude > 5) 
            {
                // Move the drone to the viewed direction
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
        else
        {

            // Recalculates the path to get to the target
            direction = pc.RecalculatePath(target);
            // Rotation needed to look at direction
            rot = Quaternion.LookRotation(direction);
            // Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);

            // Move the drone to the viewed direction
            transform.position += transform.forward * speed * Time.deltaTime;

        }

        

    }

    // Go back to original target
    void OnTriggerLeave(Collider Object)
    {

        target = prevTarget;
    }


    void OnTriggerStay(Collider Object)
    {

        // In case we are in shoot radius, shoot shoot shoot.
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
                 s.Shoot();
              }
        }
        
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

    bool InShootingRange()
    {
        if ((transform.position - target.transform.position).magnitude < shootRadius)
        {
            return true;
        }
        return false;
    }

    // Sets a new target
    public void SetTarget(Transform newTarget)
    {
        prevTarget = target;
        target = newTarget;
    }

    

}
