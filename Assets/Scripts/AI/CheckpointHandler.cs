using UnityEngine;
using System.Collections;

public class CheckpointHandler : MonoBehaviour {


    // The motherships 
    public GameObject[] ships;

    void OnTriggerEnter(Collider Obj)
    {
        // Check if this is a drone
        if (Obj.gameObject.tag == "Npc")
        {
            DroneBehaviour s = Obj.gameObject.GetComponent<DroneBehaviour>();
            GameObject p = ships[0];
            if (TeamHelper.IsSameTeam(int.Parse(p.layer.ToString()), int.Parse(Obj.gameObject.layer.ToString())))
            {
                s.target = ships[1].transform;
            }
            else
            {
                s.target = ships[0].transform;
            }
        }
    }
 
}
