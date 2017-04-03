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
//using System.Drawing;

namespace pang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        public static MainWindow instance { get; private set; } // tämän instanssin kautta voidaan kutsua MainWindow-luokan metodeita

        public static double ruudunLeveys;
        public string txt;
        private static string latauskansio = "pack://application:,,,/Pang;component/Images/";  // määritellään kansio, josta kuvat ladataan
        public static string Latauskansio
        {
            get { return latauskansio; }
        }


        // ukon lisääminen sceneen
        Ukko heebo = new Ukko(); // luodaan Ukko-luokan instanssi, eli pelaaja
        Ukko heebo2 = new Ukko(); // Kaksinpeliin toinen pelaaja

        public static MainWindow Main; //  tarvitaanko?
        public static int pallojaMax = 12;
        Pallo[] palloLista = new Pallo[pallojaMax]; // luodaan tarvittava määrä pallo-olioita


        public MainWindow()
        {
            InitializeComponent();
            instance = this;    // tämän kautta kutsutaan MainWindow-instanssin metodeita

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (en tiedä tarvitaanko lopulta)

            for (int i = 0; i < pallojaMax; i++)    // luodaan pallo-instanssit
            {
                palloLista[i] = new Pallo();
                palloLista[i].Numero = i + 1;

                if (i < 2) AddCanvasChild(palloLista[i].ball); // lisätään pallo-oliot sceneen (canvasiin), aluksi 2kpl
            }
            
            heebo.LuoUkko();    // luodaan pelaaja
            AddCanvasChild(heebo.pelaaja); // ja liitetään canvasiin

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            

            // heebo2.LuoUkko();    // luodaan pelaaja nro 2 
            // AddCanvasChild(heebo2.pelaaja); // ja liitetään canvasiin
        }

        // tällä metodilla saadaan lisättyä canvakseen elementti toisen luokan kautta
        public void AddCanvasChild(UIElement child)
        {
            scene.Children.Add(child);
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

                    System.Diagnostics.Debug.WriteLine(Latauskansio+"fire.mp3"); // debuggia
                    break;
                case "jokumuu":
                   
                    break;
            }
                mediaElementti.Play();  // soita ääni
        }


        #region EVENTS
        // näppäinkomennot
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
