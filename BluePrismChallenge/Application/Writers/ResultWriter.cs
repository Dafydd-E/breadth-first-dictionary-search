using Application.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace Application.Writers
{
    /// <summary>
    /// Writes the results of the search to an output file.
    /// </summary>
    public class ResultWriter : IWriter<Node>
    {
        /// <summary>
        /// Gets the output path.
        /// </summary>
        private string Path { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<ResultWriter> Logger { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultWriter"/> class.
        /// </summary>
        /// <param name="path">The full path to the output file.</param>
        /// <param name="logger">The logger instance.</param>
        public ResultWriter(string path, ILogger<ResultWriter> logger)
        {
            this.Path = path;
            this.Logger = logger;
        }

        /// <summary>
        /// Writes the given node and all it's parent's to the output file.
        /// </summary>
        /// <param name="node">The node and it's parent's to write 
        /// to the output file.
        /// </param>
        public void Write(Node node)
        {
            using (StreamWriter stream = new StreamWriter(File.Create(this.Path)))
            {
                // Use a string builder to ouput result from the 
                // start word to the end word.
                StringBuilder builder = new StringBuilder();
                
                while (node != null)
                {
                    builder.Insert(0, $"{node.Word}\n");
                    node = node.Parent;
                }
                this.Logger.LogInformation($"Writing to output file {this.Path}");
                stream.Write(builder.ToString());
            }
        }
    }
}
