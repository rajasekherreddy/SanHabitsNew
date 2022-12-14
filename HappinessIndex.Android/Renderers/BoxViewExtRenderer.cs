using Android.Content;
using Android.Graphics.Drawables;
using HappinessIndex.Controls;
using HappinessIndex.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BoxViewExt), typeof(BoxViewExtRenderer))]
namespace HappinessIndex.Droid.Renderers
{
    public class BoxViewExtRenderer : BoxRenderer
    {
        public BoxViewExtRenderer(Context context)
            : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);
            
            var elem = this.Element;
            if (elem != null)
            {
                GradientDrawable border = new GradientDrawable();
                border.SetCornerRadius((float)Element.CornerRadius.TopLeft * Resources.DisplayMetrics.Density);
                border.SetStroke(2, Color.FromHex("#8fc449").ToAndroid());
                SetBackgroundDrawable(border);
            }
        }
    }
}