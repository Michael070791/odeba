using System;
using System.Collections.Generic;
using odeba.ViewModels;
using Xamarin.Forms;

namespace odeba.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = Startup.GetService<LoginViewModel>();
        }
    }
}

