﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class GlobalGameState
    {
        public ulong GuildId { get; set; }
        public ulong AdminChannelId { get; set; }
        public ulong QuestCheckChannelId { get; set; }

        public DateTime QuestStartDate { get; set; } = new DateTime(2025, 7, 1);
    }
}
