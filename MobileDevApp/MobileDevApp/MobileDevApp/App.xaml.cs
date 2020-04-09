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

        public static UserInfo UserInfo
        {
            get
            {
                return Database.userInfo.FirstOrDefault();
            }
        }

        public App()
        {
            InitializeComponent();
            FillResourcesFromDb();

            //MainPage = new NavigationPage(new MainPage());
            if (!Database.userInfo.Any())
            {
                MainPage = new NavigationPage(new SignInPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        private void FillResourcesFromDb()
        {
            Resources.Add("textColor", Color.FromHex(ColourScheme.TextColour));
            Resources.Add("headerColor", Color.FromHex(ColourScheme.HeaderColour));
            Resources.Add("buttonColor", Color.FromHex(ColourScheme.ButtonColour));
            Resources.Add("pageColor", Color.FromHex(ColourScheme.PageColour));
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
