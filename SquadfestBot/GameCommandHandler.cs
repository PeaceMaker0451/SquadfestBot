using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SquadfestBot
{
    public static class GameCommandHandler
    {
        public static List<DiscordApplicationCommand> Commands = new List<DiscordApplicationCommand>()
        {
            new DiscordApplicationCommand("ping", "Пингует лидера"),
            new DiscordApplicationCommand(
            name: "say",
            description: "Говорить от лица Лидера",
            defaultMemberPermissions: Permissions.Administrator,
            options: new List<DiscordApplicationCommandOption>
            {
                new DiscordApplicationCommandOption(
                    name: "text",
                    description: "Что сказать",
                    type: ApplicationCommandOptionType.String,
                    required: true
                ),
                new DiscordApplicationCommandOption(
                    name: "channel-id",
                    description: "Куда сказать",
                    type: ApplicationCommandOptionType.Channel,
                    required: true
                )
            }),
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
                )
            }),
            new DiscordApplicationCommand(
            "team",
            "Управление командой",
            new List<DiscordApplicationCommandOption>
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
            })
        };

        public static async Task Handle(LeaderBot bot, InteractionCreateEventArgs e, GlobalGameState state)
        {
            var playerId = e.Interaction.User.Id;

            if (e.Interaction.Data.Name == "say")
            {
                // Получаем аргументы
                var textOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "text");
                var channelOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "channel-id");

                if (textOption == null || channelOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                // Значения аргументов
                string text = textOption.Value.ToString();
                ulong channelId = Convert.ToUInt64(channelOption.Value);
                var targetChannel = await bot._client.GetChannelAsync(channelId);

                // Отправляем сообщение в указанный канал
                await targetChannel.SendMessageAsync(text);

                // Эфемерный ответ пользователю
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Сообщение отправлено в канал: {targetChannel.Name}")
                    .AsEphemeral(true));
            }

            if (e.Interaction.Data.Name == "dm")
            {
                var userOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "user");
                var textOption = e.Interaction.Data.Options.FirstOrDefault(opt => opt.Name == "text");

                if (userOption == null || textOption == null)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("Ошибка: не указаны аргументы!")
                        .AsEphemeral(true));
                    return;
                }

                ulong userId = Convert.ToUInt64(userOption.Value);
                string text = textOption.Value.ToString();

                try
                {
                    // Получаем DiscordMember (а не просто DiscordUser)
                    var member = await e.Interaction.Guild.GetMemberAsync(userId);

                    // Создаём DM-канал (через Client)
                    var dmChannel = await member.CreateDmChannelAsync();

                    // Отправляем сообщение
                    await dmChannel.SendMessageAsync(text);

                    // Ответ пользователю команды (ephemeral)
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"Сообщение отправлено пользователю: {member.DisplayName}")
                        .AsEphemeral(true));
                }
                catch (Exception ex)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"Не удалось отправить ЛС ({ex.Message})")
                        .AsEphemeral(true));
                }
            }

            if (e.Interaction.Data.Name == "team")
            {
                var subCommand = e.Interaction.Data.Options.First().Name;

                var userOption = (ulong)e.Interaction.Data.Options.First().Options.First().Value;
                var member = await e.Interaction.Guild.GetMemberAsync(userOption);

                if (subCommand == "add")
                {
                    if(member.IsBot)
                    {
                        await e.Interaction.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent(bot.SaveData.TriedToAddBotInTheTeamMessage)
                            .AsEphemeral(false)
                            );
                        return;
                    }
                    
                    if (bot.Players.Any(p => p.Id == userOption))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.AlreadyInThisCommandMessage ?? "Ты уже в этой команде."));
                        return;
                    }
                    else if (Program.BotManager.Bots.Any(b => b.Players.Any(p => p.Id == userOption)))
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.AlreadyInOtherCommandMessage ?? "Ты уже в другой команде."));
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
                    await dmChannel.SendMessageAsync(bot.SaveData.AddedToTheTeamDMMessage);

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
                        await dmChannel.SendMessageAsync(bot.SaveData.RemovedFromTheTeamDMMessage);
                        
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

            //if (e.Interaction.Data.Name == "add-me")
            //{
            //    if (bot.Players.Any(p => p.Id == playerId))
            //    {
            //        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            //            new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.AlreadyInThisCommandMessage ?? "Ты уже в этой команде."));
            //        return;
            //    }
            //    else if (Program.BotManager.Bots.Any(b => b.Players.Any(p => p.Id == playerId)))
            //    {
            //        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            //        new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.AlreadyInOtherCommandMessage ?? "Ты уже в другой команде."));
            //    }
            //    else
            //    {
            //        var players = bot.Players;
            //        players.Add(new PlayerData(e.Interaction.User.Id, e.Interaction.User.Username));
            //        bot.SavePlayers(players);

            //        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            //        new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.AddedToTheTeamMessage ?? "Ты был добавлен в эту команду"));
            //    }
            //    return;
            //}

            if (e.Interaction.Data.Name == "ping")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.PingMessage ?? "Пинг-понг!"));
                return;
            }

            if (!bot.Players.Any(p => p.Id == playerId))
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(bot.SaveData.NotInThisTeamMessage ?? "Ты не в этой команде."));
                return;
            }

            

            // Тут добавляй другие команды...
        }
    }
}
