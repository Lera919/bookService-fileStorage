using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BookClass;
using Interfaces;

namespace Predicates
{
    public class PublishedDatePredicate : IPredicate<Book>
    {
        private readonly DateTime defaultMinDate = new DateTime(1900, 1, 1);
        private DateTime date;

        public PublishedDatePredicate(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date
        {
            get => this.date;
            private set => this.date =
               value < this.defaultMinDate || value > DateTime.Now
                ? throw new ArgumentException("Minimum date cannot be more than now or less than zero or more than maximum one.") : value;
        }

        public bool Invoke(Book item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.GetPublicationDate().Equals("NYP", StringComparison.InvariantCulture))
            {
                return false;
            }

            DateTime.TryParseExact(item.GetPublicationDate(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue);
            return dateValue == this.Date;
        }
    }
}
