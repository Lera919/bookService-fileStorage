using System;
using BookClass;
using BookStorages;
using Interfaces;
using NUnit.Framework;

namespace BookServiceTask.Tests
{
    [TestFixture]
    public class BookServiceTests
    {
       
            [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesSource_BookStorage))]
            public void BookService_BookStorage_BooksNotPublished_Tests(Book[] books, decimal[] price, int[] pages)
            {
                for (int i = 0; i < books.Length; i++)
                {
                    books[i].SetPrice(price[i], "USD");
                    books[i].Pages = pages[i];
                }

                BookService service = new BookService(books);
                service.Save(new BinaryBookStorage());
                service.Clear();
                service.Load(new BinaryBookStorage());
                var afterRestore = service.GetBooks();
                CollectionAssert.AreEqual(books, afterRestore);
            }

        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesSource_BookStorage_Published))]
        public void BookService_BookStorage_BooksPublished_Tests(Book[] books, decimal[] price, int[] pages, DateTime[] dates)
        {
            for (int i = 0; i < books.Length; i++)
            {
                books[i].SetPrice(price[i], "BYN");
                books[i].Pages = pages[i];
                books[i].Publish(dates[i]);
            }

            BookService service = new BookService(books);
            service.Save(new BinaryBookStorage());
            service.Clear();
            service.Load(new BinaryBookStorage());
            var afterRestore = service.GetBooks();
            CollectionAssert.AreEqual(books, afterRestore);
        }

        [Test]
            public void BookService_Load_StorageIsNull_ThrowInvalidOperationException()
               => Assert.Throws<InvalidOperationException>(() => new BookService(new Book[] { }).Load(null));

            [Test]
            public void BookService_Save_StorageIsNull_ThrowInvalidOperationException()
              => Assert.Throws<InvalidOperationException>(() => new BookService(new Book[] { }).Save(null));

        }
    }
