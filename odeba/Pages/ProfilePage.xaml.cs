using System;
using System.Collections.Generic;
using odeba.ViewModels;
using Xamarin.Forms;

namespace odeba.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = Startup.GetService<ProfileViewModel>();
        }
    }
}

