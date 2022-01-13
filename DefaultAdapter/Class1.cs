using System;
using System.Collections.Generic;
using System.Text;

namespace Adapters
{
    public class BinaryPredicateDefaultAdapter<T> : IComparer<T>
    {
        private readonly IComparer<T> comparer;
        public BinaryPredicateDefaultAdapter(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }
        public int Compare(T x, T y) => this.comparer.Compare(x, y);
    }
}
