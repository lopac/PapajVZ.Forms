using PapajVZ.Views;
using Xamarin.Forms;

namespace PapajVZ.Helpers
{
    public static class ExtensionsMethods
    {
        public static async void ScaleAnimate(this VisualElement i, uint length,double scale = 0.85)
        {
            await i.ScaleTo(scale, length, Easing.BounceOut);
            await i.ScaleTo(1, length, Easing.BounceIn);
        }

        public static void Hide(this VisualElement view)
        {
            view.IsVisible = false;
        }

        public static void Show(this VisualElement view)
        {
            view.IsVisible = true;
        }

        public static void OnItemClick(this Image button)
        {
            ActionBar.PrevClicked.Opacity = 0.1;

            button.Opacity = 1;
            button.ScaleAnimate(length: 120);

            ActionBar.PrevClicked = button;

            //NavBar.Children.ToList().ForEach(x => NavBar.Children.Remove(x));

        }

        public static string UppercaseFirst(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : char.ToUpper(value[0]) + value.Substring(1);
        }


    }
}
