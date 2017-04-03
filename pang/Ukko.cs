using pang.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
       
        public Rectangle pelaaja; // laatikko, jonka päälle pelaajahahmo rakentuu
        public Rect ukkoPuskuri = new Rect(); // laatikko, joka toimii alueena, jolta törmäys tunnistetaan
        private double sijaintix;
        private double sijaintiy = 350;
        public static int ammuksiaMax = 10;             // ammusten maksimimäärä ruudulla
        public static Ammus[] ammusLista = new Ammus[ammuksiaMax];    // ammus-lista
        private int ammusIlmassaNro = 0;

        public double SijaintiY
        {
            get
            {
                return sijaintiy;
            }
            set
            {
                sijaintiy = value;
            }
        }


        public double SijaintiX
        {
            get
            {
                return sijaintix;
            }
            set
            {
                if (value > -10 && value < MainWindow.ruudunLeveys-30) sijaintix = value; // ei anneta ukon x-arvon mennä yli reunojen
            }
        }
        public double Askel{ get; set; }
           
        public double UkonNopeus { get; set; }
        public int Elämät { get; set; }
        public DispatcherTimer timer_ukko; // ukon ajastin
        public bool SaakoLiikkua;
        public bool Osuuko { get; set; }

        public Ukko()
        {
            sijaintix = 350;
            Osuuko = false;
            Askel = 10;
            UkonNopeus = 50; // millisekunnit
            Elämät = 3;
            pelaaja = new System.Windows.Shapes.Rectangle();    // pelaajan hahmon pohjaksi luodaan rectangle
            LuoUkko();

            }
        public void LiikutaUkkoa(double sij)
        {
            // jos taimeri sallii, niin liikutetaan, eikä ukkoon ole osunut
            if (SaakoLiikkua && !Osuuko)
            {
                SijaintiX = SijaintiX + sij;  // liikutetaan 
                SaakoLiikkua = false;         // tällä rajataan timerin kautta, että liikutaan tietyllä nopeudella
            }

            Canvas.SetTop(pelaaja, SijaintiY);
            Canvas.SetLeft(pelaaja, SijaintiX);

            // törmäyksen tunnistusta varten tehty rect liikkuu pelaajahahmon mukana
            ukkoPuskuri.X = Canvas.GetLeft(pelaaja) + 28;
            ukkoPuskuri.Y = Canvas.GetTop(pelaaja) + 35;
            ukkoPuskuri.Height = pelaaja.ActualHeight;

            var apu = pelaaja.ActualWidth;  // pelaajaa luodessa actualWidth on 0, joka ei käy. Siksi tämä vertailu... Onko muuta keinoa?
            if (apu > 0)
            {
                ukkoPuskuri.Width = apu-55;
            }
            else
            {
                ukkoPuskuri.Width = 15;
            }         

            System.Diagnostics.Debug.WriteLine(" " + pelaaja.ActualWidth); // debuggia
        }


        public void LuoUkko()
        {          
            // luodaan ukon hahmo
            ImageBrush kuva = new ImageBrush();     // ladataan kuva, joka liimataan liikuteltavan laatikon päälle
            kuva.ImageSource = new BitmapImage(new Uri(pang.MainWindow.Latauskansio + "ukko.png", UriKind.Absolute)); // 
            pelaaja.HorizontalAlignment = HorizontalAlignment.Left;
            pelaaja.VerticalAlignment = VerticalAlignment.Center;
            pelaaja.Width = 80;  // määritellään laatikon (pelaajahahmon) koko
            pelaaja.Height = 120;
            pelaaja.Fill = kuva; // maalataan ukko-tekstuuri laatikon päälle
            LiikutaUkkoa(0);    // piirtää ukon (laatikon) ruutuun kertaalleen, muuten se on pelin alkaessa nurkassa
            SaakoLiikkua = true;
            Askel = 5;
            //MainWindow.instance.AddCanvasChild(pelaaja);          // tämä tehdään pääkoodissa, koska ei onnistu täällä?
            //ajastin
            timer_ukko = new DispatcherTimer(DispatcherPriority.Send);
            timer_ukko.Interval = TimeSpan.FromMilliseconds(UkonNopeus);       // Set the Interval
            timer_ukko.Tick += new EventHandler(timerukko_Tick);      // Set the callback to invoke every tick time
            timer_ukko.Start();

            // luodaan ammus-instanssit
/*            for (int i = 0; i < ammuksiaMax; i++)
            {
                ammusLista[i] = new Ammus();
                ammusLista[i].AmmusNro = i + 1;
                
            }*/

        }


        private void timerukko_Tick(object sender, EventArgs e)
        {
            SaakoLiikkua = true;

            // mitä tehdään, jos ukkoon osuu pallo
            if (Osuuko == true)
            {
                // ukkoon osui, joten tippuu alas, jonka jälkeen nollataan sijainti ja vähennetään yksi elämä
                sijaintiy += 10;
                           
                LiikutaUkkoa(0);   // ukon liikutus ja paikan päivitys

                if (sijaintiy > MainWindow.instance.Height)
                {
                    Elämät -= 1;    // vähennetään elämä
                    Osuuko = false; // nollataan sijainti
                    sijaintix = 350;
                    sijaintiy = 350;
                    LiikutaUkkoa(0);   // ukko piirtyy uudestaan ruutuun
                }

            }
        }


        public void Ammu()
        {
         
            // TOIMII YHDELLÄ, PITÄISI SAADA VAIHTUMAAN SEURAAVAAN INSTANSSIIN
            
            // ammukset...
            if (SaakoLiikkua && !Osuuko)    // jos on liikkumislupa, eikä ole pallo osunut ukkoon, niin voidaan ampua
            {
                MainWindow.instance.Soita("ampu");    // soita ampu-soundi

                if (ammusIlmassaNro < 10)
                {  // ammutaan ammuksia vuorotellen, maksimissaan 10 ilmassa    
                    ammusLista[ammusIlmassaNro] = new Ammus();
                    //ammusLista[ammusIlmassaNro].AmmusNro = ammusIlmassaNro

                    ammusLista[ammusIlmassaNro].LiikutaAmmusta();
                    ammusLista[ammusIlmassaNro].AmmusX = sijaintix + 60;
                    ammusIlmassaNro += 1;
                    
                }
                
            }
        }

    }
}
