
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
    /// <summary>                                                                               
    /// #                                                 
    /// #   Pangpeli the game. 2017 (C) Vesa Vertainen.
    /// #   19.4.2017
    /// #
    /// </summary>                                                   
    /// 
    public partial class MainWindow : Window
    { 
        public static int Level { get; set; }   // levelin numero
        public static bool LevelText = true;    // piirretään levelin numero ruutuun alussa

        private static DateTime startDate;     // ajastus level-teksteille / ukon "immuniteetti" ym.
        private static int secondDuration;     // ajasta halutaan staattinen, eikä useita instansseja
        private static Timer timer;       
        private static bool timerActive = false;

        public static double RuudunLeveys;
        private static string latauskansio = "pack://application:,,,/Pang;component/Images/";  // määritellään kansio, josta kuvat ladataan
        public static string Latauskansio
        {
            get { return latauskansio; }
        }

        public static MainWindow instance { get; private set; } // tämän instanssin kautta voidaan kutsua MainWindow-luokan metodeita

        private int poistetaanAmmus = -1;   // Muuttuja sisältää tiedon, että pitääkö ruudusta ja listasta poistaa "Ammus". -1 = ei poisteta mitään, muuten index-arvo.

        DispatcherTimer timer_Törmäys = new DispatcherTimer(DispatcherPriority.Send);   // törmäyksen tunnistuksen ajastus
        private int ukonAloitusKello;   // Uudelleenaloituksessa ukkoon ei osu pallot pariin sekuntiin
        Ukko heebo = new Ukko();        // Luodaan Ukko-luokan instanssi, eli pelaaja       

        private int pallojaRuudulla;    // Muuttuja sisältää tiedon, montako palloa ruudulla on.
        Pallo[] palloLista = new Pallo[30]; // luodaan tarvittava määrä pallo-olioita
        private List<int> poistetutPallot = new List<int>(); // tehdään lista, jossa tieto siitä, mitkä palloinstanssit on jo poistettu
        private int loytyi;             // poistettujen ja luotujen pallojen vertailuun käytettävä muuttuja
        private int pallojaLuotu; // luotujen pallojen määrä talteen

        private double xb, yb;                      // Bonuspallon sijainti,
        private bool bonusPalloLuotu;                // bonuspallo olemassa vai ei,
        BonusPallo bonusPallo = new BonusPallo();   // luodaan valmiiksi bonuspallo.

        private List<Key> napitPainettuna = new List<Key>();    // Näppäin-eventit toimii hitaasti, pelaajan reagointi saadaan paremmaksi käyttämällä näppäimille listaa.


        public MainWindow()
        {           
            Level = 0;
            InitializeComponent();
            instance = this;    // tämän kautta kutsutaan MainWindow-instanssin metodeita           

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (tarvitaanko lopullisessa?)

            txtPelaajanNimi.Text = Ukko.NykyinenPelaaja;    // alussa valittu pelaajanimi ruutuun
            
            AddCanvasChild(heebo.PelaajanHahmo);  // ja liitetään canvasiin

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            System.Diagnostics.Debug.WriteLine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)); // debuggia

            AlustaKello();           
        }


        private void AlustaKello()
        {
            // sekuntiajastin käyttöön (alustus)
            startDate = DateTime.Now;
            secondDuration = 0;

            if (!timerActive)
            {
                timer = new Timer(TimerCallback, null, 0, 1000);    // sekunnin intervalli
                timerActive = true;
            }
            else
            {
                timer.Change(0, 1000); // reset to 1 second
            }

            // Törmäyksen tunnistuksen ym pelitoimintojen ajastus            
            timer_Törmäys.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_Törmäys.Tick += new EventHandler(timertörmäys_Tick);      // Set the callback to invoke every tick time
            timer_Törmäys.Start();
        }


        // sekuntikello, alun level-teksti ja pallot liikkumatta
        private void TimerCallback(object state)
        {
            //System.Diagnostics.Debug.WriteLine("sekuntikello: " + secondDuration + "  pallojaLuotu: "+ pallojaLuotu);

            var now = DateTime.Now;
            if (now > startDate + TimeSpan.FromSeconds(1))
            {
                secondDuration += 1;

                if (secondDuration > 2 && secondDuration < 4)
                {
                    for (int i = 0; i < pallojaLuotu; i++)
                    {
                        palloLista[i].PalloSaaLiikkua = true;
                    }
                }
                if (secondDuration == 3)
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
                    palloLista[i].PalloY = 100;

                    switch (i)  // ensimmäisille palloille annetaan sijainti-arvot, ja suunnat
                    {
                        case 0:
                            palloLista[i].PalloX = 10; // ensimmäinen pallo vasempaan reunaan
                            palloLista[i].PalloMenossa = PallonSuunta.Oikea;
                            break;
                        case 1:
                            palloLista[i].PalloX = this.Width - 155; // toinen pallo oikeaan reunaan ruudunleveyden mukaan
                            palloLista[i].PalloMenossa = PallonSuunta.Vasen;
                            break;
                        default:
                            break;
                    }

                    Canvas.SetTop(palloLista[i].Ball, -100); // siirretään aluksi pois näkyvistä (muuten vilahtaa alussa)
                    AddCanvasChild(palloLista[i].Ball); // lisätään pallo-oliot sceneen (canvasiin), aluksi 2kpl
                    pallojaLuotu = i + 1;
                }            
        }

        
        // Törmäyksen tunnistus ym. tapahtumat Timerilla
        private void timertörmäys_Tick(object sender, EventArgs e)
        {
                // pelaajan liikkeet taulukon kautta
                LiikutaPelaajaa();

                // Ruudun yläreunan tekstit
                txtPelaajanElämät.Text = heebo.ElämätCounter.ToString();  // päivitetään ruutuun elämät
                txtPelaajanPisteet.Text = heebo.PisteLaskuri.ToString(); // ja PisteLaskuri

                if (LevelText && Level > 0)
                {
                    txtInfo.Text = "Level " + Level;     // level -teksti alussa ruudussa
                    txtInfo.Visibility = Visibility.Visible;
                }
                else
                {
                    txtInfo.Visibility = Visibility.Hidden;
                }
                
                // Pelin loppuminen
                if (heebo.ElämätCounter == 0)
                {
                    LevelText = true;
                    txtInfo.Text = "           Game Over \nPress enter to play Again";
                    txtInfo.Visibility = Visibility.Visible;
                    Level = 0;                    
                }

                Bonukset();

                pallojaRuudulla = 0;    // lasketaan montako palloa on canvasilla tarkkailtavalla hetkellä

                // TÖRMÄYKSEN TUNNISTUS     Ukon ja pallojen / Ammusten ja pallojen välillä
                for (int i = 0; i < pallojaLuotu; i++)    // käydään läpi kaikki pallo-instanssit
                {
                    // törmäyksen tunnistus Rect:illä, luodaan pallon ympärille rect
                    var x2 = Canvas.GetLeft(palloLista[i].Ball);
                    var y2 = Canvas.GetTop(palloLista[i].Ball);
                    Rect r2 = new Rect(x2, y2, (palloLista[i].Ball.Width), (palloLista[i].Ball.Height));

                    // Käydään läpi kaikki Ammukset, osuvatko kyseiseen palloon + yliruudun. Mutta vasta kun ammuksia on luotu.
                    if (Ukko.Ammukset.Count > 0)
                    {
                        foreach (Ammus ampuu in Ukko.Ammukset)
                        {   // osuuko ammus-rect pallo-rect:iin       tai  ammuksen Y on yli ruudun
                            
                                if (ampuu.AmmusPuskuri.IntersectsWith(r2) || ampuu.AmmusY < 0)  // Laitetaan samaan silmukkaan ammuksen poisto jos se on yli ruudun
                                {
                                    if (ampuu.AmmusY < 0)
                                    {
                                        poistetaanAmmus = Ukko.Ammukset.IndexOf(ampuu);           // otetaan muuttujaan talteen, minkä indexin ammus poistetaan kun poissa ruudusta
                                    }
                                    else
                                    {   // Ammus osuu palloon                                                            
                                        // Ammus osui palloon, pallo poksahtaa kahteen osaan...
                                        palloLista[i].Puolitus();

                                        if (palloLista[i].Ball.Width < 15) // ... ja pienin pallo häviää kokonaan.
                                        {
                                            scene.Children.Remove(palloLista[i].Ball);  // poistetaan Ball canvasilta (scene)

                                            // Tarkistetaan, onko kyseinen pallo jo poistettujen listalla, jos ei niin lisätään                                           
                                            loytyi = 0;
                                            foreach (int line in poistetutPallot)
                                            {
                                                
                                                if (line == i)
                                                {
                                                    loytyi = 1;
                                                }
                                            }
                                            if (loytyi == 0)
                                            {
                                                poistetutPallot.Add(i);    // lisätään poistettujen listaan pallon numero, ellei kyseinen pallo jo ole
                                            }   

                                            Soita("pallo_poksahtaa4");
                                            heebo.PisteLaskuri += 500;
                                            palloLista[i].PalloY = -200;
                                            palloLista[i].PalloSaaLiikkua = false;  // pallon liike seis (jos sattuu jäämään elämään)
                                    }
                                        else    // jos ei vielä häviä, niin jaetaan kahteen
                                        {
                                            JaaPallo(i, palloLista[i].Ball.Width);
                                            heebo.PisteLaskuri += 100;
                                        }
                                    }
                                    ampuu.AmmuksenNopeus = 0;     // Pysäytys
                                    ampuu.AmmusY = -100;          // ja siirto, varulta

                                    scene.Children.Remove(ampuu.Bullet);                      // poistetaan bullet canvasilta (scene)
                                    poistetaanAmmus = Ukko.Ammukset.IndexOf(ampuu);           // otetaan muuttujaan talteen, minkä indexin ammus poistetaan
                                }
                            
                        }

                        // Foreachin sisällä ei voi poistaa ammus-instanssia, joten poistetaan tässä
                        if (poistetaanAmmus != -1) PoistaAmmusJokaIlmassa(poistetaanAmmus);
                    }

                    // Osuuko ukko palloon
                    if (secondDuration > ukonAloitusKello + 2) // Ukko on kuolematon pari sekuntia edellisestä "kuolemasta"
                    {
                        if (heebo.UkkoPuskuri.IntersectsWith(r2))
                        {
                            heebo.Osuuko = true;    // jos osuu niin ukon "Osuuko"-bool on true (ja lähtee elämä)
                            heebo.BonusTaso = 0;    // BonusTaso nollautuu
                            ukonAloitusKello = secondDuration;  // Tehdään ukosta kuolematon tästä hetkestä muutama sekunti eteenpäin
                        }
                    }

                    pallojaRuudulla++;  // Lasketaan montako palloa on canvasilla       
            }


            // int palloBalanssi = pallojaRuudulla - poistetutPallot.Count;
            //System.Diagnostics.Debug.WriteLine("erotus:  " + palloBalanssi + "ruudull:" + pallojaRuudulla + "  poistetut:"+poistetutPallot.Count); // debuggia


            // Nollaus pelin alussa
            if (pallojaRuudulla == 0 && Level == 0)
            {                
                Level++;
                LevelText = true;
                LuoPallot();
                AlustaKello();
                heebo.SijaintiX = 350;    // nollataan sijainti
                ukonAloitusKello = secondDuration;                
            }

            // jos kaikki pallot ammuttu -> next level   /   Tai uuden pelin aloitus
            if (poistetutPallot.Count == 16 || Level == -1)
                {
                foreach (Ammus ampuu in Ukko.Ammukset)
                {
                    scene.Children.Remove(ampuu.Bullet);    // poistetaan bulletit canvasilta, jotta ei ammuta heti seuraavankin levelin palloja
                    ampuu.AmmusY = 0;                       // siirretään myös sijaintinsa puolesta ulos
                }

                pallojaLuotu = 0;
                    AlustaKello();

                    if (Level == -1) Level = 0; // Level -1 tarkoittaa, että aloitetaan uusi peli, sitten nollattava

                    Level++;
                    LevelText = true;
                    LuoPallot();
                    
                    heebo.SijaintiX = 350;    // nollataan sijainti
                    ukonAloitusKello = secondDuration;
                    poistetutPallot.Clear();
                }            
        }



        public void Bonukset()
        {
            // Bonuspallojen luonti ajoittain
            if (secondDuration > 1 && secondDuration % 15 == 0 && !bonusPalloLuotu)
            {
                Random rnd = new Random();
                double xsij = rnd.Next(0, 750);     // Arvotaan bonuspallon x-sijainti
                bonusPallo.PalloX = xsij;
                bonusPallo.PalloY = 120;
                bonusPallo.PalloSaaLiikkua = true;  // Annetaan pallolle lupa liikkua heti
                bonusPalloLuotu = true;             // Törmäyksen tunnistusta varten                
                
                AddCanvasChild(bonusPallo.Ball);    // Piirretään bonus canvasiin.
                Canvas.SetTop(bonusPallo.Ball, -100); // siirretään aluksi pois näkyvistä (muuten vilahtaa alussa)
            }

            

            // Ukko <-> BonusPallo Törmäyksen tunnistus
            if (bonusPalloLuotu)
            {
                xb = Canvas.GetLeft(bonusPallo.Ball);
                yb = Canvas.GetTop(bonusPallo.Ball);

                //System.Diagnostics.Debug.WriteLine(bonusPallo.Ball.ActualHeight + " <- height    width -> " + bonusPallo.Ball.ActualWidth); // debuggia

                Rect rb = new Rect(xb + 7, yb + 7, (bonusPallo.Ball.Width - 15), (bonusPallo.Ball.Height - 15));  // Rect bonuksen ympärille, jotta voidaan tunnistaa törmäys ukkoon
                
                if (heebo.UkkoPuskuri.IntersectsWith(rb))
                {
                    scene.Children.Remove(bonusPallo.Ball);
                    bonusPallo.PalloY = 600; // varulta pois ruudulta myös sijaintinsa puolesta
                    bonusPalloLuotu = false;
                    Soita("bonus");
                    heebo.BonusTaso++;
                    
                    switch (heebo.BonusTaso)
                    {
                        case 0:
                            break;
                        case 1:
                            heebo.AmmuksiaMax = 8;      // ammusten maksimimäärää ruudulla lisätään
                            heebo.AmmusTiheys = 400;
                            break;
                        case 2:
                            heebo.AmmuksiaMax = 10;
                            heebo.AmmusTiheys = 200;    // ampumisnopeus kasvaa
                            break;
                        case 3:                       
                            heebo.AmmuksiaMax = 15;
                            break;
                        case 4:
                            heebo.AmmuksiaMax = 20;
                            heebo.AmmusTiheys = 100;                            
                            break;
                        case 5:
                            heebo.ElämätCounter++;             // extra life   + tässä kohtaa tupla-Ammukset ukko-luokan kautta
                            break;
                        default:
                            heebo.AmmuksiaMax += 5;        // ammusten maksimimäärää ruudulla lisätään
                            heebo.AmmusTiheys = 70;
                            break;
                    }
                }
            }

            // Poistetaan bonuspallo ruudulta, ellei sitä ole napattu ajoissa
            if (secondDuration % 25 == 0 && bonusPalloLuotu)
            {
                scene.Children.Remove(bonusPallo.Ball);
                bonusPallo.PalloY = 600; // varulta pois ruudulta myös sijaintinsa puolesta
                bonusPalloLuotu = false;
            }
        }


        public void Soita(string ääni)
        {
            // meneekö kansiorakenne näin oikein valmiissa pelissä??            
            string path2 = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);    //toimiiko exessä?

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
                case "bonus":
                    mediaElementti2.Source = new Uri(path2 + @"\sounds\bonus.mp3", UriKind.RelativeOrAbsolute);
                    mediaElementti2.Play();  // soita ääni
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
            Ukko.Ammukset.RemoveAt(n);    // poistetaan listasta se, joka osui palloon tai on yli ruudun
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

                //System.Diagnostics.Debug.WriteLine("JaaPallo. Luotu: " + i + "   Jaettava:" + n); // debuggia
               
                // luodaan 1 palloinstanssi lisää, kun toinen on puolitettu pienemmäksi
                palloLista[i] = new Pallo();
                palloLista[i].Numero = i + 1;             
            
                switch (palloLista[n].PalloMenossa)  // metodiin tuodun pallon numeron perusteella katsotaan sen suunta
                {
                    case PallonSuunta.Oikea:          
                    palloLista[i].PalloMenossa = PallonSuunta.Vasen;    // mutta eri suunta.
                    break;
                case PallonSuunta.Vasen:
                    palloLista[i].PalloMenossa = PallonSuunta.Oikea;
                        break;
                    default:
                        break;
                }
            
                palloLista[i].Kaari = palloLista[n].Kaari;  // sinikäyrän laskemiseen käytetty halkaisija
                palloLista[i].PalloX = palloLista[n].PalloX;
                palloLista[i].PalloY = palloLista[n].PalloY;
                palloLista[i].PallonKorkeus = palloLista[n].PallonKorkeus;

                palloLista[i].Ball.Width = palloLista[n].Ball.Width;    // uudelle pallolle sama koko kuin sille, joka poksahti
                palloLista[i].Ball.Height = palloLista[n].Ball.Height;

                Canvas.SetTop(palloLista[i].Ball, -100); // aluksi pois näkyvistä, muuten vilahtaa tullessaan yläreunassa
                AddCanvasChild(palloLista[i].Ball); // lisätään pallo sceneen
                pallojaLuotu += 1;  // lisätään pallojen määrää
            
        }


        // Näppäinkomennot reagoi liian hitaasti, joten taulukkoon tallentamalla ja lukemalla saadaa pelaajan reagointi huomattavasti paremmaksi
        void LiikutaPelaajaa()
        {
            if (napitPainettuna.Contains(Key.Space)) heebo.Ammu();

            if (napitPainettuna.Contains(Key.Right)) heebo.LiikutaUkkoa(heebo.Askel);

            if (napitPainettuna.Contains(Key.Left)) heebo.LiikutaUkkoa(-heebo.Askel);



            if (!Keyboard.IsKeyDown(Key.Right))      // poistetaan napin painallus listasta, jos nappi ei ole pohjassa
            {
                if (napitPainettuna.Contains(Key.Right)) napitPainettuna.Remove(Key.Right);
            }
            if (!Keyboard.IsKeyDown(Key.Left))      // poistetaan napin painallus listasta, jos nappi ei ole pohjassa
            {
                if (napitPainettuna.Contains(Key.Left)) napitPainettuna.Remove(Key.Left);
            }
            if (!Keyboard.IsKeyDown(Key.Space))      // poistetaan napin painallus listasta, jos nappi ei ole pohjassa
            {
                if (napitPainettuna.Contains(Key.Space)) napitPainettuna.Remove(Key.Space);
            }
        }


        #region EVENTS
        // Näppäinkomennot, ukon liikkuminen + ampuminen + esc + enterillä uusi peli
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space)) if (!napitPainettuna.Contains(Key.Space)) napitPainettuna.Add(Key.Space);   // ampuminen
            if (Keyboard.IsKeyDown(Key.Right)) if (!napitPainettuna.Contains(Key.Right)) napitPainettuna.Add(Key.Right);   // liike oikealle
            if (Keyboard.IsKeyDown(Key.Left)) if (!napitPainettuna.Contains(Key.Left)) napitPainettuna.Add(Key.Left);      // liike vasemmalle

            if (Keyboard.IsKeyDown(Key.Escape)) this.Close();   // esc lopettaa

            if (Keyboard.IsKeyDown(Key.Enter))                   // enter nappi aloittaa uuden pelin
            
                {
                    if (heebo.ElämätCounter == 0)   // Uuden pelin nollaukset
                    {
                        Level = -1; // Pelin aloitus uudestaan
                        heebo.ElämätCounter = heebo.ElämiäAlussa+1;   // nollataan elämät
                        heebo.PisteLaskuri =  0;

                        for (int i = 0; i < pallojaLuotu; i++)    // käydään läpi kaikki pallo-instanssit
                        {
                            // Tarkistetaan, onko kyseinen pallo jo poistettujen listalla, jos ei poistetaan                                           
                            loytyi = 0;
                            foreach (int line in poistetutPallot)
                            {
                                if (line == i)
                                {
                                    loytyi = 1;
                                }
                            }
                            if (loytyi == 0)
                            {
                                scene.Children.Remove(palloLista[i].Ball);  // poistetaan Ball canvasilta (scene)
                            }   
                                
                        }
                    }
                }
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)    // lasketaan ruudunleveys sen muuttuessa
        {
            RuudunLeveys = scene.ActualWidth;
        }

        
        // kun ikkuna on avattu, otetaan muuttujaan ruudun leveyden tieto, ja alustetaan muuttujia
        void MainWindow_Loaded(object sender, RoutedEventArgs e)    
        {
            RuudunLeveys = scene.ActualWidth;
            Level = 0;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        

        #endregion

        
    }
}


