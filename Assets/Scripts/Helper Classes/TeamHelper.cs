using UnityEngine;
using System.Collections;

public static class TeamHelper 
{
	public static int GetTeamNumber(string tag)
	{
		string c = tag.Substring(4, 1);

		int result = -1;
		if (int.TryParse(c, out result))
			return result;
		else return -1;
	}

	public static bool IsSameTeam(int layer1, int layer2)
	{
		string layer1Name = LayerMask.LayerToName(layer1);
		string layer2Name = LayerMask.LayerToName(layer2);

		string layer1Team = layer1Name.Substring(0, 5);
		string layer2Team = layer2Name.Substring(0, 5);

		return layer1Team == layer2Team;
	}

	public static bool IsSameTeam(string tag, int layer)
	{
		string layerName = LayerMask.LayerToName(layer);
		
		string tagTeam = tag.Substring(0, 5);
		string layerTeam = layerName.Substring(0, 5);
		
		return tagTeam == layerTeam;
	}

	public static bool IsSameTeam(int layer, string tag)
	{
		return IsSameTeam(tag, layer);
	}
	
	public static string LayerToProjectileTag(int layer)
	{
		string layerName = LayerMask.LayerToName(layer);
		string team = layerName.Substring(0, 5);
		return team + "Projectile";
	}
	
	public static int ProjectileTagToLayer(string tag, LayerType layerType)
	{
		string team = tag.Substring(0, 5);
		string layerName = team + LayerTypeToString(layerType);
		return LayerMask.NameToLayer(layerName);
	}
	
	public static string LayerTypeToString(LayerType type)
	{
		switch(type)
		{
			case LayerType.Actor:
				return "Actor";
			case LayerType.Mothership:
				return "Mothership";
			case LayerType.Projectile:
				return "Projectile";
		}

		return "Invalid";
	}
}
