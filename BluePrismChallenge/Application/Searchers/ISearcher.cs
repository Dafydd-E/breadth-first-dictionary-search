using System;

namespace Application.Searchers
{
    public interface ISearcher<T, S> : IDisposable
    {
        T SearchQueue(DistinctQueue<T> queue, S target);
    }
}
