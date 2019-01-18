using Application;
using Xunit;

namespace ApplicationTest
{
    public class NodeTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Node node = new Node(0, "test");

            Assert.NotNull(node);
        }
    }
}
