using System;
using HappinessIndex.Resx;
using Rg.Plugins.Popup.Services;

namespace HappinessIndex.Views.Popup
{
    public partial class CommonMessage
    {
        public event EventHandler<DialogActionEventArgs> ActionClicked;

        public CommonMessage()
        {
            InitializeComponent();
        }

        public CommonMessage(string title, string message, string option1, string option2 = "")
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(title))
            {
                title = AppResources.AppInfo;
            }

            if (!string.IsNullOrEmpty(option2))
            {
                Option2.Text = option2;
            }
            else
            {
                Option2.IsVisible = false;
            }

            TitleXt.Text = title;
            Content.Text = message;
            Option1.Text = option1;
        }

        void Opetion1Clicked(object sender, System.EventArgs e)
        {
            ActionClicked?.Invoke(this, new DialogActionEventArgs(true));

            CloseAllPopup();
        }

        void Opetion2Clicked(System.Object sender, System.EventArgs e)
        {
            ActionClicked?.Invoke(this, new DialogActionEventArgs(false));

            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }

    public class DialogActionEventArgs : EventArgs
    {
        public bool IsPrimary { get; set; }

        public DialogActionEventArgs(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }
    }
}