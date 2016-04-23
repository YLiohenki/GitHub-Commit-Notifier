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
            ServiceScheduler.Schedule(context);
        }
    }
}