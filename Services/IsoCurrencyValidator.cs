using System;
using System.Globalization;

namespace VerificationService
{
    /// <summary>
    /// Class for validating currency strings.
    /// </summary>
    public static class IsoCurrencyValidator
    {
        /// <summary>
        /// Determines whether a specified string is a valid ISO currency symbol.
        /// </summary>
        /// <param name="currency">Currency string to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="currency"/> is a valid ISO currency symbol; <see langword="false"/> otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if currency is null or empty or whitespace or white-space.</exception>
        public static bool IsValid(string currency)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException(nameof(currency));
            }

            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                if (new RegionInfo(culture.Name).ISOCurrencySymbol == currency)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
