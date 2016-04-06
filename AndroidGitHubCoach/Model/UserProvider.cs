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

namespace AndroidGitHubCoach.Model
{
    class UserProvider : IUserProvider
    {
        public static string userName = null;
        public string GetUserName()
        {
            return userName;
        }

        public void SetUserName(string userName)
        {
            UserProvider.userName = userName;
        }
    }
}