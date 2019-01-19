namespace Application.Queues
{
    /// <summary>
    /// Defines the methods for an implementation to behave as a queue.
    /// </summary>
    /// <typeparam name="T">The type of the items contained in the queue.</typeparam>
    public interface IQueue<T>
    {
        /// <summary>
        /// Checks if the provided item exists in the queue or not.
        /// </summary>
        /// <param name="item">The item to check it's existence in the queue.</param>
        /// <returns>Value indicating if the item is in the queue or not.</returns>
        bool Contains(T item);

        /// <summary>
        /// Inserts the provided item at the end of the queue.
        /// </summary>
        /// <param name="item">The item to add to the queue.</param>
        void Enqueue(T item);

        /// <summary>
        /// Attempts to retrieve an item from the queue.
        /// </summary>
        /// <param name="item">The out parameter set to the next item in the queue.</param>
        /// <returns>Value indicating if there is an item in the queue to fetch.</returns>
        bool TryDequeue(out T item);

        /// <summary>
        /// Clears up the queue and hash to free up resources.
        /// </summary>
        void Clear();
    }
}
