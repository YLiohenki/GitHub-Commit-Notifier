using System;
using System.Collections.Generic;

using System.Net;
using TinyIoC;
using System.Json;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using System.Linq;

namespace AndroidGitHubCoach.Model
{
    public class NetCachedEventsProvider : IEventsProvider
    {
        IRepository FileRepository;
        ISettingsProvider SettingsProvider;
        public NetCachedEventsProvider()
        {
            this.SettingsProvider = TinyIoCContainer.Current.Resolve<ISettingsProvider>();
            this.FileRepository = TinyIoCContainer.Current.Resolve<IRepository>();
        }
        public List<Event> GetEvents(Context context)
        {
            var events = this.FileRepository.FetchData<List<Event>>("events");
            if (events == null || events.Count == 0)
            {
                this.Refresh(context);
            }
            return events;
        }

        public void Refresh(Context context)
        {
            var oldEvents = this.FileRepository.FetchData<List<Event>>("events");
            var recentEventTime = oldEvents.Any() ? oldEvents.Max(x => x.Time) : DateTime.MinValue;
            var id = context.Resources.GetString(Resource.String.clientid);
            var secret = context.Resources.GetString(Resource.String.clientsecret);
            List<Event> events = new List<Event>();
            var page = 0;
            while (page <= 10)
            {
                ++page;
                var url = new Uri(@"https://api.github.com/users/" + this.SettingsProvider.GetSettings().UserName + @"/events" +
                    (string.IsNullOrWhiteSpace(id) ? "?" : "?client_id=" + id + "&client_secret=" + secret + "&") + "page=" + page);
                var toAdd = FetchEvents(url);
                events.AddRange(toAdd ?? new List<Event>());
                if (toAdd == null || toAdd.Count == 0 || toAdd.Min(x => x.Time) < recentEventTime)
                    break;
            }
            oldEvents.AddRange(events.Where(e => e.Time > recentEventTime).ToList());
            this.FileRepository.StoreData<List<Event>>("events", oldEvents);
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