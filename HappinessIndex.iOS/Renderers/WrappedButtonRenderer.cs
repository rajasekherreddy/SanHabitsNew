using System;
using HappinessIndex.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(WrappedButtonRenderer))]
namespace HappinessIndex.iOS.Renderers
{
    public class WrappedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.TitleEdgeInsets = new UIEdgeInsets(4, 4, 4, 4);
                Control.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                Control.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}