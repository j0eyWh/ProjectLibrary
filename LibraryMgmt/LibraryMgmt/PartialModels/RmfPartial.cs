using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GHM;
using static LibraryMgmt.Common.ServiceClient;

namespace LibraryMgmt.ServiceReference
{
    public partial class Rmf
    {


        //Constructor for a brand new Rmf object
        public Rmf(bool isOut, string idRmf, DateTime? dateIn, string imgPath, string docId, int quantity, decimal totalValue, int firstInvNumber, string contentCat, string origin, DateTime? dateOut, string outCause)
        {

            #region Checkeing and intializing the idRmf arg;

            if (!String.IsNullOrWhiteSpace(idRmf) &&
                idRmf != "" &&
                Hm.IsValidIdRmf(idRmf)
                )
            {
                if (Client.GetRmfById(idRmf) == null)
                {
                    this.IdRmf = idRmf;
                }
                else
                {
                    throw new ArgumentException("An Rmf object with this Id already exists");
                }
            }

            else
            {
                this.IdRmf = Hm.GenerateNewId(Client.GetLastIndex());
            }

            #endregion

            #region Checking and intitializing the dateIn arg;
            if (dateIn != null)
            {
                this.DateIn = (DateTime)dateIn;
            }
            else
            {
                this.DateIn = DateTime.Now;
            }

            #endregion

            #region Checking and initializing the docId arg
            if (
                !String.IsNullOrWhiteSpace(docId) &&
                Client.GetRmfByDocId(docId) == null
                )
            {
                this.DocId = docId;
            }
            else
            {
                throw new ArgumentException(message: "The docId argument is not valid or already exists");
            }
            #endregion

            #region Checking and initializing the DocImg property
            if (File.Exists(imgPath))
            {
                this.DocImg = Hm.ImageToByteArray(imgPath);
            }
            else
            {
                this.DocImg = null;
            }
            #endregion

            #region Checking and initalizing Quantity property
            if (quantity > 0)
            {
                this.Quantity = quantity;
            }
            else
            {
                throw new ArgumentException("The quantity argument must be greater than 0");
            }
            #endregion

            #region Checking and initializing TotalValue prop
            if (totalValue > 0)
                this.TotalValue = totalValue;
            else
                throw new ArgumentException("The Total Value argument must be greater than 0");
            #endregion

            #region Checking and intializing: firstInvNumber, LastInvNumber
            int x = Client.GetLastLastInvNr();

            if (firstInvNumber > x)
            {
                this.FirstInvNr = firstInvNumber;
                this.LastInvNr = firstInvNumber + (Quantity - 1);
            }
            else
            {
                throw new ArgumentException(message: "Invalid firstInvNumeber. Probably there is already such a Inventory number");
            }

            #endregion

            #region Checking and intializing ContentCat
            if (!String.IsNullOrWhiteSpace(contentCat))
            {
                this.ContentCat = contentCat;
            }
            else
            {
                throw new ArgumentException(message: "Invalid Content category, probably it's empty");
            }
            #endregion

            #region Dealing with Out atributes
            if (isOut)
            {
                this.IsOut = isOut;
                if (dateOut != null)
                {
                    this.DateOut = dateOut;
                }
                else
                {
                    throw new ArgumentException("You must provide a DateOut if isOut argument is true");
                }
                if (!String.IsNullOrWhiteSpace(outCause))
                {
                    this.OutCause = outCause;
                }
                else
                {
                    throw new ArgumentException("Invalid outCause argument. Probably it's empty");
                }

            }
            else
            {
                this.IsOut = isOut;
                this.DateOut = null;
                this.OutCause = null;
            }
            #endregion

            #region Checking and initializing Origin
            if (!String.IsNullOrWhiteSpace(origin))
            {
                this.Origin = origin;

            }
            else
            {
                throw new ArgumentException("You cant have a null Origin");
            }
            #endregion

        }

        public Rmf()
        {
            throw new ArgumentException(message: "you cant initialize it like that");
        }

        public override string ToString()
        {
            return $"{this.IdRmf} \t{this.ContentCat} \t {this.DateIn} {this.DateOut} \t {this.DocId} \t {this.FirstInvNr} \t {this.LastInvNr} \t {this.Origin} \t {this.OutCause} \t {this.Quantity} \t {this.TotalValue}";
        }
    }
}

