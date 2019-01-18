using System;
using System.IO;
using System.Text;

namespace Application
{
    public class DictionaryBatcher
    {
        private StreamReader StreamReader { get; }

        public DictionaryBatcher(StreamReader reader)
        {
            this.StreamReader = reader;
        }

        public string GetNextBatch()
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                // TODO
                for (int i = 0; i < 500; i++)
                {
                    builder.Append(this.StreamReader?.ReadLine() ?? throw new EndOfStreamException(""));
                    builder.Append(";");
                }
            }
            catch (EndOfStreamException)
            {
                // TODO
                Console.WriteLine();
            }
            return builder.ToString();
        }
    }
}
