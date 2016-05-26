using System.Collections.Generic;
using Android.Content;

namespace AndroidGitHubContributeNotifier.Model
{
    public interface IEventsProvider
    {
        List<Event> GetEvents(Context context);
        void Refresh(Context context);
        void Clear(Context context);
    }
}