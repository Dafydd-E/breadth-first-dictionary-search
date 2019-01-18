using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Application
{
    public class DictionaryReader : IDisposable
    {
        private FileStream Stream { get; }

        public DictionaryReader(string dictionaryLocation)
        {
            this.Stream = File.OpenRead(dictionaryLocation);
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
            // Choose over reading line by line for fewer operations and better performance.
            byte[] bytes = new byte[1024];
            int readBytes = 0;
            while ((readBytes = this.Stream.Read(bytes, 0, 1024)) != 0)
            {
                string result = Encoding.UTF8.GetString(bytes);
                MatchCollection matches = node.Regex.Matches(result);

                foreach (Match match in matches)
                {
                    if (match.Value != node.Parent?.Word)
                    {
                        yield return new Node(node.Depth + 1, match.Value, node);
                    }
                }
            }

            this.Stream.Position = 0;
        }
    }
}
