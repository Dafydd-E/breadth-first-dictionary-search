using Application.Queues;
using System;

namespace Application.Searchers
{
    /// <summary>
    /// Defines the methods required to search a queue.
    /// </summary>
    /// <typeparam name="T">The type of the items contained in the queue.</typeparam>
    /// <typeparam name="S">The type of the target item/value to find.</typeparam>
    public interface ISearcher<T, S> : IDisposable
    {
        /// <summary>
        /// Searches the quee for the target.
        /// </summary>
        /// <param name="queue">The queue to search.</param>
        /// <param name="target">The item to find.</param>
        /// <returns></returns>
        T SearchQueue(IQueue<T> queue, S target);
    }
}
