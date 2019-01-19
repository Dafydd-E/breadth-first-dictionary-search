using Application.Queues;
using Application.Readers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Application.Searchers
{
    public class WordBreadthFirstSearch : ISearcher<Node, string>
    {
        private IDictionaryReader<Node> Reader { get; }
        private ILogger<WordBreadthFirstSearch> Logger { get; }

        public WordBreadthFirstSearch(IDictionaryReader<Node> reader, ILogger<WordBreadthFirstSearch> logger)
        {
            this.Reader = reader;
            this.Logger = logger;
        }

        public Node SearchQueue(IQueue<Node> nodes, string endWord)
        {
            if (nodes.TryDequeue(out Node node))
            {
                IEnumerable<Node> neighbours = this.FindNeighbours(node);

                foreach (Node neighbour in neighbours)
                {
                    if (neighbour.Word == endWord)
                    {
                        this.Logger.LogInformation($"Found end word {endWord}");
                        nodes.Clear();
                        return neighbour;
                    }

                    nodes.Enqueue(neighbour);
                }

                return this.SearchQueue(nodes, endWord);
            }

            return null;
        }

        public IEnumerable<Node> FindNeighbours(Node node)
        {
            this.Logger.LogInformation($"Finding neighbours for node {node.ToString()}.");

            string readString = string.Empty;
            while (this.Reader.Read())
            {
                MatchCollection matches = node.Regex.Matches(this.Reader.CurrentWord);

                foreach (Match match in matches)
                {
                    this.Logger.LogDebug($"Found neighbour {match.Value} for parent {node.Word}");
                    if (match.Length == 4 && match.Value != node.Parent?.Word)
                    {
                        yield return new Node(node.Depth + 1, match.Value, node);
                    }
                }
            }

            this.Reader.ResetReader();
        }

        public void Dispose()
        {
            this.Reader.Dispose();
        }
    }
}
