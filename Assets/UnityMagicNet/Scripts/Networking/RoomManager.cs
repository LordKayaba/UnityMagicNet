using System.Collections.Generic;

namespace UnityMagicNet
{
    public class RoomManager
    {
        private Dictionary<string, Room> rooms;

        public RoomManager()
        {
            rooms = new Dictionary<string, Room>();
        }

        public Room CreateRoom(string roomId)
        {
            var room = new Room(roomId);
            rooms[roomId] = room;
            return room;
        }

        public bool JoinRoom(string roomId, User user)
        {
            if (rooms.TryGetValue(roomId, out var room))
            {
                room.AddUser(user);
                return true;
            }
            return false;
        }

        public void LeaveRoom(string roomId, User user)
        {
            if (rooms.TryGetValue(roomId, out var room))
            {
                room.RemoveUser(user);
                if (room.GetUsers().Count == 0)
                {
                    rooms.Remove(roomId);
                }
            }
        }

        public Room GetRoom(string roomId)
        {
            rooms.TryGetValue(roomId, out var room);
            return room;
        }
    }
}