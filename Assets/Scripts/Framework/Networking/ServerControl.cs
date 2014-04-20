using UnityEngine;
using System.Collections;

public class ServerControl : NetworkObject 
{
	public Object RPCChannelPrefab;

	public LevelCreator CurrentLevel { get; private set; }

	public void ChangeLevel(LevelCreator level)
	{
		this.CurrentLevel = level;
		this.CurrentLevel.CreateLevel();		
	}

    public void Shutdown()
    {
        foreach (NetworkPlayer p in Network.connections)
            Network.CloseConnection(p, true);

        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

	// Use this for initialization
	protected override void Start ()
	{
		base.Start();

		this.name = "ServerControl";

		Network.InitializeServer(10, GlobalSettings.ServerPort, false);
	}

	private bool firstPlayerJoined;
    private bool firstTeamAssigned;

	private void OnServerInitialized()
	{
		// Network Initialize the RPCChannel to agree on a NetworkView with all clients.
		Network.Instantiate(this.RPCChannelPrefab, Vector3.zero, Quaternion.identity, 0);
	}

	private void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
        if (!this.firstPlayerJoined)
		{			
			ObjectRPC.LoadLevel(0);
		}

		Debug.Log("A new player has joined.");
		Debug.Log("Current number of players: " + base.Players.Count);

		foreach (Player p in base.Players.Values)
		{
			PlayerRPC.NewPlayerJoined(networkPlayer, p.NetworkPlayerInfo, p.ID);
		}		

		// Generate a viewID for the new player.
		NetworkViewID viewID = Network.AllocateViewID();

		// Notice everyone that the new player has joined.
		PlayerRPC.NewPlayerJoined(networkPlayer, viewID);

        // Sync the existing game world with the new player.
        Player newPlayer = base.Players[viewID];
        this.CurrentLevel.SyncNewPlayer(newPlayer);

        this.assignTeam(newPlayer);

        this.firstPlayerJoined = true;
	}

    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        Player disconnectedPlayer = null;

        foreach (Player p in base.Players.Values)
            if (p.NetworkPlayerInfo == player)
            {
                disconnectedPlayer = p;
                break;
            }

        Debug.Log("Team " + TeamHelper.GetTeamNumber((int)disconnectedPlayer.Team) + " has been disconnected!");

        MatchResult result = disconnectedPlayer.Team == Layers.Team1Actor ? MatchResult.Team2Win : MatchResult.Team1Win;

        MatchControl matchControl = GameObject.Find(GlobalSettings.MatchControlName).GetComponent<MatchControl>();
        matchControl.EndMatch(result);

    }

    private void assignTeam(Player player)
    {
        Layers layer = Layers.Team1Actor;

        if (this.firstPlayerJoined)
            layer = Layers.Team2Actor;

        PlayerObjects objs = base.ObjectTables.PlayerObjects[player];
        GameObject playerShip = base.ObjectTables.GetPlayerObject(player, objs.PlayerShipID);
        playerShip.layer = (int)layer;

        // TODO: Player team is NOT synced for clients at the moment.
        player.Team = layer;

        Debug.Log("Assigned layer: " + layer);

        ObjectRPC.SetObjectLayer(player, objs.PlayerShipID, layer);
    }
}
