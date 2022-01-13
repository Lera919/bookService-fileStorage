using System;
using System.Collections.Generic;
using System.Text;
using BookClass;
using NUnit.Framework;

namespace BookServiceTask.Tests
{
    class TestCasesSource
    {
        public static IEnumerable<TestCaseData> TestCasesSource_BookStorage
        {
            get
            {
                yield return new TestCaseData(
                        new[]
                    {
                    new Book("Jon Skeet", "After dark", "Manning Publications", "978-0-901-69066-1"),
                    new Book("Somebody", "After you", "Manning Publications", "3-598-21508-8"),
                    new Book("Vova Korsak", "Something", "Manning Publications", "978-5-472-01012-2"),
                    new Book("Jon", "Eat, pray, love", "Manning Publications", "978-0-901-69066-1"),
                    new Book("Vova Korsak", "La-la-land", "Manning Publications", "3-598-21508-8"),
                    new Book("Jon", "Java", "Something", "978-0-901-69066-1"),
                    new Book("Adam Levin", "XXX", "Manning Publications", "3-598-21508-8"),
                    }, new[] { 123m, 190m, 200m, 220m, 220m, 245m, 250m },
                       new[] { 200, 450, 768, 87, 165, 333, 444 });
            }
        }

        public static IEnumerable<TestCaseData> TestCasesSource_BookStorage_Published
        {
            get
            {
                yield return new TestCaseData(
                        new[]
                    {
                    new Book("Гарри  Гаррисон", "Продается планета", "Росмен", "978-0-901-69066-1"),
                    new Book("Франсис Карсак", "Робинзоны космоса", "Азбука", "978-5-389-19032-0"),
                    new Book("Джо Аберкромби", "Мудрость толпы", "Эксмо", "3-598-21508-8"),
                    new Book("Аластер Рейнольдс", "Алмазные псы", "Азбука", "978-0-901-69066-1"),
                    }, new[] { 123m, 190m, 200m, 220m },
                       new[] { 200, 450, 768, 87 },
                       new[] { new DateTime(2021, 12, 1), new DateTime(2020, 9,9), new DateTime(2011, 10, 12), new DateTime(2000, 2, 27)});
            }
        }
    }
}
