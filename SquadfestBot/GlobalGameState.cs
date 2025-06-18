using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class GlobalGameState
    {
        public ulong GuildId { get; set; }
        public ulong GlobalChannelId { get; set; }
        public ulong AdminChannelId { get; set; }
        public ulong QuestCheckChannelId { get; set; }
        public int SomeGlobalScore { get; set; }
    }
}
