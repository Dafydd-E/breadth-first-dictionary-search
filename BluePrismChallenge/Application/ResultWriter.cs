using Microsoft.Extensions.Logging;
using System.IO;

namespace Application
{
    public class ResultWriter
    {
        private string Path { get; }
        private ILogger<ResultWriter> Logger { get; }

        public ResultWriter(string path, ILogger<ResultWriter> logger)
        {
            this.Path = path;
            this.Logger = logger;
        }

        public void WriteToFile(Node node)
        {
            using (StreamWriter stream = new StreamWriter(File.Create(this.Path)))
            {
                while (node != null)
                {
                    stream.WriteLine(node.Word);
                    node = node.Parent;
                }
            }
        }
    }
}
