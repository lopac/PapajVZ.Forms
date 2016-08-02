using PapajVZ.Helpers;
using PapajVZ.Views;
using Xamarin.Forms;

namespace PapajVZ.Renderers
{
    public class ImageButton : Image
    {
        public TapGestureRecognizer Clicker = new TapGestureRecognizer
        {
            NumberOfTapsRequired = 1
        };

        public ImageButton()
        {
            //this.GestureRecognizers.Add(Clicker);
        }

        public void OnClick()
        {
            MainPage.PreviousClickedButton.Opacity = 0.1;

            this.Opacity = 1;
            this.ScaleAnimate(length: 120);

            MainPage.PreviousClickedButton = this;
        }
    }
}