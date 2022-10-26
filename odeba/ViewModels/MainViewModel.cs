using System;
using System.Reactive.Disposables;
using System.Windows.Input;
using odeba.Services.IServices;
using ReactiveUI;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace odeba.ViewModels
{
    class MainViewModel : ReactiveObject, INavigatableViewModel
    {
        private int selectedViewIndex = 0;
        private bool _isLoggedIn;
        private bool _shouldLogin;
        private bool _canRecycle;
        private ObservableAsPropertyHelper<Location> _location;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public bool CanRecycle
        {
            get => _canRecycle;
            set 
            {
                this.RaiseAndSetIfChanged(ref _canRecycle, value);
            }
        }
        public int SelectedViewIndex
        {
            get => selectedViewIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedViewIndex, value);
                //if (value == 2)
                //    Favorite.Load(null);
            }
        }
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                this.RaiseAndSetIfChanged(ref _isLoggedIn, value);
            }
        }
        public bool ShouldLogin
        {
            get => _shouldLogin;
            set
            {
                this.RaiseAndSetIfChanged(ref _shouldLogin, value);
            }
        }
        public INavigationService Navigation { get; }
        public ProfileViewModel ProfileVM { get; }
        public RecycleViewModel RecycleVM { get; }
        public LoginViewModel LoginVM { get; }
        public HomeViewModel HomeVM { get; }
        public HistoryViewModel History { get; }
        public MainViewModel(ProfileViewModel profileViewModel,
                             RecycleViewModel recycleViewModel,
                             LoginViewModel loginViewModel,
                             HomeViewModel homeViewModel, 
                             INavigationService navigation,
                             HistoryViewModel history)
        {
            ProfileVM = profileViewModel ?? throw new ArgumentNullException(nameof(profileViewModel));
            RecycleVM = recycleViewModel ?? throw new ArgumentNullException(nameof(recycleViewModel));
            LoginVM = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));
            HomeVM = homeViewModel ?? throw new ArgumentNullException(nameof(homeViewModel));
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            History = history ?? throw new ArgumentNullException(nameof(history));

            LoginCommand = new AsyncCommand(async () =>
            {
                await Navigation.PushModalAsync(new Pages.LoginPage(), true);
            });
            SignOutCommand = new Command(() =>
            {
                MainThread.BeginInvokeOnMainThread(() => SelectedViewIndex = 0);
            });
            GotoRecycle = new Command(() =>
            {
                MainThread.BeginInvokeOnMainThread(() => SelectedViewIndex = 2);
            });
            ProfileCommand = new Command(() =>
            {
                MainThread.BeginInvokeOnMainThread(() => SelectedViewIndex = 3);
            });
        }
        //commands
        public ICommand LoginCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand GotoRecycle { get; }

        public void Load(object parameter)
        {
            //Posts.Load(parameter);
            RecycleVM.Load(parameter);
            ProfileVM.Load(parameter);
        }
    }
}

