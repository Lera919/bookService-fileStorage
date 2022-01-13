using System;
using System.Globalization;
using VerificationService;

namespace BookClass
{
    /// <summary>
    /// Represents the book as a type of publication.
    /// </summary>
#pragma warning disable CA1036 // Override methods on comparable types
    public sealed class Book : IEquatable<Book>, IComparable<Book>, IComparable
    {
        private bool published;

        private DateTime datePublished;

        private int totalPages;

        private string author;

        private string title;

        private string publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="author">Autor of the book.</param>
        /// <param name="title">Title of the book.</param>
        /// <param name="publisher">Publisher of the book.</param>
        /// <exception cref="ArgumentNullException">Throw when author or title or publisher is null.</exception>
        public Book(string author, string title, string publisher)
        {
            this.Author = author is null ? throw new ArgumentNullException(nameof(author)) : author;
            this.Title = title is null ? throw new ArgumentNullException(nameof(title)) : title;
            this.Publisher = publisher is null ? throw new ArgumentNullException(nameof(publisher)) : publisher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="author">Autor of the book.</param>
        /// <param name="title">Title of the book.</param>
        /// <param name="publisher">Publisher of the book.</param>
        /// <param name="isbn">International Standard Book Number.</param>
        /// <exception cref="ArgumentNullException">Throw when author or title or publisher or ISBN is null.</exception>
        public Book(string author, string title, string publisher, string isbn)
            : this(author, title, publisher)
        {
            if (IsbnVerifier.IsValid(isbn))
            {
                this.ISBN = isbn;
            }
        }

        /// <summary>
        /// Gets author of the book.
        /// </summary>
        public string Author
        {
            get => this.author;

            private set => this.author = value;
        }

        /// <summary>
        /// Gets title of the book.
        /// </summary>
        public string Title
        {
            get => this.title;

            private set => this.title = value;
        }

        /// <summary>
        /// Gets publisher of the book.
        /// </summary>
        public string Publisher
        {
            get => this.publisher;

            private set => this.publisher = value;
        }

        /// <summary>
        /// Gets or sets total pages in the book.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throw when Pages less or equal zero.</exception>
        public int Pages
        {
            get => this.totalPages;

            set => this.totalPages = value <= 0 ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
        }

        /// <summary>
        /// Gets International Standard Book Number.
        /// </summary>
        public string ISBN { get; }

        /// <summary>
        /// Gets price.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// Gets currency.
        /// </summary>
        public string Currency { get; private set; } = RegionInfo.CurrentRegion.ISOCurrencySymbol;

        public static bool operator >(Book left, Book right) => string.Compare(left?.Title, right?.Title, StringComparison.InvariantCulture) > 0;

        public static bool operator <(Book left, Book right) => string.Compare(left?.Title, right?.Title, StringComparison.InvariantCulture) < 0;

        public static bool operator >=(Book left, Book right) => string.Compare(left?.Title, right?.Title, StringComparison.InvariantCulture) >= 0;

        public static bool operator <=(Book left, Book right) => string.Compare(left?.Title, right?.Title, StringComparison.InvariantCulture) <= 0;

        public static bool operator ==(Book left, Book right) => object.Equals(left, right);

        public static bool operator !=(Book left, Book right) => !object.Equals(left, right);

        /// <summary>
        /// Publishes the book if it has not yet been published.
        /// </summary>
        /// <param name="dateTime">Date of publish.</param>
        public void Publish(DateTime dateTime)
        {
            this.datePublished = dateTime;
            this.published = true;
        }

        /// <summary>
        /// String representation of book.
        /// </summary>
        /// <returns>Representation of book.</returns>
        public new string ToString() => $"{this.Title} by {this.Author}";

        /// <summary>
        /// Gets a information about time of publish.
        /// </summary>
        /// <returns>The string "NYP" if book not published, and the value of the datePublished if it is published.</returns>
        public string GetPublicationDate() => this.published ? this.datePublished.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "NYP";

        /// <summary>
        /// Sets the prise and currency of the book.
        /// </summary>
        /// <param name="price">Price of book.</param>
        /// <param name="currency">Currency of book.</param>
        /// <exception cref="ArgumentException">Throw when Price less than zero or currency is invalid.</exception>
        /// <exception cref="ArgumentNullException">Throw when currency is null.</exception>
        public void SetPrice(decimal price, string currency)
        {
            this.Price = price < 0 ? throw new ArgumentException("price cannot be null") : price;
            this.Currency = IsoCurrencyValidator.IsValid(currency) ? currency : throw new ArgumentException("InvalidCurrency");
        }

        /// <summary>
        /// Overrided object method equals.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>True if object as book has the same isbn.</returns>
        public override bool Equals(object obj) => !(obj as Book is null) && this.Equals(obj as Book);

        /// <summary>
        /// Compare equality of two books.
        /// </summary>
        /// <param name="other">Other book to compare with.</param>
        /// <returns>True if other book has the same isbn.</returns>
        public bool Equals(Book other) => object.Equals(this.ISBN, other?.ISBN);

        /// <summary>
        /// Calculate book hashcode.
        /// </summary>
        /// <returns>HashCode.</returns>
        public override int GetHashCode() => this.ISBN.GetHashCode(StringComparison.InvariantCulture) * this.Pages.GetHashCode() * this.Price.GetHashCode();

        /// <summary>
        /// Compare given object as book to this book.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>0 if the books have the same title, more than zero if first book apears earlier, ena less zero overwise.</returns>
        int IComparable.CompareTo(object obj)
            => obj as Book is null ? throw new ArgumentException("Other object must be Book type to be compared.") : this.CompareTo(obj as Book);

        /// <summary>
        /// Compare given book to this book.
        /// </summary>
        /// <param name="other">Book to compare with.</param>
        /// <returns>0 if the books have the same title, more than zero if first book apears earlier, ena less zero overwise.</returns>
#pragma warning disable CA1307 // Specify StringComparison
        public int CompareTo(Book other) => this.Title.CompareTo(other?.Title);
    }
}
