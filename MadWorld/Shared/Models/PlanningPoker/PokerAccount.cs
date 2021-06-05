using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Shared.Models.PlanningPoker
{
    public class PokerAccount
    {
        public int ID { get; set; }
        [Required]
        public string RoomName { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
