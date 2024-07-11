using System;
using UnityEngine;

namespace UnityMagicNet.Core
{
    public static class PacketHandler
    {
        public static string Packing(Packet packet)
        {
            packet.Token = SecurityUtils.GenerateToken();
            packet.EncryptedToken = SecurityUtils.Encrypt(packet.Token);

            string compressedData = Convert.ToBase64String(SecurityUtils.Compress(packet.Data));
            packet.Data = SecurityUtils.Encrypt(compressedData);

            string jsonData = packet.Serialize();

            return jsonData;
        }

        public static Packet UnPacking(string receivedData, bool isServer)
        {
            try
            {
                Packet packet = Packet.Deserialize(receivedData);

                string decryptedToken = SecurityUtils.Decrypt(packet.EncryptedToken);
                if (decryptedToken != packet.Token)
                {
                    Debug.LogError("Packet validation failed: Invalid token.");
                    return null;
                }

                if (isServer && packet.ProcessOnServer)
                {
                    string decryptedData = SecurityUtils.Decrypt(packet.Data);
                    packet.Data = SecurityUtils.Decompress(Convert.FromBase64String(decryptedData));
                }
                else if (!isServer)
                {
                    string decryptedData = SecurityUtils.Decrypt(packet.Data);
                    packet.Data = SecurityUtils.Decompress(Convert.FromBase64String(decryptedData));
                }

                return packet;
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to process incoming packet: " + ex.Message);
                return null;
            }
        }
    }
}