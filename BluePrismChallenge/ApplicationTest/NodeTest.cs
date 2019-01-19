using Application;
using Xunit;

namespace ApplicationTest
{
    public class NodeTest
    {
        [Fact]
        public void ConstructorSuccessTest()
        {
            Node node = new Node("test");
            Assert.NotNull(node);
        }
    }
}
