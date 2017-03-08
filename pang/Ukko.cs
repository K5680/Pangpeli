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
        private const double sijaintiy = 350;   // y:tä ei ehkä tarvitse muuttaa?
        
        public Rectangle kuutio; // laatikko, jonka päälle pelaajahahmo rakentuu

        private double sijaintix;
        public double SijaintiX
        {
            get
            {
                return sijaintix;
            }
            set
            {
                if (value > -10 && value < pang.MainWindow.ruudunLeveys-30) sijaintix = value; // ei anneta ukon x-arvon mennä yli reunojen
            }
        }

        public double UkonNopeus { get; set; }
        public int Elämät { get; set; }


      
        public Ukko()
        {
            sijaintix = 350;
            UkonNopeus = 10;
            Elämät = 3;
            kuutio = new System.Windows.Shapes.Rectangle();
            LuoUkko();

        }

        public void LiikutaUkkoa(double sij)
        {
            SijaintiX = SijaintiX + sij;  // liikutetaan 
            Canvas.SetTop(kuutio, sijaintiy);
            Canvas.SetLeft(kuutio, SijaintiX);

            System.Diagnostics.Debug.WriteLine(sijaintix); // debuggia
        }

        
        public void LuoUkko()
        {          
            // luodaan ukon hahmo
            ImageBrush kuva = new ImageBrush();     // ladataan kuva, joka liimataan liikuteltavan laatikon päälle
            kuva.ImageSource = new BitmapImage(new Uri(pang.MainWindow.Latauskansio + "ukko.png", UriKind.Absolute)); // 
            //kuutio.Fill = System.Windows.Media.Brushes.SkyBlue;
            kuutio.HorizontalAlignment = HorizontalAlignment.Left;
            kuutio.VerticalAlignment = VerticalAlignment.Center;
            kuutio.Width = 50;  // määritellään laatikon koko
            kuutio.Height = 80;
            kuutio.Fill = kuva; // maalataan laatikko "ukko.png":llä
            LiikutaUkkoa(0);    // piirtää ukon (laatikon) ruutuun
            // scene.Children.Add(kuutio);          // tämä tehdään pääkoodissa, koska ei onnistu täällä?
        }

        public void Ammu()
        {

        }

    }
}
