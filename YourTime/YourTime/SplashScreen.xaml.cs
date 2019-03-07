using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Timers;

namespace YourTime
{
    public partial class SplashScreen : ContentPage
    {
        const int Delay = 5000;
        Timer _Timer = new Timer(Delay);
        public SplashScreen()
        {
            InitializeComponent();
            _Timer.Elapsed += T_Elapsed;
            _Timer.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Stop();
            _Timer.Elapsed -= T_Elapsed;
            Device.BeginInvokeOnMainThread(() => { Navigation.PushModalAsync(new Home()); });
        }
    }
}
