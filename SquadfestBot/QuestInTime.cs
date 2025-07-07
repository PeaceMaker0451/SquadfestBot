using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public enum QuestType
    {
        Daily,
        Weekly,
        Hard,
        Secret
    }

    public class QuestInTime
    {
        public Quest Quest { get; set; }
        public QuestType QuestType { get; set; }
        public DateTime AvalableLimit { get; set; }
        public int Index { get; set; } // глобальный индекс
        public int[] BaseIndex { get; set; }
    }
}
