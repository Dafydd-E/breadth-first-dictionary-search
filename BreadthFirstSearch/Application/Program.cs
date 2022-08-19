using Application.Models;
using Application.PathFinders;
using Application.Queues;
using Application.Readers;
using Application.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace Application
{
    /// <summary>
    /// Console application which finds the shortest path between the specified
    /// start and end words.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The program logger instance.
        /// </summary>
        private static ILogger<Program> Logger { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        static void Main(string[] args)
        {
            IServiceProvider provider = ConfigureServices(new ServiceCollection(), args);
            Logger = provider.GetRequiredService<ILogger<Program>>();

            if (args.Length != Constants.ArgumentLength)
            {
                Logger.LogError($"Expecting {Constants.ArgumentLength} command line arguments");
                return;
            }

            if (args[1].Length != Constants.WordLength && args[2].Length != Constants.WordLength)
            {
                Logger.LogError($"The start and end word must both be {Constants.WordLength} letters long");
                return;
            }

            try
            {
                Logger.LogInformation($"Finding shortest sequence of {Constants.WordLength} " +
                    $"letter words between {args[1]} and {args[2]}");

                Stopwatch timer = new Stopwatch();
                timer.Start();
                IPathFinder<Node> searcher = provider.GetRequiredService<IPathFinder<Node>>();
                Node foundNode = searcher.FindPath(new Node(args[1]), new Node(args[2]));
                timer.Stop();

                Logger.LogInformation($"Search completed in {timer.ElapsedMilliseconds}ms");

                IWriter<Node> resultWriter = provider.GetRequiredService<IWriter<Node>>();
                resultWriter.Write(foundNode);
            }
            catch (IOException e)
            {
                Logger.LogError(0, e, "An error occurred whilst trying to read/write to the file system");
            }
            catch (UnauthorizedAccessException e)
            {
                Logger.LogError(0, e, $"The application does not have permission to write to {args[3]}");
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
