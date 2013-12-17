using UnityEngine;
using System.Collections;

public class LevelCreator : NetworkObject 
{
	public LevelCreator()
		: base()
	{
		base.Start();
	}

	public virtual void CreateLevel()
	{
	}

	public virtual void SyncNewPlayer(Player newPlayer)
	{
	}

}
