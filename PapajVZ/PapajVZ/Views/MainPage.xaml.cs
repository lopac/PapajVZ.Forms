using System;
using PapajVZ.Helpers;
using PapajVZ.Interfaces;
using PapajVZ.Models;
using Xamarin.Forms;

namespace PapajVZ.Views
{
    public partial class MainPage : IActionBar
    {
        public ImageButton LastClickedIb { get; set; }
        public ImageButton CarteIb { get; set; }
        public ImageButton VotesIb { get; set; }
        public ImageButton CardIb { get; set; }
        public ImageButton SettingsIb { get; set; }

        public MainPage()
        {
            InitializeComponent();

            var carteBtnGesture = new TapGestureRecognizer();
            var votesBtnGesture = new TapGestureRecognizer();
            var cardBtnGesture = new TapGestureRecognizer();
            var settingsBtnGesture = new TapGestureRecognizer();

            CarteIb = new ImageButton
            {
                Button = CarteBtn,
                Id = "carte"
            };

            VotesIb = new ImageButton
            {
                Button = VotesBtn,
                Id = "heart"
            };

            CardIb = new ImageButton
            {
                Button = CardBtn,
                Id = "card"
            };

            SettingsIb = new ImageButton
            {
                Button = SettingsBtn,
                Id = "settings"
            };

            LastClickedIb = CarteIb;


            CarteIb.Button.GestureRecognizers.Add(carteBtnGesture);
            VotesIb.Button.GestureRecognizers.Add(votesBtnGesture);
            CardIb.Button.GestureRecognizers.Add(cardBtnGesture);
            SettingsIb.Button.GestureRecognizers.Add(settingsBtnGesture);

            carteBtnGesture.Tapped += CarteOnClick;
            votesBtnGesture.Tapped += VotesOnClick;
            cardBtnGesture.Tapped += CardOnClick;
            settingsBtnGesture.Tapped += SettingsOnClick;
        }

        public void RefreshActionBar(ImageButton present)
        {
            LastClickedIb.Button.Source = $"n_{LastClickedIb.Id}";
            present.Button.Source = $"{present.Id}";
            present.Button.ScaleAnimate(length: 100);

            LastClickedIb = present;
        }


        public void CarteOnClick(object sender, EventArgs e)
        {
            RefreshActionBar(CarteIb);
        }

        public void VotesOnClick(object sender, EventArgs e)
        {
            RefreshActionBar(VotesIb);
        }

        public void CardOnClick(object sender, EventArgs e)
        {
            RefreshActionBar(CardIb);
        }

        public void SettingsOnClick(object sender, EventArgs e)
        {
            RefreshActionBar(SettingsIb);
        }

        public void CarteViewOnCreate()
        {
            throw new NotImplementedException();
        }

        public void VotesViewOnCreate()
        {
            throw new NotImplementedException();
        }

        public void CardViewOnCreate()
        {
            throw new NotImplementedException();
        }

        public void StatisticsViewOnCreate()
        {
            throw new NotImplementedException();
        }

        public void SettingsViewOnCreate()
        {
            throw new NotImplementedException();
        }
    }
}