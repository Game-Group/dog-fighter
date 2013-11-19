using UnityEngine;
using System;

public class Packet
{
	public PacketType Type { get; private set; }

	public virtual void Serialize(BitStream stream)
	{
		if (stream.isWriting)
		{
			int type = (int)this.Type;
			stream.Serialize(ref type);
		}
	}

	protected Packet (PacketType type)
	{
		this.Type = type;
	}

}
