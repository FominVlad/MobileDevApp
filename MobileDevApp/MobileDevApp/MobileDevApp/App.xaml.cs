using Xamarin.Forms;
using System.Linq;
using MobileDevApp.Models;
using System;
using System.IO;
using MobileDevApp.RemoteProviders.Implementations;
using System.Net.Http;
using MobileDevApp.RemoteProviders.Models;

namespace MobileDevApp
{
    public partial class App : Application
    {
        private static AppDbContext database;
        private static UserService userService;
        private static ChatInfoProvider chatService;
        
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

        public static UserService UserService
        {
            get
            {
                if (userService == null)
                {
                    HttpClient httpClient = new HttpClient();
                    HttpProvider httpProvider = new HttpProvider(httpClient);
                    userService = new UserService(httpProvider);
                }

                return userService;
            }
        }

        public static ChatInfoProvider ChatService
        {
            get
            {
                if (chatService == null)
                {
                    HttpClient httpClient = new HttpClient();
                    HttpProvider httpProvider = new HttpProvider(httpClient);
                    chatService = new ChatInfoProvider(httpProvider);
                }

                return chatService;
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
            Resources.Add("headerColor", Color.FromHex(ColourScheme.HeaderColour));
            Resources.Add("textColor", Color.FromHex(ColourScheme.TextColour));
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
            FillResourcesFromDb();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
