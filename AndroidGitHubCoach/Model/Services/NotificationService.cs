using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TinyIoC;

namespace AndroidGitHubCoach.Model.Services
{
    [Service]
    [IntentFilter(new String[] { "com.YLiohenki.GitHubCoach" })]
    public class NotificationService : Android.App.IntentService
    {
        IEventsProvider EventsProvider;
        IUserProvider UserProvider;
        public NotificationService()
        {
            this.EventsProvider = TinyIoCContainer.Current.Resolve<IEventsProvider>();
            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
        }
        protected override void OnHandleIntent(Intent intent)
        {
            this.EventsProvider.Refresh();
            var events = this.EventsProvider.GetEvents();
            var todayEvents = events.Where(x => x.Time.Date == DateTime.Now.Date);
            if (todayEvents.Count() < 10)
            {
                this.ShowNotification("You have only " + todayEvents.Count() + " events on GitHub today");
            }
        }

        protected void ShowNotification(string message)
        {
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("GitHub Coach - " + this.UserProvider.GetUserName())
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.Icon);

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}