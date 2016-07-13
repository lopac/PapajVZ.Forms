using System;
using System.Collections.Generic;
using PapajVZ.Controls;
using PapajVZ.Helpers;
using PapajVZ.Interfaces;
using PapajVZ.Model;
using Xamarin.Forms;


namespace PapajVZ.Views
{
    public partial class CartePage : IActionBar
    {
        public CarteView CarteView { get; set; }
        public ActionBar ActionBar { get; set; }

        public static Carte Carte { get; set; }
        public static UserVotes UserVotes { get; set; }
        public static User User { get; set; }

        public CartePage(Carte carte, UserVotes userVotes, User user)
        {
            InitializeComponent();

            Carte = carte;
            UserVotes = userVotes;
            User = user;

            ActionBar = new ActionBar(new[] {CarteBtn, VotesBtn, CardBtn, SettingsBtn});


            ActionBar.Carte.GestureRecognizer.Tapped += CarteOnClick;
            ActionBar.Votes.GestureRecognizer.Tapped += VotesOnClick;
            ActionBar.Card.GestureRecognizer.Tapped += CardOnClick;
            ActionBar.Settings.GestureRecognizer.Tapped += SettingsOnClick;


            CarteViewOnCreate();
        }


        public void CarteViewOnCreate()
        {
            CarteView = new CarteView
            {
                CarteDate = CarteDate,
                CarteHeader = new CarteHeader
                {
                    DinnerIndicator = DinnerIndicator,
                    LunchIndicator = LunchIndicator,
                    DinnerLabel = DinnerLabel,
                    LunchLabel = LunchLabel
                },
                MenuGrid = MenuGrid,
                ActionBarComponents = new List<object> {Abar, AbShadow1, AbShadow2},
            };
        }


        public void CarteOnClick(object sender, EventArgs e)
        {
            var button = sender as Image;
            button.OnItemClick();

            CarteViewOnCreate();
        }

        public void VotesOnClick(object sender, EventArgs e)
        {
            var button = sender as Image;
            button.OnItemClick();
        }

        public void CardOnClick(object sender, EventArgs e)
        {
            var button = sender as Image;
            button.OnItemClick();
        }

        public void SettingsOnClick(object sender, EventArgs e)
        {
            var button = sender as Image;
            button.OnItemClick();
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