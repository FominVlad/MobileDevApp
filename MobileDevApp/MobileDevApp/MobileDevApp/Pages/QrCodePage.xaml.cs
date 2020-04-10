﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodePage : ContentPage
    {
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        public QrCodePage(string userId)
        {
            InitializeComponent();

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            GenerateQr(userId);

            SetColourScheme();
        }

        private void GenerateQr(string userId)
        {
            ZXingBarcodeImageView barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = ScreenWidth;
            barcode.BarcodeOptions.Height = ScreenWidth;
            barcode.BarcodeValue = userId;
            barcode.BackgroundColor = Color.Transparent;

            mainStackLayout.Children.Insert(1, barcode);
        }

        private void SetColourScheme()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["headerColor"];
            ((NavigationPage)Application.Current.MainPage).BarTextColor = (Color)Application.Current.Resources["textColor"];
        }
    }
}