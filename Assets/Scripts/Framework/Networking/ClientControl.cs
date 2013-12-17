using UnityEngine;
using System.Collections;

public class ClientControl : NetworkObject 
{
	public LevelCreator CurrentLevel { get; private set; }
	
	public void ChangeLevel(LevelCreator level)
	{
		this.CurrentLevel = level;
		this.CurrentLevel.CreateLevel();
	}

	// Use this for initialization
	protected override void Start () {

		base.Start();

		this.name = "ClientControl";

		Debug.Log("Connecting");

		Network.Connect(base.NetworkControl.ServerIP, base.NetworkControl.ServerPort);
	}
	
	// Update is called once per frame
	protected override void Update () {
	
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
