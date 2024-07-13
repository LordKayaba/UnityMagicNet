using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UnityMagicNet.Core
{
    public class UdpClient
    {
        private NetManager Client;
        EventBasedNetListener listener = new EventBasedNetListener();

        public void Start(string IP, int Port, string ConnectionKey)
        {
            Client = new NetManager(listener);
            Client.Start();
            Client.Connect(IP, Port, ConnectionKey);

            listener.NetworkErrorEvent += OnNetworkError;
            listener.PeerConnectedEvent += OnPeerConnected;
            listener.PeerDisconnectedEvent += OnPeerDisconnected;
            //listener.NetworkReceiveEvent += OnNetworkReceive;
        }

        public void Update()
        {
            if(Client != null)
            Client.PollEvents();

            /*var peer = _netClient.FirstPeer;
            if (peer != null && peer.ConnectionState == ConnectionState.Connected)
            {
                //Fixed delta set to 0.05
                var pos = _clientBallInterpolated.transform.position;
                pos.x = Mathf.Lerp(_oldBallPosX, _newBallPosX, _lerpTime);
                _clientBallInterpolated.transform.position = pos;

                //Basic lerp
                _lerpTime += Time.deltaTime / Time.fixedDeltaTime;
            }
            else
            {
                _netClient.SendBroadcast(new byte[] { 1 }, 5000);
            }*/
        }

        public void Stop()
        {
            if (Client != null)
                Client.Stop();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            NetDataWriter _dataWriter = new NetDataWriter();
            _dataWriter.Put(PacketHandler.Packing("Test", "ssbkbjb", true));
            peer.Send(_dataWriter, DeliveryMethod.Sequenced);
            Debug.Log("[CLIENT] We connected to ");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
        {
            Debug.Log("[CLIENT] We received error " + socketErrorCode);
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            /*_newBallPosX = reader.GetFloat();

            var pos = _clientBall.transform.position;

            _oldBallPosX = pos.x;
            pos.x = _newBallPosX;

            _clientBall.transform.position = pos;

            _lerpTime = 0f;*/
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.BasicMessage && Client.ConnectedPeersCount == 0 && reader.GetInt() == 1)
            {
                Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
                Client.Connect(remoteEndPoint, "sample_app");
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        public void OnConnectionRequest(ConnectionRequest request)
        {

        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
        }
    }
}