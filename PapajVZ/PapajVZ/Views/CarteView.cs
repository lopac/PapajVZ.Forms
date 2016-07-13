using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PapajVZ.Controls;
using PapajVZ.Helpers;
using PapajVZ.Model;
using Xamarin.Forms;
using TapGestureRecognizer = PapajVZ.Controls.TapGestureRecognizer;

// ReSharper disable PossibleNullReferenceException

namespace PapajVZ.Views
{
    public sealed class CarteView
    {
        public CarteHeader CarteHeader { get; set; }

        public Label CarteDate { get; set; }
        public Grid MenuGrid { get; set; }
        public List<object> ActionBarComponents { get; set; }
        public List<Label> MenuTitles { get; set; }
        //public IList<StackLayout> MenuTitlesL { get; set; }
        public List<StackLayout> MenuBodies { get; set; }
        public List<Image> VoteBtns { get; set; }
        public List<Label> VoteLabels { get; set; }
        public List<StackLayout> CommentsContainers { get; set; }

        public MenuType CurrentMenuType { get; set; }

        public CarteView()
        {
           
            MenuTitles = new List<Label>();
            MenuBodies = new List<StackLayout>();
            VoteBtns = new List<Image>();
            VoteLabels = new List<Label>();
            CommentsContainers = new List<StackLayout>();

            CurrentMenuType = MenuType.Lunch;

            CarteDate.Text =
                $"{new CultureInfo("hr-HR").DateTimeFormat.GetDayName(CartePage.Carte.DateTime.DayOfWeek).ToUpperInvariant()}, {CartePage.Carte.DateTime.ToString("dd.MM.yyyy.")}";

            var row = 0;
            for (var i = 0; i < CartePage.Carte.Menus.Where(x => x.MenuType == MenuType.Lunch).ToList().Count; i++)
            {
                MenuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                MenuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var menu = CartePage.Carte.Menus.Where(x => x.MenuType == MenuType.Lunch).ToList()[i];

                MenuTitles.Add(new Label
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.Black,
                    Text = menu.Name.ToUpperInvariant(),
                    FontSize = 18
                });

                MenuGrid.Children.Add(
                    new Frame
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex("#efefef"),
                        Padding = new Thickness(10, 0, 0, 0),
                        HasShadow = false,
                        OutlineColor = Color.FromHex("#acacac"),
                        
                        Content =
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    MenuTitles[i]
                                }
                            }
                    }, 0, row++);

                var swipeLeft = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Left
                };

                var swipeRight = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Right
                };

                var doubleTap = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };




                swipeLeft.OnAction += (o, e) =>
                {
                    CarteHeader.OnLeftSwipe();
                    ChangeMenu(MenuType.Lunch);
                };

                swipeRight.OnAction += (o, e) =>
                {
                    CarteHeader.OnRightSwipe();
                    ChangeMenu(MenuType.Dinner);
                };



                var commentEntry = new Entry
                {
                    Placeholder = "Komentiraj..",
                    PlaceholderColor = Color.FromHex("#acacac"),
                    FontSize = 15,
                    BackgroundColor = Color.White,
                    TextColor = Color.Black,
                    InputTransparent = true,
                    Keyboard = Keyboard.Chat,
                };

                commentEntry.Completed += AddComment;

                MenuBodies.Add(new StackLayout
                {
                    GestureRecognizers =
                    {
                        swipeLeft,
                        swipeRight
                    },
                    Children =
                    {
                        new Label
                        {
                            Text = $"{menu.MenuId}",
                            IsVisible = false
                        }
                    }
                });



                menu.Articles.ToList().ForEach(article => MenuBodies[i].Children.Add(new Label
                {
                    Text = article.Item,
                    TextColor = Color.Black,
                    FontSize = 17
                }));

                var voteTapRecognizer = new Xamarin.Forms.TapGestureRecognizer();


                VoteBtns.Add(new Image
                {
                    Source = CartePage.UserVotes.Menus.Contains(menu.MenuId) ? "favorite.png" : "n_favorite.png",
                    WidthRequest = 24,
                    VerticalOptions = LayoutOptions.Center,
                    GestureRecognizers =
                    {
                        voteTapRecognizer
                    }
                });




                VoteLabels.Add(new Label
                {
                    FontSize = 14,
                    Opacity = 0.5,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.Black,
                    Text = $"Sviđa se: {menu.Votes}",
                    GestureRecognizers =
                    {
                        doubleTap
                    }
                });

                VoteLabels[i].ProcessGestureRecognizers();

                var carteVoteContainer = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        VoteBtns[i],
                        VoteLabels[i]
                    }
                };

                CommentsContainers.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Vertical
                });


                menu.Comments.ToList().ForEach(x => CommentsContainers[i].Children.Add(new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new Label
                        {
                            TextColor = Color.Black,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 13,
                            Text = x.User.Name
                        },
                        new Label
                        {
                            TextColor = Color.FromHex("#4c4c4c"),
                            FontSize = 13,
                            Text = x.Body
                        }
                    }
                }));

                MenuGrid.Children.Add(new StackLayout
                {
                    BackgroundColor = Color.White,
                    Padding = new Thickness(8, 0, 8, 0),
                    Children =
                    {
                        MenuBodies[i],
                        new BoxView
                        {
                            HeightRequest = 0.5,
                            WidthRequest = int.MaxValue,
                            BackgroundColor = Color.FromHex("#dcdcdc")
                        },
                        carteVoteContainer,
                        CommentsContainers[i],
                        new StackLayout
                        {
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
                            IsVisible = menu.Comments.Count > 2
                        },
                        commentEntry
                    }
                }, 0, row++);

                var i1 = i;
                voteTapRecognizer.Tapped += (o, e) =>
                {
                    OnVoteClick(VoteBtns[i1]);
                };

                var i2 = i;
                doubleTap.OnAction += (o, e) =>
                {
                    OnVoteClick(VoteBtns[i2]);
                };

                MenuBodies[i].ProcessGestureRecognizers();


                commentEntry.Focused += (o, e) =>
                {
                    ActionBarComponents.ForEach(x => ((VisualElement)x).Hide());
                };

                commentEntry.Unfocused += (o, e) =>
                {
                    ActionBarComponents.ForEach(x => ((VisualElement)x).Show());
                };


            }
        }

        private void OnVoteClick(object sender)
        {
            var voteButton = sender as Image;

            var parentLayout = voteButton.Parent as StackLayout;
            var menuId = int.Parse((((parentLayout.Parent as StackLayout).Children[0] as StackLayout).Children[0] as Label).Text);


            //unlike
            if (CartePage.UserVotes.Menus.Contains(menuId))
            {
                voteButton.Source = "n_favorite.png";
                CartePage.Carte.Menus.First(x => x.MenuId == menuId).Votes -= 1;
                (parentLayout.Children[1] as Label).Text = $"Sviđa se: {CartePage.Carte.Menus.First(x => x.MenuId == menuId).Votes}";
                CartePage.UserVotes.Menus.Remove(menuId);
            }
            else
            {
                voteButton.Source = "favorite.png";
                CartePage.Carte.Menus.First(x => x.MenuId == menuId).Votes += 1;
                (parentLayout.Children[1] as Label).Text = $"Sviđa se: {CartePage.Carte.Menus.First(x => x.MenuId == menuId).Votes}";
                CartePage.UserVotes.Menus.Add(menuId);
            }

            voteButton.ScaleAnimate(100, 1.2);

        }

        //TODO POST REQUEST TO API
        public void AddComment(object sender, EventArgs e)
        {
            var entry = sender as Entry;

            if (entry.Text == string.Empty)
            {
                return;
            }

            var menuContainer = entry.Parent as StackLayout;

            var commentsContainer = menuContainer.Children[3] as StackLayout;

            var menuId = int.Parse(((menuContainer.Children[0] as StackLayout).Children[0] as Label).Text);

            CartePage.Carte.Menus.FirstOrDefault(x => x.MenuId == menuId)?.Comments.Add(new Comment
            {
                Body = entry.Text,
                User = new User
                {
                    DeviceId = CartePage.User.DeviceId,
                    Name = CartePage.User.Name
                }
            });

            commentsContainer.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 13,
                        Text = "testing"
                    },
                    new Label
                    {
                        TextColor = Color.FromHex("#4c4c4c"),
                        FontSize = 13,
                        Text = entry.Text
                    }
                }
            });

            commentsContainer.Children.Last().ScaleAnimate(80, scale: 0.95);


            entry.Text = string.Empty;


        }

        public async void ChangeMenu(MenuType menuType)
        {
            if (CurrentMenuType == menuType)
            {
                return;
            }

            await MenuGrid.FadeTo(0.1, 100, Easing.SinOut);

            CurrentMenuType = menuType;

            for (var i = 0; i < CartePage.Carte.Menus.Where(x => x.MenuType == menuType).ToList().Count; i++)
            {
                var menu = CartePage.Carte.Menus.Where(x => x.MenuType == menuType).ToList()[i];

                MenuTitles[i].Text = menu.Name.ToUpperInvariant();
                MenuBodies[i].Children.Clear();

                MenuBodies[i].Children.Add(new Label
                {
                    Text = $"{menu.MenuId}",
                    IsVisible = false
                });

                menu.Articles.ToList().ForEach(article => MenuBodies[i].Children.Add(new Label
                {
                    Text = article.Item,
                    TextColor = Color.Black,
                    FontSize = 17
                }));

                VoteBtns[i].Source = CartePage.UserVotes.Menus.Contains(menu.MenuId) ? "favorite.png" : "n_favorite.png";



                VoteLabels[i].Text = $"Sviđa se: {menu.Votes}";

                CommentsContainers[i].Children.Clear();


                foreach (var comment in menu.Comments)
                {
                    CommentsContainers[i].Children.Add(new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                TextColor = Color.Black,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 13,
                                Text = comment.User.Name
                            },
                            new Label
                            {
                                TextColor = Color.FromHex("#4c4c4c"),
                                FontSize = 13,
                                Text = comment.Body
                            }
                        }
                    });

                }

            }

            await MenuGrid.FadeTo(1, 100, Easing.SinIn);
        }
    }
}

