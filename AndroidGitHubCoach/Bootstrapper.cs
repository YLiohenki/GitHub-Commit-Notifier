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
using AndroidGitHubCoach.Model;
using AndroidGitHubCoach.Model.Services;

namespace AndroidGitHubCoach
{
    public class Bootstrapper
    {
        public static void Run()
        {
            TinyIoCContainer.Current.Register<FileRepository>();
            TinyIoCContainer.Current.Register<IRepository>(new FileRepository());
            TinyIoCContainer.Current.Register<IUserProvider>(new UserProvider());
            TinyIoCContainer.Current.Register<IEventsProvider>(new NetCachedEventsProvider());

            TinyIoCContainer.Current.Register<LoginActivity>();
            TinyIoCContainer.Current.Register<MainActivity>();
            TinyIoCContainer.Current.Register<NotificationService>();
        }
    }
}