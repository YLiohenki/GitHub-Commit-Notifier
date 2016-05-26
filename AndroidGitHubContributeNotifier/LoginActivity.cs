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
using AndroidGitHubContributeNotifier.Model;
using TinyIoC;

namespace AndroidGitHubContributeNotifier
{
    [Activity(Label = "Login", Icon = "@drawable/icon")]
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
                var settings = this.SettingsProvider.GetSettings();
                settings.UserName = username;
                this.SettingsProvider.SetSettings(settings);
                StartActivity(typeof(MainActivity));
            }
        }
    }
}