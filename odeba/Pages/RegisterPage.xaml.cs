using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace odeba.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = Startup.GetService<ViewModels.RegisterViewModel>();
        }
    }
}

