using System.Collections.Generic;
using LiteNetLib;

namespace UnityMagicNet
{
    public class UserManager
    {
        private Dictionary<string, User> users;

        public UserManager()
        {
            users = new Dictionary<string, User>();
        }

        public void CreateUser(NetPeer peer)
        {
            string userId = "Client_" + peer.Id;
            User user = new User(userId, peer);
            users[userId] = user;
        }

        public void RemoveUser(string userId)
        {
            users.Remove(userId);
        }

        public User GetUser(string userId)
        {
            users.TryGetValue(userId, out var user);
            return user;
        }

        public int GetCount()
        {
            return users.Count;
        }
    }
}