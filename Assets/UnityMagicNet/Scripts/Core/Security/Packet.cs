using Newtonsoft.Json;
using UnityMagicNet.Core;

namespace UnityMagicNet
{
    public enum DataType
    {
        NoCompress, Compress, CompressOnServer
    }

    public class Header
    {
        [JsonProperty("G6")]
        public string Type;

        [JsonProperty("F5")]
        public DataType dataType;

        [JsonProperty("E4")]
        public string Token2;

        [JsonProperty("D3")]
        public string Data;

        public Header(string type, DataType dataType0, string data)
        {
            Type = type;
            dataType = dataType0;
            Token2 = SecurityUtils.GenerateToken();
            Data = data;
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
        public string Data;
        public string receivedData;

        public Role(Header header0, Packet packet0, string receivedData0)
        {
            header = header0;
            packet = packet0;
            receivedData = receivedData0;
            if(header.dataType == DataType.NoCompress)
            {
                Data = header.Data;
            }
            else
            {
                Data = packet.Data;
            }
        }
    }
}
