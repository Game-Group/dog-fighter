using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Respawner : MonoBehaviour 
{
	public GameObject[] SpawnsAtStart;

	private List<GameObjectRespawnTimePair> WaitingForRespawn;

	void Start () 
	{
		WaitingForRespawn = new List<GameObjectRespawnTimePair>(SpawnsAtStart.Length);
		for (int i = 0; i < SpawnsAtStart.Length; i++)
			Spawn (SpawnsAtStart[i]);
		SpawnsAtStart = null;
	}

	public void DisableAndRespawn(GameObject obj, float timeBeforeRespawn)
	{
		obj.transform.GetChild(0).gameObject.SetActive(false);
		obj.GetComponent<ShipControl>().enabled = false;
		WaitingForRespawn.Add(new GameObjectRespawnTimePair(obj, timeBeforeRespawn));
	}

	private void Respawn(GameObject obj)
	{
		obj.transform.position = gameObject.transform.position;
		obj.transform.rotation = gameObject.transform.rotation;
		obj.transform.GetChild(0).gameObject.SetActive(true);
		obj.GetComponent<ShipControl>().enabled = true;
	}

	private void Spawn(GameObject prefab)
	{
		Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
	}
	
	void Update () 
	{
		for (int i = 0; i < WaitingForRespawn.Count; i++)
		{
			WaitingForRespawn[i].RespawnTimer -= Time.deltaTime;
			if (WaitingForRespawn[i].RespawnTimer <= 0)
			{
				Respawn(WaitingForRespawn[i].Prefab);
				WaitingForRespawn.RemoveAt(i);
				i--;
			}
		}
	}
}
