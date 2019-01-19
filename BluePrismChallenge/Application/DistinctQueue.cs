using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Application
{
    public class DistinctQueue<T>
    {
        private Queue<T> Queue { get; } = new Queue<T>();
        private HashSet<int> Hash { get; } = new HashSet<int>();
        private ILogger<DistinctQueue<T>> Logger { get; }

        public DistinctQueue(ILogger<DistinctQueue<T>> logger)
        {
            this.Logger = logger;
        }

        public bool Contains(T item)
        {
            return this.Hash.Contains(item.GetHashCode());

        }

        public void Enqueue(T item)
        {
            if (this.Hash.Add(item.GetHashCode()))
            {
                this.Logger.LogDebug($"Adding item to queue {item.ToString()}");
                this.Queue.Enqueue(item);
            }
        }

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
