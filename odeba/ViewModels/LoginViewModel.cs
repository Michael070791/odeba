using System;
using System.Windows.Input;
using MvvmHelpers;
using odeba.Services.Auth;
using odeba.Services.IServices;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Rules;
using Sharpnado.TaskLoaderView;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace odeba.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public Validatable<string> Code { get; set; } = new Validatable<string>(new NotEmptyRule<string>(string.Empty));
        public Validatable<string> PhoneNumber { get; set; } = new Validatable<string>(new NotEmptyRule<string>(string.Empty));
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;
        private string verificationId;

        public LoginViewModel(INavigationService navigation,
                              ILocationService locationService,
                              IUserService userService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            CloseCommand = new AsyncCommand(async () =>
            {
                await Navigation.PopModalAsync(true);
            });
            RegisterCommand = new AsyncCommand(async () =>
            {
                await Navigation.PushModalAsync(new Pages.RegisterPage(), true);
            });
            LoginCommand = new AsyncCommand(async () =>
            {
                if (string.IsNullOrEmpty(verificationId))
                    return;
                var result = await _userService.SignInByPhone(verificationId, Code.Value);
                if (!result)
                    await Navigation.DisplayErrorAsync("Error","Opps! unsuccessful, Try again!","Ok");

                App.Login();
            });
            SendSMSCode = new AsyncCommand(async () =>
            {
                await GuardOnline(async () =>
                {
                    if (!PhoneNumber.Validate()) return;
                    verificationId = await _userService.VerifyPhoneNumber("+233" + PhoneNumber.Value);
                    if (verificationId == null)
                        await Navigation.DisplayErrorAsync("Error", "Could not verify phone number try again", "Ok");
                    await Navigation.DisplaySnackbar("SMS with code sent to your phone");
                });
            });
        }
        public INavigationService Navigation { get; }

        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand SendSMSCode { get; }
    }
}

