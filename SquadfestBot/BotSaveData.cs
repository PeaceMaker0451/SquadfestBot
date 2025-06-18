using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class BotSaveData
    {
        public string NotInThisTeamMessage { get; set; } = "Ты не в этой команде.";
        public string PingMessage { get; set; }  = "Пинг-понг!";
        public string AlreadyInOtherCommandMessage { get; set; } = "Уже в другой команде.";
        public string AlreadyInThisCommandMessage { get; set; } = "Уже в этой команде.";
        public string RemovedFromTheTeamDMMessage { get; set; } = "Ты больше не в нашей команде.";
        public string AddedToTheTeamDMMessage { get; set; } = "Добро пожаловать в нашу команду!";
        public string TriedToAddBotInTheTeamMessage { get; set; } = "Он не может быть с нами.";
    }
}
