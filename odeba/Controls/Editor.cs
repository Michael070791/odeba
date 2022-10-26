using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace odeba.Controls
{
    public class Editor : Xamarin.Forms.Editor
    {
        public Editor()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                Effects.Add(new Xamarin.CommunityToolkit.Effects.RemoveBorderEffect());
            }
        }
    }
}

