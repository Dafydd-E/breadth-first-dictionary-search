using Application.Queues;
using System;

namespace Application.Searchers
{
    public interface ISearcher<T, S> : IDisposable
    {
        T SearchQueue(IQueue<T> queue, S target);
    }
}
