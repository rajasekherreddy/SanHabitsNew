using System;
using System.Drawing;
using CoreGraphics;
using HappinessIndex.Controls;
using HappinessIndex.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BoxViewExt), typeof(BoxViewExtRenderer))]
namespace HappinessIndex.iOS.Renderers
{
    public class BoxViewExtRenderer : BoxRenderer
    {
        //this works fine on Forms nuger 4.3
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.BoxView> e)
        {
            base.OnElementChanged(e);
            var elem = this.Element;
            if (elem != null)
            {

                // Border
                this.Layer.CornerRadius = (float)elem.CornerRadius.TopLeft;
                this.Layer.Bounds.Inset(1, 1);
                Layer.BorderColor = Xamarin.Forms.Color.FromHex("#8fc449").ToCGColor();
                Layer.BorderWidth = 1;

                // Shadow
                this.Layer.ShadowColor = Xamarin.Forms.Color.FromHex("#8fc449").ToCGColor();
                this.Layer.ShadowOpacity = 0.2f;
                this.Layer.ShadowRadius = 3.0f;
                this.Layer.ShadowOffset = new SizeF(2, 2);
                //this.Layer.MasksToBounds = true;

            }
        }
    }
}