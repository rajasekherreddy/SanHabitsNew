using System;
using System.Globalization;
using HappinessIndex.Helpers;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class JournalPage : ContentPage
    {
        public JournalPage()
        {
            InitializeComponent();
            //TODO: Split this page into two views.
        }

        protected override void OnAppearing()
        {
            var dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(AppSettings.JournalDate.DayOfWeek);

            Title = $"{dayName}, {AppSettings.JournalDate:MMMM dd yyyy}".ToUpper();

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            //AppSettings.JournalDate = DateTime.Now;
            base.OnDisappearing();
        }

        //void SubmitButtonClicked(object sender, EventArgs e)
        //{
        //   // ShowPage2(null, null);
        //}

        //void SaveButtonClicked(object sender, EventArgs e)
        //{
        //   // ShowPage1(null, null);
        //}

        //void ShowPage2(object sender, EventArgs e)
        //{
        //    Page2.IsVisible = true;
        //    var parentAnimation = new Animation();

        //    var page1Animation =
        //        new Animation(v => Page1.TranslationY = v, 0, -Page1.Height, Easing.SinOut, () => { Page1.IsVisible = false; });

        //    var page2Animation =
        //        new Animation(v => Page2.TranslationY = v, Page1.Height, 0, Easing.SinOut);

        //    parentAnimation.Add(0, 1, page1Animation);
        //    parentAnimation.Add(0, 1, page2Animation);

        //    parentAnimation.Commit(this, "TranslationY");
        //}

        //void Page1SizeChanged(object sender, EventArgs e)
        //{
        //    Page2.TranslationY = Page1.Height;
        //}

        //void ShowPage1(object sender, EventArgs e)
        //{
        //    Page1.IsVisible = true;

        //    var parentAnimation = new Animation();

        //    var page1Animation =
        //        new Animation(v => Page1.TranslationY = v, -Page1.Height, 0, Easing.SinOut);

        //    var page2Animation =
        //        new Animation(v => Page2.TranslationY = v, 0, Page1.Height, Easing.SinOut, () => { Page2.IsVisible = false; });

        //    parentAnimation.Add(0, 1, page1Animation);
        //    parentAnimation.Add(0, 1, page2Animation);

        //    parentAnimation.Commit(this, "TranslationYRev");
        //}

        //void RealitySlider_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    //if (appeared)
        //    //{
        //    //    var journal = (sender as Slider).BindingContext as ISliderModel;
        //    //    journal.CanDisplaySliderLabel = true;

        //    //    //TODO: Visual State not working properly.
        //    //    //VisualStateManager.GoToState(sender as Slider, "Dragged");

        //    //    //if (stoppableTimer != null)
        //    //    //{
        //    //    //    stoppableTimer.Stop();
        //    //    //}

        //    //    //if (stoppableTimer == null)
        //    //    //{
        //    //    //    stoppableTimer = new StoppableTimer(TimeSpan.FromMilliseconds(500), () =>
        //    //    //    {
        //    //    //journal.CanDisplaySliderLabel = false;
        //    //    //stoppableTimer = null;
        //    //    //TODO: Visual State not working properly.
        //    //    //VisualStateManager.GoToState(sender as Slider, "NotDragged");
        //    //    //    });
        //    //    //}
        //    //    //stoppableTimer.Start();
        //    //}
        //}

        //void SwapPage(object sender, SwipedEventArgs e)
        //{
        //    if (e.Direction == SwipeDirection.Up)
        //    {
        //        if (Page1.IsVisible == true)
        //        {
        //            ShowPage2(null, null);
        //        }
        //    }
        //    else if (e.Direction == SwipeDirection.Down)
        //    {
        //        if (Page2.IsVisible == true)
        //        {
        //            ShowPage1(null, null);
        //        }
        //    }
        //}

    }
}