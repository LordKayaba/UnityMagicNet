using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityMagicNet
{
    [Serializable]
    public class Configuration
    {
        public string IP = "localhost";
        public int Port = 5000;
        public string ConnectionKey = "TestKey";
        public Server server;
        public Client client;
        public Panel panel;
    }

    [Serializable]
    public class Server
    {
        public bool Host;
    }

    [Serializable]
    public class Client
    {
        
    }

    [Serializable]
    public class Panel
    {
        public bool Web, App;
        public string Username = "root", Password = "1234", WebAddress = "http://*:8080/";
    }

}
