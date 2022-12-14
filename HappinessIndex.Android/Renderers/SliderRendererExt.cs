using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using HappinessIndex.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Slider), typeof(SliderRendererExt))]
namespace HappinessIndex.Droid.Renderers
{
    public class SliderRendererExt : SliderRenderer
    {
        public SliderRendererExt(Context context)
            : base(context)
        {

        }

        //protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        //{
        //    base.OnElementChanged(e);

        //    if (Control != null)
        //    {
        //        TextView textView = new TextView(this.Context);
        //        textView.Text = "Hello";
        //        textView.SetX(50);
        //        textView.SetY(10);
        //        AddView(textView);

        //        Control.ProgressChanged += Control_ProgressChanged;
        //    }
        //}

        //private void Control_ProgressChanged(object sender, Android.Widget.SeekBar.ProgressChangedEventArgs e)
        //{
        //    Console.WriteLine("Hello");
        //}
    }
}