using UnityEngine;
using System.Collections;
using System;

public class PlayerShipRPC : RPCHolder {

	public GameObject PlayerPrefab;

	#region Call Functions
	public static void CreatePlayerShip(Player player, int objectID)
	{
//		Debug.Log("Sending create player ship.");

		channel.networkView.RPC("CreatePlayerShipRPC", channel.RPCMode, player.ID, objectID);
	}

	public static void CreatePlayerShip(NetworkPlayer target, Player player, int objectID)
	{
		channel.networkView.RPC("CreatePlayerShipRPC", target, player.ID, objectID);
	}

	public static void SpawnPlayerShip(Player player, int spawnPointID, int playerShipID)
	{
//		Debug.Log ("Sending spawn player RPC");

		channel.networkView.RPC("SpawnPlayerShipRPC", RPCMode.All, player.ID, spawnPointID, playerShipID);
		
	}
	public static void SpawnPlayerShip(NetworkPlayer target, Player player, int spawnPointID, int playerShipID)
	{
//		Debug.Log ("Sending spawn player RPC");

		channel.networkView.RPC("SpawnPlayerShipRPC", target, player.ID, spawnPointID, playerShipID);			
	}

	public static void PlayerShipPosition(Player player, Vector3 position, Vector3 orientation)
	{
//		Debug.Log ("Sending player ship position.");

		channel.networkView.RPC ("PlayerShipPositionRPC", RPCMode.Others, player.ID, position, orientation);
	}

	public static void PlayerShipVelocity(Player player, int objectID, Vector3 transform, Vector3 rotation)
	{
//		Debug.Log("Sending player ship velocity.");

		channel.networkView.RPC ("PlayerShipVelocityRPC", RPCMode.Server, player.ID, transform, rotation);
		
	}

	public static void FireWeapon(Player player, bool fire) 
	{
		RPCMode mode;

		if (Network.isClient)
			mode = RPCMode.Server;
		else
			mode = RPCMode.Others;

		channel.networkView.RPC ("FireWeaponRPC", RPCMode.Server, player.ID, fire);
		
	}
	#endregion

	#region RPC Definitions
	[RPC]
	private void CreatePlayerShipRPC(NetworkViewID playerID, int objectID, NetworkMessageInfo info)
	{
//		Debug.Log("Create player ship RPC received!");

		Player owner = base.NetworkControl.Players[playerID];
		
		GameObject playerShip = (GameObject)GameObject.Instantiate(this.PlayerPrefab);
		
		if (base.NetworkControl.LocalViewID == playerID)
		{
			base.ObjectTables.ThisPlayerObjects.PlayerShipID = objectID;
			playerShip.GetComponentInChildren<Camera>().enabled = true;
		}
		else
		{
			playerShip.GetComponentInChildren<Camera>().enabled = false;
			playerShip.GetComponentInChildren<AudioListener>().enabled = false;
			playerShip.GetComponentInChildren<ShipOrientation>().enabled = false;			
			playerShip.GetComponent<ShipControl>().enabled = false;
            playerShip.GetComponent<PlayerHealthControl>().DrawHealthInfo = false;
		}

		base.ObjectTables.PlayerObjects[owner].PlayerShipID = objectID;
		base.AddToObjectTables(playerShip, playerID, objectID);
	}

	[RPC]
	private void SpawnPlayerShipRPC(NetworkViewID owner, int spawnPointID, int playerShipID)
	{
//		Debug.Log("Spawn player RPC received.");

		Player player = base.Players[owner];
		GameObject spawnPoint = base.ObjectTables.GetPlayerObject(player, spawnPointID);
		GameObject playerShip = base.ObjectTables.GetPlayerObject(player, playerShipID);

		spawnPoint.GetComponent<PlayerRespawner>().Respawn(playerShip);
	}

	[RPC]
	private void PlayerShipPositionRPC(NetworkViewID playerID, Vector3 position, Vector3 orientation)
	{
//		Debug.Log("Player ship position received.");

		GameObject playerShip = this.getPlayerShip(playerID);
		playerShip.transform.position = position;
		playerShip.transform.eulerAngles = orientation;
	}

	[RPC]
	private void PlayerShipVelocityRPC(NetworkViewID playerID, Vector3 translation, Vector3 rotation)
	{
//		Debug.Log("Player ship velocity received.");		

		GameObject playerShip = this.getPlayerShip(playerID);
		ObjectTransformer objectTransform = playerShip.GetComponent<ObjectTransformer>();

		objectTransform.Translation = translation;
		objectTransform.Rotation = rotation;
	}

	[RPC]
	private void FireWeaponRPC(NetworkViewID playerID, bool fire)
	{
		GameObject playerShip = this.getPlayerShip(playerID);

		GunSwitcher gunSwitcher = playerShip.GetComponent<GunSwitcher>();

		foreach (GameObject gun in gunSwitcher.CurrentGuns)
		{
			Shooter shooter = gun.GetComponent<Shooter>();
			shooter.KeepFiring = fire;
		}

		if (Network.isServer)
			FireWeapon(base.Players[playerID], fire);
	}

	private GameObject getPlayerShip(NetworkViewID playerID)
	{
		Player player = base.NetworkControl.Players[playerID];

		int playerShipID = base.ObjectTables.PlayerObjects[player].PlayerShipID;
		
		return base.ObjectTables.GetPlayerObject(player, playerShipID);
	}
	#endregion

	#region Singleton
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
	#endregion
}
