using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IStorage<T>
    {
        int LoadSize { get; set; }
        void Save(IEnumerable<T> collection, bool append);
        IEnumerable<T> Load();
    }
}
