using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App5.Entities;
using App5.repo;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App5
{
    public partial class App : Application
    {
        public static string AccountIdDefault = "e8b3935f-e51e-4a71-8d9c-fc52bf121dd2";
        public static string ACCOUNT_ID_KEY = "ACCOUNT_ID";
        public static string NOTE_ID_KEY = "NOTE_ID";

        public static string AccountId;
        
        public static Service Service;
        
        public App()
        {
            AccountId = "e8b3935f-e51e-4a71-8d9c-fc52bf121dd2";
            Service = new Service(new Repository());
            Service.StartAccountPullingSync();
            
            InitializeComponent();
        }

        protected override void OnStart()
        {
            var noteId = Preferences.Get(NOTE_ID_KEY, null);
            MainPage = new NavigationPage(new NoteListPage(noteId));;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}