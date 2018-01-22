using System;
using Xamarin.Forms;

namespace LibraryEverywhere.Models
{
    public class ReservedBook
    {
        public int ReservationId { get; set; }

        public int UserId { get; set; }

        public int Code { get; set; }

        public DateTime TimeOut { get; set; }

        public virtual BookCode BookCode { get; set; }

        public virtual User User { get; set; }


        public Color Color
        {
            get
            {
                if (IsExpired())
                {
                    return Color.Maroon;
                }
                return Color.Gray;
            }
           
        }

        private bool IsExpired()
        {
            return this.TimeOut<DateTime.Now;
        }
    }
}
