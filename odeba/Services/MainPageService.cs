using System;
using odeba.Contracts;
using Plugin.SharedTransitions;
using Xamarin.Forms;

namespace odeba.Services
{
    public class MainPageService : IMainPageService
    {
        readonly Lazy<SharedTransitionNavigationPage> _navigationPage = new Lazy<SharedTransitionNavigationPage>(() =>
        new SharedTransitionNavigationPage(new MainPage()));
        public Page GetMainPage() => _navigationPage.Value;
    }
}

