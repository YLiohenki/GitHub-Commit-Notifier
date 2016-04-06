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
using System.Net;
using TinyIoC;

namespace AndroidGitHubCoach.Model
{
    public class EventsProvider : IEventsProvider
    {
        IUserProvider UserProvider;
        public EventsProvider()
        {
            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
        }
        public List<Event> GetEvents(string UserName)
        {
            var webClient = new WebClient();

            var url = new Uri(@"https://api.github.com/users/" + this.UserProvider.GetUserName() + @"/events");
            return null;
        }
    }
}