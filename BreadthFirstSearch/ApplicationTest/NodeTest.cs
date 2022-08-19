using Application.Models;
using Xunit;

namespace ApplicationTest
{
    /// <summary>
    /// Unit tests for the <see cref="Node"/> class.
    /// </summary>
    public class NodeTest
    {
        private const string NodeWord = "test";

        [Fact]
        public void NodeInitialisedSuccessfullyTest()
        {
            Node node = new Node(NodeWord);
            Assert.NotNull(node);
        }
    }
}
