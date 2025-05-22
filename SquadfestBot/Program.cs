using Discord.WebSocket;
using Discord;
using System;
using PunkCommandSystem;

namespace SquadfestBot
{
    internal static class Program
    {
        public static DiscordSocketClient client;

        public static DiscordInteractionsManager interactionsManager;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
            interactionsManager = new DiscordInteractionsManager();
            
            DiscordSocketConfig config = new DiscordSocketConfig();
            config.GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent;
            client = new DiscordSocketClient(config);

            //client.MessageReceived += CommandsHandler;
            client.Log += ConsoleLog;
            client.SlashCommandExecuted += interactionsManager.SlashCommandReceiver;
            client.Ready += AddCommands;
            var token = "~~~~";

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();


            while (true)
                try
                {
                    interactionsManager.commandManager.ExecuteCommand(Console.ReadLine());
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
        }

        private static Task AddCommands()
        {
            interactionsManager.Start();

            var exampleCommand = new Command(
            _name: "example",
            _action: (p) => { return "Hello!"; },
            _parameters: new List<RequiredParameter>
            {
                new RequiredParameter(new IntParameterType(), "Number of repetitions"),
                new RequiredParameter(new StringParameterType(), "Message")
            }
);
            interactionsManager.commandManager.AddCommand(exampleCommand);



            Console.WriteLine("Команды проинициализированы");
            Console.WriteLine("----------------------------------");

            interactionsManager.CommandsList();

            interactionsManager.CreateCommands();

            return Task.CompletedTask;
        }

        private static Task ConsoleLog(LogMessage msg)
        {
            return ConsoleLog(msg.ToString());
        }

        private static Task ConsoleLog(string msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        //private static Task CommandsHandler(SocketMessage msg)
        //{
        //    //InputManager.RunCommandFromDiscord(msg);

        //    return Task.CompletedTask;
        //}
    }
}