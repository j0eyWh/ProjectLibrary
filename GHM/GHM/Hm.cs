/*#############################################
#  GHM stands for General Helper Methods      #
# Static methods that will be used throughout #
# the entire project, both on admin side      #
# as well as on client side                   #
#  @j0ey_wh                                   #
##############################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GHM
{
    public class Hm
    {
        /// <summary>
        /// Use this to transform a image to a byte array.
        /// </summary>
        /// <param name="filePath">Path to the image</param>
        /// <returns>A byte[] that you can store in a varbinary filed in a SQLDB</returns>
        public static byte[] ImageToByteArray(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileStream stream = new FileStream(
                    filePath, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);

                byte[] photo = reader.ReadBytes((int)stream.Length);

                reader.Close();
                stream.Close();

                return photo;
            }
            else return null;

        }

        public static string GenerateNewId(string lastId)
        {
            string newId;

            int id;
            int Year;

            string[] s = lastId.Split('_');

            if (!(int.TryParse(s[0], out id)))
            {
                 throw new ArgumentException(message: $"First part of lastId ({s[0]}) is not a valid integer value");
            }

            id++;
            Year = DateTime.Now.Year;

            newId = $"{id}_{Year}";
            return newId;
        }

        public static int GenerateLastInvNr(int firstInvNr, int quantity)
        {
            return firstInvNr + quantity - 1;
        }

        /// <summary>
        /// Check if a string would do as an valid IdRmf
        /// </summary>
        /// <param name="idRmf"></param>
        /// <returns></returns>
        static public bool IsValidIdRmf(string idRmf)
        {
            bool isValid;

            string[] s = idRmf.Split('_');

            int id, year;

            if (s.Count()==2)
            {
                if (Int32.TryParse(s[0], out id) && Int32.TryParse(s[1], out year))
                {
                    if (year > 1800)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                } 
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        
        static public bool IsNumber(string someString)
        {
            bool ret = true;
            long outRes;
            foreach (var item in someString)
            {
                if (!Int64.TryParse(someString, out outRes))
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }
    }
}
