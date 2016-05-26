using Android.App;
using Android.OS;
using Android.Widget;
using AndroidGitHubContributeNotifier.Model;
using TinyIoC;

namespace AndroidGitHubContributeNotifier
{
    [Activity(Label = "Settings", Icon = "@drawable/icon")]
    public class SettingsActivity : Activity
    {
        ISettingsProvider SettingsProvider = null;
        IEventsProvider EventsProvider;
        public SettingsActivity()
        {
            this.SettingsProvider = TinyIoCContainer.Current.Resolve<ISettingsProvider>();

            this.EventsProvider = TinyIoCContainer.Current.Resolve<IEventsProvider>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Settings);
        }

        protected override void OnStart()
        {
            base.OnStart();

            var userName = this.SettingsProvider.GetSettings().UserName;
            Button btn = FindViewById<Button>(Resource.Id.usernameButton);
            btn.Text = userName;
            btn.Click += delegate
            {
                StartActivity(typeof(LoginActivity));
            };

            var settings = this.SettingsProvider.GetSettings();
            CheckBox useSoundCB = FindViewById<CheckBox>(Resource.Id.notificationUseSound);
            CheckBox vibrateCB = FindViewById<CheckBox>(Resource.Id.notificationVibrate);
            Spinner periodSpinner = FindViewById<Spinner>(Resource.Id.periodOfServiceSpinner);
            useSoundCB.Checked = settings.MakeSoundNotification;
            vibrateCB.Checked = settings.VibrateNotification;
            switch (settings.PeriodNotification)
            {
                case AlarmManager.IntervalFifteenMinutes:
                    periodSpinner.SetSelection(0);
                    break;
                case AlarmManager.IntervalHalfHour:
                    periodSpinner.SetSelection(1);
                    break;
                case AlarmManager.IntervalHour:
                    periodSpinner.SetSelection(2);
                    break;
                default:
                    settings.PeriodNotification = AlarmManager.IntervalHalfHour;
                    break;
            }

            Button savebtn = FindViewById<Button>(Resource.Id.saveButton);
            savebtn.Click += delegate
            {
                settings.MakeSoundNotification = useSoundCB.Checked;
                settings.VibrateNotification = vibrateCB.Checked;
                switch (periodSpinner.SelectedItemPosition)
                {
                    case 0:
                        settings.PeriodNotification = AlarmManager.IntervalFifteenMinutes;
                        break;
                    case 1:
                        settings.PeriodNotification = AlarmManager.IntervalHalfHour;
                        break;
                    case 2:
                        settings.PeriodNotification = AlarmManager.IntervalHour;
                        break;
                    default:
                        settings.PeriodNotification = AlarmManager.IntervalHalfHour;
                        break;
                }
                this.SettingsProvider.SetSettings(settings);
                
                StartActivity(typeof(MainActivity));
            };
        }
    }
}