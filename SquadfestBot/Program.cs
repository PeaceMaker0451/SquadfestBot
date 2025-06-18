using System;
using SquadfestBot;
using DSharpPlus;
using DSharpPlus.Entities;
using PunkCommandSystem;

public class Program
{
    public static LeaderBotManager BotManager = new LeaderBotManager();
    public static async Task Main(string[] args)
    {
        await BotManager.StartAllAsync();

        await Task.Delay(-1);
    }
}

