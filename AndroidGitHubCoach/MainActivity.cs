using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidGitHubCoach.Model;
using TinyIoC;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidGitHubCoach
{
    [Activity(Label = "GitHub Coach", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        IEventsProvider EventsProvider;
        public bool bootstrapperRun = false;
        public MainActivity()
        {
            if (!bootstrapperRun)
            {
                Bootstrapper.Run();
                bootstrapperRun = true;
            }

            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();

            this.EventsProvider = TinyIoCContainer.Current.Resolve<IEventsProvider>();
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
                return;
            }
            else
            {
                TextView textView = FindViewById<TextView>(Resource.Id.username);
                textView.Text = userName;
            }
            Task.Run(() =>
            {
                var events = this.EventsProvider.GetEvents(userName);
                var lastCommitView = FindViewById<TextView>(Resource.Id.lastCommitText);
                events.Sort((a, b) => (a.Time > b.Time ? -1 : (a.Time < b.Time ? 1 : 0)));
                RunOnUiThread(() =>
                {
                    lastCommitView.Text = events.First().Time.ToString();
                });
            });
        }
    }
}

