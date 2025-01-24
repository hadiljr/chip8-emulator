using System;
using System.IO;
using Emulator;
using Hardware.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        ConfigureLogger();

        var logger = Logger.Instance.GetLogger<Program>();

        var arg = args.Length > 0 ? args[0] : "autor";
        logger.LogTrace("Starting emulator with argument: {arg}", arg);

        try
        {
            var romPath = CheckSpecialCommands(arg);
            using var emulator = new Chip8Emulator(romPath);
            emulator.Run();
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Error running emulator");
            Environment.ExitCode = 1;
        }
    }


    private static void ConfigureLogger()
    {
        //get config
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        //create logger
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(config.GetSection("Logging"));
            builder.AddConsole(); // Add console logging
        });

        //Configure custom singleton Logger
        Logger.Instance.Configure(loggerFactory);
    }

    private static string CheckSpecialCommands(string command)
    {
        switch (command)
        {
            case "autor":
                return "Content/roms/hadil_demo.ch8";
            default:
                if (!File.Exists(command))
                {
                    throw new FileNotFoundException("File not found");
                }
                return command;
        }
    }
}