using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Syncfusion.SfAutoComplete.XForms.iOS;
using Syncfusion.SfCarousel.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfRadialMenu.XForms.iOS;
using Syncfusion.SfRangeSlider.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.iOS;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.EffectsView;

namespace HappinessIndex.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Plugin.LocalNotification.NotificationCenter.AskPermission();

            Forms.SetFlags("IndicatorView_Experimental", "Expander_Experimental");

            Forms.Init();
            SfTextInputLayoutRenderer.Init();
            SfMaskedEditRenderer.Init();
            SfAutoCompleteRenderer.Init();
            SfRangeSliderRenderer.Init();
            //SfPdfDocumentViewRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfRadialMenuRenderer.Init();
            SfChartRenderer.Init();
            ImageCircleRenderer.Init();
            SfCheckBoxRenderer.Init();
            CachedImageRenderer.Init();
            SfCarouselRenderer.Init();
            SfListViewRenderer.Init();
            
            Rg.Plugins.Popup.Popup.Init();

            Syncfusion.XForms.iOS.Core.SfAvatarViewRenderer.Init();

            //CachedImageRenderer.InitImageSourceHandler();

            //Force the app to delay to load with splash screen.
            //Thread.Sleep(1000);
            LoadApplication(new App());
            Syncfusion.XForms.iOS.Buttons.SfRadioButtonRenderer.Init();
            FacebookClientManager.Initialize(app, options);
            GoogleClientManager.Initialize();

            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            FacebookClientManager.OnActivated();
            base.OnActivated(uiApplication);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if(App.LoginType == "Google")
            {
                return GoogleClientManager.OnOpenUrl(app, url, options);
            }
            
            return FacebookClientManager.OpenUrl(app, url, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }

       
    }
}
