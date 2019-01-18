using System.IO;

namespace Application
{
    public class ResultWriter
    {
        private string Path { get; }

        public ResultWriter(string path)
        {
            this.Path = path;
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
