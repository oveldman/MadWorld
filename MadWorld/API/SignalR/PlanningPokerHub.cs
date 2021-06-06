using System;
using System.Threading.Tasks;
using Business.Models.PlanningPoker;
using Business.PlanningPoker.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace API.SignalR
{
    public class PlanningPokerHub : Hub
    {
        private PokerSession Session;
        private IPokerManager _pokerManager;

        public PlanningPokerHub(PokerSession session, IPokerManager pokerManager)
        {
            Session = session;
            _pokerManager = pokerManager;
        }

        public async Task JoinRoom(string roomName, string username)
        {
            var connectionIdUser = Context.ConnectionId;
            _pokerManager.CreateOrAddToRoom(roomName, connectionIdUser, username);
            await Groups.AddToGroupAsync(connectionIdUser, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", username, $"joined the room of {roomName}");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
