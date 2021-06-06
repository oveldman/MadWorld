using System;
using Business.Models.PlanningPoker;

namespace Business.PlanningPoker.Interfaces
{
    public interface IPokerManager
    {
        bool CreateOrAddToRoom(string roomname, string connectionID, string username);
    }
}
