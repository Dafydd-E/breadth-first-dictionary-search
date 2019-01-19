using Microsoft.Extensions.Logging;
using System.IO;

namespace Application.Readers
{
    public class DictionaryReader : IDictionaryReader<Node>
    {
        private StreamReader Stream { get; }
        private ILogger<DictionaryReader> Logger { get; }
        public string CurrentWord { get; private set; }

        public DictionaryReader(string dictionaryLocation, ILogger<DictionaryReader> logger)
        {
            this.Stream = new StreamReader(File.OpenRead(dictionaryLocation));
            this.Logger = logger;
        }

        public void Dispose()
        {
            this.Stream.Close();
        }

        public bool Read()
        {
            this.CurrentWord = this.Stream.ReadLine();
            return this.CurrentWord != null;
        }

        public void ResetReader()
        {
            this.Logger.LogDebug("Returning the stream to it's starting position");
            this.Stream.BaseStream.Position = 0;
        }
    }
}
