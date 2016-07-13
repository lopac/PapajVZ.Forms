using System.Threading.Tasks;
using Android.App;
using Android.OS;
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


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);

            var splashTask = new TaskFactory().StartNew(() =>
            {
                MainActivity.Carte =
                    WebApi.GetRequest<Carte>($"http://papajvz.azurewebsites.net/api/{Api.Key}/Carte");


                MainActivity.UserVotes =
                    WebApi.GetRequest<UserVotes>(
                        $"http://papajvz.azurewebsites.net/api/{Api.Key}/Votes/{DeviceId}");
            });

            splashTask.ContinueWith(t => { StartActivity(typeof(MainActivity)); },
                TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}