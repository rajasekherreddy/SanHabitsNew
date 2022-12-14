using HappinessIndex.Resx;
using Rg.Plugins.Popup.Services;

namespace HappinessIndex.Views.Popup
{
    public partial class EnterNotes
    {
        public EnterNotes(string name)
        {
            InitializeComponent();
            TitleXt.Text = $"{AppResources.NotesFor}{name}";
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();
        }
    }
}