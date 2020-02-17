﻿using Xamarin.Forms;
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

        private static ColourSchemeDTO colourScheme;

        public static ColourSchemeDTO ColourScheme
        {
            get
            {
                if(colourScheme == null)
                {
                    colourScheme = Database.Settings.Join(Database.ColourSchemes,
                    s => s.ColourSchemeId,
                    c => c.Id,
                    (s, c) => new ColourSchemeDTO
                    {
                        HeaderColour = c.HeaderColour,
                        PageColour = c.PageColour,
                        TextColour = c.TextColour,
                        ButtonColour = c.ButtonColour
                    }).FirstOrDefault();
                }
                return colourScheme;
            }
        }

        public App()
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
