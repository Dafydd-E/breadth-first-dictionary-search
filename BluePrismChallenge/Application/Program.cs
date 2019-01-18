using System;
using System.Collections.Generic;
using System.IO;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                // TODO
                Console.WriteLine("");
                return;
            }

            if (args[1].Length != 4 && args[2].Length != 4)
            {
                Console.WriteLine("The start and end word must both be 4 letters long");
                return;
            }

            try
            {
                using (DictionaryReader dictionaryReader = new DictionaryReader(args[0]))
                {
                    Node rootNode = new Node(0, args[1]);
                    Queue<Node> queue = new Queue<Node>();
                    queue.Enqueue(rootNode);

                    Node foundNode = dictionaryReader.ProcessQueue(queue, args[2]);

                    ResultWriter resultWriter = new ResultWriter(args[3]);
                    resultWriter.WriteToFile(foundNode);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred whilst trying to " +
                    $"read the dictionary file with path {args[0]}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
