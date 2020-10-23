using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FFImageLoading.Forms.Platform;
using Prism;
using Prism.Ioc;
using Sodexo_JTH.Helpers;

namespace Sodexo_JTH.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/Applogo", Theme = "@style/splashscreen", 
        LaunchMode = LaunchMode.SingleTop,
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.Window.RequestFeature(WindowFeatures.ActionBar);

            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle); // add this line to your code, it may also be called: bundle
            Rg.Plugins.Popup.Popup.Init(this, bundle);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async override void OnUserInteraction()
        {
            base.OnUserInteraction();

            if (SessionManager.Instance.IsSessionActive)
            {
                SessionManager.Instance.EndTrackSession();
                await SessionManager.Instance.StartTrackSessionAsync();
            }
        }

        
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

