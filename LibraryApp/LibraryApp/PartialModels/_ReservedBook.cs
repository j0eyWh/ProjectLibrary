using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LibraryApp.SubServiceLayer
{
    public partial class ReservedBook
    {
        //public TYPE Type { get; set; }
        public Windows.UI.Xaml.Media.Brush ColorBrush
        {
            get
            {
                var t = this.TimeOut - DateTime.Now;
                if (t <= TimeSpan.FromDays(1))
                {
                    return new SolidColorBrush(new Color() {A=200, B = 94, G = 94, R = 255});
                }
                else
                {
                    return null;
                }
            }
            set { }
        }

    }
}
