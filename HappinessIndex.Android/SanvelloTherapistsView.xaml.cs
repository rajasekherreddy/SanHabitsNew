using HappinessIndex.ViewModels;
using Newtonsoft.Json;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappinessIndex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SanvelloTherapistsView : ContentPage
    {
        public SanvelloTherapistsView()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var providers = JsonConvert.SerializeObject(listView.SelectedItems);
            var viewModel = (SanvelloTherapistsViewModel)BindingContext;
            viewModel.providerSave(providers);
        }

    }
}