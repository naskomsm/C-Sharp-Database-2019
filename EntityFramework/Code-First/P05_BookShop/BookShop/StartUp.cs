namespace BookShop
{
    using BookShop.Models;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                DbInitializer.ResetDatabase(db);
                var result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var booksTitles = context.Books
                 .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                 .Select(b => b.Title)
                 .OrderBy(t => t)
                 .ToList();

            var sb = new StringBuilder();

            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var booksTitles = context.Books
                .Where(b => b.EditionType.ToString().ToLower() == "gold")
                .Where(b => b.Copies < 5000)
                .Select(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksTitles = context.Books
                .Where(b => b.ReleaseDate.GetValueOrDefault().Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();


            var sb = new StringBuilder();

            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var query = EF.CompileQuery<BookShopContext, string, IEnumerable<string>>(
                   (db, categoryText) => db.Books
                                .Where(b => b.BookCategories.Any(bk => bk.Category.Name.ToLower() == categoryText.ToLower()))
                                .Select(b => b.Title)
                );

            var categories = input.Split(" ");

            var sb = new StringBuilder();

            var result = new List<string>();

            foreach (var category in categories)
            {
                var current = query(context, category);
                result.AddRange(current);
            }

            foreach (var line in result.OrderBy(t => t))
            {
                sb.AppendLine(line);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var format = "dd-MM-yyyy";
            var provider = CultureInfo.InvariantCulture;
            var parsedDate = DateTime.ParseExact(date, format, provider);

            var books = context.Books
                  .Where(b => b.ReleaseDate.GetValueOrDefault() < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            var sb = new StringBuilder();
            
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorsNames = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var authorName in authorsNames)
            {
                sb.AppendLine(authorName.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var booksTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            var sb = new StringBuilder();

            foreach (var title in booksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksTitles = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new 
                {
                    AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                    b.Title
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in booksTitles)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var numberOfBooksByGivenCriteria = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList()
                .Count();

            return numberOfBooksByGivenCriteria;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsCopies = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    BookCopies = context.Books
                                        .Where(b => b.Author.AuthorId == a.AuthorId)
                                        .Select(b => b.Copies)
                                        .Sum()
                })
                .OrderByDescending(a => a.BookCopies)
                .ToList();

            var sb = new StringBuilder();

            foreach (var authorCopy in authorsCopies)
            {
                sb.AppendLine($"{authorCopy.FullName} - {authorCopy.BookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks
                                        .Select(cb => cb.Book.Price * cb.Book.Copies)
                                        .Sum()
                })
                .OrderByDescending(c => c.Profit).ThenBy(c => c.Name)
                .ToList();


            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Name} ${category.Profit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                                    .Select(cb => new
                                    {
                                        cb.Book.Title,
                                        cb.Book.ReleaseDate
                                    })
                                    .OrderByDescending(cb => cb.ReleaseDate)
                                    .Take(3)
                                    .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.GetValueOrDefault().Year})");
                }
            }


            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.GetValueOrDefault().Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return books.Count;
        }
    }
}
