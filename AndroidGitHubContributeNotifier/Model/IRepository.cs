namespace AndroidGitHubContributeNotifier.Model
{
    public interface IRepository
    {
        T FetchData<T>(string key);

        bool StoreData<T>(string key, T data);
    }
}