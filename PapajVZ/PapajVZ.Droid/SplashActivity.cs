using System.Threading.Tasks;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using PapajApi.Helper;
using PapajVZ.Droid.Helpers;
using PapajVZ.Helpers;
using PapajVZ.Model;
using PapajVZ.Models;

namespace PapajVZ.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        private string DeviceId => Device.UniqueId(this, Application);
       

        private void FetchCarte()
        {
            Shared.Carte = WebApi.GetRequest<Carte>($"http://papajvz.azurewebsites.net/api/{Api.Key}/Carte");
        }

        private void FetchUserVotes()
        {
            Shared.UserVotes =
                WebApi.GetRequest<UserVotes>($"http://papajvz.azurewebsites.net/api/{Api.Key}/Votes/{DeviceId}");
        }

      
           
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);

         
            var splashTask = new TaskFactory().StartNew(() =>
            {
                FetchCarte();
                FetchUserVotes();
                Shared.User = new User
                {
                    DeviceId = DeviceId,
                    Name = "Antonio"
                };


            });

            splashTask.ContinueWith(t =>
            {
                StartActivity(typeof(MainActivity));
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            ImageView imageView = FindViewById<ImageView>(Resource.Id.animated_loading);

            AnimationDrawable animation = (AnimationDrawable)imageView.Drawable;

            new Task(() => animation.Start()).Start();
            //TaskScheduler.FromCurrentSynchronizationContext()
        }
    }
}