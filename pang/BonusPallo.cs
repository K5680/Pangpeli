using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pang
{
    class BonusPallo : Pallo
    {                
        // default constructor
        public BonusPallo()
        {
        }

        // override-metodi pallosta
        public override void LuoPallo()
        {
            Ball = new Ellipse();

            Ball.Stroke = Brushes.Blue;          
            Ball.HorizontalAlignment = HorizontalAlignment.Left;
            Ball.VerticalAlignment = VerticalAlignment.Center;
            
            Ball.Height = 60;
            Ball.Width = 60;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(MainWindow.Latauskansio + "bonuspallo.png", UriKind.Absolute));
            Ball.Fill = tekstuuri;

            // pallon ajastin, Y-SUUNTA
            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(liikeDispatcher);       // Set the Interval, liikeDispatcher on periytyvä päivitysnopeus
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();

            // pallon ajastin, X-SUUNTA
            DispatcherTimer timer_palloX = new DispatcherTimer(DispatcherPriority.Send);
            timer_palloX = new DispatcherTimer(DispatcherPriority.Send);
            timer_palloX.Interval = TimeSpan.FromMilliseconds(1000 / 60);       // Set the Interval, 60fps X-suunta
            timer_palloX.Tick += new EventHandler(timerpalloX_Tick);      // Set the callback to invoke every tick time
            timer_palloX.Start();
        }



    }
}
