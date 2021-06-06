using System;
using System.Collections.Generic;

namespace Business.Models.PlanningPoker
{
    public class PokerRoom
    {
        public string Name { get; set; }
        public List<PokerUser> Users { get; set; } = new();
    }
}
