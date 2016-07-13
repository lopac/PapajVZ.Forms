using System;
using PapajVZ.Views;

namespace PapajVZ.Interfaces
{
    public interface IActionBar
    {
        ActionBar ActionBar { get; set; }
        CarteView CarteView { get; set; }

        void CardOnClick(object sender, EventArgs e);
        void CardViewOnCreate();
        void CarteOnClick(object sender, EventArgs e);
        void CarteViewOnCreate();
        void SettingsOnClick(object sender, EventArgs e);
        void SettingsViewOnCreate();
        void StatisticsViewOnCreate();
        void VotesOnClick(object sender, EventArgs e);
        void VotesViewOnCreate();
    }
}