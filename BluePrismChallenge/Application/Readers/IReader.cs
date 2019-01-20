using System;

namespace Application.Readers
{
    /// <summary>
    /// Defines the properties and methods required to read from a dictionary.
    /// </summary>
    public interface IReader : IDisposable
    {
        /// <summary>
        /// Gets the current read word.
        /// </summary>
        string CurrentString { get; }

        /// <summary>
        /// Reads from the stream.
        /// </summary>
        /// <returns>Value indicating if a value was read from the stream.</returns>
        bool Read();

        /// <summary>
        /// Resets the reader to the start of it's stream.
        /// </summary>
        void ResetReader();

        /// <summary>
        /// Gets a value indicating that the stream has reached the end.
        /// </summary>
        bool AtEndOfStream();
    }
}
