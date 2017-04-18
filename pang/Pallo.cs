using System;
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
                if (pallox > MainWindow.instance.Width) pallox = MainWindow.instance.Width-140;
                pallox = value;
            }
        }

        public double Angle { get; set; }
        public int Numero { get; set; }
        public double PalloY { get; set; }
        public int Kaari = 140;
        private double pallonKorkeus = 240;
        
        public double PallonKorkeus
        {   get
            {
                return pallonKorkeus;
            }
            set
            {
                pallonKorkeus = value;
            }
        }

        private double kiihtyvyys = 0.1f;

        public bool PalloSaaLiikkua { get; set; }

        public Ellipse ball;


        public Pallo()
        {
            Angle = 40;
            LuoPallo();

            // annetaan liikkua heti, jos luodaan kesken pelin, alussa ei
            if (MainWindow.LevelText)
            {
                PalloSaaLiikkua = false;
            }
            else
            {
                PalloSaaLiikkua = true;
            }
        }


        public virtual void LuoPallo()
        {
            ball = new Ellipse();
            
            ball.Stroke = Brushes.Red;
            ball.Fill = Brushes.SkyBlue;
            ball.HorizontalAlignment = HorizontalAlignment.Left;
            ball.VerticalAlignment = VerticalAlignment.Center;
            ball.Width = 130;
            ball.Height = 130;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(MainWindow.Latauskansio + "pallo.png", UriKind.Absolute));
            ball.Fill = tekstuuri;

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
            pallonKorkeus = pallonKorkeus + 45;     // pienemmät pallot pomppii matalemmalla
            Kaari = Kaari - 20;
            Angle = 40;                             // nollataan lähtökulma (sinikäyrään)
        }


        // pallon liikkeen päivitys taimerilla
        public virtual void timerpallo_Tick(object sender, EventArgs e)
        {
            if (PalloSaaLiikkua)    // Pallon liikkumislupa, tarvitaanko lopulta?
            {
                // alas tullessa kovempi vauhti kuin ylhäällä
                if (Angle > 41.5 && Angle < 45.5)
                {
                    kiihtyvyys += 0.02f + ((Convert.ToDouble(pallonKorkeus)) / 8000);    // pallonkorkeuden kautta pallon koko vaikuttaa nopeuteen
                }
                else
                {
                    kiihtyvyys = 0.1f + ((Convert.ToDouble(pallonKorkeus)) / 8000);    // pallonkorkeuden kautta pallon koko vaikuttaa nopeuteen;
                }

                // pallon liikutus sinikäyrällä ylös ja alas
                Angle = Angle + kiihtyvyys;
                if (Angle > 46.3) { Angle = 40; }           // näillä asteilla (40 - 46.3) liikkuminen näyttää sulavalta
                PalloY = pallonKorkeus + Math.Cos(Angle) * Kaari;

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
            }
          
            // Pallon liikkeen päivitys "sceneen"
            Canvas.SetLeft(ball, PalloX);
            Canvas.SetTop(ball, PalloY);            
        }

        ~Pallo()
        {
            System.Diagnostics.Debug.WriteLine(" PALLO DESTRUCTOR !! "); // debuggia
        }
    }
}
