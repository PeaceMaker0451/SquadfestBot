using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class LeaderBotManager : IDisposable
    {
        public Dictionary<string,LeaderBot> Bots { get; private set; }

        private CancellationTokenSource _questUpdateLoopCts;
        public GlobalGameState GlobalState
        {
            get
            {
                var path = "data/GlobalState.json";
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    var state = JsonSerializer.Deserialize<GlobalGameState>(json) ?? new GlobalGameState();
                    SaveGlobalState(state);
                    return state;


                }
                else
                {
                    Console.WriteLine("Создаю GlobalState.json");
                    var state = new GlobalGameState();
                    SaveGlobalState(state);
                    return state;
                }
            }
            set => SaveGlobalState(value);
        }

        public LeaderBotManager()
        {
            Bots = LoadBots();
            GlobalState = GlobalState;

            Program.ServiceModeOn += () => StopQuestUpdateLoop();
        }

        public async Task StartAllAsync()
        {
            foreach (var bot in Bots.Values)
            {
                bot.CommandReceived += OnCommandReceived;
                bot.ComponentReceived += OnComponentReceived;
                await bot.StartAsync();
            }
        }

        private async Task OnCommandReceived(LeaderBot bot, DSharpPlus.EventArgs.InteractionCreateEventArgs e)
        {
            await GameCommandHandler.Handle(bot, e, GlobalState);
        }

        private async Task OnComponentReceived(LeaderBot bot, DSharpPlus.EventArgs.InteractionCreateEventArgs e)
        {
            await GameCommandHandler.HandleComponent(bot, e, GlobalState);
        }

        private Dictionary<string, LeaderBot> LoadBots()
        {
            const string configPath = "BotsConfig.json";

            if (!File.Exists(configPath))
            {
                var sample = new List<BotConfig>
            {
                new BotConfig { Id = "bot1", Token = "PUT_YOUR_TOKEN_HERE" },
                new BotConfig { Id = "bot2", Token = "PUT_YOUR_TOKEN_HERE" }
            };

                var json = JsonSerializer.Serialize(sample, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, json);

                Console.WriteLine("BotsConfig.json создан. Заполни токены ботов.");
                return new Dictionary<string, LeaderBot>();
            }
            else
            {
                var json = File.ReadAllText(configPath);
                var configs = JsonSerializer.Deserialize<List<BotConfig>>(json) ?? new List<BotConfig>();

                var bots = new Dictionary<string, LeaderBot>();

                foreach (var cfg in configs)
                {
                    if (string.IsNullOrWhiteSpace(cfg.Token) || cfg.Token == "PUT_YOUR_TOKEN_HERE")
                    {
                        Console.WriteLine($"Токен для {cfg.Id} не задан. Пропускаю.");
                        continue;
                    }

                    var bot = new LeaderBot(cfg.Id, cfg.Token);
                    bots.Add(cfg.Id, bot);
                }

                return bots;
            }
        }

        private void SaveGlobalState(GlobalGameState state)
        {
            var path = "data/GlobalState.json";
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");

            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public string? GetPlayerLeader(ulong playerID)
        {
            foreach (var bot in Bots.Values)
            {
                var players = bot.Players;
                if (players.Any(p => p.Id == playerID))
                {
                    return bot.Id;
                }
            }

            return null;
        }

        public async Task SendAdminMessage(string text)
        {
            try
            {
                var bot = Bots.First().Value;
                var channel = await bot._client.GetChannelAsync(GlobalState.AdminChannelId);
                if (channel != null)
                {
                    await channel.SendMessageAsync(text);
                }
                else
                {
                    Console.WriteLine("AdminChannel не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке AdminMessage: {ex.Message}");
            }
        }

        public async Task SendQuestCheckMessage(LeaderBot bot, string text)
        {
            try
            {
                var channel = await bot._client.GetChannelAsync(GlobalState.QuestCheckChannelId);
                if (channel != null)
                {
                    await channel.SendMessageAsync(text);
                }
                else
                {
                    Console.WriteLine("QuestCheckChannel не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке QuestCheckMessage: {ex.Message}");
            }
        }

        public void StartQuestUpdateLoop(TimeSpan interval)
        {
            if (_questUpdateLoopCts != null)
            {
                Console.WriteLine("⚠️ Quest update loop already running!");
                return;
            }

            _questUpdateLoopCts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                Console.WriteLine($"Starting Quest Update Loop, interval = {interval}");

                while (!_questUpdateLoopCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        await UpdateTeamsQuests();

                        Console.WriteLine($"Quest update completed at {DateTime.Now}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in quest update loop: {ex}");
                    }

                    // Ждём нужный интервал
                    await Task.Delay(interval, _questUpdateLoopCts.Token);
                }

                Console.WriteLine("🛑 Quest update loop stopped.");

            }, _questUpdateLoopCts.Token);
        }

        public async Task UpdateTeamsQuests()
        {
            foreach (var bot in Bots.Values)
            {
                await GameCommandHandler.UpdateAllQuestsStates(bot);
            }
        }

        public void StopQuestUpdateLoop()
        {
            if (_questUpdateLoopCts != null)
            {
                _questUpdateLoopCts.Cancel();
                _questUpdateLoopCts = null;
            }
        }

        public void Dispose()
        {
            _questUpdateLoopCts?.Cancel();
            _questUpdateLoopCts?.Dispose();

            foreach (var bot in Bots.Values)
            {
                bot.Dispose();
            }

            Bots.Clear();

            Console.WriteLine("LeaderBotManager успешно Dispose.");
        }

        public string GetGlobalStateJson()
        {
            return JsonSerializer.Serialize(GlobalState, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        public Exception? SetGlobalStateFromJson(string json)
        {
            try
            {
                var state = JsonSerializer.Deserialize<GlobalGameState>(json);
                if (state == null)
                    return new Exception("Результат десериализации — null");

                GlobalState = state;
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

    public class BotConfig
    {
        public string Id { get; set; } = "";
        public string Token { get; set; } = "";
    }
}
