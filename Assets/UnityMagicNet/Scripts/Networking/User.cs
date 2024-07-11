using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;


namespace UnityMagicNet 
{
    public class User
    {
        public string UserId { get; private set; }
        public NetPeer Peer { get; private set; }

        public User(string userId, NetPeer peer)
        {
            UserId = userId;
            Peer = peer;
        }
    }
}