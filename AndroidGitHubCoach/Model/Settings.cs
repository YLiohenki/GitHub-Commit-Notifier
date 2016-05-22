using System;

using Android.App;

namespace AndroidGitHubCoach.Model
{
    public class Settings
    {
        public String UserName { get; set; }
        public bool MakeSoundNotification { get; set; }
        public bool VibrateNotification { get; set; }
        public long PeriodNotification { get; set; } = AlarmManager.IntervalHalfHour;
        public long HourOfNotification { get; set; } = 14;
    }
}