using System;

namespace Interfaces
{
    public interface IPredicate<in T>
    {
        bool Invoke(T item);
    }
}
