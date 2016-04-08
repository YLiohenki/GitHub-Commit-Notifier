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
using System.IO;

namespace AndroidGitHubCoach.Model
{
    class SharedFileUserProvider : IUserProvider
    {
        public string GetUserName()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, "GitHubCoach.Data.Txt");
            if (!File.Exists(filePath))
                return null;
            var userName = System.IO.File.ReadAllText(filePath);
            return userName;
        }

        public void SetUserName(string userName)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, "GitHubCoach.Data.Txt");
            System.IO.File.WriteAllText(filePath, userName);
        }
    }
}