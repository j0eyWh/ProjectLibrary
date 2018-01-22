using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryMgmt.Common;
using LibraryMgmt.ServiceReference;

namespace LibraryMgmt.ViewModels
{
    public class ServicesViewModel : ObservableBase
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private bool _reservationSatelliteState;
        public bool ReservationSatelliteState
        {
            get { return _reservationSatelliteState; }
            set {
                _reservationSatelliteState = value;
                NotifyPropertyChanged(nameof(ReservationSatelliteState));
                NotifyPropertyChanged(nameof(ReservationSatelliteStateString));
            }
        }

        private string _reservationSatelliteStateString;

        public string ReservationSatelliteStateString
        {
            get {
                if (ReservationSatelliteState)
                {
                    return "Stop";
                }
                return "Start";
            }
            set { _reservationSatelliteStateString = value; NotifyPropertyChanged(nameof(ReservationSatelliteStateString));}
        }

        public async Task InitializeAsync()
        {
            this.ReservationSatelliteState = await Client.GetReservationSateliteStateAsync();
        }

        public async Task StartReservationSatellite()
        {
            await Client.StartReservationSateliteAsync();
            await InitializeAsync();
        }

        public async Task StopReservationSatellite()
        {
            await Client.StopReservationSateliteAsync();
            await InitializeAsync();
        }



    }
}
