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
        public NetCachedEventsProvider()
        {
            this.UserProvider = TinyIoCContainer.Current.Resolve<IUserProvider>();
        }
        public List<Event> GetEvents(string UserName)
        {
            var webClient = new WebClient();

            var url = new Uri(@"https://api.github.com/users/" + this.UserProvider.GetUserName() + @"/events");
            var result = FetchEvents(url);
            return result;
        }

        private List<Event> FetchEvents(Uri url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";
            request.UserAgent = "YLiohenki/GitHubCoach";
            List<Event> result = new List<Event>();

            // Send the request to the server and wait for the response:
            using (WebResponse response = request.GetResponse())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = JsonArray.Load(stream);
                    foreach (var jsonEvent in jsonDoc)
                    {
                        var date = ((JsonValue)jsonEvent)["created_at"].ToString().Replace("\"", "");
                        var type = ((JsonValue)jsonEvent)["type"].ToString().Replace("\"", "");
                        result.Add(new Event() { Time = DateTime.Parse(date), Type = type });
                    }
                    // Return the JSON document:
                    return result;
                }
            }
        }
    }
}