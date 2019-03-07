using System;
using Xamarin.Forms;

namespace YourTime.Helpers
{
    public class BoxViewClock
    {
        //This man did a good job of making a clock-face, i changed nothing on his code.
        //Only turned it into a helper to abstract the code from my own.
        //https://www.c-sharpcorner.com/article/how-to-box-view-clock-in-xamarin-forms/

        struct HandParams
        {
            public HandParams(double width, double height, double offset) : this()
            {
                Width = width;
                Height = height;
                Offset = offset;
            }

            public double Width { private set; get; }   // fraction of radius  
            public double Height { private set; get; }  // ditto  
            public double Offset { private set; get; }  // relative to center pivot  
        }

        static readonly HandParams secondParams = new HandParams(0.02, 1.1, 0.85);
        static readonly HandParams minuteParams = new HandParams(0.05, 0.8, 0.9);
        static readonly HandParams hourParams = new HandParams(0.125, 0.65, 0.9);
        AbsoluteLayout _absoluteLayout;
        BoxView _secondHand;
        BoxView _minuteHand;
        BoxView _hourHand;
        public int RemoveHours;

        BoxView[] tickMarks = new BoxView[60];

        public BoxViewClock(AbsoluteLayout absoluteLayout, BoxView secondHand, BoxView minuteHand, BoxView hourHand)
        {
            _absoluteLayout = absoluteLayout;
            _secondHand = secondHand;
            _minuteHand = minuteHand;
            _hourHand = hourHand;

            _absoluteLayout.SizeChanged += _absoluteLayout_SizeChanged;
            // Create the tick marks (to be sized and positioned later).  
            for (int i = 0; i < tickMarks.Length; i++)
            {
                tickMarks[i] = new BoxView { Color = Color.Black };
                _absoluteLayout.Children.Add(tickMarks[i]);
            }

            Device.StartTimer(TimeSpan.FromSeconds(1.0 / 60), OnTimerTick);
        }

        private void _absoluteLayout_SizeChanged(object sender, EventArgs e)
        { 
            // Get the center and radius of the AbsoluteLayout.  
            Point center = new Point(_absoluteLayout.Width / 2, _absoluteLayout.Height / 2);
            double radius = 0.45 * Math.Min(_absoluteLayout.Width, _absoluteLayout.Height);

            // Position, size, and rotate the 60 tick marks.  
            for (int index = 0; index < tickMarks.Length; index++)
            {
                double size = radius / (index % 5 == 0 ? 15 : 30);
                double radians = index * 2 * Math.PI / tickMarks.Length;
                double x = center.X + radius * Math.Sin(radians) - size / 2;
                double y = center.Y - radius * Math.Cos(radians) - size / 2;
                AbsoluteLayout.SetLayoutBounds(tickMarks[index], new Rectangle(x, y, size, size) );
                tickMarks[index].Rotation = 180 * radians / Math.PI;
            }

            // Position and size the three hands.  
            LayoutHand(_secondHand, secondParams, center, radius);
            LayoutHand(_minuteHand, minuteParams, center, radius);
            LayoutHand(_hourHand, hourParams, center, radius);
        }
        

        void LayoutHand(BoxView boxView, HandParams handParams, Point center, double radius)
        {
            double width = handParams.Width * radius;
            double height = handParams.Height * radius;
            double offset = handParams.Offset;

            AbsoluteLayout.SetLayoutBounds(boxView,
                new Rectangle(center.X - 0.5 * width,
                              center.Y - offset * height,
                              width, height));

            // Set the AnchorY property for rotations.  
            boxView.AnchorY = handParams.Offset;
        }

        bool OnTimerTick()
        {
            // Set rotation angles for hour and minute hands.  
            DateTime dateTime = DateTime.Now.AddHours(-RemoveHours);
            _hourHand.Rotation = 30 * (dateTime.Hour % 12) + 0.5 * dateTime.Minute;
            _minuteHand.Rotation = 6 * dateTime.Minute + 0.1 * dateTime.Second;

            // Do an animation for the second hand.  
            double t = dateTime.Millisecond / 1000.0;

            if (t < 0.5)
            {
                t = 0.5 * Easing.SpringIn.Ease(t / 0.5);
            }
            else
            {
                t = 0.5 * (1 + Easing.SpringOut.Ease((t - 0.5) / 0.5));
            }

            _secondHand.Rotation = 6 * (dateTime.Second + t);
            return true;
        }
    }
}
