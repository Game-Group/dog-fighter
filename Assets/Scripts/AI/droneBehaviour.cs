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
    
    enum Behaviours{Patrol, Chase, Defend, GoTo}
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

        currentState = Behaviours.GoTo;

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

        if (currentState == Behaviours.Chase && InShootingRange())
        {
            rot = Quaternion.LookRotation(target.position - transform.position);
            // Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 8);

            if ((target.position - transform.position).magnitude > 5)
            {
                // Move the drone to the viewed direction
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            // Do we want the bot to rotate or just hang in the air
            else
            {
                transform.RotateAround(target.position, target.up, 30 * Time.deltaTime);
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

    // Go back to original target and state
    void OnTriggerLeave(Collider Object)
    {

        Debug.Log("return to previous");
        target = prevTarget;
        currentState = prevState;
    }

    void OnTriggerStay(Collider Object)
    {
        
        // Check to make sure if the object getting this close is the object we 
        // are targeting
        if (Object.transform == target.transform)
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
    }

    void OnTriggerEnter(Collider Object)
    {
        //  TODO change in some important condition like:
        //  - enough health
        //  - not state defending
        //  - is this object enemy team
        //  - some other?
        Debug.Log(Object.gameObject.tag);
        if( (Object.gameObject.tag == "Player" || Object.gameObject.tag == "Npc") &&
              (Object.gameObject.layer != this.gameObject.layer))
        {
            Debug.Log("Go after this");
            // Only chase if this object is closer than current target

            prevTarget = target;
            target = Object.transform;

            prevState = currentState;
            currentState = Behaviours.Chase;
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
