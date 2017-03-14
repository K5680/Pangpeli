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
  

        public double pallo_x = 200;
        public double pallo_y = 200;
        public double pallonKorkeus = 180;
        public static double ruudunLeveys;

        public string txt;
        
        public Ellipse pallo = new Ellipse();
        public double angle = 40;

        

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
        
        public static MainWindow Main; //  tarvitaanko?

        public MainWindow()
        {

            InitializeComponent();
            

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (en tiedä tarvitaanko lopulta)

            pallo.Stroke = System.Windows.Media.Brushes.Red;
            pallo.Fill = System.Windows.Media.Brushes.SkyBlue;
            pallo.HorizontalAlignment = HorizontalAlignment.Left;
            pallo.VerticalAlignment = VerticalAlignment.Center;
            pallo.Width = 110;
            pallo.Height = 110;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(Latauskansio + "pallo.png", UriKind.Absolute));
            pallo.Fill = tekstuuri;
            scene.Children.Add(pallo);


            heebo.LuoUkko();    // luodaan pelaaja
            AddCanvasChild(heebo.pelaaja); // ja liitetään canvasiin
            heebo2.LuoUkko();    // luodaan pelaaja nro 2 
            AddCanvasChild(heebo2.pelaaja); // ja liitetään canvasiin


            // pallon ajastin
            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main = this; //(2) Defined Main (IMP)
        }


        private void timerpallo_Tick(object sender, EventArgs e)
        {
            // pallon liikutus sinikäyrällä
            angle = angle + 0.1f;
            if (angle > 360) { angle = 0; }
            pallo_y = pallonKorkeus + Math.Cos(angle) * 140;
            
            // törmäyksen tunnistus Rect:illä
            var x2 = Canvas.GetLeft(pallo);
            var y2 = Canvas.GetTop(pallo);
            Rect r2 = new Rect(x2, y2, pallo.ActualWidth, pallo.ActualHeight);
            if (heebo.ukkoPuskuri.IntersectsWith(r2)) System.Diagnostics.Debug.WriteLine("OSUU !! "+pallo_y); // debuggia


            // törmäyksen tunnistus etäisyyden mukaan, kumpi parempi? Onko vaihtoehtoja?
            double xet = Canvas.GetLeft(pallo); //pallon x
            double yet = Canvas.GetTop(pallo); // pallon y
            double D = Math.Sqrt((xet - heebo.SijaintiX) * (xet - heebo.SijaintiX) + (yet - 350) * (yet - 350));

            if (D < (pallo.Height))
            {
                var a = Console.ReadLine();
                System.Diagnostics.Debug.WriteLine("Pallon etäisyysmittarisysteemillä osuu... " + D + " " + yet); // debuggia
            }


            // pallo_x++;
            Canvas.SetLeft(pallo, pallo_x);
            Canvas.SetTop(pallo, pallo_y);
            
            txtX.Text = ruudunLeveys.ToString();
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
