using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public double AmpumisTiheys;        // ukolla muuttuja?

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
                MainWindow.instance.scene.Children.Add(bullet);  // lisätään bullet canvasiin (scene -nimeltään)

                // ammuksen ajastin
                DispatcherTimer timer_ammus = new DispatcherTimer(DispatcherPriority.Send);
                timer_ammus.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
                timer_ammus.Tick += new EventHandler(timerammus_Tick);      // Set the callback to invoke every tick time
                timer_ammus.Start();

                // ammuksen lähtöpiste y-suunnassa
                AmmusY = 350;

                // törmäyksen tunnistusta varten pidetään "rect"-ammuksen mukana
                ammusPuskuri.X = Canvas.GetLeft(bullet) + 5;
                ammusPuskuri.Y = Canvas.GetTop(bullet) + 5;
                ammusPuskuri.Height = bullet.ActualHeight;
                ammusPuskuri.Width = bullet.ActualWidth;
            }
        }

        private void timerammus_Tick(object sender, EventArgs e)
        {
            if (AmmuksenNopeus > 0) AmmusY = AmmusY - AmmuksenNopeus;  // ruudun yli lennettyä nopeus nollataan
            // ammuksen liikutus
            Canvas.SetLeft(bullet, AmmusX);
            Canvas.SetTop(bullet, AmmusY);

            if (AmmusY < 0) PoisAmmus();
        }

        private void PoisAmmus()  
        {
            MainWindow.instance.scene.Children.Remove(bullet);  // poistetaan bullet canvasilta (scene)
                                                                // poista listasta?
            
            Ukko ukk = new pang.Ukko();

            if (AmmusNro > 0) ukk.PoistaAmmusIlmasta(0);  // poistetaan instanssi listasta
            AmmuksenNopeus = 0;
            AmmusY = 1000;
        }
    }


}
