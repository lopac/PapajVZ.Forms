using Android.App;
using Android.Content;

namespace PapajVZ.Droid.Helpers
{
    public class AppString
    {
        public static string Get(Application application, string key)
        {
            var prefs = Application.Context.GetSharedPreferences(application.PackageName, FileCreationMode.Private);
            return prefs.GetString(key, string.Empty);
        }

        public static void Save(Application application, string key, string value)
        {
            var prefs = Application.Context.GetSharedPreferences(application.PackageName, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(key, value);
            prefEditor.Commit();
        }
    }
}