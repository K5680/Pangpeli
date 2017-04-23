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
        public enum PallonSuunta
        {
            Vasen,
            Oikea
        }

        public PallonSuunta PalloMenossa { get; set; }

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
        
        private double pallonKorkeus = 240;

        private int kaari = 140;
        public int Kaari
        {
            get
            {
                return kaari;
            }
            set
            {
                kaari = value;
            }
        }

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

        public Ellipse Ball { get; set; }

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
            Ball = new Ellipse();
            
            Ball.Stroke = Brushes.Red;
            Ball.Fill = Brushes.SkyBlue;
            Ball.HorizontalAlignment = HorizontalAlignment.Left;
            Ball.VerticalAlignment = VerticalAlignment.Center;
            Ball.Width = 130;
            Ball.Height = 130;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(MainWindow.Latauskansio + "pallo.png", UriKind.Absolute));
            Ball.Fill = tekstuuri;

            // pallon ajastin
            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();
        }


        // Pallon puolitus ammukseen osuessa
        public void Puolitus()
        {
            Ball.Height = Ball.Height / 2;
            Ball.Width = Ball.Width / 2;
            pallonKorkeus = pallonKorkeus + 45;     // pienemmät pallot pomppii matalemmalla
            kaari = kaari - 20;
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
                PalloY = pallonKorkeus + Math.Cos(Angle) * kaari;

                // pallon liikutus suunnan mukaan
                if (PalloMenossa == PallonSuunta.Oikea)
                {
                    PalloX = PalloX + 5;    // pallon liikutus oikealla
                    if (PalloX > MainWindow.RuudunLeveys - Ball.Width) PalloMenossa = PallonSuunta.Vasen; // otetaan pallon leveys huomioon seinään törmätessä
                }
                else
                {
                    PalloX = PalloX - 5; // pallon liikutus vasemmalle
                    if (PalloX < 0) PalloMenossa = PallonSuunta.Oikea;
                }
            }
          
            // Pallon liikkeen päivitys "sceneen"
            Canvas.SetLeft(Ball, PalloX);
            Canvas.SetTop(Ball, PalloY);            
        }

        ~Pallo()
        {
            System.Diagnostics.Debug.WriteLine(" PALLO DESTRUCTOR !! "); // debuggia
        }
    }
}
