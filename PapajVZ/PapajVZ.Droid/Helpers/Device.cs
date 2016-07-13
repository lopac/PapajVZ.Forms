using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Java.Util;

namespace PapajVZ.Droid.Helpers
{
    public class Device
    {
        private const string UnqualifiedId = "02:00:00:00:00:00";

        public static string UniqueId(Context context, Application app)
        {
            if (string.IsNullOrEmpty(AppString.Get(app, "UUID")))
            {
                var wifi = (WifiManager)context.GetSystemService(Context.WifiService);
                var uniqueId = wifi.ConnectionInfo.MacAddress;

                if (uniqueId == UnqualifiedId || string.IsNullOrEmpty(uniqueId))
                {
                    uniqueId = UUID.RandomUUID().ToString();
                }

                AppString.Save(app, "UUID", uniqueId.Replace(":", string.Empty));
            }

            return AppString.Get(app, "UUID");
        }
    }
}