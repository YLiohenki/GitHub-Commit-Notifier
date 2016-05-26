using System.IO;
using Newtonsoft.Json;

namespace AndroidGitHubContributeNotifier.Model
{
    class FileRepository : IRepository
    {
        protected static object syncObject = new object();
        public FileRepository()
        {
        }
        public T FetchData<T>(string key)
        {
            lock (syncObject)
            {
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, "YLiohenki-GitHubContributeNotifier." + key + ".Txt");
                if (!File.Exists(filePath))
                    return default(T);
                var dataString = System.IO.File.ReadAllText(filePath);
                T result = JsonConvert.DeserializeObject<T>(dataString);
                return result;
            }
        }

        public bool StoreData<T>(string key, T data)
        {
            lock (syncObject)
            {
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, "YLiohenki-GitHubContributeNotifier." + key + ".Txt");
                File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
                return true;
            }
        }
    }
}