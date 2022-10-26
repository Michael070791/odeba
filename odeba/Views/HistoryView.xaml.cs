using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace odeba.Views
{
    public partial class HistoryView : ContentView
    {
        public HistoryView()
        {
            InitializeComponent();
            BindingContext = Startup.GetService<ViewModels.HistoryViewModel>();
        }
    }
}

