using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidGitHubCoach.Model;
using TinyIoC;
using System.Linq;
using System.Threading.Tasks;
using BarChart;
using System.Collections.Generic;
using AndroidGitHubCoach.Model.Services;

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

            this.SettingsProvider = TinyIoCContainer.Current.Resolve<ISettingsProvider>();

            this.EventsProvider = TinyIoCContainer.Current.Resolve<IEventsProvider>();
        }

        protected void ScheduleNotificationService()
        {
            if (!IsAlarmSet())
            {
                ServiceScheduler.Schedule(this.ApplicationContext);
            }
        }

        bool IsAlarmSet()
        {
            return PendingIntent.GetBroadcast(this, 0, this.notificationServiceIntent, PendingIntentFlags.NoCreate) != null;
        }

        Intent notificationServiceIntent = null;

        ISettingsProvider SettingsProvider = null;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            notificationServiceIntent = new Intent("com.YLiohenki.GitHubCoach.NotificationIntent");

            var settings = this.SettingsProvider.GetSettings();

            if (settings == null || string.IsNullOrWhiteSpace(settings.UserName))
            {
                StartActivity(typeof(LoginActivity));
                return;
            }
            else
            {
                Button btn = FindViewById<Button>(Resource.Id.username);
                btn.Text = settings.UserName;
                btn.Click += delegate
                {
                    StartActivity(typeof(SettingsActivity));
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
            this.EventsProvider.Refresh(this.ApplicationContext);
            var events = this.EventsProvider.GetEvents(this.ApplicationContext);
            DateTime startOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
            var numbers = Enumerable.Range(0, 7);
            var lastCommitView = FindViewById<TextView>(Resource.Id.lastCommitText);
            events.Sort((a, b) => (a.Time > b.Time ? -1 : (a.Time < b.Time ? 1 : 0)));
            var groups = numbers.Select(x =>
            {
                var dayEvents = events.Where(e => e.Time.Date == startOfWeek.AddDays(x));
                return new Tuple<int, DateTime>(dayEvents.Count(), startOfWeek.AddDays(x));
            });
            LinearLayout bottomLayout = FindViewById<LinearLayout>(Resource.Id.bottomLayout);
            if (events.Count > 0)
            {
                RunOnUiThread(() =>
                {
                    lastCommitView.Text = events.First().Time.ToString();
                    bottomLayout.RemoveAllViews();
                    this.ShowBarchat(groups);
                });
            }
        }

        protected void ShowBarchat(IEnumerable<Tuple<int, DateTime>> groups)
        {
            LinearLayout bottomLayout = FindViewById<LinearLayout>(Resource.Id.bottomLayout);

            var chart = new BarChartView(this)
            {
                ItemsSource = groups.Select(g => new BarModel { Value = g.Item1, Legend = g.Item2.DayOfWeek.ToString().Substring(0,2) })
            };

            bottomLayout.AddView(chart, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));
        }
    }
}

