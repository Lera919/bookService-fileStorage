using System;
using System.Collections.Generic;
using System.Text;
using BookClass;
using Interfaces;

namespace Predicates
{
    public class PagesPredicate : IPredicate<Book>
    {
        private readonly int pages;

        public PagesPredicate(int pages)
        {
            this.pages = pages;
        }

        public bool Invoke(Book item) => item?.Pages == this.pages;
    }
}
