using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BookClass;
using BookServiceTask;
using Interfaces;

namespace BookStorages
{
    public class BinaryBookStorage : IStorage<Book>
    {
        private const int MaxTitleLength = 30;
        private const int MaxNameLength = 30;
        private const int MaxIsbnLength = 17;
        private const int MaxCurrencyLength = 3;
        private const int BookSize = ((sizeof(int) + MaxNameLength) * 2) + (sizeof(int) + MaxIsbnLength) + (sizeof(int) + MaxTitleLength) + 
            (sizeof(int) + MaxCurrencyLength) + sizeof(bool)+
            sizeof(long) + sizeof(int) + sizeof(decimal); 
        // sizeof(publisher) + sizeof(str length) + sizeof(author) + sizeof(str length) +
        // sizeof(isbn) + sizeof(str length) + sizef(titleLength) + sizeof(currency) + sizeof(str length)  + MaxNameLength + sizeof(ticks) + sizeof(pages) + sizeof(price)
        public int LoadSize { get; set; }
        public IEnumerable<Book> Load()
        {
            using Stream stream = new FileStream("books.db", FileMode.Open, FileAccess.Read);
           stream.Seek(0, SeekOrigin.Begin);
            var recordsCount = (int)(stream.Length / BookSize);
            for (int i = 0; i < recordsCount; i++)
            {
                var recordBuffer = new byte[BookSize];
                stream.Read(recordBuffer, 0, BookSize);
                var record = BytesToBook(recordBuffer);
                yield return record;
            }
        }

        public void Save(IEnumerable<Book> collection, bool append)
        {
            using Stream stream = append ? new FileStream("books.db", FileMode.Append, FileAccess.Write) : new FileStream("books.db", FileMode.Truncate, FileAccess.Write);
            foreach(var book in collection)
            {
                var bytes = BookToBytes(book);
                stream.Write(bytes, 0, BookSize);
            }
        }

        /// <summary>
        /// Transforms the array of bytes into a new book.
        /// </summary>
        /// <param name="bytes">Array of bytes to be transformed.</param>
        /// <returns>Book.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the array of bytes is null.
        /// </exception>
        private static Book BytesToBook(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            using var memoryStream = new MemoryStream(bytes);
            using var binaryReader = new BinaryReader(memoryStream);
            
                var titleLength = binaryReader.ReadInt32();
                var titleBuffer = binaryReader.ReadBytes(MaxTitleLength);

                var authorLength = binaryReader.ReadInt32();
                var authorBuffer = binaryReader.ReadBytes(MaxNameLength);

                var publisherLength = binaryReader.ReadInt32();
                var publisherBuffer = binaryReader.ReadBytes(MaxNameLength);

                var isbnLength = binaryReader.ReadInt32();
                var isbnBuffer = binaryReader.ReadBytes(MaxIsbnLength);

                var currencyLength = binaryReader.ReadInt32();
                var currencyBuffer = binaryReader.ReadBytes(MaxCurrencyLength);

                var title = Encoding.UTF8.GetString(titleBuffer, 0, titleLength);
                var author = Encoding.UTF8.GetString(authorBuffer, 0, authorLength);
                var currency = Encoding.UTF8.GetString(currencyBuffer, 0, currencyLength);
                var isbn = Encoding.UTF8.GetString(isbnBuffer, 0, isbnLength);
                var publisher = Encoding.UTF8.GetString(publisherBuffer, 0, publisherLength);

                var published = binaryReader.ReadBoolean();
                var date = new DateTime(binaryReader.ReadInt64());
                var price = binaryReader.ReadDecimal();
                var pages = binaryReader.ReadInt32();
            var book = new Book(author, title, publisher, isbn);
            if (published)
            {
                book.Publish(date);
            }
            book.SetPrice(price, currency);
            book.Pages = pages;

            return book;
        }

        /// <summary>
        /// Transform the record into byte array.
        /// </summary>
        /// <param name="book">Record to transform.</param>
        /// <returns>Array of bytes represented the record.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the record is null.
        /// </exception>
        private static byte[] BookToBytes(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            var bytes = new byte[BookSize];
            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                WriteString(binaryWriter, book.Title, MaxTitleLength);
                WriteString(binaryWriter, book.Author, MaxNameLength);
                WriteString(binaryWriter, book.Publisher, MaxNameLength);
                WriteString(binaryWriter, book.ISBN, MaxIsbnLength);
                WriteString(binaryWriter, book.Currency, MaxCurrencyLength);
                string time = book.GetPublicationDate();
                if (time.Equals("NYP"))
                {
                    binaryWriter.Write(false);
                    binaryWriter.Write((long)0);
                }
                else
                {
                    binaryWriter.Write(true);
                    DateTime.TryParseExact(time, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
                    binaryWriter.Write(date.Ticks);
                }
                binaryWriter.Write(book.Price);
                binaryWriter.Write(book.Pages);
            }

            return bytes;
        }

        private static void WriteString(BinaryWriter writer, string toWrite, int maxLength)
        {
            var bytes = Encoding.UTF8.GetBytes(toWrite);
            var buffer = new byte[maxLength];
            int length = bytes.Length;
            if (length > maxLength)
            {
                length = maxLength;
            }

            Array.Copy(bytes, 0, buffer, 0, length);

            writer.Write(length);
            writer.Write(buffer);
        }
    }
}
