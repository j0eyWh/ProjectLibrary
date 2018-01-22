using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DataGateway.EntityModels;
using LibraryApi.Common;

namespace LibraryApi.Controllers
{
    [RoutePrefix("api/books")]
    [Authorize]
    public class BooksController : ApiControllerBase
    {
        [Route("book/{id}")]
        public Book GetBook(int id)
        {
            return UnitOfWork.Books.GetById(id);
        }

        [Route("{id}/amount")]
        [AllowAnonymous]
        public int GetAvailableAmount(int id)
        {
            int totalAmount = 0, reservedAmount = 0, onHandsAmount = 0;
          
            var book = GetBook(id);

            if (book != null)
            {
                totalAmount = book.Amount;
                reservedAmount = UnitOfWork.ReservedBooks.Get(x=>x.BookCode.BookId == book.BookId).Count();
                onHandsAmount = UnitOfWork.OnHandsBooks.Get(x=>x.BookCode.BookId==book.BookId).Count();
            }

            int result = totalAmount - reservedAmount - onHandsAmount;

            return result;
        }

        [Route("latest/{count}")]
        public IEnumerable<Book> GetLatestBooks(int count)
        {
            return UnitOfWork.Books.Get().OrderByDescending(x => x.RegistrationDate).ToList().Take(count);
        }

        [Route("recommended/{userId}")]
        public IEnumerable<Book> GetRecommendedBooks(int userId)
        {
            var skipBooksIds = new HashSet<int>();

            var categoriesCount = new Dictionary<string, int>();

            var readCategories =
                UnitOfWork.OwnedBooks.Get(x => x.UserId == userId, includeProperties: "BookCode.Book").OrderByDescending(x => x.LendDate)
                    .GroupBy(x => x.BookCode.Book.Category)
                    .OrderByDescending(x => x.Count());

            var reservedCategories = UnitOfWork.ReservedBooks
                .Get(x => x.UserId == userId, includeProperties: "BookCode.Book")
                .Where(x => x.UserId == userId).OrderByDescending(x => x.TimeOut)
                .GroupBy(x => x.BookCode.Book.Category)
                .OrderByDescending(x => x.Count());

            var onHandsCategories = UnitOfWork.OnHandsBooks
                .Get(x=>x.UserId==userId, includeProperties:"BookCode.Book")
                    .Where(x => x.UserId == userId).OrderByDescending(x => x.LendDate)
                    .GroupBy(x => x.BookCode.Book.Category)
                    .OrderByDescending(x => x.Count());

            foreach (var readCategory in readCategories)
            {
                categoriesCount.Add(readCategory.Key, readCategory.Count());
            }

            foreach (var reservedCategory in reservedCategories)
            {
                int v;
                if (categoriesCount.TryGetValue(reservedCategory.Key, out v))
                {
                    categoriesCount[reservedCategory.Key] += reservedCategory.Count();
                }
                else
                {
                    categoriesCount.Add(reservedCategory.Key, reservedCategory.Count());
                }
            }

            foreach (var onHandCategory in onHandsCategories)
            {
                int v;
                if (categoriesCount.TryGetValue(onHandCategory.Key, out v))
                {
                    categoriesCount[onHandCategory.Key] += onHandCategory.Count();
                }
                else
                {
                    categoriesCount.Add(onHandCategory.Key, onHandCategory.Count());
                }
            }

            readCategories.Select(x => x.Select(y => y.BookCode.BookId))
                .ToList()
                .ForEach(x => x.ToList()
                    .ForEach(y => skipBooksIds.Add(y
                    )
                )
            );

            reservedCategories.Select(x => x.Select(y => y.BookCode.BookId))
                .ToList()
                .ForEach(x => x.ToList().ForEach(y => skipBooksIds.Add(y)));

            onHandsCategories.Select(x => x.Select(y => y.BookCode.BookId))
                .ToList()
                .ForEach(x => x.ToList().ForEach(y => skipBooksIds.Add(y)));

            //skipBooksIds.AddRange(); ;

            var topCategories = categoriesCount.OrderBy(x => x.Value).Take(3);

            var recommendedBooks = new List<Book>();

            foreach (var category in topCategories)
            {
                var books = UnitOfWork.Books.Get().Where(x => !skipBooksIds.Contains(x.BookId) && x.Category == category.Key).Take(5);
                recommendedBooks.AddRange(books);
            }

            return recommendedBooks;
        }

        [Route("categories")]
        public IEnumerable<Book> GetBooksByCategories()
        {
            var bookCategories = UnitOfWork.Books.Get().GroupBy(x => x.Category).SelectMany(x => x.OrderByDescending(y => y.RegistrationDate).Take(4));
            return bookCategories;
        }

        [Route("search/{searchString}")]
        [HttpGet]
        public IEnumerable<Book> Search(string searchString)
        {
            var nString = searchString.ToLower();

            return UnitOfWork.Books.Get(x => x.Title.ToLower().Contains(nString)
                                             || x.Author.ToLower().Contains(nString)
                                             || x.Category.ToLower().Contains(nString)
                                             || x.Publisher.ToLower().Contains(nString)
                                             || x.Description.ToLower().Contains(nString));
        }

        [Route("{category}")]
        public IEnumerable<Book> GetBooksByCategory(string category)
        {
            return UnitOfWork.Books.Get().Where(x=>string.Equals(x.Category, category, StringComparison.CurrentCultureIgnoreCase));
        }

        [Route("book/{bookId}/coverImage")]
        [AllowAnonymous]
        public HttpResponseMessage GetBookCover(int bookId)
        {
            var image = UnitOfWork.Books.Get(x => x.BookId == bookId).FirstOrDefault()?.Image;

            var response = new HttpResponseMessage();

            if (image == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new ByteArrayContent(image);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            }

            return response;
        }

        [Route("{bookId}/readingPositions")]
        public IEnumerable<ReadingPosition> GetBookReadingPostions(int bookId)
        {
            return UnitOfWork.ReadingPostions.Get(x => x.BookCode.Book.BookId == bookId, includeProperties:"User");
        }

        [Route("spotlight")]
        [AllowAnonymous]
        public IEnumerable<Book> GetSpotlightBooks()
        {
            var books = UnitOfWork.Books.Get(filter: x => x.BookTags.Any(y => y.Tag == "Spotlight"), includeProperties:"BookTags");
            return books;
        } 
    }
}
