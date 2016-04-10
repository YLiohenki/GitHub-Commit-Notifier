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
using TinyIoC;

namespace AndroidGitHubCoach.Model
{
    public class UserProvider : IUserProvider
    {
        IRepository FileRepository;
        public UserProvider()
        {
            this.FileRepository = TinyIoCContainer.Current.Resolve<IRepository>();
        }
        public string GetUserName()
        {
            return this.FileRepository.FetchData<string>("username");
        }

        public void SetUserName(string userName)
        {
            this.FileRepository.StoreData("username", userName);
        }
    }
}