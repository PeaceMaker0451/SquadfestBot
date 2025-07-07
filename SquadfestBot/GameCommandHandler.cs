using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System.Text;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System.Text.Json;

namespace SquadfestBot
{
    public static class GameCommandHandler
    {
        public static List<DiscordApplicationCommand> Commands = new List<DiscordApplicationCommand>()
        {
            new DiscordApplicationCommand("ping", "Пингует лидера",
            name_localizations: new Dictionary<string, string>
            {
                { "ru", "пинг" }
            }),//ping
            new DiscordApplicationCommand("service-mode", "Режим обслуживания бота", defaultMemberPermissions: Permissions.Administrator),//ping
            new DiscordApplicationCommand("restart", "Жестко перезапускает бота.", defaultMemberPermissions: Permissions.Administrator),
            new DiscordApplicationCommand("quests", "Активные квесты",
            name_localizations: new Dictionary<string, string>
            {
                { "ru", "задания" }
            }),
            new DiscordApplicationCommand("score", "Счет вашей команды",
            name_localizations: new Dictionary<string, string>
            {
                { "ru", "счет" }
            }),
            new DiscordApplicationCommand(
            name: "say",
            description: "Говорить от лица Лидера",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "channel-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.Channel,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "text",
                    description: "Что сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "file",
                    description: "Файл, который приложить к сообщению",
                    type: ApplicationCommandOptionType.Attachment,
                    required: false
                )
            }), //say
            new DiscordApplicationCommand(
            name: "re-say",
            description: "Редактировать сообщение этого бота ",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "channel-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.Channel,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "message-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "text",
                    description: "Что сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "file",
                    description: "Файл, который приложить к сообщению",
                    type: ApplicationCommandOptionType.Attachment,
                    required: false
                )
            }), //re-say
            new DiscordApplicationCommand(
            name: "un-say",
            description: "Удалить сообщение этого бота ",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "channel-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.Channel,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "message-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
            }), //un-say
            new DiscordApplicationCommand(
            name: "dm",
            description: "Отправить сообщение пользователю в ЛС",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "user",
                    description: "Кому отправить",
                    type: ApplicationCommandOptionType.User,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "text",
                    description: "Что отправить",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "file",
                    description: "Файл, который приложить к сообщению",
                    type: ApplicationCommandOptionType.Attachment,
                    required: false
                )
            }), //dm
            new DiscordApplicationCommand(
            name: "re-dm",
            description: "Изменить сообщение бота в пользовательском ЛС",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "user",
                    description: "Кому отправить",
                    type: ApplicationCommandOptionType.User,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "message-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "text",
                    description: "Что отправить",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "file",
                    description: "Файл, который приложить к сообщению",
                    type: ApplicationCommandOptionType.Attachment,
                    required: false
                )
            }), //re-dm
            new DiscordApplicationCommand(
            name: "un-dm",
            description: "Удалить сообщение бота изи ЛС пользователя",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "user",
                    description: "Кому отправить",
                    type: ApplicationCommandOptionType.User,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "message-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
            }), //un-dm
            new DiscordApplicationCommand(
            name: "team",
            description: "Управление командой",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name:"add",
                    description: "Добавить участника",
                    type: ApplicationCommandOptionType.SubCommand,
                    options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption(
                            name: "user",
                            description:"Кого добавить",
                            type: ApplicationCommandOptionType.User,
                            required: true
                        )
                    }
                ),
                new DiscordApplicationCommandOption(
                    name: "remove",
                    description: "Удалить участника",
                    type: ApplicationCommandOptionType.SubCommand,
                    options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption(
                            name: "user",
                            description:"Кого удалить",
                            type: ApplicationCommandOptionType.User,
                            required: true
                        )
                    }
                )
            }), //team
            new DiscordApplicationCommand(
                "quest",
                "Управление квестами",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption(
                        name: "list",
                        description: "Посмотреть текущие квесты",
                        type: ApplicationCommandOptionType.SubCommand
                    ),
                    new DiscordApplicationCommandOption(
                        name: "update",
                        description: "Принудительно обновить состояния квестов и ветки",
                        type: ApplicationCommandOptionType.SubCommand
                    ),
                    new DiscordApplicationCommandOption(
                        name: "add",
                        description: "Добавить квест",
                        type: ApplicationCommandOptionType.SubCommand,
                        options: AddQuestOptions()
                    ),
                    new DiscordApplicationCommandOption(
                        name: "edit",
                        description: "Изменить квест",
                        type: ApplicationCommandOptionType.SubCommand,
                        options: EditOrDeleteQuestCommandOptions()
                    ),
                    new DiscordApplicationCommandOption(
                        name: "remove",
                        description: "Удалить квест",
                        type: ApplicationCommandOptionType.SubCommand,
                        options: EditOrDeleteQuestCommandOptions()
                    )
                }
), //quest
            new DiscordApplicationCommand(
                name: "done",
                description: "Отправить выполнение задания",
                name_localizations: new Dictionary<string, string>
                {
                    { "ru", "выполнить" }
                },
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption(
                        name: "number",
                        description: "Номер задания из /quests",
                        type: ApplicationCommandOptionType.Integer,
                        required: true
                    ),
                    new DiscordApplicationCommandOption(
                        name: "comment",
                        description: "Комментарий или описание выполнения",
                        type: ApplicationCommandOptionType.String,
                        required: true
                    ),
                    new DiscordApplicationCommandOption(
                        name: "file-one",
                        description: "Файл-доказательство (необязательно)",
                        type: ApplicationCommandOptionType.Attachment,
                        required: false
                    ),
                    new DiscordApplicationCommandOption(
                        name: "file-two",
                        description: "Файл-доказательство (необязательно)",
                        type: ApplicationCommandOptionType.Attachment,
                        required: false
                    ),
                    new DiscordApplicationCommandOption(
                        name: "file-three",
                        description: "Файл-доказательство (необязательно)",
                        type: ApplicationCommandOptionType.Attachment,
                        required: false
                    )
                }
            ), // done
            new DiscordApplicationCommand(
                name: "global-data",
                description: "Получить или изменить глобальное состояние игры",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption(
                        name: "get",
                        description: "Скачать текущее глобальное состояние",
                        type: ApplicationCommandOptionType.SubCommand
                    ),
                    new DiscordApplicationCommandOption(
                        name: "set",
                        description: "Установить новое глобальное состояние",
                        type: ApplicationCommandOptionType.SubCommand,
                        options: new List<DiscordApplicationCommandOption>
                        {
                            new DiscordApplicationCommandOption(
                                name: "file",
                                description: "JSON-файл с новым состоянием",
                                type: ApplicationCommandOptionType.Attachment,
                                required: true
                            )
                        }
                    )
                }
            ),//global-data
            new DiscordApplicationCommand(
                name: "quests-daily",
                description: "Управление ежедневными квестами",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать текущие ежедневные квесты", ApplicationCommandOptionType.SubCommand),
                    new DiscordApplicationCommandOption("set", "Установить новые ежедневные квесты", ApplicationCommandOptionType.SubCommand,
                        options: new List<DiscordApplicationCommandOption>
                        {
                            new DiscordApplicationCommandOption("file", "JSON-файл с квестами", ApplicationCommandOptionType.Attachment, true)
                        })
                }
            ),

            new DiscordApplicationCommand(
                name: "quests-weekly",
                description: "Управление еженедельными квестами",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать текущие еженедельные квесты", ApplicationCommandOptionType.SubCommand),
                    new DiscordApplicationCommandOption("set", "Установить новые еженедельные квесты", ApplicationCommandOptionType.SubCommand,
                        options: new List<DiscordApplicationCommandOption>
                        {
                            new DiscordApplicationCommandOption("file", "JSON-файл с квестами", ApplicationCommandOptionType.Attachment, true)
                        })
                }
            ),

            new DiscordApplicationCommand(
                name: "quests-hard",
                description: "Управление сложными квестами",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать текущие сложные квесты", ApplicationCommandOptionType.SubCommand),
                    new DiscordApplicationCommandOption("set", "Установить новые сложные квесты", ApplicationCommandOptionType.SubCommand,
                        options: new List<DiscordApplicationCommandOption>
                        {
                            new DiscordApplicationCommandOption("file", "JSON-файл с квестами", ApplicationCommandOptionType.Attachment, true)
                        })
                }
            ),

            new DiscordApplicationCommand(
                name: "quests-secret",
                description: "Управление секретными квестами",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать текущие секретные квесты", ApplicationCommandOptionType.SubCommand),
                    new DiscordApplicationCommandOption("set", "Установить новые секретные квесты", ApplicationCommandOptionType.SubCommand,
                        options: new List<DiscordApplicationCommandOption>
                        {
                            new DiscordApplicationCommandOption("file", "JSON-файл с квестами", ApplicationCommandOptionType.Attachment, true)
                        })
                }
            ),
            new DiscordApplicationCommand(
                name: "save-data",
                description: "Управление SaveData бота",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать SaveData", ApplicationCommandOptionType.SubCommand, options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption("bot-id", "ID бота", ApplicationCommandOptionType.String, true)
                    }),
                    new DiscordApplicationCommandOption("set", "Установить SaveData", ApplicationCommandOptionType.SubCommand, options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption("bot-id", "ID бота", ApplicationCommandOptionType.String, true),
                        new DiscordApplicationCommandOption("file", "JSON-файл SaveData", ApplicationCommandOptionType.Attachment, true)
                    })
                }
            ),

            new DiscordApplicationCommand(
                name: "personality",
                description: "Управление Personality бота",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать Personality", ApplicationCommandOptionType.SubCommand, options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption("bot-id", "ID бота", ApplicationCommandOptionType.String, true)
                    }),
                    new DiscordApplicationCommandOption("set", "Установить Personality", ApplicationCommandOptionType.SubCommand, options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption("bot-id", "ID бота", ApplicationCommandOptionType.String, true),
                        new DiscordApplicationCommandOption("file", "JSON-файл Personality", ApplicationCommandOptionType.Attachment, true)
                    })
                }
            ),

            new DiscordApplicationCommand(
                name: "players",
                description: "Управление Players бота",
                defaultMemberPermissions: Permissions.Administrator,
                options: new List<DiscordApplicationCommandOption>
                {
                    new DiscordApplicationCommandOption("get", "Скачать список игроков", ApplicationCommandOptionType.SubCommand, options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption("bot-id", "ID бота", ApplicationCommandOptionType.String, true)
                    }),
                    new DiscordApplicationCommandOption("set", "Установить список игроков", ApplicationCommandOptionType.SubCommand, options: new List<DiscordApplicationCommandOption>
                    {
                        new DiscordApplicationCommandOption("bot-id", "ID бота", ApplicationCommandOptionType.String, true),
                        new DiscordApplicationCommandOption("file", "JSON-файл Players", ApplicationCommandOptionType.Attachment, true)
                    })
                }
            )
                    };

        public static async Task Handle(LeaderBot bot, InteractionCreateEventArgs e, GlobalGameState state)
        {
            var playerId = e.Interaction.User.Id;

            var variables = new Dictionary<string, string>
            {
                ["caller"] = e.Interaction.User.Mention,
                ["caller-name"] = e.Interaction.User.Username,
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["guild-name"] = e.Interaction.Guild?.Name ?? "",
                ["guild-id"] = $"{(e.Interaction.Guild?.Id ?? 0)}",
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            try
            {
                var questChannel = await bot._client.GetChannelAsync(bot.SaveData.QuestChannelId);
                var globalChannel = await bot._client.GetChannelAsync(bot.SaveData.GlobalChannelId);
                variables["quest-channel"] = questChannel.Mention;
                variables["quest-channel"] = globalChannel.Mention;
            }
            catch { }
            


            async Task HandleBotDataCommand<T>(
            InteractionCreateEventArgs e,
            string commandName,
            Func<LeaderBot, T> getData,
            Action<LeaderBot, T> setData)
            {
                if (e.Interaction.Data.Name != commandName)
                    return;

                var sub = e.Interaction.Data.Options.First();
                var botId = sub.Options.FirstOrDefault(o => o.Name == "bot-id")?.Value?.ToString();

                if (botId == null || !Program.BotManager.Bots.TryGetValue(botId, out var bot))
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent("Бот с указанным ID не найден.")
                            .AsEphemeral(true));
                    return;
                }

                if (sub.Name == "get")
                {
                    string json = JsonSerializer.Serialize(getData(bot), new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

                    var path = Path.GetTempFileName();
                    File.WriteAllText(path, json);
                    var stream = new MemoryStream(File.ReadAllBytes(path));

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent($"Файл данных бота `{botId}`:")
                            .AddFile($"{commandName}.json", stream)
                            .AsEphemeral(true));
                    return;
                }

                if (sub.Name == "set")
                {
                    if (!Program.ServiceMode)
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("для редактирования файлов перейдите в сервисный режим").AsEphemeral(true));
                        return;
                    }

                    var fileOption = sub.Options.FirstOrDefault(o => o.Name == "file");
                    if (fileOption == null || !e.Interaction.Data.Resolved.Attachments.TryGetValue((ulong)fileOption.Value!, out var attachment))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent("Не удалось получить файл.")
                                .AsEphemeral(true));
                        return;
                    }

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    try
                    {
                        using var http = new HttpClient();
                        var json = await http.GetStringAsync(attachment.Url);

                        var data = JsonSerializer.Deserialize<T>(json);
                        if (data == null)
                        {
                            throw new JsonException("Не удалось десериализовать файл.");
                        }

                        setData(bot, data);

                        await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"Файл успешно применён к боту `{botId}` ✅"));
                    }
                    catch (Exception ex)
                    {
                        await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"Ошибка при установке данных: {ex.Message}"));
                    }
                }
            }

            if (e.Interaction.Data.Name == "restart")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("окк, щаааа").AsEphemeral(true));

                Program.RestartAsync();

                return;
            }

            await HandleBotDataCommand<BotSaveData>(e, "save-data", bot => bot.SaveData, (bot, value) => bot.SaveData = value);
            await HandleBotDataCommand<LeaderPersonality>(e, "personality", bot => bot.Personality, (bot, value) => bot.Personality = value);
            await HandleBotDataCommand<List<PlayerData>>(e, "players", bot => bot.Players, (bot, value) => bot.Players = value);

            if (e.Interaction.Data.Name == "global-data")
            {                
                var sub = e.Interaction.Data.Options.First();

                if (sub.Name == "get")
                {
                    var json = Program.BotManager.GetGlobalStateJson();
                    var tempPath = Path.GetTempFileName();
                    File.WriteAllText(tempPath, json);

                    var fs = new MemoryStream(File.ReadAllBytes(tempPath));
                    fs.Position = 0;

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent("Текущий GlobalState.json:")
                            .AddFile("GlobalState.json", fs)
                            .AsEphemeral(true));

                    return;
                }

                if (sub.Name == "set")
                {
                    if (!Program.ServiceMode)
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("для редактирования файлов перейдите в сервисный режим").AsEphemeral(true));
                        return;
                    }


                    var fileOption = sub.Options.FirstOrDefault(o => o.Name == "file");
                    if (fileOption is null)
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent("Не передан файл.")
                                .AsEphemeral(true));
                        return;
                    }

                    var fileId = (ulong)fileOption.Value!;
                    if (!e.Interaction.Data.Resolved.Attachments.TryGetValue(fileId, out var attachment))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent("Не удалось найти вложение.")
                                .AsEphemeral(true));
                        return;
                    }

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    try
                    {
                        using var http = new HttpClient();
                        var json = await http.GetStringAsync(attachment.Url);

                        var error = Program.BotManager.SetGlobalStateFromJson(json);
                        if (error != null)
                        {
                            await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                                .WithContent($"Ошибка при установке глобального состояния: {error.Message}"));
                        }
                        else
                        {
                            await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                                .WithContent("Глобальное состояние успешно обновлено ✅"));
                        }
                    }
                    catch (Exception ex)
                    {
                        await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"Ошибка обработки файла: {ex.Message}"));
                    }

                    return;
                }
            }

            if(e.Interaction.Data.Name == "quests-daily" || e.Interaction.Data.Name == "quests-weekly" || e.Interaction.Data.Name == "quests-hard" || e.Interaction.Data.Name == "quests-secret")
            {
                var sub = e.Interaction.Data.Options.First();
                var manager = Program.QuestManager;

                string name = e.Interaction.Data.Name switch
                {
                    "quests-daily" => "daily",
                    "quests-weekly" => "weekly",
                    "quests-hard" => "hard",
                    "quests-secret" => "secret",
                    _ => throw new InvalidOperationException()
                };


                if (sub.Name == "get")
                {
                    string json = e.Interaction.Data.Name switch
                    {
                        "quests-daily" => manager.GetDailyQuestsJson(),
                        "quests-weekly" => manager.GetWeeklyQuestsJson(),
                        "quests-hard" => manager.GetHardQuestsJson(),
                        "quests-secret" => manager.GetSecretQuestsJson(),
                        _ => throw new InvalidOperationException()
                    };

                    var tempPath = Path.GetTempFileName();
                    File.WriteAllText(tempPath, json);

                    var ms = new MemoryStream(File.ReadAllBytes(tempPath));
                    ms.Position = 0;

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent($"Квесты типа {name} выгружены:")
                            .AddFile($"{name}-quests.json", ms)
                            .AsEphemeral(true));
                    return;
                }

                if (sub.Name == "set")
                {
                    if (!Program.ServiceMode)
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("для редактирования файлов перейдите в сервисный режим").AsEphemeral(true));
                        return;
                    }

                    var fileOption = sub.Options.FirstOrDefault(o => o.Name == "file");
                    if (fileOption is null)
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent("Не передан файл.")
                                .AsEphemeral(true));
                        return;
                    }

                    var fileId = (ulong)fileOption.Value!;
                    if (!e.Interaction.Data.Resolved.Attachments.TryGetValue(fileId, out var attachment))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent("Не удалось найти файл вложения.")
                                .AsEphemeral(true));
                        return;
                    }

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    try
                    {
                        using var http = new HttpClient();
                        var json = await http.GetStringAsync(attachment.Url);

                        Exception? error = e.Interaction.Data.Name switch
                        {
                            "quests-daily" => manager.SetDailyQuestsFromJson(json),
                            "quests-weekly" => manager.SetWeeklyQuestsFromJson(json),
                            "quests-hard" => manager.SetHardQuestsFromJson(json),
                            "quests-secret" => manager.SetSecretQuestsFromJson(json),
                            _ => new InvalidOperationException()
                        };

                        if (error != null)
                        {
                            await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                                .WithContent($"Ошибка разбора JSON: {error.Message}"));
                        }
                        else
                        {
                            await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                                .WithContent($"Квесты типа {name} успешно обновлены ✅"));
                        }
                    }
                    catch (Exception ex)
                    {
                        await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                            .WithContent($"Ошибка обработки файла: {ex.Message}"));
                    }
                }
            }

            if (Program.ServiceMode)
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ведутся сервисные работы. Пожалуйста, попробуйте позже!")
                        .AsEphemeral());

                return;
            }

            if (e.Interaction.Data.Name == "service-mode")
            {
                Program.TurnOnServiceMode();

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Сервисный режим активирован."));
            }

            if (e.Interaction.Data.Name == "say")
            {
                var textOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "text");
                var channelOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "channel-id");
                var fileOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file");

                if (textOption == null || channelOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);


                string text = textOption.Value.ToString();
                ulong channelId = Convert.ToUInt64(channelOption.Value);
                var targetChannel = await bot._client.GetChannelAsync(channelId);

                variables["text"] = text;
                variables["channel"] = targetChannel.ToString();

                try
                {
                    var builder = new DiscordMessageBuilder()
                        .WithContent(StringTemplate.Format(text, variables));

                    if (fileOption != null)
                    {
                        await builder.ForwardFile(fileOption, e.Interaction);
                    }

                    var message = await targetChannel.SendMessageAsync(builder);

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Сообщение отправлено в канал: {targetChannel.Name}\nID сообщения: {message.Id}"));
                }
                catch (Exception ex)
                {
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Ошибка отправки сообщения в канал: {ex.Message}"));

                    Console.WriteLine(ex);
                    await Program.BotManager.SendAdminMessage(ex.ToString());
                }

                return;
            }

            if (e.Interaction.Data.Name == "re-say")
            {
                var textOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "text");
                var channelOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "channel-id");
                var messageOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "message-id");
                var fileOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file");

                if (textOption == null || messageOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);


                string text = textOption.Value.ToString();
                ulong channelId = Convert.ToUInt64(channelOption.Value);
                ulong messageId = Convert.ToUInt64(messageOption.Value);

                try
                {
                    var targetChannel = await bot._client.GetChannelAsync(channelId);
                    var message = await targetChannel.GetMessageAsync(messageId);

                    variables["text"] = text;
                    variables["channel"] = targetChannel.ToString();

                    var builder = new DiscordMessageBuilder()
                        .WithContent(StringTemplate.Format(text, variables));

                    if (fileOption != null)
                    {
                        await builder.ForwardFile(fileOption, e.Interaction);
                    }

                    await message.ModifyAsync(builder);

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Сообщение отредактировано в канале: {targetChannel.Name}"));
                }
                catch (Exception ex)
                {
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Ошибка редактирование сообщения в канал: {ex.Message}"));

                    Console.WriteLine(ex);
                    await Program.BotManager.SendAdminMessage(ex.ToString());
                }

                return;
            }

            if (e.Interaction.Data.Name == "un-say")
            {
                var channelOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "channel-id");
                var messageOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "message-id");

                if (channelOption == null || messageOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                ulong channelId = Convert.ToUInt64(channelOption.Value);
                ulong messageId = Convert.ToUInt64(messageOption.Value);

                try
                {
                    var targetChannel = await bot._client.GetChannelAsync(channelId);
                    var message = await targetChannel.GetMessageAsync(messageId);

                    await message.DeleteAsync();

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Сообщение удалено в канале: {targetChannel.Name}"));
                }
                catch (Exception ex)
                {
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Ошибка удаления сообщения в канале: {ex.Message}"));

                    Console.WriteLine(ex);
                    await Program.BotManager.SendAdminMessage(ex.ToString());
                }

                return;
            }

            if (e.Interaction.Data.Name == "dm")
            {
                var userOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "user");
                var textOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "text");
                var fileOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file");

                if (userOption == null || textOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                ulong userId = Convert.ToUInt64(userOption.Value);
                string text = textOption.Value.ToString();
                variables["text"] = text;

                try
                {
                    var member = await e.Interaction.Guild.GetMemberAsync(userId);
                    variables["user"] = member.Mention;

                    var dmChannel = await member.CreateDmChannelAsync();

                    var builder = new DiscordMessageBuilder()
                        .WithContent(StringTemplate.Format(text, variables));

                    if (fileOption != null)
                    {
                        await builder.ForwardFile(fileOption, e.Interaction);
                    }

                    var message = await dmChannel.SendMessageAsync(builder);

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Сообщение отправлено пользователю: {member.DisplayName}\nID сообщения: {message.Id}"));
                }
                catch (Exception ex)
                {
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Не удалось отправить ЛС сообщение ({ex.Message})"));
                }
            }

            if (e.Interaction.Data.Name == "re-dm")
            {
                var textOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "text");
                var userOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "user");
                var messageOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "message-id");
                var fileOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file");

                if (userOption == null || textOption == null || messageOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);


                ulong userId = Convert.ToUInt64(userOption.Value);
                string text = textOption.Value.ToString();
                variables["text"] = text;

                try
                {
                    ulong messageID = Convert.ToUInt64(messageOption.Value);

                    var member = await e.Interaction.Guild.GetMemberAsync(userId);
                    variables["user"] = member.Mention;

                    var dmChannel = await member.CreateDmChannelAsync();

                    var message = await dmChannel.GetMessageAsync(messageID);

                    var builder = new DiscordMessageBuilder()
                        .WithContent(StringTemplate.Format(text, variables));

                    if (fileOption != null)
                    {
                        await builder.ForwardFile(fileOption, e.Interaction);
                    }

                    
                    await message.ModifyAsync(builder);

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Сообщение пользователю: {member.DisplayName} успешно изменено."));
                }
                catch (Exception ex)
                {
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent($"Не удалось изменить ЛС сообщение ({ex.Message})"));
                }

                return;
            }

            if (e.Interaction.Data.Name == "team")
            {
                var subCommand = e.Interaction.Data.Options.First().Name;

                var userOption = (ulong)e.Interaction.Data.Options.First().Options.First().Value;
                var member = await e.Interaction.Guild.GetMemberAsync(userOption);
                variables["user"] = member.Mention;

                if (subCommand == "add")
                {
                    if (member.IsBot)
                    {
                        await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent(StringTemplate.Format(bot.Personality.TriedToAddBotInTheTeamMessage, variables))
                            .AsEphemeral(false)
                            );
                        return;
                    }

                    if (bot.Players.Any(p => p.Id == userOption))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder().WithContent(StringTemplate.Format(bot.Personality.AlreadyInThisCommandMessage ?? "Ты уже в этой команде.", variables)));
                        return;
                    }
                    else if (Program.BotManager.Bots.Any(b => b.Value.Players.Any(p => p.Id == userOption)))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent(StringTemplate.Format(bot.Personality.AlreadyInOtherCommandMessage ?? "Ты уже в другой команде.", variables)));
                        return;
                    }
                    var players = bot.Players;
                    players.Add(new PlayerData(member.Id, member.Username));
                    bot.SavePlayers(players);
                    await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent($"{member.Username} был добавлен в команду!")
                            .AsEphemeral(false)
                    );

                    var dmChannel = await member.CreateDmChannelAsync();
                    await dmChannel.SendMessageAsync(StringTemplate.Format(bot.Personality.AddedToTheTeamDMMessage, variables)  );

                    return;
                }
                else if (subCommand == "remove")
                {
                    var players = bot.Players;
                    int removedCount = players.RemoveAll(p => p.Id == userOption);
                    if (removedCount > 0)
                    {
                        bot.Players = players;

                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent($"{member.Username} был удалён из команды.")
                                .AsEphemeral(false)
                        );

                        var dmChannel = await member.CreateDmChannelAsync();
                        await dmChannel.SendMessageAsync(StringTemplate.Format(bot.Personality.RemovedFromTheTeamDMMessage, variables));

                        return;
                    }
                    else
                    {
                        await e.Interaction.CreateResponseAsync(
                            InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent($"{member.Username} не был найден в команде.")
                                .AsEphemeral(false)
                        );
                        return;
                    }
                }
            }

            if (e.Interaction.Data.Name == "ping")
            {
                var random = new Random();
                var phrases = bot.Personality.PingMessages;
                var selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(StringTemplate.Format(selectedPhrase ?? "Пинг-понг!", variables)));

                return;
            }

            if (e.Interaction.Data.Name == "quest")
            {
                var subcommand = e.Interaction.Data.Options.First().Name;

                var questManager = Program.QuestManager;

                if (subcommand == "list")
                {
                    var interactivity = bot._client.GetInteractivity();

                    //await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    questManager.LoadQuests();

                    var pages = new List<Page>();

                    // DAILY
                    var dailyPages = BuildQuestList(questManager.DailyQuests, QuestType.Daily, questManager.StartDate);
                    pages.AddRange(dailyPages.Select((content, i) => new Page
                    {
                        Content = $"{content}\n\n**Страница {i + 1} из {dailyPages.Count} - (Ежедневные)**"
                    }));

                    // WEEKLY
                    var weeklyPages = BuildQuestList(questManager.WeeklyQuests, QuestType.Weekly, questManager.StartDate);
                    pages.AddRange(weeklyPages.Select((content, i) => new Page
                    {
                        Content = $"{content}\n\n**Страница {i + 1} из {weeklyPages.Count} - (Еженедельные)**"
                    }));

                    // HARD
                    var hardPages = BuildQuestList(questManager.HardQuests, QuestType.Hard);
                    pages.AddRange(hardPages.Select((content, i) => new Page
                    {
                        Content = $"{content}\n\n**Страница {i + 1} из {hardPages.Count}  - (Сложные)**"
                    }));

                    // SECRET
                    var secretPages = BuildQuestList(questManager.SecretQuests, QuestType.Secret);
                    pages.AddRange(secretPages.Select((content, i) => new Page
                    {
                        Content = $"{content}\n\n**Страница {i + 1} из {secretPages.Count}  - (Секретные)**"
                    }));

                    // Пагинация
                    await interactivity.SendPaginatedResponseAsync(
                        interaction: e.Interaction,
                        ephemeral: false,
                        user: e.Interaction.User,
                        pages: pages,
                        behaviour: DSharpPlus.Interactivity.Enums.PaginationBehaviour.WrapAround,
                        deletion: DSharpPlus.Interactivity.Enums.ButtonPaginationBehavior.Disable
                    );
                }
                else if (subcommand == "update")
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Квесты обновлены.")
                        .AsEphemeral(true));

                    UpdateAllQuestsStates(bot);

                    return;
                }
                else if (subcommand == "edit" || subcommand == "remove")
                {
                    var opts = e.Interaction.Data.Options.First().Options.ToDictionary(x => x.Name, x => x.Value);

                    string type = opts["type"].ToString()!;
                    int index = Convert.ToInt32(opts["index"]);
                    int? dayOrWeek = opts.ContainsKey("day-or-week") ? Convert.ToInt32(opts["day-or-week"]) : null;

                    if (subcommand == "edit")
                    {
                        string? title = opts.ContainsKey("title") ? opts["title"]?.ToString() : null;
                        string? description = opts.ContainsKey("description") ? opts["description"]?.ToString() : null;
                        string? helpText = opts.ContainsKey("help-text") ? opts["help-text"]?.ToString() : null;
                        string? explanation = opts.ContainsKey("explanation") ? opts["explanation"]?.ToString() : null;
                        int? reward = opts.ContainsKey("reward") ? Convert.ToInt32(opts["reward"]) : null;
                        int? limit = opts.ContainsKey("limit") ? Convert.ToInt32(opts["limit"]) : null;
                        bool? globalLimit = opts.ContainsKey("global-limit") ? Convert.ToBoolean(opts["global-limit"]) : null;

                        if ((type == "daily" || type == "weekly") && dayOrWeek == null)
                        {
                            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent($"❌ Невозможно изменить квест: для ежедневных и еженедельных квестов необходимо указать параметр **\"dayOrWeek\"**")
                                .AsEphemeral(true));

                            return;
                        }

                        switch (type)
                        {
                            case "daily":
                                questManager.EditDailyQuest(dayOrWeek!.Value, index, title, description, helpText, explanation, reward, limit, globalLimit);
                                break;
                            case "weekly":
                                questManager.EditWeeklyQuest(dayOrWeek!.Value, index, title, description, helpText, explanation, reward, limit, globalLimit);
                                break;
                            case "hard":
                                questManager.EditHardQuest(index, title, description, helpText, explanation, reward, limit, globalLimit);
                                break;
                            case "secret":
                                questManager.EditSecretQuest(index, title, description, helpText, explanation, reward, limit, globalLimit);
                                break;
                        }

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent($"✅ Квест успешно отредактирован.")
                                .AsEphemeral(true));
                    }
                    else if (subcommand == "remove")
                    {
                        if ((type == "daily" || type == "weekly") && dayOrWeek == null)
                        {
                            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent($"❌ Невозможно удалить квест: для ежедневных и еженедельных квестов необходимо указать параметр **\"dayOrWeek\"**")
                                .AsEphemeral(true));

                            return;
                        }

                        switch (type)
                        {
                            case "daily":
                                questManager.RemoveDailyQuest(dayOrWeek!.Value, index);
                                break;
                            case "weekly":
                                questManager.RemoveWeeklyQuest(dayOrWeek!.Value, index);
                                break;
                            case "hard":
                                questManager.RemoveHardQuest(index);
                                break;
                            case "secret":
                                questManager.RemoveSecretQuest(index);
                                break;
                        }

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent($"❌ Квест удалён.")
                                .AsEphemeral(true));
                    }

                    return;
                }
                else if(subcommand == "add")
{
                    var opts = e.Interaction.Data.Options.First().Options.ToDictionary(x => x.Name, x => x.Value);

                    string type = opts["type"].ToString()!;
                    int? dayOrWeek = opts.ContainsKey("day-or-week") ? Convert.ToInt32(opts["day-or-week"]) : null;

                    var quest = new Quest
                    {
                        Title = opts["title"].ToString()!,
                        Description = opts["description"].ToString()!,
                        Reward = Convert.ToInt32(opts["reward"]),
                        CompletionLimit = Convert.ToInt32(opts["limit"]),
                        GlobalLimit = opts.ContainsKey("global-limit") && Convert.ToBoolean(opts["global-limit"]),
                        HelpText = opts.ContainsKey("help-text") ? opts["help-text"]!.ToString()! : "",
                        Explanation = opts.ContainsKey("explanation") ? opts["explanation"]!.ToString()! : ""
                    };

                    string msg = "";

                    if ((type == "daily" || type == "weekly") && dayOrWeek == null)
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent($"❌ Невозможно добавить квест: для ежедневных и еженедельных квестов необходимо указать параметр **\"dayOrWeek\"**")
                            .AsEphemeral(true));

                        return;
                    }

                    switch (type)
                    {
                        case "daily":
                            questManager.AddDailyQuest(dayOrWeek!.Value, quest);
                            msg = $"✅ Ежедневный квест на день {dayOrWeek} добавлен: **{quest.Title}**";
                            break;
                        case "weekly":
                            questManager.AddWeeklyQuest(dayOrWeek!.Value, quest);
                            msg = $"✅ Еженедельный квест на неделю {dayOrWeek} добавлен: **{quest.Title}**";
                            break;
                        case "hard":
                            questManager.AddHardQuest(quest);
                            msg = $"✅ Сложный квест добавлен: **{quest.Title}**";
                            break;
                        case "secret":
                            questManager.AddSecretQuest(quest);
                            msg = $"✅ Секретный квест добавлен: **{quest.Title}**";
                            break;
                    }

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent(msg).AsEphemeral(true));

                    Program.BotManager.UpdateTeamsQuests();
                    return;
                }

                return;
            }

            if (!bot.Players.Any(p => p.Id == playerId))
            {
                var random = new Random();
                var phrases = bot.Personality.NotInThisTeamMessages;
                var selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(StringTemplate.Format(selectedPhrase ?? "Ты не в этой команде.", variables)));
                return;
            }

            if (e.Interaction.Data.Name == "quests")
            {
                var active = Program.QuestManager.GetActiveQuests(team: bot.Id);
                var sb = new StringBuilder();

                var random = new Random();
                var phrases = bot.Personality.QuestListPhrases;
                var selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";

                sb.AppendLine($"_{StringTemplate.Format(selectedPhrase, variables)}_");
                sb.AppendLine($"**📋 Текущие активные квесты:**\n");

                foreach (var q in active)
                {
                    // Время в МСК
                    var mskTime = TimeZoneInfo.ConvertTimeFromUtc(q.AvalableLimit,
                        TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

                    string expires = q.AvalableLimit == DateTime.MaxValue
                        ? "∞"
                        : $"{mskTime:dd.MM HH:mm} МСК (осталось {q.AvalableLimit.Subtract(DateTime.UtcNow):d\\д\\ h\\ч\\ mm\\м})";

                    if(!q.Quest.LimitReached(bot.Id))
                    {
                        sb.AppendLine($"# `[{q.Index + 1}]` [{q.QuestType}] **{q.Quest.Title}** — *{q.Quest.Reward}₧* " +
                        $"[{(q.Quest.CompletionsByTeams.ContainsKey(bot.Id) ? q.Quest.CompletionsByTeams[bot.Id] : "0")}/{q.Quest.CompletionLimit}] *({(q.Quest.GlobalLimit ? "Глобальный" : "По командам")})* " +
                        $"\n{q.Quest.Description}\n⏳ до: **{expires}**");
                    }
                    else //{ (q.Quest.CompletedPlayers.Contains(playerId) ? "✅" : "")}
                    {
                        sb.AppendLine($"# ~~`[{q.Index + 1}]` [{q.QuestType}] **{q.Quest.Title}** — *{q.Quest.Reward}₧* " +
                        $"[{(q.Quest.CompletionsByTeams.ContainsKey(bot.Id) ? q.Quest.CompletionsByTeams[bot.Id] : "0")}/{q.Quest.CompletionLimit}] *({(q.Quest.GlobalLimit ? "Глобальный" : "По командам")})* ~~" +
                        $"\n~~{q.Quest.Description}\n⏳ до: **{expires}**~~");
                    }

                    if(q.Quest.CompletedPlayers.Count >= 1)
                    {
                        sb.AppendLine($"Квест выполнили: ");
                        foreach (var player in q.Quest.CompletedPlayers)
                        {
                            if (Program.BotManager.GetPlayerLeader(player) == bot.Id)
                            {
                                var user = await bot._client.GetUserAsync(player);
                                sb.AppendLine($"- друг **{user.Username}** {(player == playerId ? "👈 это ты 👈" : "")}");
                            }
                        }
                        foreach (var player in q.Quest.CompletedPlayers)
                        {
                            if (Program.BotManager.GetPlayerLeader(player) != bot.Id)
                            {
                                var user = await bot._client.GetUserAsync(player);
                                sb.AppendLine($"- соперник **{user.Username}** {(player == playerId ? "👈это ты👈" : "")}");
                            }
                        }
                    }

                    sb.AppendLine("");
                    //phrases = bot.SaveData.QuestCompletedPhrases;
                    //selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";
                    //sb.AppendLine($"_{selectedPhrase}_\n");

                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent(sb.ToString()));
            }

            if (e.Interaction.Data.Name == "done")
            {
                var indexOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "number");
                var commentOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "comment");
                var file1Option = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file-one");
                var file2Option = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file-two");
                var file3Option = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "file-three");

                var random = new Random();
                var phrases = bot.Personality.QuestCheckSendedPhrases;
                var selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";



                if (indexOption == null || commentOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("❌ Ошибка: нужно указать `number` и `comment`!")
                        .AsEphemeral(true));
                    return;
                }

                int index = Convert.ToInt32(indexOption.Value) - 1;
                string comment = commentOption.Value.ToString();
                ulong userId = e.Interaction.User.Id;

                variables["number"] = index.ToString();
                variables["comment"] = comment;

                // Загружаем активные квесты
                var activeQuests = Program.QuestManager.GetActiveQuests(team: bot.Id);

                if (index < 0 || index >= activeQuests.Count)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("❌ Ошибка: неверный индекс задания.")
                        .AsEphemeral(true));
                    return;
                }

                var quest = activeQuests[index];

                if (quest.Quest.CheckedPlayers.Contains(userId))
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("❌ Ошибка: твое решение уже на рассмотрении.")
                        .AsEphemeral(true));
                    return;
                }

                if (quest.Quest.CompletedPlayers.Contains(userId))
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("❌ Ошибка: ты уже выполнил этот квест.")
                        .AsEphemeral(true));
                    return;
                }

                quest.Quest.NoteCheck(userId);
                Program.QuestManager.SaveQuests();

                var sb = new StringBuilder();
                sb.AppendLine($"# ✅ Новая заявка на задание от {e.Interaction.User.Mention}:");
                sb.AppendLine($"**[{quest.QuestType}] {quest.Quest.Title}**");
                sb.AppendLine($"*\"{quest.Quest.Description}\"*");
                if (!string.IsNullOrEmpty(quest.Quest.Explanation))
                    sb.AppendLine($"({quest.Quest.Explanation})");
                sb.AppendLine($"## Комментарий: {comment}");

                // Получаем канал
                var guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);
                var checkChannel = guild.GetChannel(Program.BotManager.GlobalState.QuestCheckChannelId);

                // Создаём кнопки
                var acceptButton = new DiscordButtonComponent(
                    ButtonStyle.Success,
                    customId: $"accept|{quest.QuestType}|{quest.BaseIndex[0]}|{quest.BaseIndex[1]}|{userId}|null",
                    label: "✅ Принять");

                var rejectButton = new DiscordButtonComponent(
                    ButtonStyle.Danger,
                    customId: $"reject|{quest.QuestType}|{quest.BaseIndex[0]}|{quest.BaseIndex[1]}|{userId}|null",
                    label: "❌ Отклонить");

                DiscordMessage message;

                // Отложенный ответ (чтобы не словить тайм-аут Discord)
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                
                try
                {
                    var messageBuilder = new DiscordMessageBuilder().WithContent(sb.ToString());

                    if (file1Option != null)
                    {
                        await messageBuilder.ForwardFile(file1Option, e.Interaction);
                    }
                    if (file2Option != null)
                    {
                        await messageBuilder.ForwardFile(file2Option, e.Interaction);
                    }
                    if (file3Option != null)
                    {
                        await messageBuilder.ForwardFile(file3Option, e.Interaction);
                    }

                    messageBuilder.AddComponents(acceptButton, rejectButton);

                    message = await checkChannel.SendMessageAsync(messageBuilder);

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                           .WithContent($"✅ Твоя заявка по квесту [{index + 1}] \"{quest.Quest.Title}\" отправлена на проверку!" +
                           $"\n_{selectedPhrase}_"));

                    // Обновляем кнопки (вставляем message.Id)
                    acceptButton = new DiscordButtonComponent(
                        ButtonStyle.Success,
                        customId: $"accept|{quest.QuestType}|{quest.BaseIndex[0]}|{quest.BaseIndex[1]}|{userId}|{message.Id}",
                        label: "✅ Принять");

                    rejectButton = new DiscordButtonComponent(
                        ButtonStyle.Danger,
                        customId: $"reject|{quest.QuestType}|{quest.BaseIndex[0]}|{quest.BaseIndex[1]}|{userId}|{message.Id}",
                        label: "❌ Отклонить");

                    quest.Quest.ActiveConfirmMessages.Add(new KeyValuePair<string, ulong>(bot.Id,message.Id));
                    Program.QuestManager.SaveQuests();

                    await message.ModifyAsync(new DiscordMessageBuilder()
                        .WithContent(sb.ToString())
                        .AddComponents(acceptButton, rejectButton));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Ошибка done] {ex.Message}");

                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
                        .WithContent("❌ Произошла ошибка при обработке твоей заявки. Пожалуйста, попробуй снова."));
                }

                return;
            }

            if(e.Interaction.Data.Name == "score")
            {
                var sb = new StringBuilder();

                var bestTeam = bot.Id;
                var bestPlayer = e.Interaction.User.Id;
                int playerScore;
                var bestScore = bot.SaveData.TeamScore;

                var random = new Random();
                List<string> phrases;
                string selectedPhrase; //phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";

                foreach (var _bot in Program.BotManager.Bots.Values)
                {
                    if(_bot.SaveData.TeamScore > bestScore)
                    {
                        bestScore = _bot.SaveData.TeamScore;
                        bestTeam = _bot.Id;
                    }
                }

                playerScore = bot.Players.FirstOrDefault(p => p.Id == e.Interaction.User.Id).PersonalScore;
                bestScore = playerScore;

                foreach (var player in bot.Players)
                {
                    if(player.PersonalScore > bestScore)
                    {
                        bestScore = player.PersonalScore;
                        bestPlayer = player.Id;
                    }
                }


                sb.AppendLine($"# СЧЕТ НАШЕЙ КОМАНДЫ {bot.SaveData.TeamScore}₧");

                if (bestTeam == bot.Id && bot.SaveData.TeamScore != 0)
                {
                    phrases = bot.Personality.BestTeamScorePhrases;
                    selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";
                    sb.AppendLine($"\n_{selectedPhrase}_");
                }
                else
                {
                    phrases = bot.Personality.PatheticTeamScorePhrases;
                    selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";
                    sb.AppendLine($"\n_{selectedPhrase}_");
                }

                sb.AppendLine($"## Очки, заработанные сокомандниками");

                foreach(var player in bot.Players)
                {
                    sb.AppendLine($"**{player.Name}** - {player.PersonalScore}₧ {(player.Id == e.Interaction.User.Id? "👈 это ты 👈": "")}");
                }

                
                if(playerScore == 0)
                {
                    phrases = bot.Personality.PlayersScoreZeroPhrases;
                    selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";
                    sb.AppendLine($"\n_{selectedPhrase}_");
                }
                else if (bestPlayer == e.Interaction.User.Id)
                {
                    phrases = bot.Personality.PlayersScoreBestPhrases;
                    selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";
                    sb.AppendLine($"\n_{selectedPhrase}_");
                }
                else
                {
                    phrases = bot.Personality.PlayersScoreNormalPhrases;
                    selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";
                    sb.AppendLine($"\n_{selectedPhrase}_");
                }

                sb.AppendLine($"\n**Соперники:**");

                foreach (var _bot in Program.BotManager.Bots.Values)
                {
                    if (_bot.Id == bot.Id)
                        continue;


                    sb.AppendLine($"**{_bot.Personality.TeamName}** - {_bot.SaveData.TeamScore}₧");
                }

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(StringTemplate.Format(sb.ToString(), variables)));
            }
        }
        
        public static async Task EnsureQuestThreads(LeaderBot bot)
        {
            var variables = new Dictionary<string, string>
            {
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            DiscordGuild guild;
            DiscordChannel questChannel;
            var botSaveData = bot.SaveData;
            var botPersonality = bot.Personality;

            try
            {
                guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);
                questChannel = guild.GetChannel(bot.SaveData.QuestChannelId);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[{bot.Id}] - Ошибка получения Гильдии/Канала - {ex}");
                Program.BotManager.SendAdminMessage($"[{bot.Id}] - Ошибка получения Гильдии/Канала - {ex}");

                return;
            }


            foreach (var type in Enum.GetValues(typeof(QuestType)).Cast<QuestType>())
            {
                var title = botPersonality.QuestTypeTitles.ContainsKey(type) ? botPersonality.QuestTypeTitles[type] : type.ToString();
                var desc = botPersonality.QuestTypeDescriptions.ContainsKey(type) ? botPersonality.QuestTypeDescriptions[type] : "";

                bool needCreate = false;

                // Проверяем ветку
                DiscordThreadChannel thread = null;
                DiscordMessage mainMessage = null;

                if (botSaveData.QuestThreads.TryGetValue(type, out var threadData) && threadData.ThreadId != 0 && threadData.MessageId != 0)
                {
                    //thread = guild.Threads.ContainsKey(threadData.ThreadId) ? guild.Threads[threadData.ThreadId] : null;

                    var threads = await guild.ListActiveThreadsAsync();
                    thread = threads.Threads.First(p => p.Id == threadData.ThreadId);

                    if (thread != null)
                    {
                        try
                        {
                            mainMessage = await questChannel.GetMessageAsync(threadData.MessageId);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"[{bot.Id}] Сообщение для {type} не найдено — будет пересоздано ({ex.Message})");
                            Program.BotManager.SendAdminMessage($"[{bot.Id}] Сообщение для {type} не найдено — будет пересоздано ({ex.Message})");
                            mainMessage = null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[{bot.Id}] Ветка {type} не найдена — будет пересоздана");
                        Program.BotManager.SendAdminMessage($"[{bot.Id}] Ветка {type} не найдена — будет пересоздана");
                    }
                }
                else
                {
                    needCreate = true;
                }

                if (thread == null || mainMessage == null)
                    needCreate = true;

                if (needCreate)
                {
                    try
                    {
                        var newMsg = await questChannel.SendMessageAsync(StringTemplate.Format($"# {title}\n{desc}",variables));

                        var newThread = await newMsg.CreateThreadAsync($"{title} • {DateTime.UtcNow:dd.MM.yyyy}", AutoArchiveDuration.Week);

                        // Сохраняем
                        botSaveData.QuestThreads[type] = new QuestListThreadData
                        {
                            ThreadId = newThread.Id,
                            MessageId = newMsg.Id
                        };

                        bot.SaveData = botSaveData;

                        Console.WriteLine($"[{bot.Id}] Создана ветка {newThread.Name} для типа {type}");
                        Program.BotManager.SendAdminMessage($"[{bot.Id}] Создана ветка {newThread.Name} для типа {type}");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"[{bot.Id}] Ошибка создания квест-ветки - {ex}");
                        Program.BotManager.SendAdminMessage($"[{bot.Id}] Ошибка создания квест-ветки - {ex}");
                    }
                    

                    
                }
            }
        }

        public static async Task UpdateQuestThread(LeaderBot bot, QuestType type)
        {
            var variables = new Dictionary<string, string>
            {
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            var guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);

            var utcStartDate = DateTime.SpecifyKind(Program.QuestManager.StartDate, DateTimeKind.Utc);
            var startUnixTime = ((DateTimeOffset)utcStartDate).ToUnixTimeSeconds();

            if (!bot.SaveData.QuestThreads.ContainsKey(type))
            {
                Console.WriteLine($"[{bot.Id}] Нет ветки для {type}, запускаю создание...");
                await EnsureQuestThreads(bot);
            }

            var threadData = bot.SaveData.QuestThreads[type];

            var threads = await guild.ListActiveThreadsAsync();
            var thread = threads.Threads.First(p => p.Id == threadData.ThreadId);
            var botSaveData = bot.SaveData;
            var botPersonality = bot.Personality;

            if (thread == null)
            {
                Console.WriteLine($"[{bot.Id}] Не найдена ветка {threadData.ThreadId} — пересоздаю...");
                await EnsureQuestThreads(bot);
                threadData = botSaveData.QuestThreads[type];
                threads = await guild.ListActiveThreadsAsync();
                thread = threads.Threads.First(p => p.Id == threadData.ThreadId);
            }

            var active = Program.QuestManager.GetActiveQuests(team: bot.Id)
                .Where(q => q.QuestType == type)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"# {StringTemplate.Format(botPersonality.QuestTypeTitles.GetValueOrDefault(type, type.ToString()), variables)}");
            sb.AppendLine($"_(Обновлено <t:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}:R>)_\n");

            if (type == QuestType.Daily || type == QuestType.Weekly)
            {
                DateTime periodStart;
                DateTime periodEnd;

                var currentDay = (DateTime.UtcNow - Program.QuestManager.StartDate).Days;
                var currentWeek = currentDay / 7;

                if (type == QuestType.Daily)
                {
                    periodStart = utcStartDate.AddDays(currentDay);
                    periodEnd = periodStart.AddDays(1);
                }
                else
                {
                    
                    periodStart = utcStartDate.AddDays(currentWeek * 7);
                    periodEnd = periodStart.AddDays(7);
                }

                var startUnix = ((DateTimeOffset)periodStart).ToUnixTimeSeconds();
                var endUnix = ((DateTimeOffset)periodEnd).ToUnixTimeSeconds();

                sb.AppendLine($"**Задания от <t:{startUnix}:D>**");
                sb.AppendLine($"**Успейте их выполнить до <t:{endUnix}:D>**\n");
            }

            foreach (var q in active)
            {
                var utcAvalableTime = DateTime.SpecifyKind(q.AvalableLimit, DateTimeKind.Utc);
                var avalableUnixTime = ((DateTimeOffset)utcAvalableTime).ToUnixTimeSeconds();

                string expires = q.AvalableLimit == DateTime.MaxValue
                    ? "<Бессрочное>"
                    : $"Истекает: <t:{avalableUnixTime}:R>";

                string status = !q.Quest.LimitReached(bot.Id) ? "" : "~~";

                sb.AppendLine($"# {status}`[{q.Index + 1}]` **{q.Quest.Title}** {status}\n");
                sb.AppendLine($"{status} _{q.Quest.Description}_ {status}\n{status}(+{q.Quest.Reward}₧) [{(q.Quest.CompletionsByTeams.ContainsKey(bot.Id) ? q.Quest.CompletionsByTeams[bot.Id] : "0")}/{q.Quest.CompletionLimit}] {(q.Quest.GlobalLimit ? "[Глоб]" : "[По командам]")} {expires}{status}");
            }

            if (active.Count == 0)
            {
                sb.AppendLine("_Нет активных заданий._");
            }


            DiscordMessage myMsg = null;
            try
            {
                myMsg = await thread.GetMessageAsync(threadData.ThreadMessageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{bot.Id}] Сообщение {threadData.ThreadMessageId} в ветке {thread.Name} не найдено или не доступно. Причина: {ex.Message}");
            }

            if (myMsg != null)
            {
                await myMsg.ModifyAsync(sb.ToString());
                Console.WriteLine($"[{bot.Id}] Сообщение обновлено в ветке {thread.Name}");
            }
            else
            {
                var newMsg = await thread.SendMessageAsync(sb.ToString());
                threadData.ThreadMessageId = newMsg.Id;

                botSaveData.QuestThreads[type] = threadData;
                bot.SaveData = botSaveData;

                Console.WriteLine($"[{bot.Id}] Новое сообщение создано в ветке {thread.Name}");
            }
        }

        public static async Task CheckAndAnnounceNewQuests(LeaderBot bot, QuestType type)
        {
            var variables = new Dictionary<string, string>
            {
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            var activeQuests = Program.QuestManager.GetActiveQuests(team: bot.Id)
                .Where(q => q.QuestType == type)
                .ToList();

            var guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);
            var globalChannel = guild.GetChannel(bot.SaveData.GlobalChannelId);

            foreach (var q in activeQuests)
            {
                if (!q.Quest.PresentedByTeams.ContainsKey(bot.Id))
                    q.Quest.PresentedByTeams[bot.Id] = false;

                if (!q.Quest.PresentedByTeams[bot.Id])
                {
                    var rand = new Random();
                    string phrase = bot.Personality.QuestPresentGlobalPhrases[
                        rand.Next(bot.Personality.QuestPresentGlobalPhrases.Count)
                    ];

                    string mainMessage = $"## Новый квест **\"{q.Quest.Title}\"**!";

                    try
                    {
                        await globalChannel.SendMessageAsync($"{mainMessage}\n\n_{StringTemplate.Format(phrase, variables)}_");

                        Console.WriteLine($"Check {q.Quest.Title}");
                        Program.QuestManager.GetOriginalQuest(q).PresentedByTeams[bot.Id] = true;
                    }
                    catch(Exception ex)
                    {
                        var message = $"Ошибка анонса квестов: {bot.Id} - {q.Quest.Title} \n{ex}";
                        Console.WriteLine(message);
                        Program.BotManager.SendAdminMessage(message);
                    }
                    

                    
                }
            }

            Program.QuestManager.SaveQuests();
        }

        private static async Task<DiscordMessageBuilder> ForwardFile(this DiscordMessageBuilder message, DiscordInteractionDataOption? option, DiscordInteraction interaction)
        {
            if (option == null)
                throw new NullReferenceException("option was null");

            if (!interaction.Data.Resolved.Attachments.TryGetValue((ulong)option.Value, out var attachment))
                throw new Exception("Вложение не найдено");

            var http = new HttpClient();
            var fileBytes = await http.GetStreamAsync(attachment.Url);

            var memoryStream = new MemoryStream();
            await fileBytes.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            message.AddFile(attachment.FileName, memoryStream);
            return message;
        }

        public static async Task CheckAndAnnounceCompletedQuests(LeaderBot bot, QuestType type)
        {
            var variables = new Dictionary<string, string>
            {
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            var activeQuests = Program.QuestManager.GetActiveQuests(team: bot.Id);

            var guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);
            var globalChannel = guild.GetChannel(bot.SaveData.GlobalChannelId);

            foreach (var q in activeQuests)
            {
                if (!q.Quest.CompletedByTeams.ContainsKey(bot.Id))
                    q.Quest.CompletedByTeams[bot.Id] = false;

                if (q.Quest.LimitReached(bot.Id) && !q.Quest.CompletedByTeams[bot.Id] && q.Quest.CompletionsByTeams.ContainsKey(bot.Id) && q.Quest.CompletionsByTeams[bot.Id] > 0)
                {
                    var rand = new Random();
                    string phrase = bot.Personality.QuestCompletedGlobalPhrases[
                        rand.Next(bot.Personality.QuestCompletedGlobalPhrases.Count)
                    ];

                    try
                    {
                        string message = $"## Квест **\"{q.Quest.Title}\"** был завершён командой **{bot.Personality.TeamName}**!";
                        await globalChannel.SendMessageAsync($"{message}\n\n_{phrase}_");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"{ex}");
                    }


                    Program.QuestManager.GetOriginalQuest(q).CompletedByTeams[bot.Id] = true;
                }
                else if(q.Quest.LimitReached(bot.Id) && !q.Quest.CompletedByTeams[bot.Id])
                {
                    var rand = new Random();
                    string phrase = bot.Personality.QuestSnatchedGlobalPhrases[
                        rand.Next(bot.Personality.QuestSnatchedGlobalPhrases.Count)
                    ];

                    try
                    {
                        string message = $"## Квест **\"{q.Quest.Title}\"** был спизжен у команды **{bot.Personality.TeamName}**!";
                        await globalChannel.SendMessageAsync(StringTemplate.Format($"{message}\n\n_{phrase}_", variables));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex}");
                    }

                    Program.QuestManager.GetOriginalQuest(q).CompletedByTeams[bot.Id] = true;
                }
            }

            Program.QuestManager.SaveQuests();
        }

        public static async Task CheckAndAnnounceExpiredQuests(LeaderBot bot, QuestType type)
        {
            var variables = new Dictionary<string, string>
            {
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            string team = bot.Id;
            var fullQuests = new List<QuestInTime>();
            int globalIndex = 0;

            if (type == QuestType.Daily)
            {
                foreach (var pair in Program.QuestManager.DailyQuests)
                {
                    int dayIndex = pair.Key;
                    var questList = pair.Value;
                    int i = 0;
                    foreach (var q in questList)
                    {
                        fullQuests.Add(new QuestInTime
                        {
                            Quest = q,
                            QuestType = QuestType.Daily,
                            AvalableLimit = Program.QuestManager.StartDate.AddDays(dayIndex + 1),
                            Index = globalIndex++,
                            BaseIndex = new[] { dayIndex, i }
                        });
                        i++;
                    }
                }
            }
            else if (type == QuestType.Weekly)
            {
                foreach (var pair in Program.QuestManager.WeeklyQuests)
                {
                    int weekIndex = pair.Key;
                    var questList = pair.Value;
                    int i = 0;
                    foreach (var q in questList)
                    {
                        fullQuests.Add(new QuestInTime
                        {
                            Quest = q,
                            QuestType = QuestType.Weekly,
                            AvalableLimit = Program.QuestManager.StartDate.AddDays((weekIndex + 1) * 7),
                            Index = globalIndex++,
                            BaseIndex = new[] { weekIndex, i }
                        });
                        i++;
                    }
                }
            }
            else if (type == QuestType.Hard)
            {
                int i = 0;
                foreach (var q in Program.QuestManager.HardQuests)
                {
                    fullQuests.Add(new QuestInTime
                    {
                        Quest = q,
                        QuestType = QuestType.Hard,
                        AvalableLimit = DateTime.MaxValue,
                        Index = globalIndex++,
                        BaseIndex = new[] { i, 0 }
                    });
                    i++;
                }
            }
            else if (type == QuestType.Secret)
            {
                int i = 0;
                foreach (var q in Program.QuestManager.SecretQuests)
                {
                    fullQuests.Add(new QuestInTime
                    {
                        Quest = q,
                        QuestType = QuestType.Secret,
                        AvalableLimit = DateTime.MaxValue,
                        Index = globalIndex++,
                        BaseIndex = new[] { i, 0 }
                    });
                    i++;
                }
            }

            var activeQuests = Program.QuestManager.GetActiveQuests(team: team);

            var guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);
            var globalChannel = await bot._client.GetChannelAsync(bot.SaveData.GlobalChannelId);

            foreach (var quest in fullQuests)
            {
                bool wasPresented = quest.Quest.PresentedByTeams.ContainsKey(team) && quest.Quest.PresentedByTeams[team];
                bool alreadyExpired = quest.Quest.ExpiredByTeams.ContainsKey(team) && quest.Quest.ExpiredByTeams[team];
                bool wasCompleted = quest.Quest.CompletedByTeams.ContainsKey(team) && quest.Quest.CompletedByTeams[team];

                bool isActiveNow = activeQuests.Any(q =>
                    q.QuestType == quest.QuestType &&
                    q.BaseIndex[0] == quest.BaseIndex[0] &&
                    q.BaseIndex[1] == quest.BaseIndex[1]
                );

                if(globalChannel == null)
                { 
                    var log = $"[{bot.Id}] - globalChannel оказался null и я хз почему" +
                        $"\nglobalChannel: {globalChannel}, GlobalChannelId: {bot.SaveData.GlobalChannelId}, Guild: {guild}, ({guild.GetChannel(bot.SaveData.GlobalChannelId)})";
                    var ex = new NullReferenceException(log);
                    Console.WriteLine($"{ex} - {bot._client.ToString()}");
                    await Program.BotManager.SendAdminMessage(ex.ToString());
                    return;
                }    
                
                if (wasPresented && !isActiveNow && !alreadyExpired && !wasCompleted)
                {
                    var rand = new Random();
                    string phrase = bot.Personality.QuestExpiredGlobalPhrases[
                        rand.Next(bot.Personality.QuestExpiredGlobalPhrases.Count)
                    ];
                    string message = $"## Квест **\"{quest.Quest.Title}\"** просрочен командой **{bot.Personality.TeamName}**.";
                    await globalChannel.SendMessageAsync(StringTemplate.Format($"{message}\n\n_{phrase}_", variables));

                    Quest originalQuest = Program.QuestManager.GetOriginalQuest(quest);

                    if (!originalQuest.ExpiredByTeams.ContainsKey(team))
                        originalQuest.ExpiredByTeams[team] = true;
                    else
                        originalQuest.ExpiredByTeams[team] = true;

                    Console.WriteLine($"[{bot.Id}] Объявлено просроченное задание: {quest.Quest.Title}");
                }
            }

            Program.QuestManager.SaveQuests();
        }

        public static async Task HandleComponent(LeaderBot bot, InteractionCreateEventArgs e, GlobalGameState state)
        {
            var variables = new Dictionary<string, string>
            {
                ["caller"] = e.Interaction.User.Mention,
                ["caller-name"] = e.Interaction.User.Username,
                ["team-name"] = bot.Personality.TeamName,
                ["bot-id"] = bot.Id,
                ["time-now"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["time-start"] = Program.QuestManager.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["guild-name"] = e.Interaction.Guild?.Name ?? "",
                ["guild-id"] = $"{(e.Interaction.Guild?.Id ?? 0)}",
                ["score"] = bot.SaveData.TeamScore.ToString(),
                ["n"] = "\n"
            };

            if (e.Interaction.Type != InteractionType.Component)
                return;

            if (!e.Interaction.Data.CustomId.StartsWith("accept") && !e.Interaction.Data.CustomId.StartsWith("reject"))
                return;

            var parts = e.Interaction.Data.CustomId.Split('|');
            var action = parts[0];
            var questType = parts[1];
            var questIndex0 = int.Parse(parts[2]);
            var questIndex1 = int.Parse(parts[3]);
            var playerId = ulong.Parse(parts[4]);
            var messageId = ulong.Parse(parts[5]);

            Console.WriteLine($"[{bot.Id}] [Component] {action} quest={questType} index0={questIndex0} index1={questIndex1} player={playerId} messageId={messageId}");

            bool accept = action == "accept";
            Quest quest = questType switch
            {
                "Daily" => Program.QuestManager.DailyQuests[questIndex0][questIndex1],
                "Weekly" => Program.QuestManager.WeeklyQuests[questIndex0][questIndex1],
                "Hard" => Program.QuestManager.HardQuests[questIndex0],
                "Secret" => Program.QuestManager.SecretQuests[questIndex0],
                _ => null
            };

            if (quest == null)
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"Ошибка: не найден квест типа {questType}")
                        .AsEphemeral(true));
                return;
            }

            bool reached = quest.LimitReached(bot.Id);

            var guild = await bot._client.GetGuildAsync(Program.BotManager.GlobalState.GuildId);
            var checkChannel = guild.GetChannel(Program.BotManager.GlobalState.QuestCheckChannelId);
            var msg = await checkChannel.GetMessageAsync(messageId);

            // если лимит уже исчерпан, уведомляем проверяющего
            if (reached)
            {
                quest.RemoveCheck(playerId);

                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"⚠️ Квест \"{quest.Title}\" больше не доступен для выполнения — лимит исчерпан.")
                        .AsEphemeral(true));

                // пробуем обновить исходное сообщение
                try
                {
                    await msg.ModifyAsync(new DiscordMessageBuilder()
                        .WithContent(msg.Content + $"\n\n⛔️ Действие не выполнено — лимит по квесту исчерпан."));

                    Console.WriteLine($"[{bot.Id}] Изменено сообщение {messageId} (лимит исчерпан)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{bot.Id}] Ошибка при обновлении сообщения (лимит исчерпан): {ex}");
                }

                return;
            }

            // пытаемся выполнить действие
            try
            {
                if (accept)
                {
                    quest.NoteCompletion(bot.Id, playerId);
                    bot.AddPoints(playerId, quest.Reward);

                    if(quest.LimitReached(bot.Id))
                    {
                        foreach (var messageData in quest.ActiveConfirmMessages)
                        {
                            if (!quest.GlobalLimit && messageData.Key != bot.Id)
                                continue;
                            
                            var message = await checkChannel.GetMessageAsync(messageData.Value);

                            await message.ModifyAsync(new DiscordMessageBuilder()
                                .WithContent(message.Content + $"\n\n⛔️ Действие не выполнено — лимит по квесту исчерпан."));

                            Console.WriteLine($"[{bot.Id}] Изменено сообщение {message} (лимит исчерпан)");
                        }
                    }
                }
                else
                {
                    quest.RemoveCheck(playerId);
                }

                Program.QuestManager.SaveQuests();
            }
            catch (Exception ex)
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .WithContent($"# ПАРАПАРАПАМ!!!!\nЧто-то пошло не так:\n*{ex}*")
                        .AsEphemeral(true));
                return;
            }

            // выберем фразу
            var random = new Random();
            var phrases = accept ? bot.Personality.QuestAcceptedDMPhrases : bot.Personality.QuestRejectedDMPhrases;
            var selectedPhrase = phrases.Count > 0 ? phrases[random.Next(phrases.Count)] : "";

            // пишем пользователю в ЛС
            try
            {
                var member = await e.Interaction.Guild.GetMemberAsync(playerId);
                variables["user"] = member.Mention;
                var dmChannel = await member.CreateDmChannelAsync();

                if (accept)
                {
                    await dmChannel.SendMessageAsync(
                        $"# Твое выполнение квеста **\"{quest.Title}\"** было ✅ **подтверждено**." +
                        $"\nТы заработал для своей команды **{quest.Reward} очков**!" +
                        $"\nТеперь у вашей команды {bot.SaveData.TeamScore} очков." +
                        $"\n\n_{StringTemplate.Format(selectedPhrase,variables)}_");
                }
                else
                {
                    await dmChannel.SendMessageAsync(
                        $"# Твое выполнение квеста **\"{quest.Title}\"** было ❌ **отвергнуто**." +
                        $"\n\n_{StringTemplate.Format(selectedPhrase, variables)}_");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{bot.Id}] Не удалось отправить ЛС пользователю {playerId}: {ex}");
            }

            var user = await bot._client.GetUserAsync(playerId);
            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .WithContent($"Выполнение квеста **\"{quest.Title}\"** пользователя {user.Mention} было {(accept ? "✅ **подтверждено**" : "❌ **отвергнуто**")} пользователем {e.Interaction.User.Mention}\n\n_{StringTemplate.Format(selectedPhrase, variables)}_")
                    .AsEphemeral(true));

            try
            {


                await msg.ModifyAsync(new DiscordMessageBuilder()
                    .WithContent(msg.Content + $"\n\n⛔️ Действие завершено: {(accept ? "✅ Подтверждено" : "❌ Отклонено")} пользователем {e.Interaction.User.Mention}"));

                Console.WriteLine($"[{bot.Id}] Изменено сообщение {messageId} (результат: {action})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{bot.Id}] Ошибка при обновлении сообщения: {ex}");
            }

            Program.BotManager.UpdateTeamsQuests();
        }

        public static async Task UpdateAllQuestsStates(LeaderBot bot)
        {
            foreach (var type in Enum.GetValues<QuestType>())
            {
                await CheckAndAnnounceNewQuests(bot, type);
                await CheckAndAnnounceCompletedQuests(bot, type);
                await CheckAndAnnounceExpiredQuests(bot, type);
                await UpdateQuestThread(bot, type);
            }
        }

        static List<DiscordApplicationCommandOption> AddQuestOptions()
        {
            return new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption("type", "Тип квеста", ApplicationCommandOptionType.String, true,
                    choices: new List<DiscordApplicationCommandOptionChoice>
                    {
                        new("Ежедневный", "daily"),
                        new("Еженедельный", "weekly"),
                        new("Сложный", "hard"),
                        new("Секретный", "secret")
                    }),
                new DiscordApplicationCommandOption("title", "Заголовок", ApplicationCommandOptionType.String, true),
                new DiscordApplicationCommandOption("description", "Описание", ApplicationCommandOptionType.String, true),
                new DiscordApplicationCommandOption("reward", "Награда", ApplicationCommandOptionType.Integer, true),
                new DiscordApplicationCommandOption("limit", "Лимит выполнений", ApplicationCommandOptionType.Integer, true),
                new DiscordApplicationCommandOption("day-or-week", "Номер дня (для daily) или недели (для weekly)", ApplicationCommandOptionType.Integer, false),
                new DiscordApplicationCommandOption("global-limit", "Глобальный лимит", ApplicationCommandOptionType.Boolean, false),
                new DiscordApplicationCommandOption("help-text", "Текст подсказки", ApplicationCommandOptionType.String, false),
                new DiscordApplicationCommandOption("explanation", "Описание для админов", ApplicationCommandOptionType.String, false)
            };
        }

        static List<DiscordApplicationCommandOption> EditOrDeleteQuestCommandOptions()
        {
            return new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption("type", "Тип квеста", ApplicationCommandOptionType.String, true,
                    choices: new List<DiscordApplicationCommandOptionChoice>
                    {
                        new("Ежедневный", "daily"),
                        new("Еженедельный", "weekly"),
                        new("Сложный", "hard"),
                        new("Секретный", "secret")
                    }),
                new DiscordApplicationCommandOption("index", "Индекс квеста (в списке или внутри дня/недели)", ApplicationCommandOptionType.Integer, true),
                new DiscordApplicationCommandOption("day-or-week", "Номер дня (для daily) или недели (для weekly)", ApplicationCommandOptionType.Integer, false),
                new DiscordApplicationCommandOption("title", "Новый заголовок", ApplicationCommandOptionType.String, false),
                new DiscordApplicationCommandOption("description", "Новое описание", ApplicationCommandOptionType.String, false),
                new DiscordApplicationCommandOption("reward", "Новая награда", ApplicationCommandOptionType.Integer, false),
                new DiscordApplicationCommandOption("limit", "Новый лимит выполнений", ApplicationCommandOptionType.Integer, false),
                new DiscordApplicationCommandOption("global-limit", "Глобальный лимит?", ApplicationCommandOptionType.Boolean, false),
                new DiscordApplicationCommandOption("help-text", "Новый текст подсказки", ApplicationCommandOptionType.String, false),
                new DiscordApplicationCommandOption("explanation", "Новое админ-пояснение", ApplicationCommandOptionType.String, false)
            };
        }

        private static List<string> BuildQuestList(Dictionary<int, List<Quest>> questDict, QuestType type, DateTime startDate)
        {
            var pages = new List<string>();
            var sb = new StringBuilder();

            var utcStartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var startUnixTime = ((DateTimeOffset)utcStartDate).ToUnixTimeSeconds();

            // Заголовок
            sb.AppendLine($"# 📋 **{(type == QuestType.Daily ? "Ежедневные" : "Еженедельные")} квесты**");
            sb.AppendLine($"*(Всего {(type == QuestType.Daily ? "дней" : "недель")}: {questDict.Count})*");
            sb.AppendLine($"## 📅 Начало отсчета: **<t:{startUnixTime}:f>**\n");

            // Определяем текущий день/неделю
            var currentDay = (DateTime.UtcNow - startDate).Days;
            var currentWeek = currentDay / 7;

            int maxIndex = questDict.Keys.Count > 0 ? questDict.Keys.Max() : 0;
            maxIndex += 5; // запас на будущее

            for (int i = 0; i <= maxIndex; i++)
            {
                var date = utcStartDate.AddDays(type == QuestType.Weekly ? i * 7 : i);

                string dateRange = type switch
                {
                    QuestType.Daily =>
                        $"<t:{((DateTimeOffset)date).ToUnixTimeSeconds()}:d> — <t:{((DateTimeOffset)date.AddDays(1)).ToUnixTimeSeconds()}:d>",
                    QuestType.Weekly =>
                        $"<t:{((DateTimeOffset)date).ToUnixTimeSeconds()}:d> — <t:{((DateTimeOffset)date.AddDays(7)).ToUnixTimeSeconds()}:d>",
                    _ => ""
                };

                string hereMarker = type switch
                {
                    QuestType.Daily when currentDay == i => "👉 **СЕЙЧАС** 👉 ",
                    QuestType.Weekly when currentWeek == i => "👉 **СЕЙЧАС** 👉 ",
                    _ => ""
                };

                // Заголовок дня
                //sb.AppendLine($"---\n");
                sb.AppendLine($"> ## **{(type == QuestType.Daily ? "День" : "Неделя")} {i}** — {hereMarker}{dateRange}");

                // Есть ли квесты?
                if (questDict.TryGetValue(i, out var questList) && questList.Count > 0)
                {
                    for (int y = 0; y < questList.Count; y++)
                    {
                        var q = questList[y];

                        sb.AppendLine($"> ### `**[{y}]**` **\"{q.Title}\"** (+{q.Reward}₧) — [{q.CompletionLimit}] — {(q.GlobalLimit ? "Глобальный" : "По командам")}");
                        //sb.AppendLine($"> ");
                        sb.AppendLine($"> _{q.Description}_");

                        if (!string.IsNullOrEmpty(q.Explanation))
                            sb.AppendLine($"> 🔍 *({q.Explanation})*");

                        sb.AppendLine("> "); // отступ
                    }
                }
                else
                {
                    sb.AppendLine($"> 🚫 **Нет активных квестов на этот день.**");
                }

                // Проверка длины
                if (sb.Length > 1400)
                {
                    pages.Add(sb.ToString());
                    sb.Clear();
                }
            }

            // Добавить последнюю страницу если есть
            if (sb.Length > 0)
                pages.Add(sb.ToString());

            return pages;
        }

        private static List<string> BuildQuestList(List<Quest> questList, QuestType type)
        {
            var pages = new List<string>();
            var sb = new StringBuilder();

            sb.AppendLine($"📋 **{(type == QuestType.Hard? "Сложные" : "Секретные")}** — Всего {questList.Count} квестов\n");

            int index = 0;
            foreach (var q in questList)
            {
                sb.AppendLine($"{index} — **{q.Title}** ({q.Reward}₧) — Лимит: {q.CompletionLimit} — " +
                              $"{(q.GlobalLimit ? "Глобальный" : "По командам")}");
                index++;

                if (sb.Length > 1800)
                {
                    pages.Add(sb.ToString());
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
                pages.Add(sb.ToString());

            return pages;
        }
    }
}
