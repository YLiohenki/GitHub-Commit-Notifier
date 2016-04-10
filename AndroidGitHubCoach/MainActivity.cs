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

        protected void ScheduleNotificationService()
        {
            if (!IsAlarmSet())
            {
                var alarm = (AlarmManager)this.GetSystemService(Context.AlarmService);

                var pendingServiceIntent = PendingIntent.GetService(this.ApplicationContext, 0, this.notificationServiceIntent, PendingIntentFlags.CancelCurrent);
                alarm.SetRepeating(AlarmType.RtcWakeup, 0, 60000, pendingServiceIntent);
            }
        }

        bool IsAlarmSet()
        {
            return PendingIntent.GetBroadcast(this, 0, this.notificationServiceIntent, PendingIntentFlags.NoCreate) != null;
        }

        Intent notificationServiceIntent = null;

        IUserProvider UserProvider = null;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            notificationServiceIntent = new Intent("com.YLiohenki.GitHubCoach");
            StartService(notificationServiceIntent);

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
                this.FillUIWIthEvents();
            });
        }

        protected override void OnStart()
        {
            base.OnStart();

            this.ScheduleNotificationService();
        }

        public void FillUIWIthEvents()
        {
            var events = this.EventsProvider.GetEvents();
            var lastCommitView = FindViewById<TextView>(Resource.Id.lastCommitText);
            events.Sort((a, b) => (a.Time > b.Time ? -1 : (a.Time < b.Time ? 1 : 0)));
            var groups = events.GroupBy(x => x.Time.Date).Select(x => new Tuple<int, DateTime>(x.Count(), x.First().Time.Date));
            LinearLayout bottomLayout = FindViewById<LinearLayout>(Resource.Id.bottomLayout);
            if (events.Count > 0)
            {
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
            }
        }
    }
}

