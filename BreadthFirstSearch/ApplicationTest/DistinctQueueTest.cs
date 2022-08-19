using Application.Models;
using Application.Queues;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ApplicationTest
{
    /// <summary>
    /// Unit tests for the <see cref="DistinctQueue{T}"/> class.
    /// </summary>
    public class DistinctQueueTest
    {
        private readonly Node FirstNode = new Node("Test");
        private readonly Node SecondNode = new Node("Test2");

        [Fact]
        public void ContainsTest()
        {
            DistinctQueue<Node> queue = new DistinctQueue<Node>(
                ServiceHelper.GetService<ILogger<DistinctQueue<Node>>>());

            queue.Enqueue(FirstNode);

            Assert.True(queue.Contains(FirstNode));
            Assert.False(queue.Contains(SecondNode));

            queue.TryDequeue(out Node node);

            Assert.Equal(FirstNode, node);

        }

        [Fact]
        public void TryDequeueTest()
        {
            DistinctQueue<Node> queue = new DistinctQueue<Node>(
                ServiceHelper.GetService<ILogger<DistinctQueue<Node>>>());

            queue.Enqueue(FirstNode);
            Assert.True(queue.TryDequeue(out Node node));
            Assert.Equal(FirstNode, node);
            Assert.False(queue.TryDequeue(out node));
        }

        [Fact]
        public void EnqueueTest()
        {
            DistinctQueue<Node> queue = new DistinctQueue<Node>(
                ServiceHelper.GetService<ILogger<DistinctQueue<Node>>>());

            queue.Enqueue(FirstNode);
            Assert.True(queue.Contains(FirstNode));
        }
    }
}
