using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookClass;
using Interfaces;

namespace BookServiceTask
{
    public class BookService
    {
        private readonly List<Book> books;

        public BookService()
        {
            this.books = new List<Book>();
        }

        public BookService(IEnumerable<Book> books)
            : this()
        {
            this.books.AddRange(books);
        }

        public void Add(Book book)
        {
            if (this.books.Contains(book))
            {
                throw new ArgumentException("Book already exist");
            }

            this.books.Add(book);
        }

        public void Remove(Book book)
        {
            if (this.books.Contains(book))
            {
                throw new ArgumentException("Book already exist");
            }

            this.books.Remove(book);
        }

        public IReadOnlyCollection<Book> GetBooks()
        {
            return new ReadOnlyCollection<Book>(this.books);
        }

        public IEnumerable<Book> FindBy(IPredicate<Book> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return FindByIterator(predicate);
            IEnumerable<Book> FindByIterator(IPredicate<Book> predicate)
            {
                foreach (var book in this.books)
                {
                    if (predicate.Invoke(book))
                    {
                        yield return book;
                    }
                }
            }
        }

        public IEnumerable<Book> SortBy(IComparer<Book> comparer)
        {
            if (comparer is null)
            {
                comparer = Comparer<Book>.Default;
            }

            for (int i = 0; i < this.books.Count; i++)
            {
                for (int j = 0; j < this.books.Count - i - 1; j++)
                {
                    if (comparer?.Compare(this.books[j], this.books[j + 1]) == 1)
                    {
                        Book buf = this.books[j + 1];
                        this.books[j + 1] = this.books[j];
                        this.books[j] = buf;
                    }
                }
            }

            return this.books;
        }

        public void Save(IStorage<Book> storage, bool append = false)
        {
            if (storage is null)
            {
                throw new InvalidOperationException("No storage was set.");
            }

            storage.Save(this.books, append);
        }

        public void Load(IStorage<Book> storage)
        {
            if (storage is null)
            {
                throw new InvalidOperationException("No storage was set.");
            }

            this.books.AddRange(storage.Load());
        }

        public void Clear() => this.books.Clear();
    }
}
