using System;

namespace PapajVZ.Interfaces
{
    public enum ActionView
    {
        Carte,Votes,Card,Settings
    };
    public interface IActionViews
    { 
        ActionView CurrentView { get; set; }
        void CardViewOnCreate(object sender, EventArgs e);
        void CarteViewOnCreate(object sender, EventArgs e);
        void SettingsViewOnCreate(object sender, EventArgs e);
        void VotesViewOnCreate(object sender, EventArgs e);
    }
}