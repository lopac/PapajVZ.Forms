using System;
using PapajVZ.Models;

namespace PapajVZ.Interfaces
{
   
    interface IActionBar
    {
       void CarteViewOnCreate();
       void VotesViewOnCreate();
       void CardViewOnCreate();
       void StatisticsViewOnCreate();
       void SettingsViewOnCreate();
       void RefreshActionBar(ImageButton present);

        void CarteOnClick(object sender, EventArgs e);
        void VotesOnClick(object sender, EventArgs e);
        void CardOnClick(object sender, EventArgs e);
        void SettingsOnClick(object sender, EventArgs e);
    }
}
