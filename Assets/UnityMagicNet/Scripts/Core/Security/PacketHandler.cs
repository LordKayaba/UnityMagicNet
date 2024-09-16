using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityMagicNet.Core
{
    public static class PacketHandler
    {
        public async static Task<string> Packing(string type, string data, DataType dataType)
        {
            string jsonData = "";

            try
            {
                if (dataType != DataType.NoCompress)
                {
                    Header header = new Header(type, dataType, "");
                    string encryptedHeader = await SecurityUtils.Encrypt(header.Serialize());

                    Packet packet = new Packet(encryptedHeader, data, header.Token2);

                    Task<byte[]> compressedDataTask = SecurityUtils.Compress(packet.Data);
                    Task<string> encryptedDataTask = SecurityUtils.Encrypt(Convert.ToBase64String(await compressedDataTask));

                    packet.Data = await encryptedDataTask;

                    jsonData = packet.Serialize();
                }
                else
                {
                    Header header = new Header(type, dataType, data);
                    string encryptedHeader = await SecurityUtils.Encrypt(header.Serialize());

                    Packet packet = new Packet(encryptedHeader, "", header.Token2);
                    jsonData = packet.Serialize();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in Packing: {ex.Message}");
                throw;
            }

            return jsonData;
        }

        public static async Task<Role> UnPacking(string receivedData, bool isServer)
        {
            try
            {
                Packet packet = Packet.Deserialize(receivedData);

                Header header = Header.Deserialize(await SecurityUtils.Decrypt(packet.Header));

                if (packet.Token != header.Token2)
                {
                    Debug.LogError("Packet validation failed: Invalid token.");
                    return null;
                }

                if ((isServer && header.dataType == DataType.CompressOnServer) || (!isServer && header.dataType != DataType.NoCompress))
                {
                    string decryptedData = await SecurityUtils.Decrypt(packet.Data);
                    packet.Data = await SecurityUtils.Decompress(Convert.FromBase64String(decryptedData));
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
