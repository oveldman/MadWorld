using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Models.PlanningPoker;
using Business.PlanningPoker.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Website.Shared.Models.PlanningPoker;

namespace API.SignalR
{
    public class PlanningPokerHub : Hub
    {
        private IPokerManager _pokerManager;

        public PlanningPokerHub(IPokerManager pokerManager)
        {
            _pokerManager = pokerManager;
        }

        public async Task JoinRoom(string roomName, string username)
        {
            var connectionIdUser = Context.ConnectionId;
            _pokerManager.CreateOrAddToRoom(roomName, connectionIdUser, username);
            await Groups.AddToGroupAsync(connectionIdUser, roomName);
            await RoomMembersChanged(roomName);
        }

        public async Task SetCard(string username, double cardValue)
        {
            string connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.GetRoomName(connectionIdUser);
            PokerUser pokerUser = _pokerManager.GetUser(connectionIdUser);

            PokerCard card = new PokerCard
            {
                MemberID = pokerUser.Id,
                CardValue = cardValue
            };

            await Clients.Group(roomname).SendAsync("SetCard", card);
        }

        public async Task ResetCards(string username)
        {
            string connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.GetRoomName(connectionIdUser);
            await Clients.Group(roomname).SendAsync("ResetCards", true);
        }

        private async Task RoomMembersChanged(string roomname)
        {
            List<PokerUser> users = _pokerManager.GetUsersFromRoom(roomname);
            List<PokerMember> members = users.Select(u => new PokerMember
            {
                ID = u.Id,
                Username = u.Name,
                RoomName = roomname
            }).ToList();

            await Clients.Group(roomname).SendAsync("MembersChanged", members);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.RemoveUserFromRoom(connectionIdUser);
            await RoomMembersChanged(roomname);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
