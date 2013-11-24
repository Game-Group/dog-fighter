using UnityEngine;
using System.Collections;

public class GameObjectRespawnTimePair
{
	public GameObject Prefab;
	public float RespawnTimer;

	public GameObjectRespawnTimePair(GameObject prefab, float respawnTime)
	{
		this.Prefab = prefab;
		this.RespawnTimer = respawnTime;
	}
}
