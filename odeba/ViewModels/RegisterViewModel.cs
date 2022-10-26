using System;
using System.Reactive;
using System.Windows.Input;
using MvvmHelpers;
using odeba.Services.Auth;
using odeba.Services.IServices;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Rules;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace odeba.ViewModels
{
    public class RegisterViewModel: BaseViewModel
    {
        private string _phoneNumber;
        private string _smsCode;
        private bool _isPhoneValid = false;
        private bool _displayError = false;
        private string verificationId;

        public bool DisplayError
        {
            get => _displayError;
            set => SetProperty(ref _displayError, value);
        }
        public bool IsPhoneValide
        {
            get => _isPhoneValid;
            set => SetProperty(ref _isPhoneValid, value, onChanged: () =>
            {
                _displayError = value;
            });
        }
        public Validatable<string> Code { get; set; } = new Validatable<string>(new NotEmptyRule<string>(string.Empty));
        public Validatable<string> PhoneNumber { get; set; } = new Validatable<string>(new NotEmptyRule<string>(string.Empty));
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;

        public string VerificationId { get; set; }
        public RegisterViewModel(INavigationService navigation,
                                IUserService userService,
                                ILocationService locationService)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));

            CloseCommand = new AsyncCommand(async () =>
            {
                await Navigation.PopModalAsync(true);
            });
            RegisterCommand = new AsyncCommand( async () =>
            {
                if (PhoneNumber.Validate() && Code.Validate())
                {
                    var result = await _userService.SignInByPhone(verificationId, Code.Value);
                    if (result)
                    {
                        await Navigation.DisplayErrorAsync("Success", "Successful", "Ok");
                        //App.Login();
                    }
                    else
                    {
                        await Navigation.DisplayErrorAsync("Error", "Could not verify phone number try again", "Ok");
                    }
                }
            });
            SendSmsCommand = new AsyncCommand(async () =>
            {
                if (!PhoneNumber.Validate()) return;
                IsBusy = true;
                verificationId = await _userService.VerifyPhoneNumber("+233" + PhoneNumber.Value);
                if (verificationId == null)
                    await Navigation.DisplayErrorAsync("Error", "Could not verify phone number try again", "Ok");
                IsBusy = false;
            });
        }
        public INavigationService Navigation { get; }

        public ICommand SendSmsCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }
    }
}

