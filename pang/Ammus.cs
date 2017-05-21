using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pang
{
    class Ammus
    {

        public double AmmusX { get; set; }
        public double AmmusY { get; set; }
        public double AmmuksenNopeus { get; set; }
        public bool SaaAmpua { get; set; }
        public int AmmusNro { get; set; }
        //public double AmpumisTiheys;        // ukolla muuttuja?

        public Ellipse Bullet = new Ellipse();
        public Rect AmmusPuskuri = new Rect(); // laatikko, joka toimii alueena, jolta törmäys tunnistetaan

        public Ammus()
        {
            SaaAmpua = true;
            LuoAmmus();
            LiikutaAmmusta();
        }

        public void LuoAmmus()
        {                  
            Bullet.Stroke = System.Windows.Media.Brushes.YellowGreen;
            Bullet.Fill = System.Windows.Media.Brushes.Yellow;
            Bullet.HorizontalAlignment = HorizontalAlignment.Left;
            Bullet.VerticalAlignment = VerticalAlignment.Center;
            Bullet.Width = 10;
            Bullet.Height = 10;
        }


        public void LiikutaAmmusta()
        {
            if (SaaAmpua)
            {
                // ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
                // tekstuuri.ImageSource = new BitmapImage(new Uri(MainWindow.Latauskansio + "pallo.png", UriKind.Absolute));
                // bullet.Fill = tekstuuri;

                // ammuksen ajastin
                DispatcherTimer timer_ammus = new DispatcherTimer(DispatcherPriority.Send);
                timer_ammus.Interval = TimeSpan.FromMilliseconds(17);       // Set the Interval             LIIKE noin 60fps!
                timer_ammus.Tick += new EventHandler(timerammus_Tick);      // Set the callback to invoke every tick time
                timer_ammus.Start();

                // ammuksen lähtöpiste y-suunnassa
                AmmusY = 380;
                Canvas.SetTop(Bullet, -100);
                MainWindow.instance.AddCanvasChild(Bullet);  // lisätään bullet canvasiin (scene -nimeltään)
            }
        }

        private void timerammus_Tick(object sender, EventArgs e)
        {
            if (AmmuksenNopeus > 0) AmmusY = AmmusY - AmmuksenNopeus;  // ruudun yli lennettyä nopeus nollataan
            // ammuksen liikutus
            Canvas.SetLeft(Bullet, AmmusX);
            Canvas.SetTop(Bullet, AmmusY);

            // törmäyksen tunnistusta varten pidetään "rect"-ammuksen mukana
            AmmusPuskuri.X = Canvas.GetLeft(Bullet) + 5;
            AmmusPuskuri.Y = Canvas.GetTop(Bullet) + 5;
            AmmusPuskuri.Height = Bullet.ActualHeight;
            AmmusPuskuri.Width = Bullet.ActualWidth;
        }
       
    }
}
