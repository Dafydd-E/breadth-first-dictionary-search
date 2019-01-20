using Application.Queues;
using Application.Readers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Application.Searchers
{
    /// <summary>
    /// Searhes the queue using the Breadth-First search algorithm.
    /// </summary>
    public class WordBreadthFirstSearch : ISearcher<Node>
    {
        /// <summary>
        /// Gets the dictionary reader.
        /// </summary>
        private IDictionaryReader Reader { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<WordBreadthFirstSearch> Logger { get; }

        /// <summary>
        /// Initialises a new instance of the 
        /// <see cref="WordBreadthFirstSearch"/> class.
        /// </summary>
        /// <param name="reader">The dictionary reader.</param>
        /// <param name="logger">The logger instance.</param>
        public WordBreadthFirstSearch(
            IDictionaryReader reader, 
            ILogger<WordBreadthFirstSearch> logger)
        {
            this.Reader = reader;
            this.Logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public Node Search(IQueue<Node> nodes, Node target)
        {
            if (nodes.TryDequeue(out Node node))
            {
                IEnumerable<Node> neighbours = this.FindNeighbours(node);

                foreach (Node neighbour in neighbours)
                {
                    if (neighbour.Word.Equals(target.Word,  StringComparison.OrdinalIgnoreCase))
                    {
                        this.Logger.LogInformation($"Found end word {target.ToString()}");
                        nodes.Clear();
                        return neighbour;
                    }

                    nodes.Enqueue(neighbour);
                }

                return this.Search(nodes, target);
            }

            return null;
        }

        /// <summary>
        /// Finds the neighbouring node's to the given node.
        /// </summary>
        /// <param name="node">The node to find it's neighbours.</param>
        /// <returns>List of neighouring nodes.</returns>
        public IEnumerable<Node> FindNeighbours(Node node)
        {
            this.Logger.LogInformation($"Finding neighbours for node {node.ToString()}.");

            string readString = string.Empty;
            while (this.Reader.Read())
            {
                MatchCollection matches = node.Regex.Matches(this.Reader.CurrentString);

                foreach (Match match in matches)
                {
                    this.Logger.LogTrace($"Found neighbour {match.Value} for parent {node.Word}");
                    if (match.Length == 4 && match.Value != node.Parent?.Word)
                    {
                        yield return new Node(match.Value, node);
                    }
                }
            }

            this.Reader.ResetReader();
        }

        /// <summary>
        /// Disposes the dictionary reader.
        /// </summary>
        public void Dispose()
        {
            this.Reader.Dispose();
        }
    }
}
