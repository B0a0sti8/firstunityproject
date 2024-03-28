using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public struct MultiplayerPlayerData : IEquatable<MultiplayerPlayerData>, INetworkSerializable
{
    public ulong clientId;

    public bool Equals(MultiplayerPlayerData other)
    {
        return clientId == other.clientId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
    }
}
