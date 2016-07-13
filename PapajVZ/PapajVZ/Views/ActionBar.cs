using System.Collections.Generic;
using PapajVZ.Models;
using Xamarin.Forms;

namespace PapajVZ.Views
{
    public class ActionBar
    {
        public static Image PrevClicked { get; set; }
        public ImageButton Carte { get; set; }
        public ImageButton Votes { get; set; }
        public ImageButton Card { get; set; }
        public ImageButton Settings { get; set; }

        public ActionBar(IList<Image> buttons)
        {
            Carte = new ImageButton
            {
                Button = buttons[0]
            };

            Votes = new ImageButton
            {
                Button = buttons[1]
            };

            Card = new ImageButton
            {
                Button = buttons[2]
            };

            Settings = new ImageButton
            {
                Button = buttons[3]
            };

            PrevClicked = Carte.Button;


            Carte.Button.GestureRecognizers.Add(Carte.GestureRecognizer);
            Votes.Button.GestureRecognizers.Add(Votes.GestureRecognizer);
            Card.Button.GestureRecognizers.Add(Card.GestureRecognizer);
            Settings.Button.GestureRecognizers.Add(Settings.GestureRecognizer);

        }

        
    }
}
