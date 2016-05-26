using Android.App;
using Android.Content;

namespace AndroidGitHubContributeNotifier.Model.Services
{
    [BroadcastReceiver(Name = "com.YLiohenki.GitHubContributeNotifier.BootReceiver")]
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