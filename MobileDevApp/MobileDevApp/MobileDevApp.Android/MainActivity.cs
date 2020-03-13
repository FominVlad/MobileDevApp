using Android.App;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using Firebase.Auth;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Support.V7.App;
using Firebase;
using Android.Gms.Tasks;
using Android.Content.PM;
using Android.Views;
using Xamarin.Forms;

namespace MobileDevApp.Droid
{
    [Activity(Label = "MobileDevApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnSuccessListener, IOnFailureListener
    {
        internal static MainActivity Instance { get; private set; }

        GoogleSignInOptions gso;
        GoogleApiClient googleApiClient;

        FirebaseAuth firebaseAuth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("710622055212-va4d4gqb6f5hoj9ieca69ql8hak7c0cl.apps.googleusercontent.com")
                .RequestEmail()
                .Build();

            googleApiClient = new GoogleApiClient.Builder(this).AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();

            googleApiClient.Connect();

            firebaseAuth = GetFirebaseAuth();

            //SigninButton_Click();

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            this.RequestedOrientation = ScreenOrientation.Portrait;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        protected override void OnResume()
        {
            Instance = this;
            base.OnResume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseAuth mAuth;

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("mobiledevapp")
                    .SetApplicationId("mobiledevapp")
                    .SetApiKey("AIzaSyC-lKpysUATFcWgNputYzaZb__k9g-L_eU")
                    .SetDatabaseUrl("https://mobiledevapp.firebaseio.com")
                    .SetStorageBucket("mobiledevapp.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }
            return mAuth;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            LoadApplication(new App());
            //displayNameText.Text = "Display Name: " + firebaseAuth.CurrentUser.DisplayName;
            //emailText.Text = "Email: " + firebaseAuth.CurrentUser.Email;
            //photourlText.Text = "Photo URL: " + firebaseAuth.CurrentUser.PhotoUrl.Path;

            Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            Toast.MakeText(this, "Login Failed", ToastLength.Short).Show();
        }

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this)
                .AddOnFailureListener(this);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithFirebase(account);
                }
            }
        }

        public void SigninButton_Click()
        {
            if (firebaseAuth.CurrentUser == null)
            {
                var intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
                StartActivityForResult(intent, 1);
            }
            else
            {
                firebaseAuth.SignOut();
            }
        }
    }
}