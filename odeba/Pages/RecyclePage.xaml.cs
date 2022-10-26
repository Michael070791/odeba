using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace odeba.Pages
{
    public partial class RecyclePage : ContentPage
    {
        public RecyclePage()
        {
            InitializeComponent();
            BindingContext = Startup.GetService<ViewModels.RecycleViewModel>();
        }
    }
}

