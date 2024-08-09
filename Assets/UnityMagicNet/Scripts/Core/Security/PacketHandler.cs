using System;
using UnityEngine;

namespace UnityMagicNet.Core
{
    public static class PacketHandler
    {
        public static string Packing(string type, string data, DataType dataType)
        {
            string jsonData = "";

            if (dataType != DataType.NoCompress)
            {
                Header header = new Header(type, dataType, "");
                string header2 = SecurityUtils.Encrypt(header.Serialize());
                Packet packet = new Packet(header2, data, header.Token2);

                string compressedData = Convert.ToBase64String(SecurityUtils.Compress(packet.Data));
                packet.Data = SecurityUtils.Encrypt(compressedData);

                jsonData = packet.Serialize();
            }
            else
            {
                Header header = new Header(type, dataType, data);
                string header2 = SecurityUtils.Encrypt(header.Serialize());
                Packet packet = new Packet(header2, "",header.Token2);

                jsonData = packet.Serialize();
            }

            return jsonData;
        }

        public static Role UnPacking(string receivedData, bool isServer)
        {
            try
            {
                Packet packet = Packet.Deserialize(receivedData);
                Debug.Log(packet.Header);

                Header header = Header.Deserialize(SecurityUtils.Decrypt(packet.Header));

                if (packet.Token != header.Token2)
                {
                    Debug.LogError("Packet validation failed: Invalid token.");
                    return null;
                }

                if ((isServer && header.dataType == DataType.CompressOnServer) || (!isServer && (header.dataType != DataType.NoCompress)))
                {
                    string decryptedData = SecurityUtils.Decrypt(packet.Data);
                    packet.Data = SecurityUtils.Decompress(Convert.FromBase64String(decryptedData));
                }

                return new Role(header, packet, receivedData);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to process incoming packet: " + ex.Message);
                return null;
            }
        }
    }
}