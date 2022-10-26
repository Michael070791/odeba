using System;
using System.Reactive;
using System.Threading.Tasks;
using odeba.Services.IServices;
using Xamarin.Essentials;

namespace odeba.Services
{
    public class LocationService : ILocationService
    {
        private readonly INavigationService _navigationService;
        public LocationService(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public bool Connected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public async Task<Location> GetCurrentLocation()
        {
            if ((await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>()) != PermissionStatus.Granted)
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>().ConfigureAwait(false);

                if (status != PermissionStatus.Granted)
                {
                    return null;
                }
            }

            if (!Connected)
            {
                return null;
            }

            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                if (location == null)
                {
                    var lastLocation = await Geolocation.GetLastKnownLocationAsync();
                    if (lastLocation == null)
                    {
                        return null;
                    }

                    return lastLocation;

                }

                return location;

            }
            catch (FeatureNotEnabledException)
            {
                await _navigationService.DisplayErrorAsync("Error mesasge!","ODEBA may require GPS Enabled for better processing","Ok");
            }
            return null;
        }
    }
}

