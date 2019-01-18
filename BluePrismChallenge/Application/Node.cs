using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Application
{
    public class Node
    {
        public int Depth { get; }
        public string Word { get; }
        public Node Parent { get; }
        public IEnumerable<Node> Neighbours { get; set; }
        public Regex Regex { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Node"/> class. 
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="word"></param>
        /// <param name="parent"></param>
        public Node(int depth, string word, Node parent = null)
        {
            this.Depth = depth;
            this.Word = word;
            this.Parent = parent;

            this.Regex = new Regex(string.Format(
                Formatters.RegexFormatter,
                word[0],
                word[1],
                word[2],
                word[3]));
        }
    }
}
