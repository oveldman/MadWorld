using System;
using System.Collections.Generic;

namespace Business.Models.PlanningPoker
{
    public class PokerSession
    {
        public List<PokerRoom> Rooms { get; set; } = new();
    }
}
