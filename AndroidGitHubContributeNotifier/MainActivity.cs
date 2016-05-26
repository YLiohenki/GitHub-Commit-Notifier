using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using AndroidGitHubContributeNotifier.Model;
using TinyIoC;
using System.Linq;
using System.Threading.Tasks;
using BarChart;
using System.Collections.Generic;
using AndroidGitHubContributeNotifier.Model.Services;
using System.Globalization;

namespace AndroidGitHubContributeNotifier
{
    [Activity(Label = "GitHub Contribute Notifier", MainLauncher = true, Icon = "@drawable/icon")]
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

            notificationServiceIntent = new Intent("com.YLiohenki.GitHubContributeNotifier.NotificationIntent");

            var settings = this.SettingsProvider.GetSettings();

            if (settings == null || string.IsNullOrWhiteSpace(settings.UserName))
            {
                StartActivity(typeof(LoginActivity));
                return;
            }
            else
            {
                Button btn = FindViewById<Button>(Resource.Id.settingsButton);
                btn.Click += delegate
                {
                    StartActivity(typeof(SettingsActivity));
                };

                Button graphPeriodBtn = FindViewById<Button>(Resource.Id.graphPeriod);
                graphPeriodBtn.Click += delegate
                {
                    settings = this.SettingsProvider.GetSettings();
                    if (settings.GraphPeriod == Settings.Period.Week)
                        settings.GraphPeriod = Settings.Period.Days30;
                    else
                        settings.GraphPeriod = Settings.Period.Week;
                    this.SettingsProvider.SetSettings(settings);
                    this.FillUIWIthEvents();
                };

                TextView usernameLabel = FindViewById<TextView>(Resource.Id.usernameLabel);
                usernameLabel.Text = settings.UserName;
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
            var settings = this.SettingsProvider.GetSettings();
            DateTime startDay;
            IEnumerable<int> numbers;
            if (settings.GraphPeriod == Settings.Period.Week)
            {
                startDay = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                numbers = Enumerable.Range(0, 7);
            }
            else
            {
                startDay = DateTime.Today.AddDays(-29);
                numbers = Enumerable.Range(0, 30);
            }
            var lastCommitView = FindViewById<TextView>(Resource.Id.lastCommitText);
            events.Sort((a, b) => (a.Time > b.Time ? -1 : (a.Time < b.Time ? 1 : 0)));
            var currentGroupNumber = 0;
            var groups = numbers.Select(x =>
            {
                var dayEvents = events.Where(e => e.Time.Date == startDay.AddDays(x));
                return new Tuple<int, DateTime, int>(dayEvents.Count(), startDay.AddDays(x), currentGroupNumber++);
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

        protected void ShowBarchat(IEnumerable<Tuple<int, DateTime, int>> groups)
        {
            LinearLayout bottomLayout = FindViewById<LinearLayout>(Resource.Id.bottomLayout);
            var settings = this.SettingsProvider.GetSettings();
            var metrics = Resources.DisplayMetrics;
            var leftLegendWidth = 100f;

            var chart = new BarChartView(this)
            {
                ItemsSource = groups.Select(g => new BarModel
                {
                    Value = g.Item1,
                    Legend = settings.GraphPeriod == Settings.Period.Week ?
                        g.Item2.DayOfWeek.ToString().Substring(0, 2) :
                        g.Item3 % 5 == 0 && g.Item3 != 0?
                        (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Item2.Month).Substring(0, 2) + "/" + g.Item2.Day) :
                        g.Item3 == 29 ? "T" : (g.Item3 == 0 ? g.Item2.Day.ToString() : ""),
                    Color = g.Item3 == 29 ? Android.Graphics.Color.Blue : Android.Graphics.Color.LightBlue
                }),
                BarWidth = (metrics.WidthPixels - leftLegendWidth) / groups.Count(),
                BarOffset = 0
            };

            bottomLayout.AddView(chart, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));
        }
    }
}

