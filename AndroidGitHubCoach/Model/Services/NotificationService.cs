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
        IUserProvider UserProvider;
        public NotificationService()
        {
            if (!TinyIoCContainer.Current.TryResolve<IEventsProvider>(out this.EventsProvider))
            {
                Bootstrapper.Run();
                this.EventsProvider = TinyIoCContainer.Current.Resolve<IEventsProvider>();
            }
            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);
            return StartCommandResult.Sticky;
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (string.IsNullOrWhiteSpace(this.UserProvider.GetUserName()))
                return;
            this.EventsProvider.Refresh(this.ApplicationContext);
            var events = this.EventsProvider.GetEvents(this.ApplicationContext);
            if (events == null)
                return;
            var todayEvents = events.Where(x => x.Time.Date == DateTime.Now.Date);
            if (todayEvents.Count() < 1)
            {
                this.ShowNotification("You haven't any commits today.");
            }
        }
        public static int notificationId = 0;
        protected void ShowNotification(string message)
        {
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("GitHub Coach - " + this.UserProvider.GetUserName())
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.Icon);

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify((int)(DateTime.Now.Date - new DateTime(2000, 1, 1)).TotalDays, notification);
        }
    }
}