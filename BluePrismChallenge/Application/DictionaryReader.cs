using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Application
{
    public class DictionaryReader : IDisposable
    {
        private StreamReader Stream { get; }

        public DictionaryReader(string dictionaryLocation)
        {
            this.Stream = new StreamReader(File.OpenRead(dictionaryLocation));
        }

        public void Dispose()
        {
            this.Stream.Close();
        }
        
        public Node ProcessQueue(Queue<Node> nodes, string endWord)
        {
            if (nodes.TryDequeue(out Node node))
            {
                IEnumerable<Node> neighbours = this.FindNeighbours(node);
                foreach (Node neighbour in neighbours)
                {
                    if (neighbour.Word == endWord)
                    {
                        return neighbour;
                    }

                    nodes.Enqueue(neighbour);
                }

                return this.ProcessQueue(nodes, endWord);
            }

            return null;
        }

        public IEnumerable<Node> FindNeighbours(Node node)
        {
            string readString = string.Empty;
            while ((readString = this.Stream.ReadLine()) != null)
            {
                MatchCollection matches = node.Regex.Matches(readString);

                foreach (Match match in matches)
                {
                    if (match.Value != node.Parent?.Word)
                    {
                        yield return new Node(node.Depth + 1, match.Value, node);
                    }
                }
            }

            this.Stream.BaseStream.Position = 0;
        }
    }
}
