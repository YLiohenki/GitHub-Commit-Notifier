using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidGitHubCoach.Model;
using TinyIoC;

namespace AndroidGitHubCoach
{
    [Activity(Label = "GitHub Coach", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public bool bootstrapperRun = false;
        public MainActivity()
        {
            if (!bootstrapperRun)
            {
                Bootstrapper.Run();
                bootstrapperRun = true;
            }

            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
        }

        int count = 1;
        IUserProvider UserProvider = null;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var userName = this.UserProvider.GetUserName();

            if (string.IsNullOrWhiteSpace(userName))
            {
                StartActivity(typeof(LoginActivity));
            }
            else
            {
                TextView textView = FindViewById<TextView>(Resource.Id.username);
                textView.Text = userName;
            }

        }
    }
}

