using PapajVZ.Model;
using PapajVZ.Views;
using Xamarin.Forms;

namespace PapajVZ
{
    public class App : Application
    {
        public App()
        {
            MainPage = new CartePage();
            //MainPage = new Views.TestingPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}