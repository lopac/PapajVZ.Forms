using Android.App;
using Android.OS;
using PapajVZ.Droid.Helpers;
using PapajVZ.Model;
using Refractored.XamForms.PullToRefresh.Droid;

namespace PapajVZ.Droid.Activities
{
    [Activity(Label = "PapajVZ", Icon = "@drawable/icon",Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Carte Carte { get; set; }
        public static UserVotes UserVotes { get; set; }
        private string DeviceId => Device.UniqueId(this, Application);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            PullToRefreshLayoutRenderer.Init();

            LoadApplication(new App(Carte, UserVotes, new User {DeviceId = DeviceId,Name = UserName.Name}));
        }
    }
}