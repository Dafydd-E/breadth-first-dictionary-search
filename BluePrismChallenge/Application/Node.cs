using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Application
{
    /// <summary>
    /// Class to model a word in the word ladder
    /// </summary>
    public class Node
    {
        public string Word { get; }
        public Node Parent { get; }
        public Regex Regex { get; }

        private IEnumerable<Node> Neighbours { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Node"/> class. 
        /// </summary>
        /// <param name="word">The word for the node.</param>
        /// <param name="parent">The parent <see cref="Node"/> instance.</param>
        public Node(string word, Node parent = null)
        {
            this.Word = word;
            this.Parent = parent;

            this.Regex = new Regex(string.Format(
                Constants.RegexFormatter,
                word[0],
                word[1],
                word[2],
                word[3]), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns the hash code of the <see cref="Word"/> property value.
        /// </summary>
        public override int GetHashCode()
        {
            return this.Word.GetHashCode();
        }

        /// <summary>
        /// Returns the <see cref="Word"/> property value.
        /// </summary>
        public override string ToString()
        {
            return this.Word;
        }
    }
}
