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

namespace AndroidGitHubCoach.Model.Services
{
    [BroadcastReceiver(Name = "com.YLiohenki.GitHubCoach.BootReceiver")]
    [IntentFilter(new string[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.LowPriority)]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Bootstrapper.Run();
            var alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);
            var notificationServiceIntent = new Intent("com.YLiohenki.GitHubCoach.NotificationIntent");
            var pendingServiceIntent = PendingIntent.GetService(context, 0, notificationServiceIntent, PendingIntentFlags.CancelCurrent);
            alarm.SetRepeating(AlarmType.Rtc, SystemClock.ElapsedRealtime(), 900000, pendingServiceIntent);
        }
        int notificationId = 0;
        protected void ShowNotification(string message, Context context)
        {
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle("GitHub Coach")
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.Icon);

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify(notificationId++, notification);
        }
    }
}