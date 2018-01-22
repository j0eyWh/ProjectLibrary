using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProjectLibraryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISubServiceLayer" in both code and config file together.
    [ServiceContract]
    public interface ISubServiceLayer
    {
        [OperationContract]  
        User GetUser(string idnp);

        [OperationContract]
        bool CanLogIn(string name, string idnp);

        [OperationContract]
        byte[] GetUserImage(string idnp);

        [OperationContract]
        int GetUserBooksOnHandsCount(User user);
        // TODO: Add your service operations here

        [OperationContract]
        IEnumerable<OnHand> GetUserBook(int userId);

        [OperationContract]
        IEnumerable<ReservedBook> GetUserReservedBooks(int userId);

        [OperationContract]
        IEnumerable<Book> GetBooksCategories();

        [OperationContract]
        IEnumerable<Book> GetBooksByCategory(string category);

        [OperationContract]
        int GetAvailableCount(int bookId);

        [OperationContract]
        Book GetBookById(int bookId);

        [OperationContract]
        bool HasReserved(int userId, int bookId);

        [OperationContract]
        bool HasOnHands(int userId, int bookId);

        [OperationContract]
        void AddReservation(int userId, int bookId);

        [OperationContract]
        IEnumerable<Book> SearchBooks(string searchString);

        [OperationContract]
        IEnumerable<BookCodeOwned> GetUserPreviouslyOwnedBooks(int userId);

        [OperationContract]
        IEnumerable<Book> GetNewestBooks(int count);

        [OperationContract]
        IEnumerable<Book> GetRecommendedBooks(int userId);
    }
}
