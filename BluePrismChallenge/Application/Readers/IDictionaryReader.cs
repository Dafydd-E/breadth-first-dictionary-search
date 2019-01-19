using System;
using System.Collections.Generic;

namespace Application.Readers
{
    public interface IDictionaryReader<T> : IDisposable
    {
        IEnumerable<T> FindNeighbours(T item);
    }
}
