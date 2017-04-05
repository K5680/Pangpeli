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
        public enum pallonSuunta
        {
            Vasen,
            Oikea
        }

        public pallonSuunta palloMenossa { get; set; }
        private double pallox;
        public double PalloX {
            get
            {
                return pallox;
            }
            set
            {   // varmistetaan, ettei palloa aseteta ruudun ulkopuolelle
                if (pallox < 0) pallox = 0;
                if (pallox > MainWindow.instance.Width) pallox = 100; //MainWindow.instance.Width-140;
                pallox = value;
            }
        }
        public double PalloY { get; set; }
        public double PallonKorkeus { get; set; }
        public double Angle { get; set; }
        public int Numero { get; set; }

        //private double pallox;
        private double pallonKorkeus;

        public Ellipse ball;

        private static Random rnd = new Random(); // arvotaan sijainti, ei ehkä jää lopulliseen peliin
        public static int GetRandom()
        {
            return rnd.Next(50,500);
        }

        public Pallo()
        {
            double n = GetRandom(); // sijainnin arvontaan
            PalloX = n;
            pallonKorkeus = 220;
            Angle = 40;
            LuoPallo();
        }

        public void LuoPallo()
        {
            ball = new Ellipse();

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


        // Pallon puolitus ammukseen osuessa
        public void Puolitus()
        {
            ball.Height = ball.Height / 2;
            ball.Width = ball.Width / 2;
        }


        // pallon liikkeen päivitys taimerilla
        private void timerpallo_Tick(object sender, EventArgs e)
        {
            // pallon liikutus sinikäyrällä ylös ja alas
            Angle = Angle + 0.1f;
            if (Angle > 360) { Angle = 0; }
            PalloY = pallonKorkeus + Math.Cos(Angle) * 140;

            // pallon liikutus suunnan mukaan
            if (palloMenossa == pallonSuunta.Oikea)
            {
                PalloX = PalloX + 5;    // pallon liikutus oikealla
                if (PalloX > MainWindow.ruudunLeveys - ball.Width) palloMenossa = pallonSuunta.Vasen; // otetaan pallon leveys huomioon seinään törmätessä
            }
            else
            {
                PalloX = PalloX - 5; // pallon liikutus vasemmalle
                if (PalloX < 0) palloMenossa = pallonSuunta.Oikea;
            }

            // pallo_x++;
            Canvas.SetLeft(ball, PalloX);
            Canvas.SetTop(ball, PalloY);
        }
    }












          /*  // törmäyksen tunnistus etäisyyden mukaan, kumpi parempi?  Käytössä nyt rect-tunnistus.
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
