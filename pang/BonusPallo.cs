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


        public override void LuoPallo()
        {
            ball = new Ellipse();

            ball.Stroke = System.Windows.Media.Brushes.Red;
            ball.Fill = System.Windows.Media.Brushes.SkyBlue;
            ball.HorizontalAlignment = HorizontalAlignment.Left;
            ball.VerticalAlignment = VerticalAlignment.Center;
            ball.Width = 110;
            ball.Height = 110;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(MainWindow.Latauskansio + "bonuspallo.png", UriKind.Absolute));
            ball.Fill = tekstuuri;

            // pallon ajastin
            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();
        }

    }
}
