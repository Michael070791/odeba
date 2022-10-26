using System;
using Microsoft.AppCenter.Distribute;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Sharpnado.MaterialFrame.Droid;
using Sharpnado.Presentation.Forms.Droid;
using FFImageLoading.Forms.Platform;

namespace odeba.Droid
{
    [Activity(Label = "odeba", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
           ServicePointManager
          .ServerCertificateValidationCallback +=
           (sender, cert, chain, sslPolicyErrors) => true;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Distribute.SetEnabledForDebuggableBuild(false);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //GoogleVisionBarCodeScanner.Droid.RendererInitializer.Init();

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            SharpnadoInitializer.Initialize(enableInternalLogger: true, enableInternalDebugLogger: true);
            AndroidMaterialFrameRenderer.ThrowStopExceptionOnDraw = false;
            AndroidMaterialFrameRenderer.BlurAutoUpdateDelayMilliseconds = 200;
            AndroidMaterialFrameRenderer.BlurProcessingDelayMilliseconds = 100;

            Startup.Init(RegisterServices);
            LoadApplication(new App());
        }
        private void RegisterServices(IServiceCollection services)
        {
            CachedImageRenderer.Init(true);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
