﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using PapajVZ.Controls;
using PapajVZ.Helpers;
using PapajVZ.Interfaces;
using PapajVZ.Model;
using PapajVZ.Models;
using PapajVZ.Renderers;
using Xamarin.Forms;
using TapGestureRecognizer = PapajVZ.Controls.TapGestureRecognizer;

namespace PapajVZ.Views
{
    public partial class CartePage : IActionViews
    {

        public CartePage()
        {
            InitializeComponent();


            PresentMenu(LunchGrid, x => x.MenuType == MenuType.Lunch);
            PresentMenu(DinnerGrid, x => x.MenuType == MenuType.Dinner);
            //PresentMenu(VotesGrid, where: x => Shared.UserVotes.Menus.Contains(x.MenuId));

            //default view
            _currentView = ActionView.Carte;

            PreviousClickedButton = CarteBtn;
            CarteViewOnCreate(null, null);
        }

        public static ImageButton PreviousClickedButton { get; set; }

        private MenuType _currentMenuType;
        private ActionView _currentView;

        private MenuType CurrentMenuType
        {
            get { return _currentMenuType; }
            set
            {
                if (value != _currentMenuType)
                {
                    if (value == MenuType.Lunch)
                    {
                        DinnerIndicator.Color = Color.FromHex("#dcdcdc");
                        DinnerLabel.TextColor = Color.FromHex("#dcdcdc");

                        LunchIndicator.Color = Color.Black;
                        LunchLabel.TextColor = Color.Black;

                        LunchIndicator.ScaleAnimate(100, 0.8);
                        LunchLabel.ScaleAnimate(100, 0.8);

                        DinnerGrid.Hide();
                        LunchGrid.Show();
                    }
                    else if (value == MenuType.Dinner)
                    {
                        LunchIndicator.Color = Color.FromHex("#dcdcdc");
                        LunchLabel.TextColor = Color.FromHex("#dcdcdc");

                        DinnerIndicator.Color = Color.Black;
                        DinnerLabel.TextColor = Color.Black;

                        DinnerIndicator.StepFade();
                        DinnerLabel.ScaleAnimate(100, 0.8);


                        LunchGrid.Hide();
                        DinnerGrid.Show();
                    }
                    _currentMenuType = value;
                }
            }
        }

        public ActionView CurrentView
        {
            get { return _currentView; }
            set
            {
                if (value != _currentView)
                {
                    switch (_currentView)
                    {
                        case ActionView.Carte:
                            if (CurrentMenuType == MenuType.Lunch)
                            {
                                LunchGrid.Hide();
                            }
                            else
                            {
                                DinnerGrid.Hide();
                            }
                            break;
                        case ActionView.Votes:
                            VotesGrid.Hide();
                            break;
                        case ActionView.Card:
                            CardGrid.Hide();
                            break;
                        case ActionView.Settings:
                            SettingsGrid.Hide();
                            break;
                    }

                    //show  new view
                    switch (value)
                    {
                        case ActionView.Carte:
                            if (CurrentMenuType == MenuType.Lunch)
                            {
                                LunchGrid.Show();
                            }
                            else
                            {
                                DinnerGrid.Show();
                            }
                            break;
                        case ActionView.Votes:
                            VotesGrid.Show();
                            break;
                        case ActionView.Card:
                            CardGrid.Show();
                            break;
                        case ActionView.Settings:
                            SettingsGrid.Show();
                            break;
                    }

                    _currentView = value;
                }
            }
        }


        public void CarteViewOnCreate(object sender, EventArgs e)
        {
            (sender as ImageButton)?.OnClick();
            CurrentView = ActionView.Carte;

            CarteDate.Text = $"{new CultureInfo("hr-HR").DateTimeFormat.GetDayName(Shared.Carte.DateTime.DayOfWeek).ToUpperInvariant()}, {Shared.Carte.DateTime.ToString("dd.MM.yyyy.")}";

        }


        public void VotesViewOnCreate(object sender, EventArgs e)
        {
            (sender as ImageButton).OnClick();
            CurrentView = ActionView.Votes;


        }

        public void CardViewOnCreate(object sender, EventArgs e)
        {
            (sender as ImageButton)?.OnClick();
            CurrentView = ActionView.Card;

        }

        public void SettingsViewOnCreate(object sender, EventArgs e)
        {
            (sender as ImageButton)?.OnClick();
            CurrentView = ActionView.Settings;

        }


        private void OnSwipeLeft(BaseGestureRecognizer o, GestureRecognizerState e)
        {
            CurrentMenuType = MenuType.Lunch;
        }

        private void OnSwipeRight(BaseGestureRecognizer o, GestureRecognizerState e)
        {
            CurrentMenuType = MenuType.Dinner;
        }

        private void PresentMenu(Grid menuGrid, Expression<Func<Menu, bool>> where)
        {
            menuGrid.Children.Clear();
            menuGrid.RowDefinitions.Clear();

            var menuTitles = new List<Label>();
            var menuBodies = new List<StackLayout>();
            var voteButtons = new List<Image>();
            var voteLabels = new List<Label>();
            var commentContainers = new List<StackLayout>();

            

            var row = 0;
            for (var i = 0; i < Shared.Carte.Menus.Where(where.Compile()).ToList().Count; i++)
            {
                menuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                menuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var menu = Shared.Carte.Menus.Where(where.Compile()).ToList()[i];

                menuTitles.Add(new Label
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.Black,
                    Text = menu.Name.ToUpperInvariant(),
                    FontSize = 18
                });

                menuGrid.Children.Add(new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#efefef"),
                    Padding = new Thickness(10, 0, 0, 0),
                    Children =
                    {
                        new StackLayout
                        {
                            HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal, Children =
                            {
                                menuTitles[i]
                            }
                        }
                    }
                }, 0, row++);

                var menuSwipeLeft = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Left
                };

                var menuSwipeRight = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Right
                };

                var commentBodySwipeLeft = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Left
                };

                var commentBodySwipeRight = new SwipeGestureRecognizer
                {
                    Direction = SwipeGestureRecognizerDirection.Right
                };

                var doubleTap = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };


                menuSwipeLeft.OnAction += OnSwipeLeft;
                commentBodySwipeLeft.OnAction += OnSwipeLeft;

                menuSwipeRight.OnAction += OnSwipeRight;
                commentBodySwipeRight.OnAction += OnSwipeRight;


                var commentEntry = new Entry
                {
                    Placeholder = "Komentiraj..",
                    PlaceholderColor = Color.FromHex("#acacac"),
                    FontSize = 15,
                    BackgroundColor = Color.White,
                    TextColor = Color.Black,
                    InputTransparent = true,
                    Keyboard = Keyboard.Chat
                };

                commentEntry.Completed += Comment.Add;

                menuBodies.Add(new StackLayout
                {
                    GestureRecognizers =
                    {
                        menuSwipeLeft, menuSwipeRight
                    },
                    Children =
                    {
                        new Label
                        {
                            Text = $"{menu.MenuId}", IsVisible = false
                        }
                    }
                });


                menu.Articles.ToList().ForEach(article => menuBodies[i].Children.Add(new Label
                {
                    Text = article.Item,
                    TextColor = Color.Black,
                    FontSize = 17
                }));

                var voteTapRecognizer = new Xamarin.Forms.TapGestureRecognizer();


                voteButtons.Add(new Image
                {
                    Source = Shared.UserVotes.Menus.Contains(menu.MenuId) ? "favorite.png" : "n_favorite.png",
                    WidthRequest = 24,
                    VerticalOptions = LayoutOptions.Center,
                    GestureRecognizers =
                    {
                        voteTapRecognizer
                    }
                });


                voteLabels.Add(new Label
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

                voteLabels[i].ProcessGestureRecognizers();

                var carteVoteContainer = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        voteButtons[i], voteLabels[i]
                    }
                };

                commentContainers.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Vertical,
                    BackgroundColor = Color.White,
                    GestureRecognizers =
                    {
                        commentBodySwipeLeft, commentBodySwipeRight
                    }
                });


                Comment.Render(menu.Comments, commentContainers[i]);


                menuGrid.Children.Add(new StackLayout
                {
                    BackgroundColor = Color.White,
                    Padding = new Thickness(8, 0, 8, 0),
                    Children =
                    {
                        menuBodies[i], new BoxView
                        {
                            HeightRequest = 0.5, WidthRequest = int.MaxValue, BackgroundColor = Color.FromHex("#dcdcdc")
                        },
                        carteVoteContainer, commentContainers[i], new StackLayout
                        {
                            VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal, BackgroundColor = Color.White, Children =
                            {
                                new Label
                                {
                                    TextColor = Color.Black, FontSize = 15, Opacity = 0.6, Text = "Pokaži sve komentare.."
                                }
                            },
                            IsVisible = menu.Comments.Count > 4
                        },
                        commentEntry
                    }
                }, 0, row++);


                var j = i;

                voteTapRecognizer.Tapped += (o, ea) => { Vote.OnVoteButtonClick(voteButtons[j]); };
                doubleTap.OnAction += (o, ea) => { Vote.OnVoteButtonClick(voteButtons[j]); };

                menuBodies[i].ProcessGestureRecognizers();
                commentContainers.Last().ProcessGestureRecognizers();
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            //MenuGrid.StepFade();
        }
    }
}