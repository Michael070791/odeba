using System;
using Akavache.Sqlite3.Internal;
using odeba.Services.IServices;
using System.Threading.Tasks;
using MvvmHelpers;
using Microsoft.AppCenter.Crashes;

namespace odeba.ViewModels
{
    
    public class ViewModelBase : BaseViewModel, INavigatableViewModel
    {

        
        public ViewModelBase()
        {
        }

        public void Load(object parameter)
        {
            parameter = null;
        }

        internal async Task<bool> GuardOnline(Func<Task> action, bool silence = false)
        {
            var location = Startup.GetService<ILocationService>();
            if (!location.Connected)
            {
                if (silence) return false;
                IsBusy = false;
                await Startup.GetService<INavigationService>().DisplaySnackbar("No internet connection!");
                return false;
            }

            return await InternalTryCatch(action, silence);
        }

        protected async Task<bool> GuardOnline(Action action, bool silence = false)
        {
            var location = Startup.GetService<ILocationService>();
            if (!location.Connected)
            {
                if (silence)
                    return false;
                IsBusy = false;
                await Startup.GetService<INavigationService>().DisplaySnackbar("No internet connection!");
                return false;
            }

            return await InternalTryCatch(() =>
            {
                action.Invoke();
                return Task.CompletedTask;
            }, silence);
        }
        protected async Task TryCatch(Func<Task> action)
        {
            _ = await InternalTryCatch(action);
        }

        private async Task<bool> InternalTryCatch(Func<Task> action, bool silent = false)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                IsBusy = false;
                if (silent) return false;
                await Startup.GetService<INavigationService>().DisplaySnackbar(ex.Message);
            }

            return false;
        }
    }
   
}

