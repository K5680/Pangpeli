using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pang
{
    class Ammus
    {

        public double AmmusX;
        public double AmmusY;
        public double AmmuksenNopeus;
        public bool SaaAmpua;
        public int AmmusNro;
        //public double AmpumisTiheys;        // ukolla muuttuja?

        public Ellipse bullet;
        public Rect ammusPuskuri = new Rect(); // laatikko, joka toimii alueena, jolta törmäys tunnistetaan

        public Ammus()
        {
            SaaAmpua = true;
            LuoAmmus();
            LiikutaAmmusta();
        }

        public void LuoAmmus()
        {
            bullet = new Ellipse();
            
            bullet.Stroke = System.Windows.Media.Brushes.YellowGreen;
            bullet.Fill = System.Windows.Media.Brushes.Yellow;
            bullet.HorizontalAlignment = HorizontalAlignment.Left;
            bullet.VerticalAlignment = VerticalAlignment.Center;
            bullet.Width = 10;
            bullet.Height = 10;
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
                timer_ammus.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
                timer_ammus.Tick += new EventHandler(timerammus_Tick);      // Set the callback to invoke every tick time
                timer_ammus.Start();

                // ammuksen lähtöpiste y-suunnassa
                AmmusY = 350;                
                MainWindow.instance.AddCanvasChild(bullet);  // lisätään bullet canvasiin (scene -nimeltään)
            }
        }

        private void timerammus_Tick(object sender, EventArgs e)
        {
            if (AmmuksenNopeus > 0) AmmusY = AmmusY - AmmuksenNopeus;  // ruudun yli lennettyä nopeus nollataan
            // ammuksen liikutus
            Canvas.SetLeft(bullet, AmmusX);
            Canvas.SetTop(bullet, AmmusY);

            // törmäyksen tunnistusta varten pidetään "rect"-ammuksen mukana
            ammusPuskuri.X = Canvas.GetLeft(bullet) + 5;
            ammusPuskuri.Y = Canvas.GetTop(bullet) + 5;
            ammusPuskuri.Height = bullet.ActualHeight;
            ammusPuskuri.Width = bullet.ActualWidth;
        }
       
    }
}
