﻿using UnityEngine;
using System.Collections;

public class ClientControl : NetworkObject {

	// Use this for initialization
	protected override void Start () {

		base.Start();

		Debug.Log("Connecting");

		Network.Connect(base.NetworkControl.ServerIP, base.NetworkControl.ServerPort);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnConnectedToServer()
	{
		Debug.Log("Connected to server.");
	}

	private void OnFailedToConnect(NetworkConnectionError error) 
	{
		GameObject.Destroy(this);
	}
}
