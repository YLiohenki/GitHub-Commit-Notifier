using Android.App;
using Android.Content;
using Android.OS;
using TinyIoC;

namespace AndroidGitHubContributeNotifier.Model.Services
{
    public class ServiceScheduler
    {
        public static void Schedule(Context context)
        {
            var settingsProvider = TinyIoCContainer.Current.Resolve<ISettingsProvider>();
            var alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);
            var notificationServiceIntent = new Intent("com.YLiohenki.GitHubContributeNotifier.NotificationIntent");
            var pendingServiceIntent = PendingIntent.GetService(context, 0, notificationServiceIntent, PendingIntentFlags.CancelCurrent);
            alarm.SetRepeating(AlarmType.Rtc, SystemClock.ElapsedRealtime(), settingsProvider.GetSettings().PeriodNotification, pendingServiceIntent);
        }
    }
}