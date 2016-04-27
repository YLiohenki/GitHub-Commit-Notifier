using Android.App;
using Android.OS;
using Android.Widget;
using AndroidGitHubCoach.Model;
using TinyIoC;

namespace AndroidGitHubCoach
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
        }
    }
}