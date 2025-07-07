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
        public int CompletionLimit { get; set; } = 1;

        public Dictionary<string, int> CompletionsByTeams { get; set; } = new Dictionary<string, int>();

        public int Completions
        {
            get
            {
                int _Complitions = 0;

                foreach (var item in CompletionsByTeams)
                {
                    _Complitions += item.Value;
                }

                return _Complitions;
            }
        }

        public List<ulong> CheckedPlayers { get; set; } = new List<ulong>();
        public List<ulong> CompletedPlayers { get; set; } = new List<ulong>();
        public List<KeyValuePair<string,ulong>> ActiveConfirmMessages { get; set; } = new List<KeyValuePair<string, ulong>>();
        public bool GlobalLimit { get; set; } = false;
        public Dictionary<string, bool> PresentedByTeams { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> CompletedByTeams { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, bool> ExpiredByTeams { get; set; } = new Dictionary<string, bool>();


        public void NoteCheck(ulong PlayerID)
        {
            if (CheckedPlayers.Contains(PlayerID) || CompletedPlayers.Contains(PlayerID))
                throw new InvalidOperationException("Quest was already checked/completed by this user");

            CheckedPlayers.Add(PlayerID);
        }

        public void RemoveCheck(ulong PlayerID)
        {
            if (!CheckedPlayers.Contains(PlayerID))
                throw new InvalidOperationException("Quest wasn't checked by this user");

            CheckedPlayers.Remove(PlayerID);
        }
        
        public void NoteCompletion(string team, ulong PlayerID)
        {
            if (CompletedPlayers.Contains(PlayerID))
                throw new InvalidOperationException("Quest was already completed by this user");

            RemoveCheck(PlayerID);

            if (!CompletionsByTeams.ContainsKey(team))
                CompletionsByTeams.Add(team, 0);

            CompletionsByTeams[team]++;

            CompletedPlayers.Add(PlayerID);
        }

        public bool LimitReached(string team)
        {
            if(GlobalLimit)
            {
                if (Completions >= CompletionLimit)
                {
                    return true;
                }
                else
                { return false; }
            }
            else
            {
                if (CompletionsByTeams.ContainsKey(team) && CompletionsByTeams[team] >= CompletionLimit)
                {
                    return true;
                }
                else
                { return false; }
            }
        }
    }

    //public class ConfirmMessageData
    //{
    //    public ulong MessageId { get; set; }
    //    public string TeamId { get; set; }

    //    public ConfirmMessageData() { } // вот он — пустой

    //    public ConfirmMessageData(ulong messageId, string teamId)
    //    {
    //        MessageId = messageId;
    //        TeamId = teamId;
    //    }
    //}
}
