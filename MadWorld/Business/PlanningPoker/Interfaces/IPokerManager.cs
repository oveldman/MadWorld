using System;
using System.Collections.Generic;
using Business.Models.PlanningPoker;

namespace Business.PlanningPoker.Interfaces
{
    public interface IPokerManager
    {
        bool CreateOrAddToRoom(string roomname, string connectionID, string username);
        List<PokerUser> GetUsersFromRoom(string roomname);
        string RemoveUserFromRoom(string connectionID);
    }
}
