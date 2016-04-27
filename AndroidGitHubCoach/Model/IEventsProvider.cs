using System.Collections.Generic;
using Android.Content;

namespace AndroidGitHubCoach.Model
{
    public interface IEventsProvider
    {
        List<Event> GetEvents(Context context);
        void Refresh(Context context);
    }
}