using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UnityMagicNet
{
    public class Room
    {
        public string RoomId { get; private set; }
        private List<User> users;

        public Room(string roomId)
        {
            RoomId = roomId;
            users = new List<User>();
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public void Broadcast(NetDataWriter writer, DeliveryMethod deliveryMethod)
        {
            foreach (var user in users)
            {
                user.Peer.Send(writer, deliveryMethod);
            }
        }
    }
}