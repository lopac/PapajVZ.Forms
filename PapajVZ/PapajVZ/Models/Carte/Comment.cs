using System;
using System.Collections.Generic;
using System.Linq;
using PapajVZ.Helpers;
using PapajVZ.Models;
using Xamarin.Forms;

namespace PapajVZ.Model
{

    public class Comment
    {

        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User User { get; set; }
        public virtual Menu Menu { get; set; }


        public static void Add(object sender, EventArgs e)
        {
            var entry = sender as Entry;

            if (entry.Text == string.Empty)
            {
                return;
            }

            var menuContainer = entry.Parent as StackLayout;

            var menuId = int.Parse(((menuContainer.Children[0] as StackLayout).Children[0] as Label).Text);

            var comment = new Comment
            {
                Body = entry.Text,
                User = Shared.User
            };

            entry.Text = string.Empty;

            Shared.Carte.Menus.FirstOrDefault(x => x.MenuId == menuId)?.Comments.Add(comment);

            Render(new List<Comment> { comment }, menuContainer.Children[3] as StackLayout, fromUser: true);



        }

        public static void Render(IEnumerable<Comment> comments, StackLayout container, bool fromUser = false)
        {

            comments.ToList().ForEach(comment => container.Children.Add(
              new StackLayout
              {
                  Orientation = StackOrientation.Horizontal,
                  Children =
                  {
                        new Label
                        {
                            TextColor = comment.User.DeviceId == Shared.User.DeviceId ? Color.Maroon : Color.Black,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 13,
                            Text = comment.User.Name
                        },
                        new Label
                        {
                            TextColor = Color.FromHex("#4c4c4c"),
                            FontSize = 13,
                            Text = comment.Body
                        },
                        new Image
                        {
                            Source = "remove.png",
                            IsVisible = comment.User.DeviceId == Shared.User.DeviceId,
                            Opacity = 0.7,
                            WidthRequest = 18,
                            HorizontalOptions = LayoutOptions.EndAndExpand
                        }
                  },
                  //shown later from animation
                  IsVisible = !fromUser
              }));

            if (fromUser)
            {
                var commentView = container.Children.Last();
                commentView.StepFade(1, 0.015);
                commentView.Show();
            }
        }
    }
}