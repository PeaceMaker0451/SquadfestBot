using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class BotSaveData
    {
        public int TeamScore { get; set; }
        public ulong QuestChannelId { get; set; }
        public ulong GlobalChannelId { get; set; }
        public Dictionary<QuestType, QuestListThreadData> QuestThreads { get; set; } = new();
    }

    public class QuestListThreadData
    {
        public ulong MessageId { get; set; }
        public ulong ThreadId { get; set; }
        public List<ulong> ThreadMessagesId { get; set; } = new List<ulong>();

    }
}
