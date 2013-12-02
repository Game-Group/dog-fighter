using UnityEngine;
using System.Collections;
using System;

public class PlayerShipRPC : RPCHolder {

	public GameObject PlayerPrefab;

	public static void CreatePlayerShip(Player player, int objectID)
	{
		Debug.Log("Sending create player ship.");

		channel.networkView.RPC("CreatePlayerShipRPC", channel.RPCMode, player.ID, objectID);
	}

	public static void SinglCreatePlayerShip(NetworkPlayer target, Player player, int objectID)
	{
		channel.networkView.RPC("CreatePlayerShipRPC", target, player.ID, objectID);
	}

	public static void PlayerShipPosition(Player player, int objectID, Vector3 position, Vector3 orientation)
	{
//		Debug.Log ("Sending player ship position.");

		channel.networkView.RPC ("PlayerShipPositionRPC", RPCMode.Others, player.ID, objectID, position, orientation);
	}

	public static void PlayerShipVelocity(Player player, int objectID, Vector3 transform, Vector3 rotation)
	{
//		Debug.Log("Sending player ship velocity.");

		channel.networkView.RPC ("PlayerShipVelocityRPC", RPCMode.Server, player.ID, objectID, transform, rotation);
		
	}

	[RPC]
	private void CreatePlayerShipRPC(NetworkViewID playerID, int objectID, NetworkMessageInfo info)
	{
		Debug.Log("Create player ship RPC received!");
		
		GameObject playerShip = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
		
		if (base.NetworkControl.LocalViewID == playerID)
		{
//			Debug.Log("Yes!");
			playerShip.GetComponentInChildren<Camera>().enabled = true;
		}
		else
		{
			playerShip.GetComponentInChildren<Camera>().enabled = false;
			playerShip.GetComponentInChildren<AudioListener>().enabled = false;
			playerShip.GetComponentInChildren<ShipOrientation>().enabled = false;			
			playerShip.GetComponent<ShipControl>().enabled = false;
		}

		base.ObjectTables.PlayerShips.Add(base.Players[playerID], playerShip);
		base.AddToObjectTables(playerShip, playerID, objectID);
	}

	[RPC]
	private void PlayerShipPositionRPC(NetworkViewID playerID, int objectID, Vector3 position, Vector3 orientation)
	{
//		Debug.Log("Player ship position received.");

		Player player = base.NetworkControl.Players[playerID];

		GameObject playerShip = base.ObjectTables.GetPlayerObject(player, objectID);
		playerShip.transform.position = position;
		playerShip.transform.eulerAngles = orientation;
	}

	[RPC]
	private void PlayerShipVelocityRPC(NetworkViewID playerID, int objectID, Vector3 translation, Vector3 rotation)
	{
//		Debug.Log("Player ship velocity received.");		

		Player player = base.NetworkControl.Players[playerID];

		GameObject playerShip = base.ObjectTables.GetPlayerObject(player, objectID);
		ObjectTransformer objectTransform = playerShip.GetComponent<ObjectTransformer>();

		objectTransform.Translation = translation;
		objectTransform.Rotation = rotation;
	}

	private static PlayerShipRPC channel
	{
		get
		{
			if (channel_ == null)
				channel_ = GameObject.Find(NetworkControl.RPCChannelObject).GetComponent<PlayerShipRPC>();
			return channel_;
		}
	}

	private static PlayerShipRPC channel_;
}
