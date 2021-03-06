using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.PlanningPoker
{
    public class PokerAccount
    {
        [Required]
        public string RoomName { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
