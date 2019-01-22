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

        /// <summary>
        /// The location of the dictionary in the local file system.
        /// </summary>
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

        /// <summary>
        /// Reads all the four letter words from the local dictionary file.
        /// </summary>
        /// <returns><see cref="IEnumerable{string}"/> containing all the four
        /// letter words in the dictionary.</returns>
        public IEnumerable<string> Read()
        {
            using (var stream = new StreamReader(this.DictionaryLocation))
            {
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();
                    if (line.Length == Constants.WordLength)
                    {
                        this.Logger.LogTrace($"Adding {line} to the cache");
                        yield return line;
                    }
                }
            }
        }
    }
}