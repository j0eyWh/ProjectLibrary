using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;

namespace LibraryEverywhere.Common
{
    public static class StreamHelpers
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
