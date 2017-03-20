using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pang
{
    class Pallo
    {
        public double PalloX { get; set; }
        public double PalloY { get; set; }
        public double PallonKorkeus { get; set; }
        public double Angle;

        //private double pallox;
        private double palloy;
        private double pallonKorkeus;
        private double angle;

        public Ellipse ball;

        private static Random rnd = new Random(); // arvotaan sijainti, ei ehkä jää lopulliseen peliin
        public static int GetRandom()
        {
            return rnd.Next(50,500);
        }

        public Pallo()
        {
            
            double n = GetRandom();
            System.Diagnostics.Debug.WriteLine("random: " + n); // debuggia
            PalloX = n;
            n = GetRandom();
            System.Diagnostics.Debug.WriteLine("random: " + n); // debuggia
            PalloY = n;

            pallonKorkeus = 180;
            angle = 40;
            ball = new Ellipse();
            LuoPallo();
            
        }

        public void LuoPallo()
        {
            ball.Stroke = System.Windows.Media.Brushes.Red;
            ball.Fill = System.Windows.Media.Brushes.SkyBlue;
            ball.HorizontalAlignment = HorizontalAlignment.Left;
            ball.VerticalAlignment = VerticalAlignment.Center;
            ball.Width = 110;
            ball.Height = 110;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(MainWindow.Latauskansio + "pallo.png", UriKind.Absolute));
            ball.Fill = tekstuuri;
            //MainWindow.instance.scene.Children.Add(ball);  // ei toimi, tehdään pääkoodissa

            // pallon ajastin
            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();
        }

        private void timerpallo_Tick(object sender, EventArgs e)
        {
            // pallon liikutus sinikäyrällä
            angle = angle + 0.1f;
            if (angle > 360) { angle = 0; }
            palloy = pallonKorkeus + Math.Cos(angle) * 140;

            Angle = angle;
            // pallo_x++;
            Canvas.SetLeft(ball, PalloX);
            Canvas.SetTop(ball, palloy);
        }


    }

    /*
    // törmäyksen tunnistus Rect:illä
    var x2 = Canvas.GetLeft(pallo);
    var y2 = Canvas.GetTop(pallo);
    Rect r2 = new Rect(x2, y2, pallo.ActualWidth, pallo.ActualHeight);
            if (heebo.ukkoPuskuri.IntersectsWith(r2)) System.Diagnostics.Debug.WriteLine("OSUU !! " + pallo_y); // debuggia


            // törmäyksen tunnistus etäisyyden mukaan, kumpi parempi? Onko vaihtoehtoja?
            double xet = Canvas.GetLeft(pallo); //pallon x
    double yet = Canvas.GetTop(pallo); // pallon y
    double D = Math.Sqrt((xet - heebo.SijaintiX) * (xet - heebo.SijaintiX) + (yet - 350) * (yet - 350));

            if (D < (ball.Height))
            {
                var a = Console.ReadLine();
                System.Diagnostics.Debug.WriteLine("Pallon etäisyysmittarisysteemillä osuu... " + D + " " + yet); // debuggia
            }

*/

    

}
