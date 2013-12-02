using UnityEngine;
using System.Collections.Generic;

public class PlayerObjectTable : MonoBehaviour
{
	public IDictionary<Player, GameObject> PlayerShips { get; private set; }

	public PlayerObjectTable()
	{
		this.PlayerShips = new Dictionary<Player,  GameObject>(10);
		this.objectTables = new Dictionary<NetworkViewID, ObjectTable>(10);
	}

	public void AddPlayerTable(Player player)
	{
		if (!this.objectTables.ContainsKey(player.ID))
			this.objectTables.Add(player.ID, new ObjectTable());
		else
			throw new UnityException("Table for given player already exists.");
	}
	
	public void RemovePlayerTable(Player player)
	{
		if (this.objectTables.ContainsKey(player.ID))
			this.objectTables.Remove(player.ID);
		else
			throw new UnityException("Table for given player does not exist.");
	}

	public GameObject GetPlayerObject(Player player, int objID)
	{
		ObjectTable table;
		
		if (this.objectTables.TryGetValue(player.ID, out table))
			return table.GetObject(objID);
		else
			throw new UnityException("Table for given player does not exist.");
	}

	public void AddPlayerObject(Player player, int objID, GameObject obj)
	{
		ObjectTable table;
		
		if (this.objectTables.TryGetValue(player.ID, out table))
			table.AddObject(objID, obj);
		else
			throw new UnityException("Table for given player does not exist.");
	}

	public void RemovePlayerObject(Player player, int objID)
	{
		ObjectTable table;
		
		if (this.objectTables.TryGetValue(player.ID, out table))
			table.RemoveObject(objID);
		else
			throw new UnityException("Table for given player does not exist.");
	}



	private IDictionary<NetworkViewID, ObjectTable> objectTables;
}
