using Newtonsoft.Json;

namespace UnityMagicNet.Core
{
    public class Header
    {
        [JsonProperty("F5")]
        public string Type;

        [JsonProperty("E4")]
        public bool ProcessOnServer;

        [JsonProperty("D3")]
        public string Token2;

        public Header(string type, bool processOnServer)
        {
            Type = type;
            ProcessOnServer = processOnServer;
            Token2 = SecurityUtils.GenerateToken();
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Header Deserialize(string jsonData)
        {
            return JsonConvert.DeserializeObject<Header>(jsonData);
        }
    }

    public class Packet
    {
        [JsonProperty("A0")]
        public string Header;

        [JsonProperty("B1")]
        public string Token;

        [JsonProperty("C2")]
        public string Data;

        public Packet(string header, string data, string token)
        {
            Header = header;
            Token = token;
            Data = data;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Packet Deserialize(string jsonData)
        {
            return JsonConvert.DeserializeObject<Packet>(jsonData);
        }
    }

    public class Role
    {
        public Header header;
        public Packet packet;
        public string receivedData;

        public Role(Header header0, Packet packet0, string receivedData0)
        {
            header = header0;
            packet = packet0;
            receivedData = receivedData0;
        }
    }
}
