using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Android.App;
using Android.Media;
using Android.OS;
using Felipecsl.GifImageViewLibrary;
using ModernHttpClient;
using PapajApi.Helper;
using PapajVZ.Droid.Helpers;
using PapajVZ.Helpers;
using PapajVZ.Model;

namespace PapajVZ.Droid.Activities
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        private string DeviceId => Device.UniqueId(this, Application);

        private void FetchCarte()
        {
            MainActivity.Carte = WebApi.GetRequest<Carte>($"http://papajvz.azurewebsites.net/api/{Api.Key}/Carte");
        }

        private void FetchUserVotes()
        {
            MainActivity.UserVotes =
                WebApi.GetRequest<UserVotes>($"http://papajvz.azurewebsites.net/api/{Api.Key}/Votes/{DeviceId}");
        }


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);

            var loadingView = FindViewById<GifImageView>(Resource.Layout.loadingView);

            var client = new HttpClient(new NativeMessageHandler());
            var bytes = await client.GetByteArrayAsync("github");
            loadingView.SetBytes(bytes);
            loadingView.StartAnimation();


            var splashTask = new TaskFactory().StartNew(() =>
            {
                var tasks = new List<Task>
                {
                    new TaskFactory().StartNew(FetchCarte),
                    new TaskFactory().StartNew(FetchUserVotes)
                };

                tasks.ForEach(x => x.Wait());
            });

            await splashTask.ContinueWith(t => StartActivity(typeof(MainActivity)),TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}