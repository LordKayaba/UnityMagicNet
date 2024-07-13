using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UnityMagicNet.Core
{
    public class UdpServer : INetLogger
    {
        private NetManager Server;
        private NetPeer _ourPeer;
        private NetDataWriter _dataWriter;

        EventBasedNetListener listener = new EventBasedNetListener();

        string connectionKey;
        public void Start(int Port, string ConnectionKey)
        {
            connectionKey = ConnectionKey;
            NetDebug.Logger = this;
            _dataWriter = new NetDataWriter();
            Server = new NetManager(listener);
            Server.Start(Port);

            listener.NetworkErrorEvent += OnNetworkError;
            listener.ConnectionRequestEvent += OnConnectionRequest;
            listener.PeerConnectedEvent += OnPeerConnected;
            listener.PeerDisconnectedEvent += OnPeerDisconnected;
            listener.NetworkReceiveEvent += OnNetworkReceive;

        }

        public void Update()
        {
            if(Server != null)
            Server.PollEvents();
        }

        void FixedUpdate()
        {
            /*if (_ourPeer != null)
            {
                _serverBall.transform.Translate(1f * Time.fixedDeltaTime, 0f, 0f);
                _dataWriter.Reset();
                _dataWriter.Put(_serverBall.transform.position.x);
                _ourPeer.Send(_dataWriter, DeliveryMethod.Sequenced);
            }*/
        }

        public void Stop()
        {
            NetDebug.Logger = null;
            if (Server != null)
                Server.Stop();
        }
        public void OnPeerConnected(NetPeer peer)
        {
            Debug.Log("[SERVER] We have new peer ");
            _ourPeer = peer;
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
        {
            Debug.Log("[SERVER] error " + socketErrorCode);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader,
            UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.Broadcast)
            {
                Debug.Log("[SERVER] Received discovery request. Send discovery response");
                NetDataWriter resp = new NetDataWriter();
                resp.Put(1);
                Server.SendUnconnectedMessage(resp, remoteEndPoint);
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.AcceptIfKey(connectionKey);
            Debug.Log("client Request");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("[SERVER] peer disconnected " + ", info: " + disconnectInfo.Reason);
            if (peer == _ourPeer)
                _ourPeer = null;
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            Debug.Log(reader.GetString());
        }

        public void WriteNet(NetLogLevel level, string str, params object[] args)
        {
            Debug.LogFormat(str, args);
        }
    }
}