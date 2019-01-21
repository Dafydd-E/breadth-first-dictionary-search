using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace Application.Readers
{
    /// <summary>
    /// Reads from a local TXT dictionary file.
    /// </summary>
    public class DictionaryReader : IReader
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<DictionaryReader> Logger { get; }

        private string DictionaryLocation { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryReader"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DictionaryReader(string dictionaryLocation, ILogger<DictionaryReader> logger)
        {
            this.Logger = logger;
            this.DictionaryLocation = dictionaryLocation;
        }

        public IEnumerable<string> Read()
        {
            using (var stream = new StreamReader(this.DictionaryLocation))
            {
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();
                    if (line.Length == Constants.WordLength)
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}