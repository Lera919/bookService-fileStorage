using System;
using System.Text.RegularExpressions;

namespace VerificationService
{
    /// <summary>
    /// Verifies if the string representation of number is a valid ISBN-10 or ISBN-13 identification number of book.
    /// </summary>
    public static class IsbnVerifier
    {
        /// <summary>
        /// Verifies if the string representation of number is a valid ISBN-10 or ISBN-13 identification number of book.
        /// </summary>
        /// <param name="isbn">The string representation of book's isbn.</param>
        /// <returns>true if number is a valid ISBN-10 or ISBN-13 identification number of book, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if isbn is null.</exception>
        public static bool IsValid(string isbn)
        {
            if (isbn is null)
            {
                throw new ArgumentNullException(nameof(isbn));
            }

            if (isbn == "")
            {
                return true;
            }

            string pattern = @"^[0-9]{0,3}-??[0-9]{1}-??[0-9]{3}-??[0-9]{5}-??[0-9|X]{1}$";
            if (Regex.IsMatch(isbn, pattern))
            {
                if (isbn.StartsWith("978-"))
                {
                    return Check13SymbolsIsbn(isbn);
                }
                else
                {
                    return Check10SymboldIsbn(isbn);
                }

            }

            return false;
        }

        private static bool Check13SymbolsIsbn(string isbn)
        {
            int[] numbers = new int[13];
            int i = 0;
            foreach (char symbol in isbn)
            {
                if (char.IsDigit(symbol))
                {
                    numbers[i] = symbol - '0';
                    i++;
                }
            }

            int chekSum = (10 - ((numbers[0] + numbers[2] + numbers[4] + numbers[6] + numbers[8] + numbers[10] + numbers[12] +
                3 * (numbers[1] + numbers[3] + numbers[5] + numbers[7] + numbers[9] + numbers[11])) % 10)) % 10;
            return chekSum == 0;
        }

        private static bool Check10SymboldIsbn(string isbn)
        {
            int k = 10, checksum = 0;
            foreach (char symbol in isbn)
            {
                if (char.IsDigit(symbol))
                {
                    checksum += (symbol - '0') * k;
                    k--;
                }
            }

            if (isbn[^1] == 'X')
            {
                checksum += 10;
            }

            return checksum % 11 == 0;
        }
    }
}
