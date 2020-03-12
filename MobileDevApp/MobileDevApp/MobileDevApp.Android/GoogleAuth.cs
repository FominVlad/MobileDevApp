using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(MobileDevApp.Droid.GoogleAuth))]
namespace MobileDevApp.Droid
{
    class GoogleAuth : IGoogleAuth
    {
        public void GetGoogle()
        {
            MainActivity.Instance.SigninButton_Click();
        }
    }
}