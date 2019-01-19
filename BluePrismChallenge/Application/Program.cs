using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.IO;
using Application.Readers;
using Application.Searchers;
using Application.Queues;
using System.Diagnostics;

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
                using (ISearcher<Node, string> searcher = provider.GetRequiredService<ISearcher<Node, string>>())
                {
                    Stopwatch timer = new Stopwatch();

                    Node rootNode = new Node(0, args[1]);
                    IQueue<Node> queue = new DistinctQueue<Node>(
                        provider.GetRequiredService<ILogger<DistinctQueue<Node>>>());

                    queue.Enqueue(rootNode);

                    Logger.LogInformation($"Finding shortest sequence of 4 " +
                        $"letter words between {args[1]} and {args[2]}");

                    timer.Start();
                    Node foundNode = searcher.SearchQueue(queue, args[2]);
                    timer.Stop();

                    Logger.LogInformation($"Search completed in {timer.ElapsedMilliseconds}ms");

                    ResultWriter resultWriter = new ResultWriter(
                        args[3], 
                        provider.GetRequiredService<ILogger<ResultWriter>>());

                    resultWriter.WriteToFile(foundNode);
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
            services
                .AddLogging(options => options.AddConsole())
                .AddTransient<IDictionaryReader<Node>, DictionaryReader>(provider =>
                {
                    return new DictionaryReader(args[0], provider.GetRequiredService<ILogger<DictionaryReader>>());
                })
                .AddTransient<ISearcher<Node, string>, WordBreadthFirstSearch>();

            return services.BuildServiceProvider();
        }
    }
}
