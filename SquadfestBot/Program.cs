using System;
using SquadfestBot;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

public class Program
{
    public static LeaderBotManager BotManager;
    public static QuestManager QuestManager;

    public static bool ServiceMode { get; private set; } = false;
    public static Action ServiceModeOn;
    public static async Task Main(string[] args)
    {
        try
        {
            BotManager = new LeaderBotManager();
            QuestManager = new QuestManager();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации бота - {ex}");
            Console.ReadLine();
        }
        
        await StartAsync();
        await Task.Delay(-1);
    }

    public static async Task StartAsync()
    {
        ServiceMode = false;
        try
        {
            await BotManager.StartAllAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ошибка запуска BotManager! - {ex}");
            BotManager.Dispose();
        }
        
        try
        {
            if (BotManager.Bots.Count > 0)
                BotManager.StartQuestUpdateLoop(TimeSpan.FromMinutes(BotManager.GlobalState.QuestUpdateLoopTimer));
            else
                Console.WriteLine("Не задано ни одного бота.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка запуска цикла обновления квестов! - {ex}");
        }

        
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

        BotManager = new LeaderBotManager();
        QuestManager = new QuestManager();

        await StartAsync();

        Console.WriteLine("Перезапуск завершён.");
    }

    public static void TurnOnServiceMode()
    {
        ServiceMode = true;
        ServiceModeOn?.Invoke();

        try
        {
            BotManager.StopQuestUpdateLoop();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}

