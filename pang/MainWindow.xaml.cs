using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using static pang.Pallo;
//using System.Drawing;

namespace pang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public static MainWindow instance { get; private set; } // tämän instanssin kautta voidaan kutsua MainWindow-luokan metodeita
        public static MainWindow Main; //  tarvitaanko?

        public static double ruudunLeveys;
        public string txt;
        private static string latauskansio = "pack://application:,,,/Pang;component/Images/";  // määritellään kansio, josta kuvat ladataan
        public static string Latauskansio
        {
            get { return latauskansio; }
        }

        List<Key> NapitAlhaallaLista = new List<Key>();     // KESKEN, usean näppäimen painallus tällä kuntoon?

        // ukon lisääminen sceneen
        Ukko heebo = new Ukko(); // luodaan Ukko-luokan instanssi, eli pelaaja
        Ukko heebo2 = new Ukko(); // Kaksinpeliin toinen pelaaja

        public static int pallojaMax = 12;
        Pallo[] palloLista = new Pallo[pallojaMax]; // luodaan tarvittava määrä pallo-olioita

      //  public Rectangle re = new Rectangle(); // Ukon törmäyspuskurin testaukseen
      //  public Rectangle rep = new Rectangle(); // Pallon törmäyspuskurin testaukseen

        public MainWindow()
        {
            InitializeComponent();
            instance = this;    // tämän kautta kutsutaan MainWindow-instanssin metodeita

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (tarvitaanko lopullisessa?)

            heebo.LuoUkko();    // luodaan pelaaja
            AddCanvasChild(heebo.pelaaja); // ja liitetään canvasiin
                                           // heebo2.LuoUkko();    // luodaan pelaaja nro 2 
                                           // AddCanvasChild(heebo2.pelaaja); // ja liitetään canvasiin


            /*      Ukon törmäyspuskurin testaukseen
            re.Fill = System.Windows.Media.Brushes.SkyBlue;
            re.Width = 100;
            re.Height = 100;

            Canvas.SetTop(re, 100);
            Canvas.SetLeft(re, 100);
            AddCanvasChild(re);
            /*      Pallon törmäyspuskurin testaukseen 
            rep.Fill = System.Windows.Media.Brushes.SkyBlue;
            rep.Width = 100;
            rep.Height = 100;

            Canvas.SetTop(rep, 100);
            Canvas.SetLeft(rep, 100);
            AddCanvasChild(rep);
            */

            // luodaan pallo-instanssit
            for (int i = 0; i < pallojaMax; i++)    
            {
                palloLista[i] = new Pallo();
                palloLista[i].Numero = i + 1;

                if (i < 2) AddCanvasChild(palloLista[i].ball); // lisätään pallo-oliot sceneen (canvasiin), aluksi 2kpl

                switch (i)  // ensimmäisille palloille annetaan sijainti-arvot, ja suunnat
                {
                    case 0:
                        palloLista[i].PalloX = 10; // ensimmäinen pallo vasempaan reunaan
                        palloLista[i].palloMenossa = pallonSuunta.Oikea;
                        break;
                    case 1:
                        palloLista[i].PalloX = this.Width-140; // toinen pallo oikeaan reunaan ruudunleveyden mukaan
                        palloLista[i].palloMenossa = pallonSuunta.Vasen;
                        break;
                    default:
                        break;
                }
            }

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            // törmäyksen tunnistuksen ajastus
            DispatcherTimer timer_Törmäys = new DispatcherTimer(DispatcherPriority.Send);
            timer_Törmäys.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_Törmäys.Tick += new EventHandler(timertörmäys_Tick);      // Set the callback to invoke every tick time
            timer_Törmäys.Start();

        }

        // tällä metodilla saadaan lisättyä canvakseen elementti toisen luokan kautta
        public void AddCanvasChild(UIElement child)
        {
            scene.Children.Add(child);
        }

        // törmäyksen tunnistus timerilla
        private void timertörmäys_Tick(object sender, EventArgs e)
        {
          /* Ukon törmäyspuskurin testaukseen
            re.Width = heebo.ukkoPuskuri.Width;
            re.Height = heebo.ukkoPuskuri.Height;

            Canvas.SetLeft(re, heebo.ukkoPuskuri.X);
            Canvas.SetTop(re, heebo.ukkoPuskuri.Y);

            /* Pallon törmäyspuskurin testaukseen
            rep.Width = palloLista[1].ball.ActualWidth;
            rep.Height = palloLista[1].ball.ActualHeight;

            Canvas.SetLeft(rep, palloLista[1].PalloX);
            Canvas.SetTop(rep, palloLista[1].PalloY);
            */

            // ukon ja pallojen välinen tunnistus
            for (int i = 0; i < pallojaMax; i++)    // käydään läpi kaikki pallo-instanssit
            {
                // törmäyksen tunnistus Rect:illä
                var x2 = Canvas.GetLeft(palloLista[i].ball);
                var y2 = Canvas.GetTop(palloLista[i].ball);

                Rect r2 = new Rect(x2, y2, (palloLista[i].ball.ActualWidth), (palloLista[i].ball.ActualHeight));
            
                if (heebo.ukkoPuskuri.IntersectsWith(r2))   // osuuko ukko palloon
                {
                    System.Diagnostics.Debug.WriteLine("OSUU palloon nro:" + i); // debuggia
                    heebo.Osuuko = true; // jos osuu niin ukon "Osuuko"-bool on true (ja lähtee elämä)
                }
            }
        }


        public void Soita(string ääni)
        {
            switch (ääni)   // valitse soitettava ääni
            {
                case "ampu":    // lataa ääni
                    mediaElementti.Source = new Uri(@"C:\Users\Vesada\Source\Repos\Pangpeli - Harjoitustyö\pang\Images\fire.mp3", UriKind.RelativeOrAbsolute);    // miksei toimi! (MainWindow.Latauskansio + "fire.mp3", UriKind.Absolute);  

                    // miksei toimi!?  
                    //mediaElementti.Source = new Uri(MainWindow.Latauskansio + "fire.mp3", UriKind.Absolute);
                    //mediaElementti.LoadedBehavior = MediaState.Manual;

                    System.Diagnostics.Debug.WriteLine(Latauskansio + "fire.mp3"); // debuggia
                    break;
                case "jokumuu":

                    break;
            }
            mediaElementti.Play();  // soita ääni
        }


        #region EVENTS
        // näppäinkomennot                          // HUOM ei ota vastaan kuin yhden näppäimen kerrallaan, ongelma kaksinpelissä!
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            // player ONE
            if (e.Key == Key.Right)
            {
                this.Title = "Go right";
                heebo.LiikutaUkkoa(heebo.Askel);
            }
            else if (e.Key == Key.Left)
            {
                this.Title = "Go left";
                heebo.LiikutaUkkoa(-(heebo.Askel));
            }

            if (e.Key == Key.Escape) // esc lopettaa
            {
                this.Close();
            }

            // kakkospelaaja
            if (e.Key == Key.A)
            {
                heebo2.LiikutaUkkoa(-(heebo2.Askel));
            }

            if (e.Key == Key.D)
            {
                heebo2.LiikutaUkkoa((heebo2.Askel));
            }

            if (e.Key == Key.Space)
            {
                heebo.Ammu();
            }
        }
        
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)    // lasketaan ruudunleveys sen muuttuessa
        {
            ruudunLeveys = scene.ActualWidth;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)    // kun ikkuna on avattu, otetaan muuttujaan ruudun leveyden tieto
        {
            ruudunLeveys = scene.ActualWidth;
        }
        #endregion

    }
}
