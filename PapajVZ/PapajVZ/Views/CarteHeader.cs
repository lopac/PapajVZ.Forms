using PapajVZ.Helpers;
using Xamarin.Forms;

namespace PapajVZ.Views
{
    public class CarteHeader
    {
        
        public Label LunchLabel { get; set; }
        public Label DinnerLabel { get; set; }
        public BoxView LunchIndicator { get; set; }
        public BoxView DinnerIndicator { get; set; }

        public void OnLeftSwipe()
        {
            DinnerIndicator.Color = Color.FromHex("#dcdcdc");
            DinnerLabel.TextColor = Color.FromHex("#dcdcdc");

            LunchIndicator.Color = Color.Black;
            LunchLabel.TextColor = Color.Black;

            LunchIndicator.ScaleAnimate(100, 1.2);
            LunchLabel.ScaleAnimate(100, 1.2);
        }

        public void OnRightSwipe()
        {
            LunchIndicator.Color = Color.FromHex("#dcdcdc");
            LunchLabel.TextColor = Color.FromHex("#dcdcdc");

            DinnerIndicator.Color = Color.Black;
            DinnerLabel.TextColor = Color.Black;

            DinnerIndicator.ScaleAnimate(100,1.2);
            DinnerLabel.ScaleAnimate(100,1.2);
        }
    }
}