using System;
using CoreAnimation;
using CoreGraphics;
using CoreText;
using Foundation;
using HappinessIndex.Controls;
using HappinessIndex.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Slider), typeof(SliderRendererExt))]
namespace HappinessIndex.iOS.Renderer
{
    public class SliderRendererExt : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ClipsToBounds = false;
                ClipsToBounds = false;
                Control.Transform = CGAffineTransform.MakeScale(0.6F, 0.6F);

                //Control.ValueChanged += Control_ValueChanged;
                //Control.TouchDown += Control_TouchDown;
                //Control.TouchUpInside += Control_TouchUpInside;
                //Control.TouchUpOutside += Control_TouchUpInside;
            }
            else if (e.OldElement != null && Control != null)
            {
                //Control.ValueChanged -= Control_ValueChanged;
                //Control.TouchDown -= Control_TouchDown;
                //Control.TouchUpInside -= Control_TouchUpInside;
                //Control.TouchUpOutside -= Control_TouchUpInside;
            }
        }

        bool touchDown = false;
        private void Control_TouchUpInside(object sender, EventArgs e)
        {
            touchDown = false;
        }

        private void Control_TouchDown(object sender, EventArgs e)
        {
            touchDown = true;
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            if (Element is SliderExt && (Element as SliderExt).ShowLabel && touchDown)
            {
                SetNeedsDisplay();
            }
        }

        public override void Draw(CGRect rect)
        {
            if (Element is SliderExt && (Element as SliderExt).ShowLabel && touchDown)
            {
                var value = Control.Value;
                var valueForText = Math.Round(value);
                var thumbRect = Control.ThumbRectForBounds(this.Bounds, Control.TrackRectForBounds(this.Bounds), value);

                var context = UIGraphics.GetCurrentContext();
                context.ScaleCTM(1, -1);

                var font = new CTFont("SanFrancisco", 13);

                string text;
                UIColor color;
                float offet = -0;

                if (valueForText == 10)
                {
                    text = App.GetString("Excellent");
                    offet = 25;
                    color = Color.FromHex("#8fc449").ToUIColor();
                }
                else if (valueForText > 6)
                {
                    text = App.GetString("Good");
                    color = Color.FromHex("#8fc449").ToUIColor();
                }
                else if (valueForText > 3)
                {
                    text = App.GetString("Fair");
                    color = Color.FromHex("#e07919").ToUIColor();
                }
                else
                {
                    text = App.GetString("Terrible");
                    color = UIColor.Red;
                }

                DrawText(context, text, -thumbRect.X + offet, -5, color.CGColor, font);
            }
        }

        void DrawText(CGContext context, string text, nfloat x, nfloat y, CGColor color, CTFont font)
        {
            context.TranslateCTM(-x, -(y + font.CapHeightMetric));
            context.SetFillColor(color);

            var attributedString = new NSAttributedString(text,
                new CTStringAttributes
                {
                    ForegroundColorFromContext = true,
                    Font = font
                });

            CGRect sizeOfText;
            using (var textLine = new CTLine(attributedString))
            {
                textLine.Draw(context);
                sizeOfText = textLine.GetBounds(CTLineBoundsOptions.UseOpticalBounds);
            }

            // Reset the origin back to where is was
            context.TranslateCTM(x - sizeOfText.Width, y + sizeOfText.Height);
        }
    }
}