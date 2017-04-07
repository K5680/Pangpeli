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
                                                                                                //      TODO    TODO    TODO    TODO
namespace pang                                                                                  // * highscore -lista / pelaajan lisäys / pelaajan lataus / pelaajan poisto
{                                                                                               // * pallojen lentorata paremmaksi
    /// <summary>                                                                               // * level päättyy, kun pallot ammuttu -> seuraava level
    /// Interaction logic for MainWindow.xaml                                                   // * pisteet
    /// </summary>                                                                              // * bonukset
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

        private int poistetaanAmmus = -1;   // Muuttuja sisältää tiedon, että pitääkö ruudusta ja listasta poistaa "Ammus". -1 = ei poisteta mitään, muuten index-arvo.

        List<Key> NapitAlhaallaLista = new List<Key>();     // KESKEN, usean näppäimen painallus tällä kuntoon?

        // ukon lisääminen sceneen
        Ukko heebo = new Ukko(); // luodaan Ukko-luokan instanssi, eli pelaaja
        Ukko heebo2 = new Ukko(); // Kaksinpeliin toinen pelaaja

        Pallo[] palloLista = new Pallo[30]; // luodaan tarvittava määrä pallo-olioita
        public int pallojaLuotu;

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

            // luodaan pallo-instanssit, aluksi vain 2 kpl
            for (int i = 0; i < 2; i++) 
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
                        palloLista[i].PalloX = this.Width - 140; // toinen pallo oikeaan reunaan ruudunleveyden mukaan
                        palloLista[i].palloMenossa = pallonSuunta.Vasen;
                        break;
                    default:
                        break;
                }
                pallojaLuotu = i+1;
            }

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            // törmäyksen tunnistuksen ajastus
            DispatcherTimer timer_Törmäys = new DispatcherTimer(DispatcherPriority.Send);
            timer_Törmäys.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_Törmäys.Tick += new EventHandler(timertörmäys_Tick);      // Set the callback to invoke every tick time
            timer_Törmäys.Start();

        }


        // törmäyksen tunnistus timerilla
        private void timertörmäys_Tick(object sender, EventArgs e)
        {
            pelaajanElämät.Text = heebo.Elämät.ToString();  // päivitetään ruutuun elämät

            
            // TÖRMÄYKSEN TUNNISTUS     Ukon ja pallojen / Ammusten ja pallojen välillä
            for (int i = 0; i < pallojaLuotu; i++)    // käydään läpi kaikki pallo-instanssit
            {
                // törmäyksen tunnistus Rect:illä, luodaan pallon ympärille rect
                var x2 = Canvas.GetLeft(palloLista[i].ball);
                var y2 = Canvas.GetTop(palloLista[i].ball);
                Rect r2 = new Rect(x2, y2, (palloLista[i].ball.ActualWidth), (palloLista[i].ball.ActualHeight));


                // Käydään läpi kaikki ammukset, osuvatko kyseiseen palloon + yliruudun. Mutta vasta kun ammuksia on luotu.
                if (Ukko.ammukset.Count > 0)
                {
                    foreach (Ammus ampuu in Ukko.ammukset)
                    {
                        //System.Diagnostics.Debug.WriteLine("puskur: " + ampuu.ammusPuskuri);    // debuggia
                        //System.Diagnostics.Debug.WriteLine("nro: " + ampuu.AmmusNro);           // debuggia

                        if (ampuu.ammusPuskuri.IntersectsWith(r2) || ampuu.AmmusY < 0)  // Laitetaan samaan silmukkaan ammuksen poisto jos se on yli ruudun
                        {
                            if (ampuu.AmmusY < 0)   //debuggia
                            {
                                System.Diagnostics.Debug.WriteLine("- 1. Ammus yli ruudun! Ammuksen nro: " + ampuu.AmmusNro + " index: " + Ukko.ammukset.IndexOf(ampuu)); // debuggia
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("osuu palloon: " + i + " /"+pallojaLuotu); // debuggia
                                // Ammus osui palloon, pallo poksahtaa kahteen osaan...
                                palloLista[i].Puolitus();
                                if (palloLista[i].ball.Width < 10) // ... ja pienin pallo häviää kokonaan.
                                {
                                    MainWindow.instance.scene.Children.Remove(palloLista[i].ball);  // poistetaan bullet canvasilta (scene)
                                }
                                else    // jos ei vielä häviä, niin jaetaan kahteen
                                {
                                    JaaPallo(i);
                                }
                        //      System.Diagnostics.Debug.WriteLine("- - 2. Osui palloon Ammuksen nro: " + ampuu.AmmusNro + " index: " + Ukko.ammukset.IndexOf(ampuu)); // debuggia
                            }

                            ampuu.AmmuksenNopeus = 0;     // Pysäytys
                            ampuu.AmmusY = 1000;          // ja siirto, varulta
                            MainWindow.instance.scene.Children.Remove(ampuu.bullet);  // poistetaan bullet canvasilta (scene)
                            poistetaanAmmus = Ukko.ammukset.IndexOf(ampuu);           // otetaan muuttujaan talteen, minkä indexin 
                        }
                    }

                    // Foreachin sisällä ei voi poistaa ammus-instanssi, joten poistetaan tässä
                    if (poistetaanAmmus != -1) PoistaAmmusJokaIlmassa(poistetaanAmmus);
                }


                // Osuuko ukko palloon
                if (heebo.ukkoPuskuri.IntersectsWith(r2))
                {
                    System.Diagnostics.Debug.WriteLine("Ukko OSUU palloon nro:" + i + "   "  +heebo.ukkoPuskuri); // debuggia
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
                    
        //         var sijainti = Latauskansio+"fire.mp3";
        //         mediaElementti.Source = new Uri(sijainti, UriKind.RelativeOrAbsolute);    // miksei toimi !? 
        //         System.Diagnostics.Debug.WriteLine(sijainti); // debuggia

                    break;
                case "jokumuu":

                    break;
            }
            mediaElementti.Play();  // soita ääni
        }

        // tällä metodilla saadaan lisättyä canvakseen elementti toisen luokan kautta
        public void AddCanvasChild(UIElement child)
        {
            scene.Children.Add(child);
        }

        public void PoistaAmmusJokaIlmassa(int n)
        {
            //System.Diagnostics.Debug.WriteLine("poisto, ammukset.Count:" + Ukko.ammukset.Count()); // debuggia
            //heebo.ammukset.RemoveAt(0);    // poistetaan listasta alin   
            foreach (Ammus ammus in Ukko.ammukset)
            {
                System.Diagnostics.Debug.WriteLine("poistetaan ammusinstanssi indexillä "+n); // debuggia
            }

            Ukko.ammukset.RemoveAt(n);    // poistetaan listasta se, joka osui palloon tai on yli ruudun
            poistetaanAmmus = -1;
        }


        public void JaaPallo(int n)
        {
                var i = pallojaLuotu; // lasketaan montako palloa on jo luotu ja asetetaan se seuraavan pallon numeroksi

                System.Diagnostics.Debug.WriteLine("JaaPallo. Luotu: " + i + "   Jaettava:" + n); // debuggia
               
                // luodaan 1 palloinstanssi lisää, kun toinen on puolitettu pienemmäksi
                palloLista[i] = new Pallo();
                palloLista[i].Numero = i + 1;             

                switch (palloLista[n].palloMenossa)  // metodiin tuodun pallon numeron perusteella katsotaan sen suunta
                {
                    case pallonSuunta.Oikea:          
                    palloLista[i].palloMenossa = pallonSuunta.Vasen;    // mutta eri suunta.
                    break;
                case pallonSuunta.Vasen:
                    palloLista[i].palloMenossa = pallonSuunta.Oikea;
                        break;
                    default:
                        break;
                }

                palloLista[i].Kaari = palloLista[n].Kaari;  // sinikäyrän laskemiseen käytetty halkaisija
                palloLista[i].PalloX = palloLista[n].PalloX;
                palloLista[i].PalloY = palloLista[n].PalloY;
                palloLista[i].PallonKorkeus = palloLista[n].PallonKorkeus;

                palloLista[i].ball.Width = palloLista[n].ball.Width;    // uudelle pallolle sama koko kuin sille, joka poksahti
                palloLista[i].ball.Height = palloLista[n].ball.Height;

                AddCanvasChild(palloLista[i].ball); // lisätään pallo sceneen

                pallojaLuotu += 1;  // lisätään pallojen määrää
         }
        



#region EVENTS
        // näppäinkomennot                          // HUOM ei ota vastaan kuin yhden näppäimen kerrallaan, ongelma kaksinpelissä!
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            
            // player ONE
            if (e.Key == Key.Right || e.Key == Key.Right && e.Key == Key.Space)
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

            if (e.Key == Key.Space)
            {
                heebo.Ammu();
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
