using Microsoft.Extensions.Logging;
using System.IO;

namespace Application.Readers
{
    /// <summary>
    /// Reads from a local TXT dictionary file.
    /// </summary>
    public class DictionaryReader : IReader
    {
        /// <summary>
        /// Gets the file stream reader.
        /// </summary>
        private StreamReader Stream { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<DictionaryReader> Logger { get; }

        /// <summary>
        /// Gets the current word read by the stream.s
        /// </summary>
        public string CurrentString { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryReader"/> class.
        /// </summary>
        /// <param name="dictionaryLocation">The location of the dictionary.</param>
        /// <param name="logger">The logger.</param>
        public DictionaryReader(string dictionaryLocation, ILogger<DictionaryReader> logger)
        {
            this.Stream = new StreamReader(File.OpenRead(dictionaryLocation));
            this.Logger = logger;
        }

        /// <summary>
        /// Closes the stream reader.
        /// </summary>
        public void Dispose()
        {
            this.Stream.Close();
        }

        /// <summary>
        /// Reads the next line from the stream.
        /// </summary>
        /// <returns>Value indicating if a value was obtained from the stream.</returns>
        public bool Read()
        {
            this.CurrentString = this.Stream.ReadLine();
            return this.CurrentString != null;
        }

        /// <summary>
        /// Sets the <see cref="Stream"/> back to the beginning of the file.
        /// </summary>
        public void ResetReader()
        {
            this.Logger.LogDebug("Returning the stream to it's starting position");
            this.Stream.BaseStream.Position = 0;
        }
    }
}
