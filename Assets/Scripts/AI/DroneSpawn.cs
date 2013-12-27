﻿using UnityEngine;
using System.Collections;

public class DroneSpawn : MonoBehaviour {


    // TODO: Add mid level checkpoints (easy)
    // TODO: Only spawn in case enough creeps died. (hard)
    public Transform[] spawn_locations;
    [HideInInspector]
    Transform[] destination;

    public int startTime;
    public int repeatTime;
    public GameObject drone;

    // an optional checkpoint to which the drones can fly
    public GameObject checkpoint;

    // The number of drones spawned each interval
    public int drone_number;
    int drones_per_location;
    
    // Use this for initialization
	void Start () {
      
        // The total of drones should be divisible by the number of spawn locations;
        if (drone_number % spawn_locations.Length == 0)
        {
            drones_per_location = drone_number / spawn_locations.Length;
        }
        else
        {
            Debug.LogError("Number of Drones is not devisable by amount of spawn locations");
        }

        destination = new Transform[spawn_locations.Length];
	    //Find the corresponding destination locations
        // In case we have a checkpoint, the destination becomes that checkpoint,
        //  Else we will just use end position
        if (checkpoint == null)
        {
            GameObject[] candidates = GameObject.FindGameObjectsWithTag("Mothership");
            if (candidates.Length > 1)
            {
                DroneSpawn s;
                // Check for both objects if it is the mothership we want
                if (candidates[0].transform == this.transform)
                {
                    s = candidates[1].GetComponent<DroneSpawn>();
                }
                else
                {
                    s = candidates[0].GetComponent<DroneSpawn>();
                }

                Transform[] temp_spawn = s.spawn_locations;

                // Copy the spawn locations on the opposing mothership
                // as the destination of our mothership
                for (int i = 0; i < temp_spawn.Length; i++)
                {
                    destination[temp_spawn.Length - 1 - i] = temp_spawn[i];
                }

            }
            else
            {
                Debug.LogError("No Destination Mothership in scene");
            }
        }
        else
        {
            for (int i = 0; i < destination.Length; i++)
                {
                    destination[i] = checkpoint.transform;
                }
        }
        // Start the spawn calls in startTime seconds
        InvokeRepeating("SpawnDrones", startTime, repeatTime);
       
    }

    // Spawn drones from each start location to each destination location
    void SpawnDrones()
    {
            //FIXME: This is an ugly fix for not spawning on each other
            Vector3 space = new Vector3(0, 0 , 70);
            for (int j = 0; j < drones_per_location; j ++)
            {
                
                for (int i = 0; i < spawn_locations.Length; i++)
                {
                    GameObject droneInstance = (GameObject)Instantiate(drone, spawn_locations[i].position  + (space * j), spawn_locations[i].rotation);
                    droneInstance.tag = "Npc";
                    if(TeamHelper.IsSameTeam(gameObject.layer, 8))
                    {
                        droneInstance.layer = 8;
                    }
                    else{
                        droneInstance.layer = 11;
                    }
			        DroneBehaviour behav = droneInstance.GetComponent<DroneBehaviour>();
			        behav.target = destination[i];
                }
            }


    }

}