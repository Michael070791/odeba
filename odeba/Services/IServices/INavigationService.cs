using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace odeba.Services.IServices
{
    public interface INavigationService
    {
        Task ConfirmAsync(Action<DialogResult> callback, string title = null, string message = null, string yesText = "Yes", string noText = "No", bool displayNoButton = true);
        Task DisplayErrorAsync(string title = "Error", string message = "An unexpected error has occured", string yesText = "Ok");
        Task PushAsync(Page page, bool animated = true);
        Task PushModalAsync(Page modalPage, bool animated = true);
        Task PopModalAsync(bool animated = true);
        Task PushAsyncWithSharedTransition(Page page, string groupId);
        Task DisplaySnackbar(string message);
        Task PopAsync();
    }
    public enum DialogResult
    {
        Yes = 1,
        No = 2,
        Unknown = -1
    }
}

