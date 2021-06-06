using System;
using System.Collections.Generic;
using Business.Models.PlanningPoker;

namespace Business.PlanningPoker.Interfaces
{
    public interface IPokerManager
    {
        bool CreateOrAddToRoom(string roomname, string connectionID, string username);
        List<PokerUser> GetUsersFromRoom(string roomname);
        PokerUser GetUser(string connectionID);
        string GetRoomName(string connectionID);
        string RemoveUserFromRoom(string connectionID);
    }
}
