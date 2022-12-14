using System;
using HappinessIndex.Controls;
using HappinessIndex.iOS.Renderers;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.MaskedEdit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SfMaskedEditExt), typeof(SfMaskedEditRendererExt))]
namespace HappinessIndex.iOS.Renderers
{
    public class SfMaskedEditRendererExt : SfMaskedEditRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SfMaskedEdit> e)
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