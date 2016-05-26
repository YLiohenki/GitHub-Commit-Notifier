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
using AndroidGitHubContributeNotifier.Model;
using AndroidGitHubContributeNotifier.Model.Services;
using System.Threading;

namespace AndroidGitHubContributeNotifier
{
    public class Bootstrapper
    {
        public static void Run()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            TinyIoCContainer.Current.Register<FileRepository>();
            TinyIoCContainer.Current.Register<IRepository>(new FileRepository());
            TinyIoCContainer.Current.Register<ISettingsProvider>(new SettingsProvider());
            TinyIoCContainer.Current.Register<IEventsProvider>(new NetCachedEventsProvider());

            TinyIoCContainer.Current.Register<LoginActivity>();
            TinyIoCContainer.Current.Register<MainActivity>();
            TinyIoCContainer.Current.Register<SettingsActivity>();
            TinyIoCContainer.Current.Register<NotificationService>();
        }
    }
}