using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryService.Satellites
{
    internal class ReservationsSatellite
    {
        public static bool IsRunning
        {
            get;
            set;
        } 

        public static void Start()
        {
            IsRunning = true;
            ProcessReservations();
        }

        private static async void ProcessReservations()
        {
            while (IsRunning)
            {
                var service = new LibraryService();
                service.DeleteExpiredReservations();

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        public static void Stop()
        {
            IsRunning = false;
        }


    }
}
