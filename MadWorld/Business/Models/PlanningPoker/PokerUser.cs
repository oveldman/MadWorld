using System;
namespace Business.Models.PlanningPoker
{
    public class PokerUser
    {
        public int Id { get; set; }
        public Guid ClientSession { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}
