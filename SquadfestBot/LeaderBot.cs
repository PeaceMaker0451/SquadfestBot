using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SquadfestBot
{
    public class LeaderBot : IDisposable
    {
        public string Id { get; }
        public string Token { get; }

        public readonly DiscordClient _client;
        private readonly SlashCommandsExtension _slash;

        public event Func<LeaderBot, InteractionCreateEventArgs, Task>? CommandReceived;
        public event Func<LeaderBot, DSharpPlus.EventArgs.InteractionCreateEventArgs, Task>? ComponentReceived;

        private string DataPath => $"data/{Id}/";
        public LeaderBot(string id, string token)
        {
            Id = id;
            Token = token;

            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.GuildMessages
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

            _client.ComponentInteractionCreated += async (s, e) =>
            {
                if (ComponentReceived != null)
                    await ComponentReceived.Invoke(this, e);
            };


            _slash = _client.UseSlashCommands();

            EnsureFilesExist();
        }

        public async Task StartAsync()
        {
            await _client.ConnectAsync();

            _client.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = DSharpPlus.Interactivity.Enums.PaginationBehaviour.WrapAround,
                Timeout = TimeSpan.FromMinutes(5)
            });

            await GameCommandHandler.EnsureQuestThreads(this);

            try
            {
                await _client.BulkOverwriteGuildApplicationCommandsAsync(Program.BotManager.GlobalState.GuildId, GameCommandHandler.Commands.ToArray());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Невозможно обновить команды для гилдьии - {ex}");
            }
            
        }

        private void EnsureFilesExist()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);

            if (!File.Exists(DataPath + "Leader.json"))
            {
                var save = new BotSaveData();
                SaveData = save;
            }

            if (!File.Exists(DataPath + "Personality.json"))
            {
                var save = new LeaderPersonality();
                Personality = save;
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

        public LeaderPersonality Personality
        {
            get
            {
                var json = File.ReadAllText(DataPath + "Personality.json");
                return JsonSerializer.Deserialize<LeaderPersonality>(json) ?? new LeaderPersonality();
            }
            set
            {
                var json = JsonSerializer.Serialize(value, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText(DataPath + "Personality.json", json);
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

        public void AddPoints(ulong playerId, int points)
        {
            var newSaveData = SaveData;
            newSaveData.TeamScore += points;
            SaveData = newSaveData;

            var players = Players;
            players.Find(p => p.Id == playerId).PersonalScore += points;
            SavePlayers(players);
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

        public void Dispose()
        {
            _client?.DisconnectAsync().GetAwaiter().GetResult();
            _client?.Dispose();

            Console.WriteLine($"LeaderBot {_client?.CurrentUser?.Username} успешно Dispose.");
        }

        public string GetSaveDataJson() => SerializeToJson(SaveData);
        public string GetPersonalityJson() => SerializeToJson(Personality);
        public string GetPlayersJson() => SerializeToJson(Players);

        public Exception? SetSaveDataFromJson(string json)
        {
            var error = TryDeserialize<BotSaveData>(json, out var data);
            if (error != null) return error;
            SaveData = data!;
            return null;
        }

        public Exception? SetPersonalityFromJson(string json)
        {
            var error = TryDeserialize<LeaderPersonality>(json, out var data);
            if (error != null) return error;
            Personality = data!;
            return null;
        }

        public Exception? SetPlayersFromJson(string json)
        {
            var error = TryDeserialize<List<PlayerData>>(json, out var data);
            if (error != null) return error;
            Players = data!;
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

        private Exception? TryDeserialize<T>(string json, out T? result)
        {
            try
            {
                result = JsonSerializer.Deserialize<T>(json);
                if (result == null)
                    throw new Exception("Результат десериализации — null");
                return null;
            }
            catch (Exception ex)
            {
                result = default;
                return ex;
            }
        }
    }
}
