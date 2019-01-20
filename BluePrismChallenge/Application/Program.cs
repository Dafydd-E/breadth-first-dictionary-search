using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.IO;
using Application.Readers;
using Application.Searchers;
using Application.Queues;
using System.Diagnostics;
using Application.Writers;
using Microsoft.Extensions.Configuration;

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
                using (ISearcher<Node> searcher = provider.GetRequiredService<ISearcher<Node>>())
                {
                    Stopwatch timer = new Stopwatch();

                    Node rootNode = new Node(args[1]);
                    IQueue<Node> queue = new DistinctQueue<Node>(
                        provider.GetRequiredService<ILogger<DistinctQueue<Node>>>());

                    queue.Enqueue(rootNode);

                    Logger.LogInformation($"Finding shortest sequence of 4 " +
                        $"letter words between {args[1]} and {args[2]}");

                    timer.Start();
                    Node foundNode = searcher.Search(queue, new Node(args[2]));
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

        private static IServiceProvider ConfigureServices(IServiceCollection services, string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            LogLevel loggingLevel = Enum.TryParse(
                configuration.GetValue<string>("Logging:LogLevel"), out LogLevel logLevel)
                    ? logLevel
                    : LogLevel.Information;

            services
                .AddLogging(options => options.AddConsole().SetMinimumLevel(loggingLevel))
                .AddTransient<IDictionaryReader, DictionaryReader>(provider =>
                {
                    return new DictionaryReader(args[0], provider.GetRequiredService<ILogger<DictionaryReader>>());
                })
                .AddTransient<IWriter<Node>, ResultWriter>(provider =>
                {
                    return new ResultWriter(args[3], provider.GetRequiredService<ILogger<ResultWriter>>());
                })
                .AddTransient<ISearcher<Node>, WordBreadthFirstSearch>();

            return services.BuildServiceProvider();
        }
    }
}
