using TinyIoC;

namespace AndroidGitHubContributeNotifier.Model
{
    public class SettingsProvider : ISettingsProvider
    {
        IRepository FileRepository;
        static Settings currentSettings;
        public SettingsProvider()
        {
            this.FileRepository = TinyIoCContainer.Current.Resolve<IRepository>();
        }
        public Settings GetSettings()
        {
            if (currentSettings == null)
            currentSettings = this.FileRepository.FetchData<Settings>("settings");
            if (currentSettings == null)
                currentSettings = new Settings();
            return currentSettings;
        }

        public void SetSettings(Settings settings)
        {
            currentSettings = settings;
            this.FileRepository.StoreData("settings", settings);
        }
    }
}