﻿using Application.Models;
using Application.Queues;
using Application.Readers;
using Application.Searchers;
using Application.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace Application
{
    class Program
    {
        private static ILogger<Program> Logger { get; set; }

        static void Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            IServiceProvider provider = ConfigureServices(serviceCollection, args);
            Logger = provider.GetRequiredService<ILogger<Program>>();

            if (args.Length != Constants.ArgumentLength)
            {
                Logger.LogError("Expecting 4 command line arguments");
                return;
            }

            if (args[1].Length != Constants.WordLength && args[2].Length != Constants.WordLength)
            {
                Logger.LogError("The start and end word must both be 4 letters long");
                return;
            }

            try
            {
                using (IPathFinder<Node> searcher = provider.GetRequiredService<IPathFinder<Node>>())
                {
                    Stopwatch timer = new Stopwatch();

                    Node rootNode = new Node(args[1]);
                    IQueue<Node> queue = new DistinctQueue<Node>(
                        provider.GetRequiredService<ILogger<DistinctQueue<Node>>>());

                    queue.Enqueue(rootNode);

                    Logger.LogInformation($"Finding shortest sequence of 4 " +
                        $"letter words between {args[1]} and {args[2]}");

                    timer.Start();
                    Node foundNode = searcher.Search(new Node(args[1]), new Node(args[2]));
                    timer.Stop();

                    Logger.LogInformation($"Search completed in {timer.ElapsedMilliseconds}ms");

                    IWriter<Node> resultWriter = provider.GetRequiredService<IWriter<Node>>();
                    resultWriter.Write(foundNode);
                }
            }
            catch (IOException e)
            {
                Logger.LogError("An error occurred whilst trying to " +
                    $"read the dictionary file with path {args[0]}", e);
            }

            Console.WriteLine("Press enter to exit.");
            Console.Read();
        }

        /// <summary>
        /// Gets the configuration populated with the values from the appsettings.json file.
        /// </summary>
        private static IConfigurationRoot GetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                // Assume that the appsettings.json file is in the 
                // same directory as the application.
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        /// Configues the dependency injection container with the different services for the application.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="args">The application command line arguments.</param>
        /// <returns>The built service provider.</returns>
        private static IServiceProvider ConfigureServices(
            IServiceCollection services,
            string[] args)
        {
            IConfigurationRoot configuration = GetConfiguration();

            LogLevel loggingLevel = Enum.TryParse(
                configuration.GetValue<string>("Logging:LogLevel"), out LogLevel logLevel)
                    ? logLevel
                    : LogLevel.Information;

            services
                .AddLogging(options => options
                    .AddConsole()
                    .SetMinimumLevel(loggingLevel))
                .AddTransient<IReader, DictionaryReader>(provider =>
                {
                    return new DictionaryReader(
                        args[0],
                        provider.GetRequiredService<ILogger<DictionaryReader>>());
                })
                .AddTransient<IWriter<Node>, ResultWriter>(provider =>
                {
                    return new ResultWriter(
                        args[3],
                        provider.GetRequiredService<ILogger<ResultWriter>>());
                })
                .AddTransient<IQueue<Node>, DistinctQueue<Node>>()
                .AddTransient<IPathFinder<Node>, WordBreadthFirstSearch>();

            return services.BuildServiceProvider();
        }
    }
}
