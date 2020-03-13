using Xamarin.Forms;
using System.Linq;
using MobileDevApp.Models;
using System;
using System.IO;

namespace MobileDevApp
{
    public partial class App : Application
    {
        private static AppDbContext database;
        public static AppDbContext Database
        {
            get
            {
                if(database == null)
                {
                    database = new AppDbContext();
                }

                return database;
            }
        }

        public static ColourSchemeDTO ColourScheme
        {
            get
            {
                return Database.Settings.Join(Database.ColourSchemes,
                    s => s.ColourSchemeId,
                    c => c.Id,
                    (s, c) => new ColourSchemeDTO
                    {
                        SchemeType = c.SchemeType,
                        HeaderColour = c.HeaderColour,
                        PageColour = c.PageColour,
                        TextColour = c.TextColour,
                        ButtonColour = c.ButtonColour
                    }).FirstOrDefault();
            }
        }

        public App()
        {
            InitializeComponent();
            //MainPage = new NavigationPage(new MainPage());
            MainPage = new NavigationPage(new SignInPage());
        }

        public void CreateNewPage()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
