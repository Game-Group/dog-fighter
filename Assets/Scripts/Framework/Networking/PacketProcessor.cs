using UnityEngine;
using System;
using System.Collections.Generic;

public class PacketProcessor 
{
	public void SendPacket(BitStream stream, Packet packet)
	{
		if (stream.isReading)
			throw new UnityException("Given stream is a read-only stream.");

		stream.Serialize(ref 1);

		packet.Serialize(stream);
	}

	public void SendPackets(BitStream stream, IList<Packet> packets)
	{
		int nrOfPackets = packet.Count;

		stream.Serialize(ref nrOfPackets);

		foreach (Packet packet in packets)
			packet.Serialize(stream);			
	}

	public void ProcessPackets(BitStream stream)
	{
		int nrOfPackets;
		stream.Serialize(ref nrOfPackets);

		for (int i = 0; i < nrOfPackets; i++)
		{

		}
	}

	private IList<Action> processingFunctions;

}
