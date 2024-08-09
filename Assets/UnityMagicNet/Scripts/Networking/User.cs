using LiteNetLib;
using LiteNetLib.Utils;
using UnityMagicNet.Core;

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

        public void Send(string type, DataType dataType,string data)
        {
            NetDataWriter _dataWriter = new NetDataWriter();
            _dataWriter.Put(PacketHandler.Packing(type, data, dataType));
            Peer.Send(_dataWriter, DeliveryMethod.Sequenced);
        }
    }
}