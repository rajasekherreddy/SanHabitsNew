using System;
using HappinessIndex.Controls;
using HappinessIndex.iOS.Renderers;
using Syncfusion.SfAutoComplete.XForms;
using Syncfusion.SfAutoComplete.XForms.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SfAutoCompleteExt), typeof(SfAutoCompleteRendererExt))]
namespace HappinessIndex.iOS.Renderers
{
    public class SfAutoCompleteRendererExt : SfAutoCompleteRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SfAutoComplete> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Xamarin.Forms.Application.Current.Resources.TryGetValue("TransparentButtonText", out object backgroundColor);
                Control.TintColor = ((Color)backgroundColor).ToUIColor();
            }
        }
    }
}