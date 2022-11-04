﻿using RomTools.Services;
using RomTools.Services.CommandLineParser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace RomTools;

[ExcludeFromCodeCoverage]
public static class Program
{
    static async Task<int> Main()
    {
        using IHost host = CreateHostBuilder(null).Build();

        var program = host.Services.GetService<IProgram>(); ;
        return await program.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) => services
        .AddSingleton<ICommandLineArgumentService, CommandLineArgumentsService>()
        .AddSingleton<ICommandLineParserService, CommandLineParserService>((IServiceProvider _) => { return CommandLineParserService.CreateDefaultInstance(); })
        .AddSingleton<IHelpMessageFormatter, HelpMessageFormatter>()
        .AddSingleton<IPruneRomsService, PruneRomsService>()
        .AddSingleton<IProgram, RomToolsProgram>());
}