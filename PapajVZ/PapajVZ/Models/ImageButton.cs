using Xamarin.Forms;

namespace PapajVZ.Models
{
    public class ImageButton
    {
        public Image Button { get; set; }
        public TapGestureRecognizer GestureRecognizer { get; set; }

        public ImageButton()
        {
            GestureRecognizer = new TapGestureRecognizer();
        }

    }
}
