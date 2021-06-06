using System;
using System.Linq;
using Business.Models.PlanningPoker;
using Business.PlanningPoker.Interfaces;

namespace Business.PlanningPoker
{
    public class PokerManager : IPokerManager
    {
        private PokerSession Session;

        public PokerManager(PokerSession session)
        {
            Session = session;
        }

        public bool CreateOrAddToRoom(string roomname, string connectionID, string username)
        {
            if (string.IsNullOrEmpty(roomname) || string.IsNullOrEmpty(connectionID) || string.IsNullOrEmpty(username))
            {
                return false;
            }

            PokerRoom room = GetRoom(roomname);
            AddUser(room, connectionID, username);

            return true;
        }

        private PokerRoom GetRoom(string roomname)
        {
            if (Session.Rooms.Any(r => r.Name.Equals(roomname)))
            {
                return Session.Rooms.First(r => r.Name.Equals(roomname));
            }

            PokerRoom newRoom = new()
            {
                Name = roomname
            };

            Session.Rooms.Add(newRoom);
            return newRoom;
        }

        private void AddUser(PokerRoom room, string connectionID, string username)
        {
            room.Users.Add(new PokerUser {
                Id = room.Users.Count + 1,
                ConnectionId = connectionID,
                Name = username
            });
        }
    }
}
