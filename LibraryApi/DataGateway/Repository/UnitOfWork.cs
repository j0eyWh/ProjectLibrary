using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGateway.EntityModels;
using DataGateway.RepositoryInfrastructure;

namespace DataGateway.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private LibraryModel context = new LibraryModel();

        private Repository<User> _usersRepository;
        public Repository<User> Users
        {
            get
            {
                if (_usersRepository == null)
                {
                    this._usersRepository = new Repository<User>(context);
                }
                return _usersRepository;
            }

        }

        private Repository<Book> _booksRepository;
        public Repository<Book> Books
        {
            get
            {
                if (_booksRepository == null)
                {
                    this._booksRepository = new Repository<Book>(context);
                }
                return _booksRepository;
            }

        }

        private Repository<BookCodesOwned> _ownedRepository;

        public Repository<BookCodesOwned> OwnedBooks
        {
            get
            {
                if (_ownedRepository == null)
                {
                    this._ownedRepository = new Repository<BookCodesOwned>(context);
                }
                return _ownedRepository;
            }
        }

        private Repository<ReservedBook> _reservedBooks;

        public Repository<ReservedBook> ReservedBooks
        {
            get
            {
                if (_reservedBooks == null)
                {
                    this._reservedBooks = new Repository<ReservedBook>(context);
                }
                return _reservedBooks;
            }

        }

        private Repository<OnHand> _onHandsBooks;

        public Repository<OnHand> OnHandsBooks
        {
            get
            {
                if (_onHandsBooks == null)
                {
                    this._onHandsBooks = new Repository<OnHand>(context);
                }
                return _onHandsBooks;
            }
        }


        private Repository<BookCode> _bookCodes;

        public Repository<BookCode> BookCodes
        {
            get
            {
                if (_bookCodes == null)
                {
                    this._bookCodes = new Repository<BookCode>(context);
                }
                return _bookCodes;
            }
        }

        private Repository<UserCategory> _userCategories;
        public Repository<UserCategory> UserCategories
        {
            get
            {
                if (_userCategories == null)
                {
                    _userCategories = new Repository<UserCategory>(context);
                }
                return _userCategories;
            }
        }

        private Repository<ReadingPosition> _readingPositions;

        public Repository<ReadingPosition> ReadingPostions
        {
            get
            {
                if (_readingPositions==null)
                {
                    _readingPositions = new Repository<ReadingPosition>(context);
                }
                return _readingPositions;
            }
        }

        public void Save()
        {
            this.context.SaveChanges();
        }


        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
