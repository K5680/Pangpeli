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
        public Ellipse pallo = new System.Windows.Shapes.Ellipse();
        public double angle = 40;

        private static string latauskansio = "pack://application:,,,/Pang;component/Images/";  // määritellään kansio, josta kuvat ladataan
        public static string Latauskansio
        {
            get { return latauskansio; }
        }


        Ukko heebo = new pang.Ukko(); // luodaan Ukko-luokan instanssi, eli pelaaja

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);   // kutsutaan metodia, kun ikkuna on latautunut
            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);    // luodaan eventhandleri ikkunan koon muutokselle (en tiedä tarvitaanko lopulta)

        //pallo.Stroke = System.Windows.Media.Brushes.Red;
        pallo.Fill = System.Windows.Media.Brushes.SkyBlue;
            pallo.HorizontalAlignment = HorizontalAlignment.Left;
            pallo.VerticalAlignment = VerticalAlignment.Center;
            pallo.Width = 100;
            pallo.Height = 100;

            ImageBrush tekstuuri = new ImageBrush();                // kuva ladataan resursseista
            tekstuuri.ImageSource = new BitmapImage(new Uri(Latauskansio + "pallo.png", UriKind.Absolute));
            pallo.Fill = tekstuuri;
            scene.Children.Add(pallo);

            // ukon lisääminen sceneen
            heebo.LuoUkko();
            scene.Children.Add(heebo.kuutio);
            

            // Create a Timer with a highest priority
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Interval = TimeSpan.FromMilliseconds(10);// Set the Interval
            timer.Tick += new EventHandler(timer1_Tick);// Set the callback to invoke every tick time
            // Start the timer
            timer.Start();

            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);       // Set the Interval
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);      // Set the callback to invoke every tick time
            timer_pallo.Start();


        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)    // kun ikkuna on avattu, otetaan muuttujaan ruudun leveyden tieto
        {
                ruudunLeveys = scene.ActualWidth;
        }

        private void timerpallo_Tick(object sender, EventArgs e)
        {
            angle = angle + 0.1f;
            if (angle > 360) { angle = 0; }
            pallo_y = pallonKorkeus + Math.Cos(angle) * 140;

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            // pallo_x++;

            Canvas.SetLeft(pallo, pallo_x);
            Canvas.SetTop(pallo, pallo_y);
            //            string str = angle + "   " +(200 + Math.Sin(angle) * 70).ToString();        
            txtX.Text = ruudunLeveys.ToString();

        }


        // näppäinkomennot
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Right)
            {
                this.Title = "Go right";
                heebo.LiikutaUkkoa(15);
            }
            else if (e.Key == Key.Left)
            {
                this.Title = "Go left";
                heebo.LiikutaUkkoa(-15);
            }
            else if (e.Key == Key.Escape) // esc lopettaa
            {
                this.Close();
            }
        }

        // lasketaan ruudunleveys
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ruudunLeveys = scene.ActualWidth;
        }
            
    }
}
