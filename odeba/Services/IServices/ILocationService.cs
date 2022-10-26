using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace odeba.Services.IServices
{
    public interface ILocationService
    {
        bool Connected { get; }
        Task<Location> GetCurrentLocation();
    }
}

