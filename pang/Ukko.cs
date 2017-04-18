﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace pang
{
     class Ukko
    {      
       
        public static string NykyinenPelaaja;   // pitää tiedon alkuvalikosta valitusta pelaajasta

        public Rectangle pelaaja; // laatikko, jonka päälle pelaajahahmo rakentuu
        public Rect ukkoPuskuri = new Rect(); // laatikko, joka toimii alueena, jolta törmäys tunnistetaan
        private double sijaintix;
        private double sijaintiy = 350;

        public int ammuksiaMax = 2;            // ammusten maksimimäärä ruudulla
        public int ammusTiheys = 700;      // kuinka nopeasti voi ampua uuden

        public int Pisteet { get; set; }

        public static List<Ammus> ammukset = new List<Ammus>();    // ammus-lista   
        private int ammusIlmassaNro = 0;                           // pitää yllä tietoa ammusten numeroista
        public double ammusKohta = 57;
        public int bonusTaso = 0;                  // Bonukset lähtee nollasta.

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
        public bool SaakoAmpua;
        public bool Osuuko { get; set; }

        Highscore enkka = new Highscore();  // highscore-lista

        public Ukko()
        {
            sijaintix = 350;
            Pisteet = 0;
            Osuuko = false;
            Askel = 10;
            UkonNopeus = 50; // millisekunnit
            Elämät = 10;
            pelaaja = new Rectangle();    // pelaajan hahmon pohjaksi luodaan rectangle
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

            var apu = pelaaja.ActualWidth;  // pelaajaa luodessa actualWidth on 0, joka ei käy. Siksi tämä vertailu... 
            if (apu > 0)
            {
                ukkoPuskuri.Width = apu-55;
            }
            else
            {
                ukkoPuskuri.Width = 15;
            }
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
            Askel = 15;

            // AJASTIMET PELAAJALLE
            // Liikkumisen ajastin
            timer_ukko = new DispatcherTimer(DispatcherPriority.Send);
            timer_ukko.Interval = TimeSpan.FromMilliseconds(UkonNopeus);       // Set the Interval
            timer_ukko.Tick += new EventHandler(timerukko_Tick);      // Set the callback to invoke every tick time
            timer_ukko.Start();

            // Ampumistiheyden ajastin
            DispatcherTimer timer_tiheys = new DispatcherTimer(DispatcherPriority.Send);
            timer_tiheys.Interval = TimeSpan.FromMilliseconds(ammusTiheys);    // asetetaan intervalli, jolla voi ampua
            timer_tiheys.Tick += new EventHandler(timertiheys_Tick);      // Set the callback to invoke every tick time
            timer_tiheys.Start();
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

                if (sijaintiy > MainWindow.instance.Height) // Kun ukko on tippunut ruudun alapuolelle...
                {                   
                    // jos peli ei vielä loppunut, nollataan
                    if (Elämät > 1)
                    {
                        Elämät -= 1;        // vähennetään elämä
                        Osuuko = false;
                        sijaintix = 350;    // nollataan sijainti
                        sijaintiy = 350;
                        LiikutaUkkoa(0);    // ukko piirtyy uudestaan ruutuun                        
                    }
                    else if (Elämät > 0)    // Jos peli loppui asetetaan lopuksi elämät nollaan ja tallennetaan highscore
                    {
                        Elämät -= 1;                        // vähennetään elämä     
                        enkka.TallennaPisteet(Pisteet);     // tallennetaan highscore
                    }
                }
            }
        }


        public void Ammu()
        {
                   
            // ammukset...
            if (SaakoLiikkua && !Osuuko && SaakoAmpua)    // jos on liikkumislupa, ampumislupa, eikä ole pallo osunut ukkoon, niin voidaan ampua
            {

                if (ammukset.Count < ammuksiaMax+1) // ammutaan ammuksia, maksimissaan 10 ilmassa    
                {
                    if (bonusTaso > 6)  // tietyllä bonustasolla ammuksia alkaa lentää leveämmältä alalta
                    {
                        if (ammusIlmassaNro % 2 == 0)
                        {
                            ammusKohta = 35;
                        }
                        else
                        {
                            ammusKohta = 57;
                        }
                    }

                    ammukset.Add(new Ammus{ AmmusY = 370, AmmusX = sijaintix + ammusKohta, AmmuksenNopeus = 10, SaaAmpua = true, AmmusNro = ammusIlmassaNro});
                    ammusIlmassaNro += 1;
                    if (ammusIlmassaNro == 10) ammusIlmassaNro = 0; // ammus-instanssin numeroa kierrätetään 1-10

                    System.Diagnostics.Debug.WriteLine("  AMMUTAAN, COUNT " + ammukset.Count); // debuggia
                    MainWindow.instance.Soita("ampu");    // soita ampu-soundi
                }

                SaakoAmpua = false; // saa ampua vasta kun timer antaa luvan
            }
        }



        // nollataan ajastimella ampumistiheys
        private void timertiheys_Tick(object sender, EventArgs e)
        {
            SaakoAmpua = true;
        }

    }
}
