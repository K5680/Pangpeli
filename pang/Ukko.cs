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
     class Ukko
    {
        private const double sijaintiy = 250;

        public Rectangle kuutio;

        public double SijaintiX { get; set; }
        public double UkonNopeus { get; set; }
        public int Elämät { get; set; }

        private static string latauskansio = "pack://application:,,,/Pang;component/Images/";

        public static string Latauskansio
        {
            get { return latauskansio; }
            set {}
        }
      
        public Ukko()
        {
            SijaintiX = 100;
            UkonNopeus = 10;
            Elämät = 3;
            kuutio = new System.Windows.Shapes.Rectangle();
            LuoUkko();

        }

        public void LiikutaUkkoa()
        {
            Canvas.SetTop(kuutio, sijaintiy);
            Canvas.SetLeft(kuutio, SijaintiX);
            System.Diagnostics.Debug.WriteLine(SijaintiX); // debuggia
        }


        public void LuoUkko()
        {          
            // luodaan ukon hahmo
            ImageBrush kuva = new ImageBrush();
            kuva.ImageSource = new BitmapImage(new Uri(latauskansio + "ukko.png", UriKind.Absolute)); // new BitmapImage(new Uri("kuvat.otsikko", UriKind.Relative));
            kuutio.Fill = System.Windows.Media.Brushes.SkyBlue;
            kuutio.HorizontalAlignment = HorizontalAlignment.Left;
            kuutio.VerticalAlignment = VerticalAlignment.Center;
            kuutio.Width = 50;
            kuutio.Height = 80;
            kuutio.Fill = kuva;
            LiikutaUkkoa();            
            //scene.Children.Add(kuutio);          // tämä tehdään pääkoodissa, koska ei onnistu täällä?
        }

        public void Ammu()
        {

        }

    }
}
