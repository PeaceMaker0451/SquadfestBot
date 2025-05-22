using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PunkCommandSystem;

namespace SquadfestBot
{
    public class UserParameterType : IParameterType
    {
        public string TypeName => "discordUser";

        public Parameter Parse(string value)
        {
            return new StringParameter(value);
        }
    }

    public class UserParameter : Parameter
    {
        private string value;

        public UserParameter(string value)
        {
            this.value = value;
            this.ParameterType = new StringParameterType();
        }

        public override object GetValue()
        {
            return value;
        }
    }

    public class DiscordInteractionsManager
    {
        public CommandManager commandManager;

        public void Start()
        {
            ParameterTypeRegistry.RegisterParameterType(new UserParameterType());
            
            commandManager = new CommandManager();
        }

        public async Task CreateCommands()
        {
            Console.WriteLine("СБРОС КОМАНД");

            Program.client.Rest.DeleteAllGlobalCommandsAsync();

            await ApplyProgramCommandsToDiscordSlashCommands();

            Console.WriteLine("ВСЕ КОМАНДЫ БЫЛИ ПЕРЕСОЗДАНЫ ЗАНОВО.\nЗАКОММЕНТИРУЙ ЭТИ СТРОКИ, ЕСЛИ ЭТО НЕ БЫЛО НУЖНО");
        }

        private async Task ApplyProgramCommandsToDiscordSlashCommands()
        {
            foreach (var command in commandManager.CommandsList())
            {
                var globalCommand = new SlashCommandBuilder();
                globalCommand.WithName(command.Name());
                globalCommand.WithDescription(command.Description());

                foreach (var item in command.RequiredParameters())
                {
                    if(item.ParameterType is IntParameterType)
                    {
                        globalCommand.AddOption(item.name, ApplicationCommandOptionType.Integer, item.description, isRequired: true);
                    }
                    else if(item.ParameterType is StringParameterType)
                    {
                        globalCommand.AddOption(item.name, ApplicationCommandOptionType.String, item.description, isRequired: true);
                    }
                    else if(item.ParameterType is FloatParameterType)
                    {
                        globalCommand.AddOption(item.name, ApplicationCommandOptionType.Number, item.description, isRequired: true);
                    }
                    else if(item.ParameterType is UserParameterType)
                    {
                        globalCommand.AddOption(item.name, ApplicationCommandOptionType.User, item.description, isRequired: true);
                    }
                }

                await Program.client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
        }

        public async Task SlashCommandReceiver(SocketSlashCommand command)
        {
            string _command = "";

            _command += command.Data.Name;


            _command += " ";

            foreach (var option in command.Data.Options)
            {
                Console.WriteLine($"{option.Type.GetTypeCode()}");

                if (option.Type == ApplicationCommandOptionType.String)
                {
                    _command += "\"";
                    _command += option.Value;
                    _command += "\"";
                }
                else if (option.Type == ApplicationCommandOptionType.User)
                {
                    _command += ((SocketUser)option.Value).Username;
                }
                /*else if (option.Type == ApplicationCommandOptionType.Channel)
                {
                    _command += ((SocketUser)option.Value).Id;
                }*/
                else
                {
                    _command += option.Value;
                }
                _command += " ";
            }

            Console.WriteLine(_command);

            ulong guildId = (ulong)command.GuildId;

            SocketGuild socketGuild = Program.client.GetGuild(guildId);

            await command.RespondAsync(commandManager.ExecuteCommand(_command));


        }


        public string CommandsList()
        {
            string result = "";
            foreach (var item in commandManager.CommandsList())
            {
                result += $"{item.Name()} - {item.Description()} {item.RequiredParameters()}";
                result += "\n";
            }

            return result;
        }
    }
}
