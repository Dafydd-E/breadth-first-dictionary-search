using Application.Readers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Application.Searchers
{
    public class WordBreadthFirstSearch : ISearcher<Node, string>
    {
        private IDictionaryReader<Node> Reader { get; }
        private ILogger<WordBreadthFirstSearch> Logger {get;}

        public WordBreadthFirstSearch(IDictionaryReader<Node> reader, ILogger<WordBreadthFirstSearch> logger)
        {
            this.Reader = reader;
            this.Logger = logger;
        }

        public Node SearchQueue(DistinctQueue<Node> nodes, string endWord)
        {
            if (nodes.TryDequeue(out Node node))
            {
                IEnumerable<Node> neighbours = this.Reader.FindNeighbours(node);

                foreach (Node neighbour in neighbours)
                {
                    if (neighbour.Word == endWord)
                    {
                        this.Logger.LogInformation($"Found end word {endWord}");
                        nodes.Clear();
                        return neighbour;
                    }

                    if (!nodes.Contains(neighbour))
                    {
                        nodes.Enqueue(neighbour);
                    }
                }

                return this.SearchQueue(nodes, endWord);
            }

            return null;
        }

        public void Dispose()
        {
            this.Reader.Dispose();
        }
    }
}
