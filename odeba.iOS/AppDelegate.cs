using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Firebase.Auth;
using Foundation;
using Google.SignIn;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using odeba.iOS.Settings;
using odeba.Services.IServices;
using Sharpnado.Presentation.Forms.iOS;
using UIKit;
using Xamarin;
using Xamarin.Forms;

namespace odeba.iOS
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
            Firebase.Core.App.Configure();
            global::Xamarin.Forms.Forms.Init();
            //Xamarin.FormsGoogleMaps.Init("AIzaSyBrWwDljiSe0L0BOOMNnGAkHYP3ci-e22M");
            Sharpnado.Shades.iOS.iOSShadowsRenderer.Initialize();
            FormsMaps.Init();
            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();
            
            Sharpnado.Tabs.iOS.Preserver.Preserve();
            SharpnadoInitializer.Initialize();
            IQKeyboardManager.SharedManager.Enable = true;
            Startup.Init(services =>
            {
                services.AddSingleton(Firebase.Core.Options.DefaultInstance);
                //services.AddSingleton<IFirebaseCloudFunctions, FirebaseCloudFunctions>();
                //services.AddScoped<IGetDeviceInfo, GetDevice>();
                //services.AddScoped<ICrossAutofillManager, AppleAutofillManager>();
                //services.AddScoped<IImageRotator, ImageRotator>();
                //services.AddScoped<IAppleSignInService, AppleSignInService>();
                //services.AddScoped<IGoogleSignInService, GoogleSignInService>();
                //services.AddScoped<IFacebookSignInService, FacebookSignInService>();
                //services.AddScoped<IFirebaseAnalyticsService, FirebaseAnalyticsService>();
                services.AddScoped<IProjectSettings, ProjectSettings>();
            });
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var urlString = url?.ToString() ?? string.Empty;

            //if (urlString.StartsWith("fb"))
            //    return Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(app, url, options);

            //if (Xamarin.Forms.Application.Current is App application &&
            //    !string.IsNullOrEmpty(urlString) &&
            //    application.AppReceivedWorkaround(new Uri(urlString)))
            //    return true;

            return Auth.DefaultInstance.CanHandleUrl(url) || SignIn.SharedInstance.HandleUrl(url);
        }
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication,
           NSObject annotation)
        {
            return OpenUrl(application, url, (NSDictionary)null);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity,
        UIApplicationRestorationHandler completionHandler)
        {
            //if (userActivity.ActivityType == NSUserActivityType.BrowsingWeb)
            //{
            //    var url = userActivity.WebPageUrl?.ToString();
            //    if (Xamarin.Forms.Application.Current is App app && !string.IsNullOrEmpty(url) &&
            //        app.AppReceivedWorkaround(new Uri(url)))
            //    {
            //        return false;
            //    }
            //}

            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
#if !DEBUG
            Firebase.Auth.Auth.DefaultInstance.SetApnsToken(deviceToken, Firebase.Auth.AuthApnsTokenType.Production);
#else
            Firebase.Auth.Auth.DefaultInstance.SetApnsToken(deviceToken, Firebase.Auth.AuthApnsTokenType.Sandbox);
#endif
           // FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        //public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        //{
        //    FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        //}

        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.
            if (Firebase.Auth.Auth.DefaultInstance.CanHandleNotification(userInfo))
            {
                completionHandler(UIBackgroundFetchResult.NewData);
                return;
            }

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            //try
            //{
            //    FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            //}
            //catch (Exception ex)
            //{
            //    Crashes.TrackError(ex);
            //}
            // Do your magic to handle the notification data

            completionHandler(UIBackgroundFetchResult.NewData);
        }

    }
}

