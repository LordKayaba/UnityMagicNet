using Newtonsoft.Json;

namespace UnityMagicNet.Core
{
    public enum PacketType
    {
        Login,
        Message,
        GameData,
        OtherTypes
    }

    public class Packet
    {
        [JsonProperty("A0")]
        public PacketType Type { get; set; }
        [JsonProperty("B1")]
        public string Token { get; set; }
        [JsonProperty("C2")]
        public string EncryptedToken { get; set; }
        [JsonProperty("D3")]
        public string Data { get; set; }
        [JsonProperty("E4")]
        public bool ProcessOnServer { get; set; }

        public Packet(PacketType type, string data, bool processOnServer)
        {
            Type = type;
            Data = data;
            ProcessOnServer = processOnServer;
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
}
