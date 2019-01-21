using Application.Models;
using Application.Queues;
using Application.Readers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Application.PathFinders
{
    /// <summary>
    /// Searhes the queue using the Breadth-First search algorithm.
    /// </summary>
    public class WordBreadthFirstSearch : IPathFinder<Node>
    {
        /// <summary>
        /// Gets the dictionary reader.
        /// </summary>
        private IReader Reader { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<WordBreadthFirstSearch> Logger { get; }

        private IQueue<Node> Queue { get; }

        /// <summary>
        /// Initialises a new instance of the 
        /// <see cref="WordBreadthFirstSearch"/> class.
        /// </summary>
        /// <param name="reader">The dictionary reader.</param>
        /// <param name="logger">The logger instance.</param>
        public WordBreadthFirstSearch(
            IReader reader,
            IQueue<Node> queue,
            ILogger<WordBreadthFirstSearch> logger)
        {
            this.Reader = reader;
            this.Logger = logger;
            this.Queue = queue;
        }

        /// <summary>
        /// Executes the search from the start node until the target is found.
        /// </summary>
        /// <param name="nodes"><see cref="IQueue{Node}"/> containing the nodes
        /// to explore adjacent words in the dictionary.</param>
        /// <param name="target">The item to search for.</param>
        /// <returns>The found object.</returns>
        public Node FindPath(Node start, Node target)
        {
            this.Queue.Enqueue(start);

            while (this.Queue.TryDequeue(out Node node))
            {
                IEnumerable<Node> neighbours = this.FindNeighbours(node, this.Reader.Read());
                foreach (Node neighbour in neighbours)
                {
                    if (neighbour.Word.Equals(target.Word, StringComparison.OrdinalIgnoreCase))
                    {
                        this.Logger.LogInformation($"Found end word {target.ToString()}");
                        this.Queue.Clear();
                        return neighbour;
                    }

                    this.Queue.Enqueue(neighbour);
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the neighbouring node's to the given node.
        /// </summary>
        /// <param name="node">The node to find it's neighbours.</param>
        /// <returns>List of neighouring nodes.</returns>
        public IEnumerable<Node> FindNeighbours(Node node, IEnumerable<string> collection)
        {
            this.Logger.LogInformation($"Finding neighbours for node {node.ToString()}.");

            foreach (string word in collection)
            {
                MatchCollection matches = node.Regex.Matches(word);

                foreach (Match match in matches)
                {
                    this.Logger.LogTrace($"Found neighbour {match.Value} for parent {node.Word}");
                    if (!match.Value.Equals(node.Parent?.Word, StringComparison.OrdinalIgnoreCase) &&
                        !match.Value.Equals(node.Word, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return new Node(match.Value, node);
                    }
                }
            }
        }
    }
}
