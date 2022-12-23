using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Models;
using HappinessIndex.ViewModels;

namespace HappinessIndex.Views
{
    
    public partial class MicroWorkoutTimerPage
    {
        public EventHandler<EventArgs> SaveClicked;

        public EventHandler<EventArgs> TaskCompleted;
        private MicroWorkout microWorkout;

        MircoWorkoutListViewModel MircoWorkoutListViewModel;
       
        public MicroWorkoutTimerPage()
        {
            InitializeComponent();
        }

        public MicroWorkoutTimerPage(MicroWorkout microWorkout)
        {
            InitializeComponent();
            this.microWorkout = microWorkout;

            MircoWorkoutListViewModel = new MircoWorkoutListViewModel();
            this.BindingContext = MircoWorkoutListViewModel;

        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();

        }
        void SaveDetails(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();
            TaskCompleted?.Invoke(this, new EventArgs());

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MircoWorkoutListViewModel.MicroWorkout = microWorkout;
        }
    }
}

