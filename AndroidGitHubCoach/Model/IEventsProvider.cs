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
    public interface IEventsProvider
    {
        List<Event> GetEvents(Context context);
        void Refresh(Context context);
    }
}