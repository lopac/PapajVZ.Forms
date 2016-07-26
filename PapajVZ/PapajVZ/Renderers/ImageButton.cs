using Xamarin.Forms;

namespace PapajVZ.Renderers
{
    public class ImageButton : Xamarin.Forms.Image
    {
        public ImageButton()
        {
            GestureRecognizers.Add(new TapGestureRecognizer()));
        }
    }
}
