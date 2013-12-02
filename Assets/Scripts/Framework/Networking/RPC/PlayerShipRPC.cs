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
		
		return base.ObjectTables.PlayerShips[player];
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
