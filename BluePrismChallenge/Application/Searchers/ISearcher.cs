using Application.Queues;
using System;
using System.Collections.Generic;

namespace Application.Searchers
{
    public interface ISearcher<T, S> : IDisposable
    {
        T SearchQueue(IQueue<T> queue, S target);
    }
}
