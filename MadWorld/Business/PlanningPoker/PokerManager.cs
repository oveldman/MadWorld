﻿using System;
using System.Collections.Generic;
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

        public string GetRoomName(string connectionID)
        {
            PokerRoom room = FindRoomByUserID(connectionID);
            return room != null ? room.Name : string.Empty;
        }

        public PokerUser GetUser(string connectionID)
        {
            PokerRoom room = FindRoomByUserID(connectionID);
            PokerUser user = room.Users.FirstOrDefault(u => u.ConnectionId.Equals(connectionID));
            return user ?? new PokerUser();
        }

        public List<PokerUser> GetUsersFromRoom(string roomname)
        {
            PokerRoom room = Session.Rooms.FirstOrDefault(r => r.Name.Equals(roomname));
            return room != null ? room.Users : new List<PokerUser>();
        }

        public string RemoveUserFromRoom(string connectionID)
        {
            if (string.IsNullOrEmpty(connectionID))
            {
                return string.Empty;
            }

            string roomname = string.Empty;

            PokerRoom room = FindRoomByUserID(connectionID);

            if (room != null)
            {
                roomname = room.Name;
                RemoveUser(room, connectionID);
                RemoveRoomIfEmty(room);
            }

            return roomname;
        }

        private void AddUser(PokerRoom room, string connectionID, string username)
        {
            int biggestID = room.Users.Any() ? room.Users.Max(u => u.Id) : 0;

            room.Users.Add(new PokerUser
            {
                Id = biggestID + 1,
                ConnectionId = connectionID,
                Name = username
            });
        }

        private PokerRoom FindRoomByUserID(string connectionID)
        {
            return Session.Rooms.FirstOrDefault(r => r.Users.Any(u => u.ConnectionId.Equals(connectionID)));
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

        private void RemoveRoomIfEmty(PokerRoom room)
        {
            if (!room.Users.Any())
            {
                Session.Rooms.Remove(room);
            }
        }

        private void RemoveUser(PokerRoom room, string connectionID)
        {
            room.Users.Remove(room.Users.FirstOrDefault(u => u.ConnectionId.Equals(connectionID)));
        }
    }
}
