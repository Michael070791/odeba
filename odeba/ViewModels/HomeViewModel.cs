using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;

namespace odeba.ViewModels
{
    public class HomeViewModel: BaseViewModel, INavigatableViewModel
    {

        public HomeViewModel()
        {

        }
        ObservableRangeCollection<Models.MachineModel> MachineLocations { get; } = new ObservableRangeCollection<Models.MachineModel>();

        public List<Models.MachineModel> LoadMachineLoacations()
        {
            List<Models.MachineModel> machines = new List<Models.MachineModel>()
            {
                new Models.MachineModel
                {
                    Id = "RRS-001-22",
                    City = "Accra",
                    Latitude = 5.594780,
                    Longitude = -0.185080
                }
            };
            MachineLocations.ReplaceRange(machines);
            return MachineLocations.ToList();
        }
        public void Load(object parameter)
        {
           
        }
    }
}

