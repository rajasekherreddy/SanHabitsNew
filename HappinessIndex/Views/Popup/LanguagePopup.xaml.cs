using System;
using System.Collections.Generic;
using System.Globalization;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace HappinessIndex.Views.Popup
{
    public partial class LanguagePopup
    {
        public EventHandler<EventArgs> SaveClicked;

        internal List<String> SelectedItems { get; set; }

        public LanguagePopup(string[] items, string[] selectedItems)
        {
            InitializeComponent();
            
            SelectedItems = new List<string>(selectedItems);
            ItemsView.BindingContext = SelectedItems;
            ItemsView.ItemsSource = new List<string>(items);
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(object sender, System.EventArgs e)
        {
            CloseAllPopup();
            if (SelectedItems.Contains(""))
            {
                SelectedItems.Remove("");
            }
            SaveClicked?.Invoke(string.Join(",", SelectedItems), new EventArgs());
        }

        void SfCheckBox_StateChanged(System.Object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            SfCheckBox checkBox = sender as SfCheckBox;
            var text = checkBox.Text;

            if (checkBox.IsChecked.Value && !SelectedItems.Contains(text))
            {
                SelectedItems.Add(text);
            }
            else if (!checkBox.IsChecked.Value && SelectedItems.Contains(text))
            {
                SelectedItems.Remove(text);
            }
        }
    }

    public class StringToLanguageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                var currentItem = values[0].ToString();
                var source = values[1] as List<string>;

                if (source.Contains(currentItem))
                {
                    return true;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}