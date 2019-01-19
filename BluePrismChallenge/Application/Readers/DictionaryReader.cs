using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Application.Readers
{
    public class DictionaryReader : IDictionaryReader<Node>
    {
        private StreamReader Stream { get; }
        private ILogger<DictionaryReader> Logger { get; }

        public DictionaryReader(string dictionaryLocation, ILogger<DictionaryReader> logger)
        {
            this.Stream = new StreamReader(File.OpenRead(dictionaryLocation));
            this.Logger = logger;
        }

        public void Dispose()
        {
            this.Stream.Close();
        }

        public IEnumerable<Node> FindNeighbours(Node node)
        {
            this.Logger.LogInformation($"Finding neighbours for node {node.ToString()}.");

            string readString = string.Empty;
            while ((readString = this.Stream.ReadLine()) != null)
            {
                MatchCollection matches = node.Regex.Matches(readString);

                foreach (Match match in matches)
                {
                    this.Logger.LogDebug($"Found neighbour {match.Value} for parent {node.Word}");
                    if (match.Length == 4 && match.Value != node.Parent?.Word)
                    {
                        yield return new Node(node.Depth + 1, match.Value, node);
                    }
                }
            }

            this.Logger.LogDebug("Returning the stream to it's starting position");
            this.Stream.BaseStream.Position = 0;
        }
    }
}
