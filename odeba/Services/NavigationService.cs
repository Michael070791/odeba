using System;
using System.Linq;
using System.Threading.Tasks;
using odeba.Services.IServices;
using odeba.Views.Dialogs;
using Plugin.SharedTransitions;
using Prism.Navigation.Xaml;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace odeba.Services
{
    public class NavigationService : INavigationService
    {
        private INavigation Navigation => Application.Current.MainPage.Navigation;

        public Task ConfirmAsync(Action<DialogResult> callback, string title = null, string message = null, string yesText = "Yes", string noText = "No", bool displayNoButton = true)
        {
            var modal = new ResponseDialog
            {
                Title = title ?? "Warning",
                DisplayText = message ?? "Are you sure?",
                Callback = callback,
                DisplayNoButton = displayNoButton,
                YesText = yesText ?? "Yes",
                NoText = noText ?? "No"
            };
            return PushModalAsync(modal);
        }

        public Task DisplayErrorAsync(string title = "Error", string message = "An error has occured", string yesText = "Ok")
        {
            return ConfirmAsync(null, title, message, yesText, displayNoButton: false);
        }

        //snackbar
        public Task DisplaySnackbar(string message)
        {
            var messageOptions = new MessageOptions
            {
                Font = Font.OfSize("Regular", NamedSize.Small),
                Message = message,
                Foreground = Color.White,
                Padding = 2
            };
            var snackOptions = new SnackBarOptions
            {
                MessageOptions = messageOptions,
                BackgroundColor = Color.DarkGray,
                CornerRadius = 5,
                IsRtl = true
            };
            var page = GetTopMostPage();
            return page.DisplaySnackBarAsync(snackOptions);
        }

        public Task PopAsync()
        {
            return Navigation.PopAsync();
        }

        public Task PopModalAsync(bool animated = true)
        {
            return Navigation.PopModalAsync(animated);
        }

        public Task PushAsync(Page page, bool animated = true)
        {
            return Navigation.PushAsync(page, animated);
        }

        public Task PushAsyncWithSharedTransition(Page page, string groupId)
        {
            Page currentPage = (Application.Current.MainPage as SharedTransitionNavigationPage).CurrentPage;
            SharedTransitionNavigationPage.SetTransitionDuration(currentPage, 300);
            SharedTransitionNavigationPage.SetTransitionSelectedGroup(currentPage, groupId);
            return PushAsync(page);
        }

        public Task PushModalAsync(Page modalPage, bool animated = true)
        {
            return Navigation.PushModalAsync(modalPage, animated);
        }
        private Page GetTopMostPage()
        {
            return Navigation.ModalStack.Any() ?
                Navigation.ModalStack.Last() :
                Navigation.NavigationStack.LastOrDefault() ?? Application.Current.MainPage;
        }
    }
}

