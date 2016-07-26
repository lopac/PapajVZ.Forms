using Android.App;
using Android.OS;
using PapajVZ.Droid.Helpers;
using PapajVZ.Model;
using Refractored.XamForms.PullToRefresh.Droid;

namespace PapajVZ.Droid
{
    [Activity(Label = "PapajVZ", Icon = "@drawable/icon",Theme = "@android:style/Theme.Holo.Light", NoHistory = true)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            PullToRefreshLayoutRenderer.Init();

            LoadApplication(new App());
        }
    }
}