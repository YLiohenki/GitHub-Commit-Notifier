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

namespace AndroidGitHubCoach.Model
{
    public class Settings
    {
        public String UserName { get; set; }
        public bool MakeSoundNotification { get; set; }
        public bool VibrateNotification { get; set; }
    }
}