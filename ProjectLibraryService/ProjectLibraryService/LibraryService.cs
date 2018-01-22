using ProjectLibraryService.Reports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class LibraryService : ILibraryService
    {
        /// <summary>
        /// Main DbContext.
        /// </summary>
        internal RmfModel DbContext { get; set; }

        public LibraryService()
        {
            DbContext = new RmfModel();
        }


        private bool ExistsRmf(string id)
        {
            if (DbContext.Rmf.Where(x => x.IdRmf == id).Count() == 0)
                return false;

            else
                return true; 
        }

        /// <summary>
        /// Find a Rmf object by its unique DocId
        /// </summary>
        /// <param name="docId">the id of the document</param>
        /// <returns>a Rmf object with id == docId</returns>
        public Rmf GetRmfByDocId(string docId)
        {
            var s = DbContext.Rmf.Where(x => x.DocId == docId);

            if (s.Count()>0)
            {
                return s.Single();
            }
            else
                return null;
        }

        
        //Here will get the list of all Rmf in the database
        public List<Rmf> GetRmfList()
        {
            return DbContext.Rmf.OrderBy(x => x.IdRmf).ToList();
        }

        public List<Rmf> GetInRmf()
        {
            return DbContext.Rmf.Where(x => (x.IsOut == false || x.IsOut == null)).OrderBy(x => x.DateIn).ToList();
        }

        public List<Rmf> GetOutRmf()
        {
            return DbContext.Rmf.Where(x => (x.IsOut == true)).OrderBy(x => x.DateIn).ToList();
        }

        public Rmf GetRmfById(string idRmf)
        {
            if (ExistsRmf(idRmf))
                return DbContext.Rmf.Where(x => x.IdRmf == idRmf).Single();
            else
                return null;
        }

        

        /// <summary>
        /// Insert a new Rmf entry into the DB
        /// </summary>
        /// <param name="newRmf">an object of type Rmf that will be insertet</param>
        /// <returns>true if it was insertetd succcesfuly</returns>
        public bool InsertRmf(Rmf newRmf)
        {
            try
            {
                DbContext.Rmf.Add(newRmf);
                DbContext.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw dbEx;
            }
        }

        /// <summary>
        /// Delete an existing Rmf object from the db;
        /// </summary>
        /// <param name="id">id of the Rmf object that will be deleted</param>
        /// <returns></returns>
        public bool DeleteRmf(string id) {

            if (!ExistsRmf(id))
            {
                throw new ArgumentException(message: "There isn't an element with this id:" + id);
            }
            else
            {
                try
                {
                    DbContext.Rmf.Remove(DbContext.Rmf.Where(x => x.IdRmf == id).Single());
                    DbContext.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException dbEx)
                {
                    throw dbEx;
                }
            }
        }

        public void DeleteMultipleRmf(List<Rmf> l)
        {
            foreach (var item in l)
            {
                DeleteRmf(item.IdRmf);
            }
        }

        #region Updating stuff
        /// <summary>
        /// Update an existing Rmf object in db
        /// </summary>
        /// <param name="rmfObj">object to be </param>
        /// <returns>true if update succeded</returns>
        public bool UpdateRmf(Rmf rmfObj)
        {
            if (!ExistsRmf(rmfObj.IdRmf))
            {
                throw new ArgumentException(message: "There isn't an element with this id:" + rmfObj.IdRmf);
            }

            else
            {
                try
                {
                    DbContext.Rmf.Attach(rmfObj);
                    var d = DbContext.Entry(rmfObj);
                    d.Property(x => x.DocId).IsModified = true;
                    //d.Property(x => x.DocOutId).IsModified = true;
                    d.Property(x => x.DocImg).IsModified = true;
                    d.Property(x => x.Quantity).IsModified = true;
                    d.Property(x => x.TotalValue).IsModified = true;
                    d.Property(x => x.FirstInvNr).IsModified = true;
                    d.Property(x => x.ContentCat).IsModified = true;
                    d.Property(x => x.Origin).IsModified = true;
                    d.Property(x => x.LastInvNr).IsModified = true;
                    d.Property(x => x.OutCause).IsModified = true;
                    d.Property(x => x.IsOut).IsModified = true;
                    d.Property(x => x.DateOut).IsModified = true;
                    DbContext.SaveChanges();

                }
                catch (DbEntityValidationException dbEx)
                {

                    throw dbEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return true;
        }

      
        #endregion

        /// <summary>
        /// A method that will get the last index;
        /// This will help to generate a index for a new Rmf object
        /// </summary>
        /// <returns></returns>
        public string GetLastIndex()
        {
            List<string> idList = new List<string>();
            List<int> idNr = new List<int>();

            foreach (var item in GetRmfList())
            {
                string[] s = item.IdRmf.Split('_');
                idNr.Add(int.Parse(s[0]));
            }

            var last = 0;
            if (idNr.Any())
            {
                last = idNr.Max();
            }
            
            return $"{last}_2015";
        }

        public int GetLastLastInvNr()
        {
            var list = GetRmfList().OrderBy(z => z.LastInvNr);

            if (list.Any())
            {
                return list.Last().LastInvNr;
            }

            return 1;
        }



        #region AnualReports
        public AnualReport GenerateAnualReport()
        {
            List<Rmf> list = this.DbContext.Rmf.ToList();

            AnualReport _report = new AnualReport();

            Dictionary<int, AnualReportData> _aboutEntries = new Dictionary<int, AnualReportData>();
            Dictionary<int, AnualReportData> _aboutExits = new Dictionary<int, AnualReportData>();

            List<AnualReportData> _aboutEntriesList = new List<AnualReportData>();
            List<AnualReportData> _aboutExitsList = new List<AnualReportData>();

            //Getting all the years of activity
            var years = list.OrderBy(x => x.DateIn.Year)
                .GroupBy(x => x.DateIn.Year)
                .Select(x => new
                {
                    year = x.FirstOrDefault().DateIn.Year
                })
                .Select(x => x.year);

            var yearOut = list.Where(x=>x.DateOut!= null).GroupBy(x => x.DateOut.Value.Year)
                .Select(x => new
                {
                    year = x.FirstOrDefault().DateOut.Value.Year
                })
                .Select(x => x.year);

            List<int> activityYears = years.Union(yearOut.ToList()).ToList();


            //Population dictionaries with keys
            foreach (var year in activityYears)
            {
                _aboutEntries[year] = new AnualReportData { AverageValue = 0, NrOfCollections = 0, TotalQuantity = 0, TotalValue = 0, Year = year };
                _aboutExits[year] = new AnualReportData { AverageValue = 0, NrOfCollections = 0, TotalQuantity = 0, TotalValue = 0, Year = year };
            }

            //Geting data about in-activity by years
            var queryEn = list.GroupBy(x => x.DateIn.Year)
                .Select(x => new
                {
                    year = x.FirstOrDefault().DateIn.Year,
                    totalQuantity = x.Sum(y => y.Quantity),
                    totalValue = x.Sum(y => y.TotalValue),
                    nrOfCollections = x.Count(),
                    averageValue = x.Average(y => y.TotalValue)

                });

            
            //Populatin the _aboutEntriesList
            foreach (var item in queryEn)
            {
                _aboutEntriesList.Add(new AnualReportData
                {
                    Year = item.year,
                    AverageValue = (decimal)item.averageValue,
                    NrOfCollections = item.nrOfCollections,
                    TotalQuantity = item.totalQuantity,
                    TotalValue = (decimal)item.totalValue

                    
                });
            }

            //Geting data about Exit Activity by years
            var queryEx = list.Where(x => x.IsOut == true)
                .GroupBy(x => x.DateOut.Value.Year)
                .Select(x => new
                {
                    year = x.FirstOrDefault().DateOut.Value.Year,
                    totalQuantity = x.Sum(y => y.Quantity),
                    totalValue = x.Sum(y => y.TotalValue),
                    nrOfCollection = x.Count(),
                    averageValue = x.Average(y => y.TotalValue)
                });

            //Populatin the _aboutExitsList
            foreach (var item in queryEx)
            {
                _aboutExitsList.Add(new AnualReportData
                {
                    Year = item.year,
                    TotalValue = (decimal)item.totalValue,
                    NrOfCollections = item.nrOfCollection,
                    AverageValue = (decimal)item.averageValue,
                    TotalQuantity = item.totalQuantity

                });
            }

            //Populating dictionaries
            foreach (var item in _aboutEntriesList)
            {
                _aboutEntries[item.Year] = item;
            }
            foreach (var item in _aboutExitsList)
            {
                _aboutExits[item.Year] = item;
            }

            //Build the object
            _report.AboutEntries = _aboutEntries.OrderBy(x=>x.Key).ToDictionary(x=>x.Key, x=>x.Value);
            _report.AboutExits = _aboutExits.OrderBy(x=>x.Key).ToDictionary(x=>x.Key, x=>x.Value);

            //return
            return _report;
        }

        #endregion

        #region Part3Report
        public Part3Report GeneratePart3Report()
        {
            Dictionary<int, Part3ReportData> _d = new Dictionary<int, Part3ReportData>();


            //Getting years of activity
            var years = GetRmfList().GroupBy(x => x.DateIn.Year)
                .Select(x => new
                {
                    year = x.FirstOrDefault().DateIn.Year
                })
                .Select(x=>x.year);
            var years2 = GetRmfList().Where(x=>x.DateOut!= null)
                .GroupBy(x => x.DateOut.Value.Year)
                .Select(x => new
                {
                    year = x.FirstOrDefault().DateOut.Value.Year
                })
                .Select(x=>x.year)
                .Union(years);
            //Populating dictionary with years of activiti an objects that are 0
            //In case there is no activity 
            foreach (var year in years2)
            {
                _d.Add(year,
                    new Part3ReportData { Year = year, OutBooks = 0, RegisteredBooks = 0 });
            }

            foreach (var year in years2)
            {
                _d[year].RegisteredBooks = GetRmfList().Where(x => x.DateIn.Year <= year).Sum(x => x.Quantity);
                _d[year].OutBooks = GetRmfList().Where(x => x.DateOut != null).Where(x => x.DateOut.Value.Year == year).Sum(x => x.Quantity);
            }

            return new Part3Report { DataCollection = _d.OrderBy(x=>x.Key).ToDictionary(x=>x.Key, x=>x.Value) };


        }
        #endregion

        #region BookCodes
        public List<BookCode> GetBookCodes(Book book)
        {
            return DbContext.BookCodes.Where(x => x.BookId == book.BookId).ToList();
        }

        
        #endregion

        #region Books

        public List<Book> GetAllBooks()
        {
            if (DbContext.Books !=null)
            {
                return DbContext.Books.OrderBy(x => x.BookId).ToList();

            }

            else
            {
                return null;
            }
        }

        public List<Book> GetBooksByRmfId(string rmfId)
        {
            return DbContext.Books.Where(x => x.CollectionId == rmfId).ToList();
        }

        public void AddBook(Book book)
        {

            //Checking if the books is not null;
            if (book == null)
            {
                ArgumentException argE = new ArgumentException("Argument: book can't be null");
                FaultException<ArgumentException> faultE = new FaultException<ArgumentException>(argE, argE.Message);
                throw faultE;
            }
            //Checking if the book does already exist;
            if (DbContext.Books.Where(x => x.BookId == book.BookId).Count() == 1)
            { 
                ArgumentException argE = new ArgumentException(message: "Book's already registered");
                FaultException<ArgumentException> faultE = new FaultException<ArgumentException>(argE, argE.Message);
                throw faultE;
            }
            //Check if the book has a valid CollectionId;
            if (DbContext.Rmf.Where(x => x.IdRmf == book.CollectionId).Count() == 0)
            {
                ArgumentException argE = new ArgumentException(message: $"Book's collection Id does not exist {book.CollectionId}");
                FaultException<ArgumentException> faultE = new FaultException<ArgumentException>(argE, argE.Message);
                throw faultE;
            }
            //Checking if the book's collection is not set out;
            if (DbContext.Rmf.Where(x => x.IdRmf == book.CollectionId).Single().IsOut == true)
            {
                ArgumentException argE = new ArgumentException(message: $"Book's collection is set out {book.CollectionId}");
                FaultException<ArgumentException> faultE = new FaultException<ArgumentException>(argE, argE.Message);
                throw faultE;
            }

            //Check if the book category isn't null or empty
            if (String.IsNullOrEmpty(book.Category))
            {
                ArgumentException argE = new ArgumentException(message: $"Book's category is null or empty");
                FaultException<ArgumentException> faultE = new FaultException<ArgumentException>(argE, argE.Message);
                throw faultE;
            }

            int alreadyRegisterdBooksAmount = 0;

            if(DbContext.Books.Where(x => x.CollectionId == book.CollectionId).Count() >= 1)
            {
                alreadyRegisterdBooksAmount = DbContext.Books.Where(x => x.CollectionId == book.CollectionId).Sum(x => x.Amount);    
            }

            int collectionQuantity = DbContext.Rmf.Where(x => x.IdRmf == book.CollectionId).Single().Quantity;

            int maxPossibleAmount = collectionQuantity - alreadyRegisterdBooksAmount;

            if (book.Amount > collectionQuantity)
            {
                
                ArgumentException arg = new ArgumentException($"Book amount ({book.Amount}) can't be bigger than its collection Quantity: {collectionQuantity}");
                FaultException<ArgumentException> fe = new FaultException<ArgumentException>(arg, arg.Message);
                throw fe;
            }

            if (book.Amount > maxPossibleAmount)
            {
                ArgumentException arg = new ArgumentException($"Book amount ({book.Amount}) can't be bigger than amount of books already registered: {alreadyRegisterdBooksAmount}");
                FaultException<ArgumentException> fe = new FaultException<ArgumentException>(arg, arg.Message);
                throw fe;
            }
                

            DbContext.Books.Add(book);

            List<BookCode> codes = new List<BookCode>();

            for (int i = 0; i < book.Amount; i++)
            {
                codes.Add(new BookCode() { BookId = book.BookId });
            }

            DbContext.BookCodes.AddRange(codes);
            DbContext.SaveChanges();

            
        } 

        public void DeleteBook(Book book)
        {
            if (book!=null && DbContext.Books.Contains(book))
            {
                DbContext.Books.Remove(book);
                DbContext.BookCodes.RemoveRange(DbContext.BookCodes.Where(x => x.BookId == book.BookId));
                DbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"Error: There isn't a book with this Id: {book.BookId}");
            }
        }

        public void DeleteBookById(int bookId)
        {

            Book toBeDeleted;
            if (DbContext.Books.Where(x => x.BookId == bookId).Count() == 1)
            {
                toBeDeleted = DbContext.Books.Where(x => x.BookId == bookId)?.Single();
            }
            else
            {
                toBeDeleted = null;
            }

            
            if (toBeDeleted!=null)
            {
                DbContext.Books.Remove(toBeDeleted);
                DbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"Error: There isn't a book with this Id: {bookId}");
            }
        }


        public void UpdateBook(Book book)
        {
            DbContext.Books.Attach(book);
            var d = DbContext.Entry(book);
            d.Property(x => x.Amount).IsModified = true;
            d.Property(x => x.Author).IsModified = true;
            d.Property(x => x.CanLend).IsModified = true;
            d.Property(x => x.CollectionId).IsModified = true;
            d.Property(x => x.Description).IsModified = true;
            d.Property(x => x.Image).IsModified = true;
            d.Property(x => x.Price).IsModified = true;
            d.Property(x => x.Publisher).IsModified = true;
            d.Property(x => x.RegistrationDate).IsModified = true;
            d.Property(x => x.Title).IsModified = true;
            d.Property(x => x.Year).IsModified = true;
            d.Property(x => x.Category).IsModified = true;


            int alreadyRegisterdBooksAmount = 0;
            if (DbContext.Books.Where(x => x.CollectionId == book.CollectionId).Count() >= 1)
                           alreadyRegisterdBooksAmount = DbContext.Books.Where(x => x.CollectionId == book.CollectionId).Sum(x => x.Amount);
      
            int collectionQuantity = DbContext.Rmf.Where(x => x.IdRmf == book.CollectionId).Single().Quantity;
            int registeredAmount = 0;
            int id = book.BookId;

            if(DbContext.Books.Where(x => x.CollectionId == book.CollectionId  && x.BookId != id).Any())
            {
                registeredAmount = DbContext.Books.Where(x => x.CollectionId == book.CollectionId && x.BookId != id).Sum(x => x.Amount);
            }


            if (book.Amount>collectionQuantity)
            {
                ArgumentException argE = new ArgumentException(message: $"Book amount cant be bigger than colection quantity {book.CollectionId}: {collectionQuantity}");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }

            if (book.Amount + registeredAmount > collectionQuantity)
            {
                ArgumentException argE = new ArgumentException(message: $"Book amount + books already registered is bigger than collection quantity {book.Amount + registeredAmount}>{collectionQuantity}");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }

            int numberOfCodes = DbContext.BookCodes.Where(x => x.BookId == book.BookId).Count();

            if (book.Amount > numberOfCodes)
            {
                int x = book.Amount - numberOfCodes;
                for (int i = 0; i < x; i++)
                {
                    DbContext.BookCodes.Add(new BookCode()
                    {
                        BookId = book.BookId,
                    });
                }
            }
            if (book.Amount < numberOfCodes)
            {
                int x = numberOfCodes - book.Amount;
                for (int i = 0; i < x; i++)
                {
                    BookCode c = DbContext.BookCodes.Where(s => s.BookId == book.BookId).OrderByDescending(xs => xs.Code).First();
                    DbContext.BookCodes.Remove(c);
                    DbContext.SaveChanges();
                }
            }

            DbContext.SaveChanges();

        }

        #endregion

        #region Users

        public List<User> GetUsers()
        {
            return DbContext.Users.ToList();
        }

        public User GetUserById(int id)
        {
            if (DbContext.Users.Where(x => x.UserId == id).Any())
            {
                return DbContext.Users.Where(x => x.UserId == id).Single();
            }
            else
            {
                ArgumentException argE = new ArgumentException(message: "There isn't a user with this id");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }
            
        }

        public void AddUser(User user)
        {
            if (user!=null)
            {
                if (DbContext.Users.Any(x=>x.Email == user.Email))
                {
                    ArgumentException argE = new ArgumentException(message: "An user with this idnp is already registered");
                    FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                    throw f;
                }
                if (DbContext.Users.Any(x => x.Idnp == user.Idnp))
                {
                    ArgumentException argE = new ArgumentException(message: "An user with this idnp is already registered");
                    FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                    throw f;
                }
                DbContext.Users.Add(user);
                DbContext.SaveChanges();
            }
            else
            {
                ArgumentException argE = new ArgumentException(message: "User can't be null");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }
        }

        public async Task UpdateUser(User user)
        {
            DbContext.Users.Attach(user);

            var d = DbContext.Entry(user);
            d.Property(x => x.BirthDate).IsModified = true;
            d.Property(x => x.Name).IsModified = true;
            d.Property(x => x.UserPic).IsModified = true;
            d.Property(x => x.Surname).IsModified = true;
            d.Property(x => x.Idnp).IsModified = true;
            d.Property(x => x.PhoneNumber).IsModified = true;
            d.Property(x => x.Email).IsModified = true;

            if (DbContext.Users.Any(x => x.UserId != user.UserId && x.Idnp == user.Idnp))
            {
                ArgumentException argE = new ArgumentException("An user with this idnp is already registered");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }

            await DbContext.SaveChangesAsync();

        }

        public void DeleteUser(User user)
        {
            if (user!=null)
            {
                DbContext.Users.Remove(DbContext.Users.Single(x => x.UserId == user.UserId));
                DbContext.SaveChanges();

            }
        }
        #endregion


        #region OnHands Region

        public void AddReg(OnHand registration)
        {
            if (!DbContext.Users.Where(x=>x.UserId == registration.UserId).Any())
            {
                ArgumentException argE = new ArgumentException(message: "An user with this Id does not exist");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }

            if (!DbContext.BookCodes.Where(x=>x.Code == registration.Code).Any())
            {
                ArgumentException argE = new ArgumentException(message: "A book with this Code does not exist");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;

            }

            if (DbContext.OnHandsBooks.Where(x => x.Code == registration.Code).Any())
            {
                ArgumentException argE = new ArgumentException(message: "A book with this Code is already on hands");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;

            }

            if (registration.ReturnDate < DateTime.Now)
            {
                ArgumentException argE = new ArgumentException(message: "Return Date must be in the future");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }

            if (registration.LendDate > DateTime.Now)
            {
                ArgumentException argE = new ArgumentException(message: "Lend date must be current date or in the past");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }

           
            DbContext.OnHandsBooks.Add(registration);
            DbContext.SaveChanges();
        }

        public List<OnHand> GetOnHands(User user)
        {
            return DbContext.OnHandsBooks.Where(x => x.UserId == user.UserId).Include(x=>x.BookCode.Book).ToList();
        }

        public List<OnHand> GetBookCodesOnHand(Book book)
        {
            int id = book.BookId;

            List<OnHand> _list = new List<OnHand>();

            foreach (var item in DbContext.BookCodes.Where(x=>x.BookId == book.BookId))
            {
                if (DbContext.OnHandsBooks.Where(x => x.Code == item.Code).Any())
                {
                    _list.Add(DbContext.OnHandsBooks.Where(x => x.Code == item.Code).Single());
                }
            }

            return _list;
        }

        public List<BookCode> GetBookCodesNotOnHand(Book book)
        {
            int id = book.BookId;

            List<BookCode> _list = new List<BookCode>();

            foreach (var item in DbContext.BookCodes.Where(x => x.BookId == book.BookId))
            {
                if (!DbContext.OnHandsBooks.Where(x => x.Code == item.Code).Any() && !DbContext.ReservedBooks.Any(x=>x.Code == item.Code))
                {
                    _list.Add(DbContext.BookCodes.Where(x => x.Code == item.Code).Single());
                }
            }

            return _list;
        }

        public List<OnHand> GetAllOnHands()
        {
            return DbContext.OnHandsBooks.OrderByDescending(x => x.OnHandId).ToList();
        }

        public void DeleteOnHand(OnHand onHand)
        {
            if (!DbContext.OnHandsBooks.Where(x=>x.OnHandId == onHand.OnHandId).Any())
            {
                ArgumentException argE = new ArgumentException(message: "There isn't a record with this id");
                FaultException<ArgumentException> f = new FaultException<ArgumentException>(argE, argE.Message);
                throw f;
            }
            else
            {
                OnHand toBeRemoved = DbContext.OnHandsBooks.Where(x => x.OnHandId == onHand.OnHandId).Single();
                BookCodeOwned bookCodeOwned = new BookCodeOwned() { Code = toBeRemoved.Code, UserId = toBeRemoved.UserId, LendDate = toBeRemoved.LendDate, ReturnDate=DateTime.Now };
                DbContext.BookCodesOwned.Add(bookCodeOwned);
                DbContext.OnHandsBooks.Remove(toBeRemoved);
                DbContext.SaveChanges();
            }
        }

        #endregion

        #region BookCodesOwned
        public List<BookCodeOwned> GetUserOwnedBooks(int userId)
        {
            return DbContext.BookCodesOwned.Where(bc => bc.UserId == userId).Include(bc => bc.User).Include(x => x.BookCode.Book).ToList();
        }
        #endregion

        #region ReservedBooks
        public void AddReservationPrimitive(int bookCode, int userId, TimeSpan expiresIn)
        {
            if (DbContext.OnHandsBooks.Any(x=>x.Code==bookCode &&x.UserId==userId))
                throw new ArgumentException("This book is already onHands. You cant reserve a book that is already on hands.");

            DbContext.ReservedBooks.Add(new ReservedBook
            {
                Code = bookCode,
                UserId = userId,
                TimeOut = DateTime.Now + expiresIn
                
            });

            DbContext.SaveChanges();
        }

        public void AddReservation(BookCode bookCode, User user, TimeSpan expiresIn)
        {
            AddReservationPrimitive(bookCode.Code, user.UserId, expiresIn);
        }

        public void DeleteBookReservation(int reservationId)
        {
            var reservation = DbContext.ReservedBooks.Where(x => x.ReservationId == reservationId).FirstOrDefault();
            if (reservation != null)
            {
                DbContext.ReservedBooks.Remove(reservation); 
            }

            DbContext.SaveChanges();
        }

        public void DeleteExpiredReservations()
        {
            var expiredReservations = DbContext.ReservedBooks
                .Where(x => x.TimeOut < DateTime.Now);
            DbContext.ReservedBooks.RemoveRange(expiredReservations);

            DbContext.SaveChanges();
        }

        public IEnumerable<ReservedBook> GetReservedBooks(int userId) {
            return DbContext.ReservedBooks.Where(x => x.UserId == userId).Include(x => x.BookCode.Book).Include(x=>x.User);
        }

        public IEnumerable<ReservedBook> GetReservations()
        {
            return DbContext.ReservedBooks.Include(x => x.BookCode.Book);
        }

        #endregion

        #region Satelites

        public void StartReservationSatelite()
        {
            Satellites.ReservationsSatellite.Start();
        }

        public void StopReservationSatelite()
        {
            Satellites.ReservationsSatellite.Stop();
        }

        public bool GetReservationSateliteState()
        {
            return Satellites.ReservationsSatellite.IsRunning;
        }
        #endregion

        #region BooksReport
        public BooksReport GenerateBookReport()
        {
            BooksReport report = new BooksReport();

            report.TotalAmount = DbContext.Books.Sum(x => x.Amount);
            report.TitlesCount = DbContext.Books.Count();

            var q = DbContext.Books
                .GroupBy(x => x.Category)
                .Select(x => new
                {
                    category = x.FirstOrDefault().Category,
                    categoryTitlesCount = x.Count(),
                    categoryAmount = x.Sum(s => s.Amount)

                });

            foreach (var item in q)
            {
                report.CategoryInfo.Add(
                    item.category,
                    new BooksReportData()
                    {
                        Category = item.category,
                        CategoryAmount = item.categoryAmount,
                        CategoryTitlesCount = item.categoryTitlesCount
                    }
                    );
            }

            return report;

        }

        #endregion
        
    }
}
