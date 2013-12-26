using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalSettings : MonoBehaviour 
{
	public static bool SinglePlayer = false;
	
	public static bool HasFocus { get; private set;}

    public static IList<GameObject> Team1Npcs;
    public static IList<GameObject> Team2Npcs;
	
	// Use this for initialization
	void Start () 
    {
        Team1Npcs = new List<GameObject>();
        Team2Npcs = new List<GameObject>();

        DontDestroyOnLoad(this);
	}

    public static void AddNpc(GameObject npc)
    {
        int team = TeamHelper.GetTeamNumber(npc.layer);

        if (team == 1)
            Team1Npcs.Add(npc);
        else Team2Npcs.Add(npc);
    }

    public static void RemoveNpc(GameObject npc)
    {
        int team = TeamHelper.GetTeamNumber(npc.layer);

        if (team == 1)
            Team1Npcs.Remove(npc);
        else Team2Npcs.Remove(npc);
    }

	void OnApplicationFocus(bool focus) 
	{
    	HasFocus = focus;    
		//Debug.Log("Focus = " + this.HasFocus);
	}
}
