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

        public async Task JoinRoom(string roomName, string username, Guid currentSession)
        {
            if (string.IsNullOrEmpty(roomName) || string.IsNullOrEmpty(username)) return;

            var connectionIdUser = Context.ConnectionId;
            _pokerManager.CreateOrAddToRoom(roomName, connectionIdUser, username, currentSession);
            await Groups.AddToGroupAsync(connectionIdUser, roomName);
            await RoomMembersChanged(roomName);
        }

        public async Task SetCard(string username, PokerCardTypes cardValue)
        {
            string connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.GetRoomName(connectionIdUser);
            PokerUser pokerUser = _pokerManager.GetUser(connectionIdUser);

            PokerCard card = new PokerCard
            {
                MemberID = pokerUser.Id,
                CardValue = cardValue
            };

            if (string.IsNullOrEmpty(roomname) || string.IsNullOrEmpty(pokerUser?.ConnectionId)) return;
            await Clients.Group(roomname).SendAsync("SetCard", card);
        }

        public async Task RevealCards(string username)
        {
            string connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.GetRoomName(connectionIdUser);

            if (string.IsNullOrEmpty(roomname)) return;
            await Clients.Group(roomname).SendAsync("RevealCards", true);
        }

        public async Task ResetCards(string username)
        {
            string connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.GetRoomName(connectionIdUser);

            if (string.IsNullOrEmpty(roomname)) return;
            await Clients.Group(roomname).SendAsync("ResetCards", true);
        }

        private async Task RoomMembersChanged(string roomname)
        {
            if (string.IsNullOrEmpty(roomname)) return;

            List<PokerUser> users = _pokerManager.GetUsersFromRoom(roomname);
            List<PokerMember> members = users.Select(u => new PokerMember
            {
                ID = u.Id,
                Username = u.Name,
                SessionID = u.ClientSession,
                RoomName = roomname
            }).ToList();

            await Clients.Group(roomname).SendAsync("MembersChanged", members);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionIdUser = Context.ConnectionId;
            string roomname = _pokerManager.RemoveUserFromRoom(connectionIdUser);

            if (!string.IsNullOrEmpty(roomname)) {
                await RoomMembersChanged(roomname);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
