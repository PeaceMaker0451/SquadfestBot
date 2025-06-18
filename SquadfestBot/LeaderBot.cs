using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace SquadfestBot
{
    public class LeaderBot
    {
        public string Id { get; }
        public string Token { get; }

        public readonly DiscordClient _client;
        private readonly SlashCommandsExtension _slash;

        public event Func<LeaderBot, InteractionCreateEventArgs, Task>? CommandReceived;

        private string DataPath => $"data/{Id}/";
        public LeaderBot(string id, string token)
        {
            Id = id;
            Token = token;

            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            _client.Ready += (s, e) =>
            {
                Console.WriteLine($"[{Id}] Ready.");
                return Task.CompletedTask;
            };

            _client.InteractionCreated += async (s, e) =>
            {
                if (CommandReceived != null)
                    await CommandReceived.Invoke(this, e);
            };

            _slash = _client.UseSlashCommands();

            EnsureFilesExist();
        }

        public async Task StartAsync()
        {
            await _client.ConnectAsync();

            await _client.BulkOverwriteGlobalApplicationCommandsAsync(GameCommandHandler.Commands.ToArray());
        }

        private void EnsureFilesExist()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);

            if (!File.Exists(DataPath + "Leader.json"))
            {
                var save = new BotSaveData
                {
                    NotInThisTeamMessage = "Ты не в этой команде.",
                    PingMessage = "Пинг от лидера!"
                };
                SaveData = save;
            }

            if (!File.Exists(DataPath + "Players.json"))
            {
                var emptyPlayers = new List<PlayerData>();
                Players = emptyPlayers;
            }
        }

        public BotSaveData SaveData
        {
            get
            {
                var json = File.ReadAllText(DataPath + "Leader.json");
                return JsonSerializer.Deserialize<BotSaveData>(json) ?? new BotSaveData();
            }
            set
            {
                var json = JsonSerializer.Serialize(value, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText(DataPath + "Leader.json", json);
            }
        }

        public List<PlayerData> Players
        {
            get
            {
                var json = File.ReadAllText(DataPath + "Players.json");
                return JsonSerializer.Deserialize<List<PlayerData>>(json) ?? new List<PlayerData>();
            }
            set
            {
                SavePlayers(value);
            }
        }

        public void SavePlayers(List<PlayerData> players)
        {
            Console.WriteLine($"Сохранение Players.json для {Id}... ({players.Count})");
            var json = JsonSerializer.Serialize(players, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(DataPath + "Players.json", json);
            Console.WriteLine($"{json}");
        }
    }
}
