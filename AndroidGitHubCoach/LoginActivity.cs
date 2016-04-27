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
using AndroidGitHubCoach.Model;
using TinyIoC;

namespace AndroidGitHubCoach
{
    [Activity(Label = "GitHub Coach")]
    public class LoginActivity : Activity
    {
        ISettingsProvider SettingsProvider;
        public LoginActivity()
        {
            this.SettingsProvider = TinyIoCContainer.Current.Resolve<ISettingsProvider>();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            Button btn = FindViewById<Button>(Resource.Id.logindone);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            EditText editText = FindViewById<EditText>(Resource.Id.loginusername);

            var username = editText.Text;

            if (!string.IsNullOrWhiteSpace(username))
            {
                this.UserProvider.SetUserName(username);
                StartActivity(typeof(MainActivity));
            }
        }
    }
}