﻿
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using static pang.Pallo;

namespace pang                                                                                  
{                                                                                               
    /// <summary>                                                                               // * level päättyy, kun pallot ammuttu -> seuraava level
    /// Interaction logic for MainWindow.xaml                                                   
    /// </summary>                                                                              // * bonukset
    public partial class MainWindow : Window
    {

        public static MainWindow instance { get; private set; } // tämän instanssin kautta voidaan kutsua MainWindow-luokan metodeita
                   
        public static int Level = 1;            // levelin numero
        public static bool LevelText = true;    // piirretään levelin numero ruutuun alussa
           
        private DateTime startDate;     // ajastus level-teksteille (ym)
        private int secondDuration;
        private Timer timer;
       
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

        //BonusPallo[] bonusPalloLista = new BonusPallo[10]; // luodaan tarvittava määrä bonuspalloja
        public int bonusPalloLuotu;


        public MainWindow()
        {
            InitializeComponent();
            instance = this;    // tämän kautta kutsutaan MainWindow-instanssin metodeita

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (tarvitaanko lopullisessa?)

            txtPelaajanNimi.Text = Ukko.NykyinenPelaaja;    // alussa valittu pelaajanimi ruutuun

            heebo.LuoUkko();                // luodaan pelaaja
            AddCanvasChild(heebo.pelaaja);  // ja liitetään canvasiin

            LuoPallot();
            
            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            // törmäyksen tunnistuksen ajastus
            DispatcherTimer timer_Törmäys = new DispatcherTimer(DispatcherPriority.Send);
            timer_Törmäys.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_Törmäys.Tick += new EventHandler(timertörmäys_Tick);      // Set the callback to invoke every tick time
            timer_Törmäys.Start();

            AlustaKello();
        }


        private void AlustaKello()
        {
            // sekuntiajastin käyttöön (alustus)
            startDate = DateTime.Now;
            secondDuration = 0;
            timer = new Timer(TimerCallback, null, 0, 1000);    // sekunnin intervalli
        }


        // sekuntikello, alun level-teksti ja pallot liikkumatta
        private void TimerCallback(object state)
        {
            var now = DateTime.Now;
            if (now > startDate + TimeSpan.FromSeconds(1))
            {
                secondDuration += 1;                

                if (secondDuration > 1)
                {
                    for (int i = 0; i < pallojaLuotu; i++)
                    {
                        palloLista[i].PalloSaaLiikkua = true;
                    }                        
                }
                if (secondDuration > 2)
                {
                    LevelText = false;
                }
            }
        }


        public void LuoPallot()
        {          

            // luodaan pallo-instanssit, aluksi vain 2 kpl
            for (int i = 0; i < 2; i++)
            {
                palloLista[i] = new Pallo();
                palloLista[i].Numero = i + 1;

                if (i < 2) AddCanvasChild(palloLista[i].ball); // lisätään pallo-oliot sceneen (canvasiin), aluksi 2kpl
                palloLista[i].PalloY = 100;

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
                pallojaLuotu = i + 1;
            }
        }


        // törmäyksen tunnistus ym. timerilla
        private void timertörmäys_Tick(object sender, EventArgs e)
        {
            // Bonuspallojen luonti ajoittain
            if (secondDuration > 1 && secondDuration % 10 == 0)
            {
                if (bonusPalloLuotu == 0)
                { 
                    BonusPallo bonusPallo = new BonusPallo();
                    bonusPallo.Numero = 0;
                    AddCanvasChild(bonusPallo.ball);
                    bonusPallo.PalloX = 100;
                    bonusPallo.PalloY = 100;
                    bonusPallo.PalloSaaLiikkua = true;
                    bonusPalloLuotu = 1;
                }
            }

            // Ruudun yläreunan tekstit
            txtPelaajanElämät.Text = heebo.Elämät.ToString();  // päivitetään ruutuun elämät
            txtPelaajanPisteet.Text = heebo.Pisteet.ToString(); // ja pisteet
            if (LevelText)
            {
                txtInfo.Text = "Level " + Level;     // level -teksti alussa ruudussa
                txtInfo.Visibility = Visibility.Visible;
            }
            else
            {
                txtInfo.Visibility =  Visibility.Hidden;
            }


            // Pelin loppumisen teksti
            if (heebo.Elämät == 0)
            {
                LevelText = true;
                txtInfo.Text = "GaMe OvER";
                txtInfo.Visibility = Visibility.Visible;                
            }

            int pallojaRuudulla = 0;    // lasketaan montako palloa vielä on canvasilla

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
                    {   // osuuko ammus-rect pallo-rect:iin       tai  ammuksen Y on yli ruudun
                        if (ampuu.ammusPuskuri.IntersectsWith(r2) || ampuu.AmmusY < 0)  // Laitetaan samaan silmukkaan ammuksen poisto jos se on yli ruudun
                        {
                            if (ampuu.AmmusY < 0)   
                            {
                                poistetaanAmmus = Ukko.ammukset.IndexOf(ampuu);           // otetaan muuttujaan talteen, minkä indexin ammus poistetaan kun poissa ruudusta
                            }
                            else
                            {   // Ammus osuu palloon                                                            
                                    // Ammus osui palloon, pallo poksahtaa kahteen osaan...
                                    palloLista[i].Puolitus();
                                    if (palloLista[i].ball.Width < 10) // ... ja pienin pallo häviää kokonaan.
                                    {
                                        scene.Children.Remove(palloLista[i].ball);  // poistetaan ball canvasilta (scene)
                                        Soita("pallo_poksahtaa4");
                                        heebo.Pisteet += 500;
                                        //System.Diagnostics.Debug.WriteLine("poistetaan pallo NRO :  " + i); // debuggia
                                        palloLista[i].PalloX = -100;
                                        palloLista[i].PalloSaaLiikkua = false;  // pallon liike seis (jos sattuu jäämään elämään)
                                    }
                                    else    // jos ei vielä häviä, niin jaetaan kahteen
                                    {
                                        JaaPallo(i, palloLista[i].ball.Width);
                                        heebo.Pisteet += 100;
                                    }                                
                            }
                            ampuu.AmmuksenNopeus = 0;     // Pysäytys
                            ampuu.AmmusX = -100;          // ja siirto, varulta
                            scene.Children.Remove(ampuu.bullet);                      // poistetaan bullet canvasilta (scene)
                            poistetaanAmmus = Ukko.ammukset.IndexOf(ampuu);           // otetaan muuttujaan talteen, minkä indexin ammus poistetaan
                        }
                    }

                    // Foreachin sisällä ei voi poistaa ammus-instanssia, joten poistetaan tässä
                    if (poistetaanAmmus != -1) PoistaAmmusJokaIlmassa(poistetaanAmmus);
                }

                
                // Osuuko ukko palloon
                if (scene.Children.Contains(palloLista[i].ball)) // ei tehdä törmäystunnistusta jos pallo ei ole canvasilla (jostain syystä näkymättömiä palloja jää?)
                {
                    pallojaRuudulla++;  // Lasketaan montako palloa on canvasilla
                    if (heebo.ukkoPuskuri.IntersectsWith(r2))
                    {                     
                        heebo.Osuuko = true; // jos osuu niin ukon "Osuuko"-bool on true (ja lähtee elämä)
                    }
                }
            }

            // jos kaikki pallot ammuttu -> nextille levelille
            if (pallojaRuudulla == 0)   
            {
                AlustaKello();
                Level++;
                LevelText = true;
                LuoPallot();
            }
        }


        public void Soita(string ääni)
        {
            // meneekö kansiorakenne näin oikein valmiissa pelissä??            
            string path2 = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            switch (ääni)   // valitse soitettava ääni              // elementtejä on monta, jotta voi olla monta ääntä yhtäaikaa
            {
                case "ampu":    // lataa ääni                              
                    mediaElementti.Source = new Uri(path2 + @"\sounds\fire.mp3", UriKind.RelativeOrAbsolute);
                    mediaElementti.Play();  // soita ääni
                    break;
                case "pallo_poksahtaa":
                    mediaElementti2.Source = new Uri(path2 + @"\sounds\balloonpop.mp3", UriKind.RelativeOrAbsolute);
                    mediaElementti2.Play();  // soita ääni
                    break;
                case "pallo_poksahtaa2":
                    mediaElementti3.Source = new Uri(path2 + @"\sounds\balloonpop2.mp3", UriKind.RelativeOrAbsolute);
                    mediaElementti3.Play();  // soita ääni
                    break;
                case "pallo_poksahtaa3":
                    mediaElementti4.Source = new Uri(path2 + @"\sounds\balloonpop3.mp3", UriKind.RelativeOrAbsolute);
                    mediaElementti4.Play();  // soita ääni
                    break;
                case "pallo_poksahtaa4":
                    mediaElementti2.Source = new Uri(path2 + @"\sounds\balloonpop4.mp3", UriKind.RelativeOrAbsolute);
                    mediaElementti2.Play();  // soita ääni
                    break;
                case "jokumuu":

                    break;
            }
            
        }

        // tällä metodilla saadaan lisättyä canvakseen elementti toisen luokan kautta
        public void AddCanvasChild(UIElement child)
        {
            scene.Children.Add(child);
        }


        public void PoistaAmmusJokaIlmassa(int n)
        {            
            foreach (Ammus ammus in Ukko.ammukset)
            {
                System.Diagnostics.Debug.WriteLine("poistetaan ammusinstanssi indexillä "+n); // debuggia
            }

            Ukko.ammukset.RemoveAt(n);    // poistetaan listasta se, joka osui palloon tai on yli ruudun
            poistetaanAmmus = -1;
        }


        public void JaaPallo(int n, double halkaisija)
        {
            // soitetaan poksahdusääni koon mukaan
            if (halkaisija > 50)
            {
                Soita("pallo_poksahtaa");
            }
            else if (halkaisija > 25)
            {
                Soita("pallo_poksahtaa2");
            }
            else if (halkaisija > 12)
            {
                Soita("pallo_poksahtaa3");
            }

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
        // näppäinkomennot                          // HUOM ei ota vastaan kuin yhden näppäimen kerrallaan
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {

            // player ONE
            if (e.Key == Key.Right)
            {
                heebo.LiikutaUkkoa(heebo.Askel);
            }
            else if (e.Key == Key.Left)
            {
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
