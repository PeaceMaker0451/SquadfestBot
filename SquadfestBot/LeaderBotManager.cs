using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class LeaderBotManager
    {
        public List<LeaderBot> Bots { get; private set; }
        public GlobalGameState GlobalState
        {
            get
            {
                var path = "data/GlobalState.json";
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<GlobalGameState>(json) ?? new GlobalGameState();
                }
                else
                {
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
        }

        public async Task StartAllAsync()
        {
            foreach (var bot in Bots)
            {
                bot.CommandReceived += OnCommandReceived;
                await bot.StartAsync();
            }
        }

        private async Task OnCommandReceived(LeaderBot bot, DSharpPlus.EventArgs.InteractionCreateEventArgs e)
        {
            await GameCommandHandler.Handle(bot, e, GlobalState);
        }

        private List<LeaderBot> LoadBots()
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
                return new List<LeaderBot>();
            }
            else
            {
                var json = File.ReadAllText(configPath);
                var configs = JsonSerializer.Deserialize<List<BotConfig>>(json) ?? new List<BotConfig>();

                var bots = new List<LeaderBot>();

                foreach (var cfg in configs)
                {
                    if (string.IsNullOrWhiteSpace(cfg.Token) || cfg.Token == "PUT_YOUR_TOKEN_HERE")
                    {
                        Console.WriteLine($"Токен для {cfg.Id} не задан. Пропускаю.");
                        continue;
                    }

                    var bot = new LeaderBot(cfg.Id, cfg.Token);
                    bots.Add(bot);
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
    }

    public class BotConfig
    {
        public string Id { get; set; } = "";
        public string Token { get; set; } = "";
    }
}
