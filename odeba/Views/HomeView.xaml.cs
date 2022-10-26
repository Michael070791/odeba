using System;
using System.Collections.Generic;
using System.Reflection;
using odeba.Services.IServices;
using odeba.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace odeba.Views
{
    public partial class HomeView : ContentView
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Pin> PinList { get; set; }
        public HomeView()
        {
            InitializeComponent();

            BindingContext = Startup.GetService<ViewModels.HomeViewModel>();
            var locationService = Startup.GetService<ILocationService>();
            locationService.GetCurrentLocation().ContinueWith(r =>
            {
                var result = r.Result;
                if (result == null) return;
                Latitude = result.Latitude;
                Longitude = result.Longitude;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    odebaMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Latitude, Longitude),
                    Distance.FromKilometers(10)));
                    //ApplyMapTheme();
                    LoadRecyclePoints();
                });

            });
        }
        void LoadRecyclePoints()
        {
            var results = Startup.GetService<HomeViewModel>().LoadMachineLoacations();
            if (results.Count < 1)
                return;
            foreach (var item in results)
            {
                Pin machinePin = new Pin
                {
                    Label = item.Id,
                    Address = item.City,
                    Position = new Position(item.Latitude, item.Longitude),
                    Type = PinType.Place
                    

                };
                odebaMap.Pins.Add(machinePin);
            }
           
        }
        //void ApplyMapTheme()
        //{
        //    var assembly = typeof(HomeView).GetTypeInfo().Assembly;
        //    var stream = assembly.GetManifestResourceStream($"odeba.Resources.MapResources.MapTheme.json");
        //    string themeFile;
        //    using(var reader = new System.IO.StreamReader(stream))
        //    {
        //        themeFile = reader.ReadToEnd();
        //        odebaMap.MapStyle = MapStyle.FromJson(themeFile);
                
        //    }
        //}
    }
}

