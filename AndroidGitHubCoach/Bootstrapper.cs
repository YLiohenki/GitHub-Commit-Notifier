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

namespace AndroidGitHubCoach
{
    public class Bootstrapper
    {
        public static void Run()
        {
            TinyIoCContainer.Current.Register<IUserProvider>(new SharedFileUserProvider());
            TinyIoCContainer.Current.Register<IEventsProvider>(new NetEventsProvider());

            TinyIoCContainer.Current.Register<LoginActivity>();
        }
    }
}