using System;
using Akavache;
using System.ComponentModel.Design;
using System.Net.Http;
using Akavache.Sqlite3;
using Xamarin.Essentials;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using static odeba.Startup;
using SQLitePCL;
using odeba.Pages;
using odeba.ViewModels;
using odeba.Services.IServices;
using odeba.Services;
using Prism.Forms.LazyView;
using odeba.Contracts;
using odeba.Views;
using odeba.Services.Auth;
using Firebase.Database;
using Firebase.Database.Offline;
using odeba.Services.Database;

[assembly: ExportFont("Font Awesome 5 Free-Solid-900.otf", Alias = "fas")]
[assembly: ExportFont("Font Awesome 5 Free-Regular-400.otf", Alias = "far")]
[assembly: ExportFont("OpenSansBold.ttf", Alias = "FontBold")]
[assembly: ExportFont("OpenSans-ExtraBold.ttf", Alias = "FontExtraBold")]
[assembly: ExportFont("OpenSans-BoldItalic.ttf", Alias = "FontBoldItalic")]
[assembly: ExportFont("OpenSans-Italic.ttf", Alias = "FontItalic")]
[assembly: ExportFont("OpenSans-Light.ttf", Alias = "FontLight")]
[assembly: ExportFont("OpenSans-Regular.ttf", Alias = "FontRegular")]
[assembly: ExportFont("OpenSans-SemiBold.ttf", Alias = "FontSemiBold")]

namespace odeba
{
    [Preserve(AllMembers = true)]
    public static class Startup
    {
        public static IServiceProvider ServiceProvider;

        public delegate void ConfigureServices(IServiceCollection services);
        public static void Init(ConfigureServices configureServices = null)
        {
            var services = new ServiceCollection();

            services.AddSingleton(Akavache.BlobCache.UserAccount);

            services.AddSingleton(CrossFirebaseAuth.Current);
            services.AddSingleton(CrossFirebaseAuth.Current.Instance);
            services.AddSingleton(CrossCloudFirestore.Current);
           
            services.AddScoped(provider =>
            {
                var settings = provider.GetRequiredService<IProjectSettings>();
                var auth = provider.GetRequiredService<IFirebaseAuth>();
                return new Firebase.Storage.FirebaseStorage(settings.FirebaseStorageBucket,
                    new Firebase.Storage.FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () =>
                        {
                            var currentUser = auth.Instance.CurrentUser;
                            if (currentUser != null)
                            {
                                return await currentUser.GetIdTokenAsync(false);
                            }

                            return null;
                        }
                    });
            });

            services.AddScoped(provider =>
            {
                var settings = provider.GetRequiredService<IProjectSettings>();
                var auth = provider.GetRequiredService<IFirebaseAuth>();
                var config = new FirebaseClient(settings.FirebaseClientBaseUrl, new FirebaseOptions
                {
                    OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
                    AuthTokenAsyncFactory = async () =>
                    {
                        var currentUser = auth.Instance.CurrentUser;
                        if (currentUser != null)
                        {
                            return await currentUser.GetIdTokenAsync(false);
                        }

                        return null;
                    }
                });
                return config;
            });

            services.AddSingleton<Database>();
            services.AddSingleton<IMainPageService, MainPageService>();
            services.AddSingleton<App>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IUserService, UserService>();
            //services.AddSingleton<IPreferenceService, PreferenceService>();

            //pages
            services.AddSingleton<RegisterPage>();
            services.AddSingleton<LoginPage>();
            services.AddSingleton<RecyclePage>();
            services.AddSingleton<ProfilePage>();
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<ProfileViewModel>();
            services.AddSingleton<RecycleViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<HistoryViewModel>();



#if !DEBUG

            AppCenter.Start("android=2796dbc0-f962-4568-ac58-a5fe0eab57a6;" /*+
                  "uwp={Your UWP App secret here};" +
                  "ios=0dafe1cb-6e2c-4dfc-9742-09e39d1a917d"*/,
                   typeof(Crashes), typeof(Analytics), typeof(Distribute));
#endif

            Xamarin.Forms.Device.SetFlags(new[] {
                "CollectionView_Experimental",
                "Shapes_Experimental",
                "CarouselView_Experimental",
                "Expander_Experimental",
                "FastRenderers_Experimental",
                "SwipeView_Experimental",
                "Markup_Experimental"
            });
            RegisterRoutes();
            configureServices?.Invoke(services);

            ServiceProvider = services.BuildServiceProvider();
            BlobCache.ApplicationName = "odeba";
            Akavache.Registrations.Start("odeba");
            services.AddSingleton((s) => BlobCache.LocalMachine);
            services.AddSingleton((s) => MessagingCenter.Instance);
        }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }
        public static T GetService<T>() => ServiceProvider.GetRequiredService<T>();
    }
    public static class LinkerPreserve
    {
        static LinkerPreserve()
        {
            var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
            var suName = typeof(SQLitePersistentBlobCache).FullName;
        }
    }
}

