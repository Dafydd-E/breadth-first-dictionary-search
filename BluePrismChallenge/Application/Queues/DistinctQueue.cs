using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Application.Queues
{
    /// <summary>
    /// Queue which only contains unique elements.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the queue.</typeparam>
    public class DistinctQueue<T> : IQueue<T>
    {
        /// <summary>
        /// Gets the queue of items.
        /// </summary>
        private Queue<T> Queue { get; } = new Queue<T>();

        /// <summary>
        /// Gets the hash of items in the queue.
        /// </summary>
        private HashSet<int> Hash { get; } = new HashSet<int>();

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        private ILogger<DistinctQueue<T>> Logger { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DistinctQueue{T}"/> class.
        /// </summary>
        /// <param name="logger">The application logger.</param>
        public DistinctQueue(ILogger<DistinctQueue<T>> logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Checks if the provded item's hash is in contained in the 
        /// <see cref="HashSet{T}"/>.
        /// </summary>
        /// <param name="item">The item to check it's existence in the 
        /// <see cref="HashSet{T}"/>
        /// </param>
        /// <returns>Value indicating if the item exists in the 
        /// <see cref="HashSet{T}"/> or not.
        /// </returns>
        public bool Contains(T item)
        {
            this.Logger.LogTrace($"Checking if the item {item} is contained in the hash.");
            return this.Hash.Contains(item.GetHashCode());

        }

        /// <summary>
        /// Puts the provided item into the end of the queue,
        /// if it is not in the queue already.
        /// </summary>
        /// <param name="item">The item to add to the queue.</param>
        public void Enqueue(T item)
        {
            if (this.Hash.Add(item.GetHashCode()))
            {
                this.Logger.LogTrace($"Adding item to queue {item.ToString()}");
                this.Queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Attempts to retrieve an item from the queue.
        /// </summary>
        /// <param name="item">Out parameter set to the next item in the queue if it exist.</param>
        /// <returns>Value indicating if the next item exists in the 
        /// queue or not.
        /// </returns>
        public bool TryDequeue(out T item)
        {
            return this.Queue.TryDequeue(out item);
        }

        /// <summary>
        /// Clears up the queue and hash to free up resources.
        /// </summary>
        public void Clear()
        {
            this.Queue.Clear();
            this.Hash.Clear();
        }
    }
}
