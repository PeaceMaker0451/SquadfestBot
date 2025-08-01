using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class QuestManager
    {
        private readonly string questFolder = "data/quests/";

        public Dictionary<int, List<Quest>> DailyQuests { get; set; } = new();
        public Dictionary<int, List<Quest>> WeeklyQuests { get; set; } = new();
        public List<Quest> HardQuests { get; set; } = new();
        public List<Quest> SecretQuests { get; set; } = new();

        public DateTime StartDate
        {
            get => Program.BotManager.GlobalState.QuestStartDate;
            set
            {
                Program.BotManager.GlobalState.QuestStartDate = value;
                Program.BotManager.GlobalState = Program.BotManager.GlobalState; // сохраняем
            }
        }

        public QuestManager()
        {
            if (!Directory.Exists(questFolder))
                Directory.CreateDirectory(questFolder);

            var currentDay = (DateTime.UtcNow - StartDate).Days;
            var currentWeek = (currentDay) / 7;

            LoadQuests();
        }

        public void LoadQuests()
        {
            DailyQuests = Load<Dictionary<int, List<Quest>>>("daily.json");
            WeeklyQuests = Load<Dictionary<int, List<Quest>>>("weekly.json");
            HardQuests = Load<List<Quest>>("hard.json");
            SecretQuests = Load<List<Quest>>("secret.json");
        }

        public void SaveQuests()
        {
            Save("daily.json", DailyQuests);
            Save("weekly.json", WeeklyQuests);
            Save("hard.json", HardQuests);
            Save("secret.json", SecretQuests);
        }

        public List<QuestInTime> GetActiveQuests(int dayOffset = 0, string team = "global")
        {
            LoadQuests();

            var active = new List<QuestInTime>();

            if (DateTime.UtcNow < StartDate)
                return active;
            
            var currentDay = (DateTime.UtcNow - StartDate).Days + dayOffset;
            var currentWeek = (currentDay) / 7;

            int globalIndex = 0;

            if (DailyQuests.TryGetValue(currentDay, out var daily))
            {
                int i = 0;
                foreach(var q in daily)
                {
                    active.Add(new QuestInTime
                    {
                        Quest = q,
                        QuestType = QuestType.Daily,
                        AvalableLimit = StartDate.AddDays(currentDay + 1),
                        Index = globalIndex++,
                        BaseIndex = new int[2] {currentDay, i}
                    });
                    i++;
                }
                
                
            }

            if (WeeklyQuests.TryGetValue(currentWeek, out var weekly))
            {
                int i = 0;
                foreach (var q in weekly)
                {
                    active.Add(new QuestInTime
                    {
                        Quest = q,
                        QuestType = QuestType.Weekly,
                        AvalableLimit = StartDate.AddDays((currentWeek + 1) * 7),
                        Index = globalIndex++,
                        BaseIndex = new int[2] { currentDay / 7, i }
                    });
                    i++;
                }
            }

            foreach (var q in HardQuests)
            {
                active.Add(new QuestInTime
                {
                    Quest = q,
                    QuestType = QuestType.Hard,
                    AvalableLimit = DateTime.MaxValue,
                    Index = globalIndex++,
                    BaseIndex = new int[] {HardQuests.IndexOf(q), 0}
                });
            }

            foreach (var q in SecretQuests)
            {
                active.Add(new QuestInTime
                {
                    Quest = q,
                    QuestType = QuestType.Secret,
                    AvalableLimit = DateTime.MaxValue,
                    Index = globalIndex++,
                    BaseIndex = new int[] { SecretQuests.IndexOf(q), 0 }
                });
            }

            return active
                .OrderBy(q => q.QuestType)
                .ThenBy(q => q.AvalableLimit)
                .ToList();
        }

        public void AddDailyQuest(int day, Quest quest)
        {
            LoadQuests();

            if (!DailyQuests.ContainsKey(day))
                DailyQuests.Add(day, new List<Quest>());

            DailyQuests[day].Add(quest);

            SaveQuests();
        }

        public void AddWeeklyQuest(int week, Quest quest)
        {
            LoadQuests();

            if (!WeeklyQuests.ContainsKey(week))
                WeeklyQuests.Add(week, new List<Quest>());

            WeeklyQuests[week].Add(quest);

            SaveQuests();
        }

        public void AddHardQuest(Quest quest)
        {
            LoadQuests();
            HardQuests.Add(quest);
            SaveQuests();
        }

        public void AddSecretQuest(Quest quest)
        {
            LoadQuests();
            SecretQuests.Add(quest);
            SaveQuests();
        }

        public void EditDailyQuest(int day, int index, string? title = null, string? desc = null, string? help = null, string? expl = null, int? reward = null, int? limit = null, bool? global = null)
        {
            LoadQuests();
            var quest = DailyQuests[day][index];
            if (title != null) quest.Title = title;
            if (desc != null) quest.Description = desc;
            if (help != null) quest.HelpText = help;
            if (expl != null) quest.Explanation = expl;
            if (reward != null) quest.Reward = reward.Value;
            if (limit != null) quest.CompletionLimit = limit.Value;
            if (global != null) quest.GlobalLimit = global.Value;
            SaveQuests();
        }

        public void EditWeeklyQuest(int week, int index, string? title = null, string? desc = null, string? help = null, string? expl = null, int? reward = null, int? limit = null, bool? global = null)
        {
            LoadQuests();
            var quest = WeeklyQuests[week][index];
            if (title != null) quest.Title = title;
            if (desc != null) quest.Description = desc;
            if (help != null) quest.HelpText = help;
            if (expl != null) quest.Explanation = expl;
            if (reward != null) quest.Reward = reward.Value;
            if (limit != null) quest.CompletionLimit = limit.Value;
            if (global != null) quest.GlobalLimit = global.Value;
            SaveQuests();
        }

        public void EditHardQuest(int index, string? title = null, string? desc = null, string? help = null, string? expl = null, int? reward = null, int? limit = null, bool? global = null)
        {
            LoadQuests();
            var quest = HardQuests[index];
            if (title != null) quest.Title = title;
            if (desc != null) quest.Description = desc;
            if (help != null) quest.HelpText = help;
            if (expl != null) quest.Explanation = expl;
            if (reward != null) quest.Reward = reward.Value;
            if (limit != null) quest.CompletionLimit = limit.Value;
            if (global != null) quest.GlobalLimit = global.Value;
            SaveQuests();
        }

        public void EditSecretQuest(int index, string? title = null, string? desc = null, string? help = null, string? expl = null, int? reward = null, int? limit = null, bool? global = null)
        {
            LoadQuests();
            var quest = SecretQuests[index];
            if (title != null) quest.Title = title;
            if (desc != null) quest.Description = desc;
            if (help != null) quest.HelpText = help;
            if (expl != null) quest.Explanation = expl;
            if (reward != null) quest.Reward = reward.Value;
            if (limit != null) quest.CompletionLimit = limit.Value;
            if (global != null) quest.GlobalLimit = global.Value;
            SaveQuests();
        }

        public void RemoveDailyQuest(int day, int index)
        {
            LoadQuests();
            DailyQuests[day].RemoveAt(index);
            SaveQuests();
        }

        public void RemoveWeeklyQuest(int week, int index)
        {
            LoadQuests();
            WeeklyQuests[week].RemoveAt(index);
            SaveQuests();
        }

        public void RemoveHardQuest(int index)
        {
            LoadQuests();
            HardQuests.RemoveAt(index);
            SaveQuests();
        }

        public void RemoveSecretQuest(int index)
        {
            LoadQuests();
            SecretQuests.RemoveAt(index);
            SaveQuests();
        }

        public Quest GetOriginalQuest(QuestInTime questInTime)
        {
            return questInTime.QuestType switch
            {
                QuestType.Daily => DailyQuests[questInTime.BaseIndex[0]][questInTime.BaseIndex[1]],
                QuestType.Weekly => WeeklyQuests[questInTime.BaseIndex[0]][questInTime.BaseIndex[1]],
                QuestType.Hard => HardQuests[questInTime.BaseIndex[0]],
                QuestType.Secret => SecretQuests[questInTime.BaseIndex[0]],
                _ => throw new InvalidOperationException("Unknown quest type")
            };
        }

        public string GetDailyQuestsJson() => SerializeToJson(DailyQuests);
        public string GetWeeklyQuestsJson() => SerializeToJson(WeeklyQuests);
        public string GetHardQuestsJson() => SerializeToJson(HardQuests);
        public string GetSecretQuestsJson() => SerializeToJson(SecretQuests);

        public Exception? SetDailyQuestsFromJson(string json)
        {
            var error = TryDeserialize<Dictionary<int, List<Quest>>>(json, out var data);
            if (error != null) return error;
            DailyQuests = data!;
            SaveQuests();
            return null;
        }

        public Exception? SetWeeklyQuestsFromJson(string json)
        {
            var error = TryDeserialize<Dictionary<int, List<Quest>>>(json, out var data);
            if (error != null) return error;
            WeeklyQuests = data!;
            SaveQuests();
            return null;
        }

        public Exception? SetHardQuestsFromJson(string json)
        {
            var error = TryDeserialize<List<Quest>>(json, out var data);
            if (error != null) return error;
            HardQuests = data!;
            SaveQuests();
            return null;
        }

        public Exception? SetSecretQuestsFromJson(string json)
        {
            var error = TryDeserialize<List<Quest>>(json, out var data);
            if (error != null) return error;
            SecretQuests = data!;
            SaveQuests();
            return null;
        }

        private string SerializeToJson<T>(T data)
        {
            return JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private Exception? TryDeserialize<T>(string json)
        {
            try
            {
                _ = JsonSerializer.Deserialize<T>(json);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private Exception? TryDeserialize<T>(string json, out T? data)
        {
            try
            {
                data = JsonSerializer.Deserialize<T>(json);
                if (data == null)
                    throw new Exception("Результат десериализации null");
                return null;
            }
            catch (Exception ex)
            {
                data = default;
                return ex;
            }
        }

        private T Load<T>(string filename) where T : new()
        {
            var path = Path.Combine(questFolder, filename);

            if (!File.Exists(path))
            {
                Save(filename, new T());
                Console.WriteLine($"Создан пустой файл {filename}");
                return new T();
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }) ?? new T();
        }

        private void Save<T>(string filename, T data)
        {
            var path = Path.Combine(questFolder, filename);
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(path, json);
        }
    }
}
