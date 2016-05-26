using System;

using Android.App;

namespace AndroidGitHubContributeNotifier.Model
{
    public class Settings
    {
        public enum Period
        {
            Week,
            Days30
        }
        public String UserName { get; set; }
        public bool MakeSoundNotification { get; set; }
        public bool VibrateNotification { get; set; }
        public long PeriodNotification { get; set; } = AlarmManager.IntervalHalfHour;
        public long HourOfNotification { get; set; } = 14;
        public Period GraphPeriod { get; set; } = Period.Week;
    }
}