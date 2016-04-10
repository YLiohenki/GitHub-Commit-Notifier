using System;
using System.Collections.Generic;

using System.Net;
using TinyIoC;
using System.Json;
using System.Threading.Tasks;
using System.IO;

namespace AndroidGitHubCoach.Model
{
    public class NetCachedEventsProvider : IEventsProvider
    {
        IUserProvider UserProvider;
        protected static List<Event> Events;
        protected static DateTime LastTimeFetched;
        public NetCachedEventsProvider()
        {
            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
        }
        public List<Event> GetEvents()
        {
            if (Events == null || Events.Count == 0 || (DateTime.UtcNow - LastTimeFetched) > TimeSpan.FromMinutes(10))
            {
                this.Refresh();
            }
            return Events;
        }

        public void Refresh()
        {
            var url = new Uri(@"https://api.github.com/users/" + this.UserProvider.GetUserName() + @"/events");
            Events = FetchEvents(url);
        }

        private List<Event> FetchEvents(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";
            request.UserAgent = "YLiohenki/GitHubCoach";
            List<Event> result = new List<Event>();
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue jsonDoc = JsonArray.Load(stream);
                        foreach (var jsonEvent in jsonDoc)
                        {
                            var date = ((JsonValue)jsonEvent)["created_at"].ToString().Replace("\"", "");
                            var type = ((JsonValue)jsonEvent)["type"].ToString().Replace("\"", "");
                            result.Add(new Event() { Time = DateTime.Parse(date), Type = type });
                        }
                        LastTimeFetched = DateTime.UtcNow;
                        return result;
                    }
                }
            }
            catch (WebException ex)
            {
                return new List<Event>();
            }
        }
    }
}