namespace Application
{
    /// <summary>
    /// Constants used by the application.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Regex to find words which differ by one letter to the given four letter word.
        /// </summary>
        public const string RegexFormatter = @"\w{1}+{2}+{3}+|{0}+\w{2}+{3}+|{0}+{1}+\w{3}+|{0}+{1}+{2}+\w";

        /// <summary>
        /// The number of arguments expected by the application.
        /// </summary>
        public const int ArgumentLength = 4;

        /// <summary>
        /// The length of the words contained in the word ladder.
        /// </summary>
        public const int WordLength = 4;
    }
}
