using System.Linq;
using PapajVZ.Helpers;
using PapajVZ.Models;
using Xamarin.Forms;

namespace PapajVZ.Model
{
    public class Vote
    {
        public bool Unvoting { get; set; }
        public string DeviceId { get; set; }
        public int MenuId { get; set; }

        //TODO POST REQ TO API
        public static void OnVoteButtonClick(Image voteButton)
        {
            var parentLayout = voteButton.Parent as StackLayout;
            var menuId = int.Parse((((parentLayout.Parent as StackLayout).Children[0] as StackLayout).Children[0] as Label).Text);

            //unlike
            if (Shared.UserVotes.Menus.Contains(menuId))
            {
                voteButton.Source = "n_favorite.png";
                Shared.Carte.Menus.First(x => x.MenuId == menuId).Votes -= 1;
                Shared.UserVotes.Menus.Remove(menuId);
                (parentLayout.Children[1] as Label).Text = $"Sviđa se: {Shared.Carte.Menus.First(x => x.MenuId == menuId).Votes}";
                Shared.UserVotes.Menus.Remove(menuId);
            }
            else
            {
                voteButton.Source = "favorite.png";
                Shared.Carte.Menus.First(x => x.MenuId == menuId).Votes += 1;
                Shared.UserVotes.Menus.Add(menuId);
                (parentLayout.Children[1] as Label).Text = $"Sviđa se: {Shared.Carte.Menus.First(x => x.MenuId == menuId).Votes}";
                Shared.UserVotes.Menus.Add(menuId);
            }


            voteButton.ScaleAnimate(100, 1.2);


        }
    }
}