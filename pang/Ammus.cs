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

        public Ellipse bullet;

        public Ammus()
        {
            SaaAmpua = true;
            LuoAmmus();
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

                // pallon ajastin
                DispatcherTimer timer_ammus = new DispatcherTimer(DispatcherPriority.Send);
                timer_ammus.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
                timer_ammus.Tick += new EventHandler(timerammus_Tick);      // Set the callback to invoke every tick time
                timer_ammus.Start();

                // ammuksen lähtöpiste y-suunnassa
                AmmusY = 350;
            }

        }

        private void timerammus_Tick(object sender, EventArgs e)
        {
            AmmusY = AmmusY - 10;
            // ammuksen liikutus
            Canvas.SetLeft(bullet, AmmusX);
            Canvas.SetTop(bullet, AmmusY);

            if (AmmusY < 0) PoisAmmus();
        }

        private void PoisAmmus()  // destruktori
        {
            MainWindow.instance.scene.Children.Remove(bullet);  // lisätään bullet canvasiin (scene -nimeltään)
            // poista listasta?

        }
    }

}
