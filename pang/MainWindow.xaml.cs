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

        // tällä metodilla saadaan lisättyä canvakseen elementti toisen luokan kautta
        public void AddCanvasChild(UIElement child)
        {
            scene.Children.Add(child);
        }
       
        // ukon lisääminen sceneen
        Ukko heebo = new Ukko(); // luodaan Ukko-luokan instanssi, eli pelaaja
        Ukko heebo2 = new Ukko(); // Kaksinpeliin toinen pelaaja
        Pallo balli = new Pallo();
        Pallo balli1 = new Pallo();
        Pallo balli2 = new Pallo();
        Pallo balli3 = new Pallo();
        public static MainWindow Main; //  tarvitaanko?
            
        public MainWindow()
        {
            
            InitializeComponent();
            instance = this;    // tämä kautta kutsutaan MainWindow-instanssin metodeita

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (en tiedä tarvitaanko lopulta)

            heebo.LuoUkko();    // luodaan pelaaja
            AddCanvasChild(heebo.pelaaja); // ja liitetään canvasiin


            balli.LuoPallo();
            balli.PalloX = 100;
            AddCanvasChild(balli.ball); // ja liitetään canvasiin

            balli1.LuoPallo();
            balli1.PalloX = 200;
            AddCanvasChild(balli1.ball); // ja liitetään canvasiin

            balli2.LuoPallo();
            AddCanvasChild(balli2.ball); // ja liitetään canvasiin

            balli3.LuoPallo();
            AddCanvasChild(balli3.ball); // ja liitetään canvasiin


            // heebo2.LuoUkko();    // luodaan pelaaja nro 2 
            // AddCanvasChild(heebo2.pelaaja); // ja liitetään canvasiin

        }


        public void Soita(string ääni)
        {
            switch (ääni)   // valitse soitettava ääni
            {
                case "ampu":    // lataa ääni
                    mediaElementti.Source = new Uri("C:\\Users\\Vesada\\Source\\Repos\\Pangpeli - Harjoitustyö\\pang\\Images\\fire.mp3", UriKind.RelativeOrAbsolute);         
                    break;
                case "jokumuu":
                   
                    break;
            }
                mediaElementti.Play();  // soita ääni
        }


        // näppäinkomennot
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // player ONE
            if (e.Key == Key.Right)
            {
                this.Title = "Go right";
                heebo.LiikutaUkkoa(heebo.Askel);
            }

            if (e.Key == Key.Left)
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


    }
}
