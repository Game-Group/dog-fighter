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
            //playerShip.GetComponentInChildren<AudioListener>().enabled = false;
            //playerShip.GetComponentInChildren<ShipOrientation>().enabled = false;			
            playerShip.GetComponent<ShipControl>().enabled = false;
            playerShip.GetComponent<SoftwareMouse>().enabled = false;
            playerShip.GetComponent<HUD>().enabled = false;
            playerShip.GetComponent<PlayerHealthControl>().DrawHealthInfo = false;
            playerShip.GetComponentInChildren<ThirdPersonCrosshair>().enabled = false;
        }

        ObjectSync objSync = playerShip.GetComponent<ObjectSync>();
        objSync.Type = ObjectSyncType.PlayerShip;

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
	private void FireWeaponRPC(NetworkViewID playerID, bool fire)
	{
		GameObject playerShip = base.GetPlayerShip(playerID);

		GunSwitcher gunSwitcher = playerShip.GetComponent<GunSwitcher>();

		foreach (GameObject gun in gunSwitcher.CurrentGuns)
		{
			Shooter shooter = gun.GetComponent<Shooter>();
			shooter.KeepFiring = fire;
		}

		if (Network.isServer)
			FireWeapon(base.Players[playerID], fire);
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
