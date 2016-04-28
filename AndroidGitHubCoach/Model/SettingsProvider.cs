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
using TinyIoC;

namespace AndroidGitHubCoach.Model
{
    public class SettingsProvider : ISettingsProvider
    {
        IRepository FileRepository;
        public SettingsProvider()
        {
            this.FileRepository = TinyIoCContainer.Current.Resolve<IRepository>();
        }
        public Settings GetSettings()
        {
            var settings = this.FileRepository.FetchData<Settings>("settings");
            if (settings == null)
                settings = new Settings();
            return settings;
        }

        public void SetSettings(Settings settings)
        {
            this.FileRepository.StoreData("settings", settings);
        }
    }
}