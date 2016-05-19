using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using TinyIoC;

namespace AndroidGitHubCoach.Model.Services
{
    [Service(Name = "com.YLiohenki.GitHubCoach.NotificationService")]
    [IntentFilter(new String[] { "com.YLiohenki.GitHubCoach.NotificationIntent" })]
    public class NotificationService : Android.App.IntentService
    {
        IEventsProvider EventsProvider;
        ISettingsProvider SettingsProvider;
        public NotificationService()
        {
            if (!TinyIoCContainer.Current.TryResolve<IEventsProvider>(out this.EventsProvider))
            {
                Bootstrapper.Run();
                this.EventsProvider = TinyIoCContainer.Current.Resolve<IEventsProvider>();
            }
            this.SettingsProvider = TinyIoCContainer.Current.Resolve<ISettingsProvider>();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);
            return StartCommandResult.Sticky;
        }

        protected override void OnHandleIntent(Intent intent)
        {
            var settings = this.SettingsProvider.GetSettings();
            if (settings == null || string.IsNullOrWhiteSpace(settings.UserName))
                return;

            var oldEvents = this.EventsProvider.GetEvents(this.ApplicationContext);
            var todayOldEvents = oldEvents.Where(x => x.Time.Date == DateTime.Now.Date);
            if (todayOldEvents.Count() > 0)
            {
                return;
            }

            this.EventsProvider.Refresh(this.ApplicationContext);
            var events = this.EventsProvider.GetEvents(this.ApplicationContext);
            if (events == null)
                return;
            var todayEvents = events.Where(x => x.Time.Date == DateTime.Now.Date);
            if (todayEvents.Count() < 10)
            {
                this.ShowNotification("You haven't any commits today.");
            }
        }
        public static int notificationId = 0;
        protected void ShowNotification(string message)
        {
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("GitHub Coach - " + this.SettingsProvider.GetSettings().UserName)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.Icon);

            var settings = this.SettingsProvider.GetSettings();

            builder.SetDefaults((settings.MakeSoundNotification ? NotificationDefaults.Sound : 0) | (settings.VibrateNotification ? NotificationDefaults.Vibrate : 0));

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify((int)(DateTime.Now.Date - new DateTime(2000, 1, 1)).TotalDays, notification);
        }
    }
}