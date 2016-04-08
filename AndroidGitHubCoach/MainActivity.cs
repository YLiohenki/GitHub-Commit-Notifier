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
                Button btn = FindViewById<Button>(Resource.Id.username);
                btn.Text = userName;
                btn.Click += delegate
                {
                    StartActivity(typeof(LoginActivity));
                };
            }
            Task.Run(() =>
            {
                var events = this.EventsProvider.GetEvents(userName);
                var lastCommitView = FindViewById<TextView>(Resource.Id.lastCommitText);
                events.Sort((a, b) => (a.Time > b.Time ? -1 : (a.Time < b.Time ? 1 : 0)));
                var groups = events.GroupBy(x => x.Time.Date).Select(x => new Tuple<int, DateTime> (x.Count(), x.First().Time.Date));
                LinearLayout bottomLayout = FindViewById<LinearLayout>(Resource.Id.bottomLayout);
                RunOnUiThread(() =>
                {
                    lastCommitView.Text = events.First().Time.ToString();
                    bottomLayout.RemoveAllViews();
                    foreach (var gr in groups.OrderByDescending(x => x.Item2))
                    {
                        bottomLayout.AddView(new TextView(this.BaseContext)
                        {
                            Text = "Date: " + gr.Item2.ToShortDateString() + " Number of commits: " + gr.Item1,
                            TextSize = 20
                        });
                    }
                });
            });
        }
    }
}

