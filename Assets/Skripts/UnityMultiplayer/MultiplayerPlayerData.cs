using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Unity.Collections;

public struct MultiplayerPlayerData : IEquatable<MultiplayerPlayerData>, INetworkSerializable
{
    public ulong clientId;
    public int colorId;
    public FixedString128Bytes playerName;
    public FixedString128Bytes characterName;

    public bool Equals(MultiplayerPlayerData other)
    {
        return clientId == other.clientId && colorId == other.colorId && playerName == other.playerName && characterName == other.characterName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref characterName);
    }
}
