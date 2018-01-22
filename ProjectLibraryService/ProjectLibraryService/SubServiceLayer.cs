using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProjectLibraryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SubServiceLayer" in both code and config file together.
    public class SubServiceLayer : ISubServiceLayer
    {
        private LibraryService service = new LibraryService();

        #region Users
        public User GetUser(string idnp)
        {
            return service.GetUsers().SingleOrDefault(x => x.Idnp == idnp);
        }

        public bool CanLogIn(string email, string idnp)
        {
            return service.GetUsers().Any(x => x.Idnp == idnp && x.Email == email);
        }

        public byte[] GetUserImage(string idnp)
        {
            return service.GetUsers().Any(x => x.Idnp == idnp) ? service.GetUsers().Single(x => x.Idnp == idnp).UserPic : null;
        }

        #endregion

        #region Books
        public int GetUserBooksOnHandsCount(User user)
        {
            var q = service.GetOnHands(user);
            return q.Count();
        }

        public IEnumerable<OnHand> GetUserBook(int userId)
        {
            return service.DbContext.OnHandsBooks.Where(x => x.UserId == userId).Include(x => x.BookCode.Book).AsEnumerable();
        }

        public IEnumerable<BookCodeOwned> GetUserPreviouslyOwnedBooks(int userId)
        {
            var ownedCodes = service.DbContext.BookCodesOwned.Include(x=>x.BookCode.Book).Where(x=>x.UserId == userId).AsEnumerable();
            return ownedCodes;
        }


        public IEnumerable<ReservedBook> GetUserReservedBooks(int userId)
        {
            var reservedBooks = service.GetReservedBooks(userId);
            return reservedBooks;
        }

        public IEnumerable<Book> GetBooksCategories()
        {
            var bookCategories = service.GetAllBooks().GroupBy(x => x.Category).SelectMany(x => x.OrderByDescending(y => y.RegistrationDate).Take(4));
            return bookCategories;
        }

        public IEnumerable<Book> GetBooksByCategory(string category)
        {
            var books = service.GetAllBooks().Where(b => String.Equals(b.Category, category, StringComparison.CurrentCultureIgnoreCase));
            return books;
        }

        public int GetAvailableCount(int bookId)
        {
            int totalAmount = 0, reservedAmount = 0, onHandsAmount = 0;
            var book = service.GetAllBooks().FirstOrDefault(x => x.BookId == bookId);

            if (book != null)
            {
                totalAmount = book.Amount;
                reservedAmount = service.GetReservations().Count(x => x.BookCode.Book.BookId == book.BookId);
                onHandsAmount = service.GetAllOnHands().Count(x => x.Book.BookId == book.BookId);
            }

            int result = totalAmount - reservedAmount - onHandsAmount;

            return result;
        }

        public Book GetBookById(int bookId)
        {
            return service.GetAllBooks().FirstOrDefault(x => x.BookId == bookId);
        }

        public bool HasReserved(int userId, int bookId)
        {
            return
                service.DbContext.ReservedBooks.Include(x => x.BookCode.Book)
                    .Any(x => x.UserId == userId && x.BookCode.BookId == bookId);
        }

        public bool HasOnHands(int userId, int bookId)
        {
            return
                service.DbContext.OnHandsBooks.Include(x => x.BookCode.Book)
                    .Any(x => x.UserId == userId && x.BookCode.BookId == bookId);
        }

        public void AddReservation(int userId, int bookId)
        {
            var bookCode = service.DbContext.BookCodes.First(x => x.BookId == bookId).Code;

            service.AddReservationPrimitive(bookCode,userId, TimeSpan.FromDays(3));
        }

        public IEnumerable<Book> SearchBooks(string searchString)
        {
            var nString = searchString.ToLower();

            return service.DbContext.Books
                .Where(x => x.Title.ToLower().Contains(nString)
                            || x.Author.ToLower().Contains(nString)
                            || x.Category.ToLower().Contains(nString)
                            || x.Publisher.ToLower().Contains(nString)
                            || x.Description.ToLower().Contains(nString));
        }

        public IEnumerable<Book> GetNewestBooks(int count)
        {
            return service.DbContext.Books.OrderByDescending(x => x.RegistrationDate).Take(count);
        }

        public IEnumerable<Book> GetRecommendedBooks(int userId)
        {
            var skipBooksIds = new HashSet<int>();

            var categoriesCount = new Dictionary<string, int>();

            var readCategories =
                service.DbContext.BookCodesOwned.Include(x => x.BookCode.Book).Where(x=>x.UserId == userId).OrderByDescending(x=>x.LendDate)
                    .GroupBy(x => x.BookCode.Book.Category)
                    .OrderByDescending(x => x.Count());

            var reservedCategories =
                service.DbContext.ReservedBooks.Include(x => x.BookCode.Book)
                    .Where(x => x.UserId == userId).OrderByDescending(x=>x.TimeOut)
                    .GroupBy(x => x.BookCode.Book.Category)
                    .OrderByDescending(x => x.Count());

            var onHandsCategories =
                service.DbContext.OnHandsBooks.Include(x => x.BookCode.Book)
                    .Where(x => x.UserId == userId).OrderByDescending(x=>x.LendDate)
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

            readCategories.Select(x=>x.Select(y=>y.BookCode.BookId))
                .ToList()
                .ForEach(x=>x.ToList()
                    .ForEach(y=>skipBooksIds.Add(y
                    )
                )
            );

            reservedCategories.Select(x=>x.Select(y=>y.BookCode.BookId))
                .ToList()
                .ForEach(x=>x.ToList().ForEach(y=>skipBooksIds.Add(y)));

            onHandsCategories.Select(x => x.Select(y => y.BookCode.BookId))
                .ToList()
                .ForEach(x => x.ToList().ForEach(y => skipBooksIds.Add(y)));

            //skipBooksIds.AddRange(); ;

            var topCategories = categoriesCount.OrderBy(x => x.Value).Take(3);

            var recommendedBooks = new List<Book>();

            foreach (var category in topCategories)
            {
                var books = service.DbContext.Books.Where(x => !skipBooksIds.Contains(x.BookId) && x.Category == category.Key).Take(5);
                recommendedBooks.AddRange(books);
            }

            return recommendedBooks;

        }

        #endregion




    }
}
