using System;
using System.Collections.Generic;

namespace Application.Readers
{
    public interface IDictionaryReader<T> : IDisposable
    {
        string CurrentWord { get; }
        bool Read();
        void ResetReader();
    }
}
