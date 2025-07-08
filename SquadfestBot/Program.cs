using System;
using SquadfestBot;
using DSharpPlus;
using DSharpPlus.Entities;
using PunkCommandSystem;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static LeaderBotManager BotManager = new LeaderBotManager();
    public static QuestManager QuestManager = new QuestManager();

    public static bool ServiceMode { get; private set; } = false;
    public static Action ServiceModeOn;
    public static async Task Main(string[] args)
    {
        await StartAsync();
        await Task.Delay(-1);
    }

    public static async Task StartAsync()
    {
        ServiceMode = false;
        await BotManager.StartAllAsync();
        BotManager.StartQuestUpdateLoop(TimeSpan.FromMinutes(25));
    }

    public static async Task RestartAsync()
    {
        Console.WriteLine("Жёсткий перезапуск LeaderBotManager...");

        try
        {
            BotManager.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при dispose: {ex.Message}");
        }

        // Новый менеджер и квесты
        BotManager = new LeaderBotManager();
        QuestManager = new QuestManager();

        await StartAsync();

        Console.WriteLine("Перезапуск завершён.");
    }

    public static void TurnOnServiceMode()
    {
        ServiceMode = true;
        ServiceModeOn?.Invoke();
    }
}

