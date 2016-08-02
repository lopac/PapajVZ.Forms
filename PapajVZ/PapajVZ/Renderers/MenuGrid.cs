using System;
using System.Linq;
using PapajVZ.Controls;
using PapajVZ.Helpers;
using PapajVZ.Model;
using PapajVZ.Models;
using Xamarin.Forms;
using TapGestureRecognizer = Xamarin.Forms.TapGestureRecognizer;

namespace PapajVZ.Renderers
{
    public class MenuGrid : Grid
    {
        public MenuGrid()
        {
            RowNumber = 0;
        }

        private int RowNumber { get; set; }

        public void AddMenu(Menu menu, Action<BaseGestureRecognizer, GestureRecognizerState> onSwipeLeft, Action<BaseGestureRecognizer, GestureRecognizerState> onSwipeRight)
        {
            RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

            var articlesLayout = new StackLayout();

            menu.Articles.ToList().ForEach(article => articlesLayout.Children.Add(new Label
            {
                Text = article.Item,
                TextColor = Color.Black,
                FontSize = 17
            }));

            var commentEntry = new Entry
            {
                Placeholder = "Komentiraj..",
                PlaceholderColor = Color.FromHex("#acacac"),
                FontSize = 15,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                InputTransparent = true,
                Keyboard = Keyboard.Text
            };

            commentEntry.Completed += OnCommentEntry;

            var voteTapRecognizer = new TapGestureRecognizer();


            Children.Add(new CardLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(7, 7, 7, 7),
                Children =
                {
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                            new CardImage
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 100,
                                Children =
                                {
                                    new Label
                                    {
                                        Margin = new Thickness(0, 7, 0, 0),
                                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        Text = menu.Name.ToUpperInvariant(),
                                        FontSize = 25,
                                        TextColor = Color.White
                                    }
                                }
                            }
                        }
                    },
                    new StackLayout
                    {
                        BackgroundColor = Color.White,
                        Padding = new Thickness(8, 0, 8, 0),
                        Children =
                        {
                            new Label
                            {
                                Text = $"{menu.MenuId}",
                                IsVisible = false
                            },
                            articlesLayout
                        }
                    },
                    new BoxView
                    {
                        IsVisible = false,
                        HeightRequest = 0.5,
                        WidthRequest = short.MaxValue,
                        BackgroundColor = Color.FromHex("#dcdcdc")
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(8, 0, 8, 0),

                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Image
                            {
                                Source =
                                    Shared.UserVotes.Menus.Contains(menu.MenuId) ? "favorite.png" : "n_favorite.png",
                                WidthRequest = 24,
                                VerticalOptions = LayoutOptions.Center,
                                GestureRecognizers =
                                {
                                    voteTapRecognizer
                                }
                            },
                            new Label
                            {
                                FontSize = 14,
                                Opacity = 0.5,
                                VerticalOptions = LayoutOptions.Center,
                                TextColor = Color.Black,
                                Text = $"Sviđa se: {menu.Votes}"
                                //GestureRecognizers =
                                //{
                                //  todo   doubleTap
                                //}
                            }
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(8, 0, 8, 0),

                        VerticalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Vertical,
                        BackgroundColor = Color.White
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(8, 0, 8, 0),

                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        BackgroundColor = Color.White,
                        Children =
                        {
                            new Label
                            {
                                TextColor = Color.Black,
                                FontSize = 15,
                                Opacity = 0.6,
                                Text = "Pokaži sve komentare.."
                            }
                        },
                        IsVisible = menu.Comments.Count > 4
                    },
                    commentEntry
                }
            }, 0, RowNumber++);

            Children.Last().ProcessGestureRecognizers();


            (Children.Last() as CardLayout)?.Children.ToList().ForEach(view =>
            {
                var menuSwipeLeft = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Left
                };

                var menuSwipeRight = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Right
                };


                menuSwipeLeft.OnAction += onSwipeLeft;

                menuSwipeRight.OnAction += onSwipeRight;

                view.GestureRecognizers.Add(menuSwipeLeft);
                view.GestureRecognizers.Add(menuSwipeRight);

                view.ProcessGestureRecognizers();
            });

            menu.Comments.ToList()
                .ForEach(comment => AddComment((Children.Last() as CardLayout)?.Children[4] as StackLayout, comment,false));


            var voteButton =
                ((Children.Last() as CardLayout)?.Children[3] as StackLayout)?.Children.First() as Image;

            voteTapRecognizer.Tapped += (o, ea) => this.OnVoteButtonClick(voteButton);
            
            //doubleTap.OnAction += (o, ea) => { Vote.OnVoteButtonClick(voteButtons.Last()); };
            voteButton.ProcessGestureRecognizers();

            (Children.Last() as CardLayout)?.Children[1].ProcessGestureRecognizers();
        }

        public void OnVoteButtonClick(Image voteButton)
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

        private void OnCommentEntry(object sender, EventArgs e)
        {
            var entry = sender as Entry;

            if (entry.Text == string.Empty)
            {
                return;
            }

            var menuContainer = (entry.Parent as CardLayout).Children[4] as StackLayout;
            var menuId = int.Parse((((entry.Parent as CardLayout).Children[1] as StackLayout).Children[0] as Label).Text);


            var comment = new Comment
            {
                Body = entry.Text,
                User = Shared.User
            };

            entry.Text = string.Empty;

            Shared.Carte.Menus.FirstOrDefault(x => x.MenuId == menuId)?.Comments.Add(comment);

            AddComment(menuContainer, comment, true);
        }

        private void AddComment(StackLayout container, Comment comment, bool fromUser)
        {
            container.Children.Add(
                new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new Label
                        {
                            TextColor = comment.User.DeviceId == Shared.User.DeviceId ? Color.Navy : Color.Black,
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
                    IsVisible = !fromUser
                });

            if (fromUser)
            {
                var commentView = container.Children.Last();
                commentView.StepFade(1, 0.015);
                commentView.Show();
            }
        }

        public void Clear()
        {
            Children.Clear();
            RowDefinitions.Clear();
        }
    }
}