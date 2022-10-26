using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Akavache.Sqlite3.Internal;
using MvvmHelpers.Commands;
using odeba.Services.Auth;
using odeba.Services.IServices;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Rules;
using Xamarin.Forms;

namespace odeba.ViewModels
{
    public class RecycleViewModel : ViewModelBase
    {
        private bool _canContinue = false;
        private int _step;
        private bool _isValidEntry = true;
        private string _phoneNumber;
        private string _code;
        private string _selectedCategory;
        private String _selectedMachineUID;



        public string SelectedMachineUID
        {
            get => _selectedMachineUID;
            set => SetProperty(ref _selectedMachineUID, value);
        }
        public string SelcetedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, onChanged:() =>
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length == 11)
                    CanContinue = true;
                else _canContinue = false;
            });
        }
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value, onChanged: () =>
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length == 7)
                    CanContinue = true;
                else _canContinue = false;
            });
        }
        public bool IsValidEntry { get => _isValidEntry; set => SetProperty(ref _isValidEntry, value); }
        public int Step { get => _step; set => SetProperty(ref _step, value); }
        public bool CanContinue
        {
            get => _canContinue;
            set => SetProperty(ref _canContinue, value);
        }
        //public Validatable<string> Code { get; set; } = new Validatable<string>(new NotEmptyRule<string>(string.Empty));
        //public Validatable<string> PhoneNumber { get; set; } = new Validatable<string>(new NotEmptyRule<string>(string.Empty));
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;


        public RecycleViewModel(IUserService userService,
                                ILocationService locationService,
                                INavigationService navigation)
                        
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            ContinueCommand = new AsyncCommand(async () =>
            {
                if ( Step >= 1 && _canContinue)
                {
                    Step++;
                    //await Shell.Current.Navigation.PushModalAsync(Forms.Get<Pages.AddItemsPage>());
                }
                else
                {
                    await Navigation.DisplayErrorAsync();
                    return;
                }
            });
            Nextcommand = new AsyncCommand( async () =>
            {
                if (!_canContinue)
                {
                    await Navigation.DisplayErrorAsync();
                    return;
                }
                if (Step > 1)
                {
                    Step = 0;
                }
                else
                {
                    Step++;
                    _canContinue = false;
                }
            });
            BackCommmand = new Xamarin.Forms.Command(() =>
            {
                if(Step > 0)
                {
                    --Step;
                }
                else
                {
                    Step = 0;
                }
            });
        }

        public ICommand Nextcommand { get; }
        public ICommand ContinueCommand { get; }
        public ICommand BackCommmand { get; }

        public INavigationService Navigation { get; }



        public List<string> MachineUIDs { get; set; } = new List<string>
        {
            "RRS-INSTI-001-2022",
            "RRS-INSTI-002-2022",
            "RRS-INSTI-003-2022",
            "RRS-INSTI-004-2022"

        };
        public List<string> Categories { get; set; } = new List<string>
        {
            "Plastic Bottle",
            "Glass Bottle"
        };
    }
}

