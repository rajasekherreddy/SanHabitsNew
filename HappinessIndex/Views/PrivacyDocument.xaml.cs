using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class PrivacyDocument : ContentPage
    {
        public PrivacyDocument()
        {
            InitializeComponent();

            var privacyStream = typeof(PrivacyDocument).GetTypeInfo().Assembly.GetManifestResourceStream("HappinessIndex.PrivacyDocument.Privacy.pdf");

            //documentViwer.InputFileStream = privacyStream;
        }

        //void documentViwer_PageChanged(System.Object sender, Syncfusion.SfPdfViewer.XForms.PageChangedEventArgs args)
        //{
        //    if (args.NewPageNumber == 10)
        //    {
        //        AcceptButton.IsVisible = true;
        //    }
        //}
    }
}