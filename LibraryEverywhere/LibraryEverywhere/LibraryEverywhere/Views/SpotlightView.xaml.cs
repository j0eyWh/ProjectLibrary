using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
	public partial class SpotlightView : ContentPage
	{
		public SpotlightView ()
		{
			InitializeComponent ();
		}

	    protected override async void OnAppearing()
	    {
	        
	        base.OnAppearing();
           await SpotlightViewModel.RefreshViewModel();
	       // SpotlightViewModel.NewestPosition = 0;
	    }
	}
}
