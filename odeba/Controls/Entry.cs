using System;
namespace odeba.Controls
{
    public class Entry : Xamarin.Forms.Entry
    {
        public Entry() => Effects.Add(new Xamarin.CommunityToolkit.Effects.RemoveBorderEffect());
    }
}

