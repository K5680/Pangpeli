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

        public string Room { get; set; }

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

            // pallon ajastin
            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();
        }

    }
}
