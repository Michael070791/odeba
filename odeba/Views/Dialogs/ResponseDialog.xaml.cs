using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using odeba.Services.IServices;
using Xamarin.Forms;

namespace odeba.Views.Dialogs
{
    public partial class ResponseDialog : ContentPage
    {
        public Action<DialogResult> Callback { get; set; }
        public string DisplayText { get => (string)GetValue(DisplayTextProperty); set => SetValue(DisplayTextProperty, value); }

        public static BindableProperty DisplayTextProperty =
            BindableProperty.Create(nameof(DisplayText), typeof(string), typeof(ResponseDialog), "Are you sure?");
        public string YesText { get => (string)GetValue(YesTextProperty); set => SetValue(YesTextProperty, value); }

        public static BindableProperty YesTextProperty =
            BindableProperty.Create(nameof(YesText), typeof(string), typeof(ResponseDialog), "Yes");

        public string NoText { get => (string)GetValue(NoTextProperty); set => SetValue(NoTextProperty, value); }

        public static BindableProperty NoTextProperty =
            BindableProperty.Create(nameof(NoText), typeof(string), typeof(ResponseDialog), "No");


        public bool DisplayNoButton { get => (bool)GetValue(DisplayNoButtonProperty); set => SetValue(DisplayNoButtonProperty, value); }

        public static BindableProperty DisplayNoButtonProperty =
            BindableProperty.Create(nameof(DisplayNoButton), typeof(bool), typeof(ResponseDialog), true);

        private DialogResult _result;

        public ResponseDialog()
        {
            InitializeComponent();
            _result = DialogResult.Unknown;
        }

        private async void Yes_Tapped(object sender, EventArgs e)
        {
            _result = DialogResult.Yes;
            await Close();
        }

        private async void No_Tapped(object sender, EventArgs e)
        {
            _result = DialogResult.No;
            await Close();
        }

        private async void Close_Tapped(object sender, EventArgs e)
        {
            await Close();
        }

        private async Task Close()
        {
            Callback?.Invoke(_result);
            await Navigation.PopModalAsync();
            Callback = null;
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}

