using Application.Queues;
using System;

namespace Application.Searchers
{
    /// <summary>
    /// Defines the methods required to search a queue.
    /// </summary>
    /// <typeparam name="T">The type of the items that are being searched..</typeparam>
    public interface ISearcher<T> : IDisposable
    {
        /// <summary>
        /// Searches the source for the target item, using a queue to implement the search mechanism.
        /// </summary>
        /// <param name="queue">The queue to implement the search mechanism.</param>
        /// <param name="target">The item to find.</param>
        /// <returns></returns>
        T Search(IQueue<T> queue, T target);
    }
}
