using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading;
using FFImageLoading.Forms;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Transformations;
using Foundation;
using LibraryEverywhere.Controls;
using Plugin.Media;
using Plugin.Toasts;
using UIKit;
using Xamarin.Forms;

namespace LibraryEverywhere.iOS
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
            FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
            CrossMedia.Current.Initialize();
            var ignore = new CircleTransformation();
            var ignore4 = new BlurredTransformation();
            var ignore5 = new CachedImage();
            var ignore2 = new CarouselView();
            var ignore3 = new CarouselIndicators();
            var ignore6 = new FFImageLoading.Forms.ImageSourceConverter();
            var ignore7 = new LoginView();
            global::Xamarin.Forms.Forms.Init();
            

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                
            };

            ImageService.Instance.Initialize(config);

            DependencyService.Register<ToastNotificatorImplementation>();
            ToastNotificatorImplementation.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
