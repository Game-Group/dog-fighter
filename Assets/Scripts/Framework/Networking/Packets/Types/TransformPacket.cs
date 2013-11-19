using UnityEngine;
using System;

public class TransformPacket : Packet
{
	public Vector3 Position;
	public Vector3 Orientation;

	public TransformPacket ()
		: base(PacketType.Transform)
	{
	}

	public override void Serialize (BitStream stream)
	{
		base.Serialize (stream);

		stream.Serialize(ref this.Position);
		stream.Serialize(ref this.Orientation);
	}
}

