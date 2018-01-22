using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEverywhere.Common;
using Xamarin.Forms;

namespace LibraryEverywhere.Views
{
    public partial class MainPage : MasterDetailPage
    {
       
        public MainPage()
        {
            InitializeComponent();

            masterPage.ListView.ItemSelected += OnItemSelected;
            masterPage.SecondaryListView.ItemSelected += OnItemSelected;
            masterPage.ProfileImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(x =>
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(UserInfoView)));
                    masterPage.ListView.SelectedItem = null;

                    IsPresented = false;
                })
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item!=null)
            {
                //var p = new NavigationPage((Page) Activator.CreateInstance(item.TargetType));
                //await Detail.Navigation.PushAsync(p, true);

                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                ((ListView) sender).SelectedItem = null;

               // masterPage.ListView.SelectedItem = null;
                
                IsPresented = false;
            }
        }

    }
}
