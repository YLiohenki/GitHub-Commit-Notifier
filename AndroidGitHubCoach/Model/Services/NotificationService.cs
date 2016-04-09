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
    [Service]
    [IntentFilter(new String[] { "com.YLiohenki.GitHubCoach" })]
    public class NotificationService : Android.App.IntentService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}