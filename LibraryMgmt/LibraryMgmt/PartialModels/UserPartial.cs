using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GHM;

namespace LibraryMgmt.ServiceReference
{
    public partial class User
    {
        public User(string name, string surname, string imgPath, DateTime birthDate, string idnp, string phone, string email)
        {
            Regex phoneRegex = new Regex("^[0-9]{9,12}$");
            Regex emailRegex = new Regex(@"^\w + ([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(message: "Name can't be empty");

            if (String.IsNullOrWhiteSpace(surname))
                throw new ArgumentException(message: "Surname can't be empty");

            if (imgPath == null)
                throw new ArgumentNullException("Image path cant be null");

            if (!Hm.IsNumber(idnp))
                throw new ArgumentException("Idnp must be formed only from digits");

            if (idnp.Count() != 13)
                throw new ArgumentException("Idnp must be 13 digits long");

            if (String.IsNullOrEmpty(phone))
            {
                throw new ArgumentException("You must provide a phone number");
            }

            if (!phoneRegex.IsMatch(phone))
            {
                throw new ArgumentException("You must provide a valid phone number");
            }

            if (phoneRegex.IsMatch(email))
            {
                throw new ArgumentException("You must provide a valid email");

            }

            



            this.Name = name;
            this.Surname = surname;
            this.Idnp = idnp;
            this.UserPic = Hm.ImageToByteArray(imgPath);
            this.BirthDate = birthDate;
            this.Idnp = idnp;
            this.Email = email;
            this.PhoneNumber = phone;

        }


    }
}
