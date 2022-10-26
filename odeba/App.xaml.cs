using System;
using Akavache.Sqlite3.Internal;
using odeba.Contracts;
using odeba.ViewModels;
using Plugin.SharedTransitions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace odeba
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            Sharpnado.Tabs.Initializer.Initialize(false, false);
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            MainPage = Startup.ServiceProvider.GetService<IMainPageService>().GetMainPage();
            if (MainPage is SharedTransitionNavigationPage nav)
                if (nav.CurrentPage.BindingContext is INavigatableViewModel vm)
                {
                    vm.Load(null);
                }
        }
        public static void Login()
        {
            if (Current.MainPage is SharedTransitionNavigationPage nav)
                if (nav.CurrentPage.BindingContext is INavigatableViewModel vm)
                {
                    vm.Load(null);
                }
            // if (Current.MainPage is AppShell) return;
            //Forms.Get<ICrossAutofillManager>()?.Commit();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

