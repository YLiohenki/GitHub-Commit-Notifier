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
    public class ServiceScheduler
    {
        public static void Schedule(Context context)
        {
            var alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);
            var notificationServiceIntent = new Intent("com.YLiohenki.GitHubCoach.NotificationIntent");
            var pendingServiceIntent = PendingIntent.GetService(context, 0, notificationServiceIntent, PendingIntentFlags.CancelCurrent);
            alarm.SetRepeating(AlarmType.Rtc, SystemClock.ElapsedRealtime(), AlarmManager.IntervalHalfHour, pendingServiceIntent);
        }
    }
}