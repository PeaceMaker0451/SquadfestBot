using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class Quest
    {
        public string Title { get; set; } = "*Заголовок задания*";
        public string Description { get; set; } = "Описание задания.";
        public string HelpText { get; set; } = "Текст, который может юыть выведен, как подсказка к заданию.";
        public string Explanation { get; set; } = "Admin-Only объяснение того, как должно выглядеть выполненное задание";

        public int Reward { get; set; } = 100;
        public int ComplitionLimit { get; set; } = 1;

        public Dictionary<string, int> ComplitionsByPlayers { get; set; } = new Dictionary<string, int>();

        public int Complitions
        {
            get
            {
                int _maxComplitions = 0;
                
                if(GlobalLimit)
                {
                    foreach(var item in ComplitionsByPlayers)
                    {
                        _maxComplitions += item.Value;
                    }
                }
                else
                {
                    foreach (var item in ComplitionsByPlayers)
                    {
                        if (item.Value > _maxComplitions)
                            _maxComplitions = item.Value;
                    }
                }

                return _maxComplitions;
            }
        }

        public List<ulong> RewardedPlayers { get; set; } = new List<ulong>();

        public bool Active { get; set; } = true;
        public bool GlobalLimit { get; set; } = false;
        public bool HasHelpText { get; set; } = false;

        public void NoteComplition(ulong PlayerID)
        {
            RewardedPlayers.Add(PlayerID);
        }

    }
}
