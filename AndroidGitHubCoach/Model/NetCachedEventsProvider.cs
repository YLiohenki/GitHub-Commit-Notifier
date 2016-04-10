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
        IRepository FileRepository;
        public NetCachedEventsProvider()
        {
            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
            this.FileRepository = TinyIoCContainer.Current.Resolve<IRepository>();
        }
        public List<Event> GetEvents()
        {
            var events = this.FileRepository.FetchData<List<Event>>("events");
            if (events == null || events.Count == 0)
            {
                this.Refresh();
            }
            return events;
        }

        public void Refresh()
        {
            var url = new Uri(@"https://api.github.com/users/" + this.UserProvider.GetUserName() + @"/events");
            var events = FetchEvents(url);
            this.FileRepository.StoreData<List<Event>>("events", events);
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