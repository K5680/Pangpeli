using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using System.Windows.Threading;


namespace pang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double x = 100;
        public double pallo_x = 200;
        public double pallo_y = 70;
        public int y = 10;
        public string txt;
        public Ellipse pallo = new System.Windows.Shapes.Ellipse();
        public Rectangle kuutio = new System.Windows.Shapes.Rectangle();
        public double angle = 40;

        public MainWindow()
        {
            InitializeComponent();

            //pallo.Stroke = System.Windows.Media.Brushes.Red;
            pallo.Fill = System.Windows.Media.Brushes.SkyBlue;
            pallo.HorizontalAlignment = HorizontalAlignment.Left;
            pallo.VerticalAlignment = VerticalAlignment.Center;
            pallo.Width = 50;
            pallo.Height = 50;
            // ladataan kuva
            Image kuva = new Image();
            kuva.Source = new BitmapImage(new Uri("e:\\otsikko.png", UriKind.Relative));

            ImageBrush tekstuuri = new ImageBrush();
            tekstuuri.ImageSource = new BitmapImage(new Uri("e:\\pallo.png", UriKind.Relative));
            pallo.Fill = tekstuuri;
            //pallo.Fill = new SolidColorBrush(Colors.Red);
            

            scene.Children.Add(pallo);

            kuutio.Stroke = System.Windows.Media.Brushes.Black;
            kuutio.Fill = System.Windows.Media.Brushes.SkyBlue;
            kuutio.HorizontalAlignment = HorizontalAlignment.Left;
            kuutio.VerticalAlignment = VerticalAlignment.Center;
            kuutio.Width = 50;
            kuutio.Height = 50;
            scene.Children.Add(kuutio);
            Canvas.SetTop(kuutio, 250);
            Canvas.SetLeft(kuutio, x);

       
            // Create a Timer with a highest priority
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Interval = TimeSpan.FromMilliseconds(10);// Set the Interval to 2 seconds
            timer.Tick += new EventHandler(timer1_Tick);// Set the callback to invoke every tick time
            // Start the timer
            timer.Start();


            DispatcherTimer timer_pallo = new DispatcherTimer(DispatcherPriority.Send);
            // Set the Interval to 2 seconds
            timer_pallo.Interval = TimeSpan.FromMilliseconds(50);
            // Set the callback to invoke every tick time
            timer_pallo.Tick += new EventHandler(timerpallo_Tick);
            // Start the timer
            timer_pallo.Start();



        }



        private void timerpallo_Tick(object sender, EventArgs e)
        {
            angle = angle + 0.1f;
            if (angle > 360) { angle = 0; }
            pallo_y = 170 + Math.Cos(angle) * 70;
            //pallo_x = pallo_y + Math.Cos(angle) * 50;

        
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            // pallo_x++;
            
            Canvas.SetLeft(pallo, pallo_x);
            Canvas.SetTop(pallo, pallo_y);
            //  Graphics.DrawImage(new BitmapImage(), x, y, 32);


            //    string stry = (pallo_y + Math.Sin(angle) * 150).ToString();
            //  string strx = (pallo_x + Math.Sin(angle) * 150).ToString();
            string str = angle + "   " +(200 + Math.Sin(angle) * 70).ToString();
            txtX.Text = ":  " + str;


        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // ... Test for F5 key.
            if (e.Key == Key.F5)
            {
                this.Title = "You pressed F5";
            }
            else if (e.Key == Key.Right)
            {
                this.Title = "right";
                x = x + 10;
                Canvas.SetLeft(kuutio, x);
            }
            else if (e.Key == Key.Left)
            {
                this.Title = "left";
                x = x - 10;
                Canvas.SetLeft(kuutio, x);               
            }
            else if (e.Key == Key.Escape) // esc lopettaa
            {
                this.Close();
            }

        }
    }



}
