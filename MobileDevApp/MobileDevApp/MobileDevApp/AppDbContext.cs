﻿using Microsoft.EntityFrameworkCore;
using MobileDevApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MobileDevApp
{
    public class AppDbContext : DbContext
    {
        public string dbName { get; private set; }
        private string dbPath { get; set; }

        public AppDbContext(string dbPath = null, string dbName = "testDb24674132332.db")
        {
            this.dbPath = dbPath ?? 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);

            Database.EnsureCreated();
            
            InitializeData();
        }

        private void InitializeData()
        {
            ColourScheme colourSchemeLight = new ColourScheme()
            {
                Id = 1,
                SchemeType = "Light",
                HeaderColour = "#6891ff",
                PageColour = "#ffffff",
                TextColour = "#000000",
                ButtonColour = "#e1e1e1"
            };

            ColourScheme colourSchemeDark = new ColourScheme()
            {
                Id = 2,
                SchemeType = "Dark",
                HeaderColour = "#191d58",
                PageColour = "#54555c",
                TextColour = "#dbdbdb",
                ButtonColour = "#888bae"
            };

            if (!ColourSchemes.Any())
            {
                ColourSchemes.AddRange(colourSchemeLight, colourSchemeDark);
            }

            Models.Settings settings = new Models.Settings()
            {
                Id = 1,
                ColourScheme = colourSchemeDark
            };

            if (!Settings.Any())
            {
                Settings.AddRange(settings);
            }

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        public DbSet<Models.Settings> Settings { get; set; }
        public DbSet<ColourScheme> ColourSchemes { get; set; }
    }
}
