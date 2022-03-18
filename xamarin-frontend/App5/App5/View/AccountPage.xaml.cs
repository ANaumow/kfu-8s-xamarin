using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace App5
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            
            Label.Text = "Ваш айди: " + App.AccountId;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NoteListPage(Preferences.Get(App.NOTE_ID_KEY, null)));
        }
        
    }
}