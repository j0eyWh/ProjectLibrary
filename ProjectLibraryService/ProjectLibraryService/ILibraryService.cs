using ProjectLibraryService.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;


namespace ProjectLibraryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]

        List<Rmf> GetRmfList();

        [OperationContract]
        bool InsertRmf(Rmf newRmf);

        [OperationContract]
        bool DeleteRmf(string id);

        [OperationContract]
        bool UpdateRmf(Rmf rmfObj);

        [OperationContract]
        Rmf GetRmfById(string idRmf);

        [OperationContract]
        string GetLastIndex();

        [OperationContract]
        Rmf GetRmfByDocId(string docId);

        [OperationContract]
        int GetLastLastInvNr();

        [OperationContract]
        List<Rmf> GetInRmf();

        [OperationContract]
        List<Rmf> GetOutRmf();

        [OperationContract]
        AnualReport GenerateAnualReport();

        [OperationContract]
        Part3Report GeneratePart3Report();

        [OperationContract]
        void DeleteMultipleRmf(List<Rmf> l);

        [OperationContract]
        List<Book> GetAllBooks();

        [OperationContract]
        void AddBook(Book book);

        [OperationContract]
        void DeleteBookById(int bookId);

        [OperationContract]
        [FaultContract(typeof(ArgumentException))]
        List<Book> GetBooksByRmfId(string rmfId);

        [OperationContract]
        void UpdateBook(Book book);

        [OperationContract]
        List<OnHand> GetAllOnHands();

        [OperationContract]
        [FaultContract(typeof(ArgumentException))]
        void DeleteOnHand(OnHand onHand);


        #region BookCodes
        [OperationContract]
        List<BookCode> GetBookCodes(Book book);
        #endregion

        #region  Users

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        User GetUserById(int id);

        [OperationContract]
        [FaultContract(typeof(ArgumentException))]
        void AddUser(User user);

        [OperationContract]
        [FaultContract(typeof(ArgumentException))]
        Task UpdateUser(User user);

        [OperationContract]
        void DeleteUser(User user);
        #endregion

        #region OnHandsRegion
        [OperationContract]
        void AddReg(OnHand registration);

        [OperationContract]
        List<OnHand> GetOnHands(User user);

        [OperationContract]
        List<OnHand> GetBookCodesOnHand(Book book);

        [OperationContract]
        List<BookCode> GetBookCodesNotOnHand(Book book);

        #endregion

        #region BookCodesOwned
        [OperationContract]
        List<BookCodeOwned> GetUserOwnedBooks(int userId);
        #endregion

        #region ReservedBooks
        [OperationContract]
        void AddReservationPrimitive(int bookCode, int userId, TimeSpan expiresIn);

        [OperationContract]
        void AddReservation(BookCode bookCode, User user, TimeSpan expiresIn);

        [OperationContract]
        void DeleteBookReservation(int reservationId);

        [OperationContract]
        void DeleteExpiredReservations();

        [OperationContract]
        IEnumerable<ReservedBook> GetReservedBooks(int userId);

        [OperationContract]
        IEnumerable<ReservedBook> GetReservations();
        #endregion

        #region Sattelites
        [OperationContract]
        void StopReservationSatelite();

        [OperationContract]
        void StartReservationSatelite();

        [OperationContract]
        bool GetReservationSateliteState();
        #endregion
        

        #region Reports
        [OperationContract]
        BooksReport GenerateBookReport();
        #endregion

        
        
        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "ProjectLibraryService.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

}
