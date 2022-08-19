using System.Collections.Generic;

namespace Application.Readers
{
    /// <summary>
    /// Defines the properties and methods required to read from a dictionary.
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Reads from the stream.
        /// </summary>
        /// <returns>Value indicating if a value was read from the stream.</returns>
        IEnumerable<string> Read();
    }
}
