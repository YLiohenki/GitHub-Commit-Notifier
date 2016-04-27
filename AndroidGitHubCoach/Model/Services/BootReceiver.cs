using Android.App;
using Android.Content;

namespace AndroidGitHubCoach.Model.Services
{
    [BroadcastReceiver(Name = "com.YLiohenki.GitHubCoach.BootReceiver")]
    [IntentFilter(new string[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.LowPriority)]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Bootstrapper.Run();
            ServiceScheduler.Schedule(context);
        }
    }
}