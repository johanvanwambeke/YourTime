using System;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YourTime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        Timer _Timer = new Timer(1000);
        int _RemoveHours = 0;
        Helpers.BoxViewClock Clock;

        public Home()
        {
            InitializeComponent();
            _Timer.Elapsed += _Timer_Elapsed;
            _Timer.Start();
            txtUsername.TextChanged += TxtUsername_TextChanged;

            //init our animated clock
            Clock = new Helpers.BoxViewClock(absoluteLayout, secondHand, minuteHand, hourHand);
        }

        private void _Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetTheClocks();
        }

        private void BtnHourMinus_Clicked(object sender, EventArgs e)
        {
            _RemoveHours += 1;
            Clock.RemoveHours = _RemoveHours;
            SetTheClocks();
        }

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Using an open API create a nice avatar
            imgAvatar.Source = "https://api.adorable.io/avatars/70/" + e.NewTextValue;
        }

        private void SetTheClocks()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblTimeHHMM.Text = string.Format("{0}:{1}",
                     DateTime.Now.AddHours(-_RemoveHours).Hour.ToString().PadLeft(2, '0'),
                     DateTime.Now.AddHours(-_RemoveHours).Minute.ToString().PadLeft(2, '0'));
            });

        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            //close program, there are alternatives.
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}