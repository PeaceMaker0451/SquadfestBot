using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class LeaderPersonality
    {
        public string TeamName { get; set; } = "Солнышки";
        public string AlreadyInOtherCommandMessage { get; set; } = "Уже в другой команде.";
        public string AlreadyInThisCommandMessage { get; set; } = "Уже в этой команде.";
        public string RemovedFromTheTeamDMMessage { get; set; } = "Ты больше не в нашей команде.";
        public string AddedToTheTeamDMMessage { get; set; } = "Добро пожаловать в нашу команду!";
        public string TriedToAddBotInTheTeamMessage { get; set; } = "Он не может быть с нами.";

        public List<string> NotInThisTeamMessages { get; set; } = new List<string>
        {
            "## Ты не в этой команде",
            "## Так не пойдет."
        };

        public List<string> PingMessages { get; set; } = new List<string>
        {
            "Пинг-понг!",
            "Привет!"
        };

        public List<string> QuestListPhrases { get; set; } = new List<string>
        {
            "## Ну-ка, что там у нас сегодня?",
            "## Работаем."
        };

        public List<string> QuestAcceptedDMPhrases { get; set; } = new List<string>
        {
            "Отлично сработано!",
            "Так держать!"
        };

        public List<string> QuestRejectedDMPhrases { get; set; } = new List<string>
        {
            "Тебе стоит стараться лучше.",
            "Что-то определенно неправильно."
        };

        public List<string> QuestAlreadyCompletedDMPhrases { get; set; } = new List<string>
        {
            "К сожалению, не получилось.",
            "Упс."
        };

        public List<string> QuestCompletedGlobalPhrases { get; set; } = new List<string>
        {
            "Вот это да, команда!",
            "Хороши, команда!"
        };

        public List<string> QuestSnatchedGlobalPhrases { get; set; } = new List<string>
        {
            "Художественный фильм - спиздили!",
            "а мы?!"
        };

        public List<string> QuestPresentGlobalPhrases { get; set; } = new List<string>
        {
            "Новое задание!",
            "Автоботы! Сбор!"
        };

        public List<string> QuestExpiredGlobalPhrases { get; set; } = new List<string>
        {
            "Ребята, мы не успели",
            "Недоработали"
        };

        public List<string> BestTeamScorePhrases { get; set; } = new List<string>
        {
            "Мы рвем и мечем!",
            "Ооооооо-да!!"
        };
        public List<string> PatheticTeamScorePhrases { get; set; } = new List<string>
        {
            "М-да",
            "Нам стоит стараться лучше"
        };

        public List<string> PlayersScoreBestPhrases { get; set; } = new List<string>
        {
            "Ты сегодня в ударе!",
            "Отличная работа!"
        };

        public List<string> PlayersScoreZeroPhrases { get; set; } = new List<string>
        {
            "no points??",
            "Нет очков, нет еды"
        };

        public List<string> PlayersScoreNormalPhrases { get; set; } = new List<string>
        {
            "Главное - работа в команде",
            "Как говорил мой дед - \"ААЭАЭАЭАЭАЭАЭАААА!!!!\""
        };

        public List<string> QuestCheckSendedPhrases { get; set; } = new List<string>
        {
            "Спасибо за сотрудничество!",
            "Мы сейчас подтвердим (чистая формальность) и гоооол!!"
        };

        public Dictionary<QuestType, string> QuestTypeTitles { get; set; } = new()
        {
            { QuestType.Daily, "📅 ЕЖЕДНЕВНЫЕ" },
            { QuestType.Weekly, "📆 ЕЖЕНЕДЕЛЬНЫЕ" },
            { QuestType.Hard, "🛠️ СЛОЖНЫЕ" },
            { QuestType.Secret, "❓ СЕКРЕТНЫЕ" }
        };
        public Dictionary<QuestType, string> QuestTypeDescriptions { get; set; } = new()
        {
            { QuestType.Daily, "" },
            { QuestType.Weekly, "" },
            { QuestType.Hard, "" },
            { QuestType.Secret, "" }
        };
    }
}
