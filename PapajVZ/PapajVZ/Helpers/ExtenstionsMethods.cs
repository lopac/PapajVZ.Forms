using System;
using PapajVZ.Views;
using Xamarin.Forms;

namespace PapajVZ.Helpers
{
    public static class ExtensionsMethods
    {
        public static async void ScaleAnimate(this Image i, uint length)
        {
            await i.ScaleTo(0.88, length, Easing.CubicOut);
            await i.ScaleTo(1, length, Easing.CubicIn);
        }


    }
}
