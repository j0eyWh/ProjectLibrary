using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using DataGateway.EntityModels;
using LibraryApi.Common;

namespace LibraryApi.Controllers
{
    [RoutePrefix("api/Users")]
    [Authorize]
    public class UsersController : ApiControllerBase
    {
        [Route("CanLogIn/{userName}/{idnp}")]
        [HttpGet]
        public bool CanLogIn(string userName, string idnp)
        {
            return UnitOfWork.Users.Get(x => x.Email == userName && x.Idnp == idnp).Any();
        }

        [Route("user/idnp/{idnp}")]
        [HttpGet]
        public User GetUserByIdnp(string idnp)
        {
            var user = UnitOfWork.Users.Get(x => x.Idnp == idnp).ToList().FirstOrDefault();
            return user;
        }

        [Route("user/{userId}/hasReserved/{bookId}")]
        [HttpGet]
        public bool HasReserved(int userId, int bookId)
        {
            return UnitOfWork.ReservedBooks.Get(x => x.BookCode.BookId == bookId && x.UserId == userId).Any();
        }

        [Route("user/{userId}/hasOnHands/{bookId}")]
        [HttpGet]
        public bool HasOnHands(int userId, int bookId)
        {
            return UnitOfWork.OnHandsBooks.Get(x => x.BookCode.BookId == bookId && x.UserId == userId).Any();
        }

        [Route("user/{userId}/reserve/{bookId}")]
        [HttpGet]
        public void AddReservation(int userId, int bookId)
        {
            var code = UnitOfWork.BookCodes.Get(x => x.BookId == bookId).FirstOrDefault();

            UnitOfWork.ReservedBooks.Add(new ReservedBook()
            {
                BookCode = code,
                UserId = userId,
                TimeOut = DateTime.Now + TimeSpan.FromDays(3),
            });

            UnitOfWork.Save();
        }

        [Route("user/{userId}/reserve2/{bookId}")]
        [HttpGet]
        public HttpResponseMessage Reserve(int userId, int bookId)
        {
            if (HasOnHands(userId, bookId))
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            if (HasReserved(userId, bookId))
            {
                return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
            }

            var r = new ReservedBook()
            {
                BookCode = UnitOfWork.BookCodes.Get(x => x.BookId == bookId).FirstOrDefault(),
                UserId = userId,
                TimeOut = DateTime.Now +
                TimeSpan.FromDays(3)
            };

            UnitOfWork.ReservedBooks.Add(r);
            UnitOfWork.Save();

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [Route("user/{userId}/onHands")]
        [HttpGet]
        public IEnumerable<OnHand> GetUserBooksOnHands(int userId)
        {
            var r =
                UnitOfWork.Users.Get(x => x.Idnp == userId.ToString() || x.UserId == userId, includeProperties: "OnHands.BookCode.Book").SelectMany(x => x.OnHands);
            return r;
        }

        [Route("user/{userId}/previouslyOwned")]
        [HttpGet]
        public IEnumerable<BookCodesOwned> GetUserPreviouslyOwnedBooks(int userId)
        {

            var r =
                UnitOfWork.Users.Get(x => x.Idnp == userId.ToString() || x.UserId == userId,
                    includeProperties: "BookCodesOwneds.BookCode.Book").SelectMany(x => x.BookCodesOwneds);
            return r;

        }

        [Route("user/{userId}/reserved")]
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<ReservedBook> GetUserReservedBooks(int userId)
        {
            var r =
                UnitOfWork.Users.Get(x => x.Idnp == userId.ToString() || x.UserId == userId,
                    includeProperties: "ReservedBooks.BookCode.Book").SelectMany(x => x.ReservedBooks);
            return r;
        }

        public class Model
        {
            public int userId { get; set; }
            public string category { get; set; }
        }

        [Route("{userId}/follow/{category}")]
        [HttpPost]
        public bool AddUserCategory(int userId, string category)
        {
            var model = new UserCategory()
            {
                UserId = userId,
                CategoryTitle = category
            };

            try
            {
                if (UnitOfWork.Users.Get(x=>x.UserId == userId, includeProperties: "Categories").FirstOrDefault().Categories.Any(x=>x.CategoryTitle.ToLower()==category.ToLower()))
                    return false;

                UnitOfWork.UserCategories.Add(model);
                UnitOfWork.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("{userId}/following")]
        [HttpGet]
        public IEnumerable<Book> GetFollowing(int userId)
        {
            var categories = UnitOfWork.UserCategories.Get(x => x.UserId == userId).Select(x => x.CategoryTitle.ToLower());

            return UnitOfWork.Books.Get(x => categories.Contains(x.Category.ToLower())).GroupBy(x=>x.Category).SelectMany(x=>x.Take(4));
        }

        [Route("readingPositon")]
        [HttpPost]
        public HttpResponseMessage AddReadingPostion(ReadingPosition readingPosition)
        {
            if (readingPosition == null) return Request.CreateResponse(HttpStatusCode.NoContent);

            UnitOfWork.ReadingPostions.Add(readingPosition);
            UnitOfWork.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{userId}/suitablePosition")]
        [AllowAnonymous]
        public IEnumerable<ReadingPosition> GetSuitableReadingPositions(int userId)
        {
            var ownedIds = UnitOfWork.OnHandsBooks.Get(x => x.UserId == userId, includeProperties: "BookCode.Book").Select(x => x.BookCode.BookId);

            var r = UnitOfWork.ReadingPostions.Get(x => ownedIds.Contains(x.BookCode.Book.BookId), includeProperties:"BookCode.Book, User");

            return r;

        }

        [Route("edit")]
        [HttpPost]
        public bool UpdateUser(User user)
        {
            try
            {
                UnitOfWork.Users.Update(user);
                UnitOfWork.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }


    }
}
