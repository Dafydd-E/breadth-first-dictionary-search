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
        public const string RegexFormatter = @"\w{1}+{2}+{3}+|{0}+\w{2}+{3}+|
            {0}+{1}+\w{3}+|{0}+{1}+{2}+\w";
        public const int ArgumentLength = 4;
        public const int WordLength = 4;
    }
}
