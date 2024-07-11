using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMagicNet.Core;

namespace UnityMagicNet
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager network;
        public Configuration configuration = new Configuration();

        UdpServer server = new UdpServer();
        UdpClient client = new UdpClient();

        RoomManager roomManager;
        UserManager userManager;

        void Start()
        {
            if (network == null)
            {
                network = this;
                DontDestroyOnLoad(gameObject);
                StartNetwork();
            }
        }

        void Update()
        {
            server.Update();
            client.Update();
        }

        public void StartNetwork()
        {
#if UNITY_EDITOR
            server.Start(configuration.Port, configuration.ConnectionKey);
            client.Start(configuration.IP, configuration.Port, configuration.ConnectionKey);
#elif UNITY_SERVER
            server.Start(configuration.Port, configuration.ConnectionKey);
#else
            client.Start(configuration.IP, configuration.Port, configuration.ConnectionKey);
#endif
        }

        void OnDisable()
        {
            server.Stop();
            client.Stop();
            Debug.Log("Sever Stoped!");
        }
    }
}