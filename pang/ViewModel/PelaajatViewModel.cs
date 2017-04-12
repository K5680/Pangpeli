using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pang.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace pang.ViewModel
{
    //
    //  Pelaajat - ViewModel. Alkuvalikon pelaajien listaukseen/hallintaan
    //

    [Serializable]
    class PelaajatViewModel
    {
        public string Polku { get; set; }
        
        public ObservableCollection<Pelaajat> Pelaajat
        {
            get;
            set;
        }

        
    public void LataaPelaajat()
            {
                ObservableCollection<Pelaajat> pelaajat = new ObservableCollection<Pelaajat>();               

                // nykyinen hakemisto talteen
                Polku = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            try
            {
                // jollei players-kansiota ole, luodaan se
                if (!Directory.Exists(Polku + @"\players"))
                {
                    Directory.CreateDirectory(Polku + @"\players");
                }
                
                // jollei Players.bin:iä ole, luodaan se, ja laitetaan tiedostoon pari pelaaja-oliota
                if (!File.Exists(Polku + @"\players\"+"Players.bin"))
                {
                    pelaajat.Add(new Pelaajat { PlayerName = "Guest Player", PlayerPoints = 0 });

                    // open stream for writing objects
                    Stream writeStream = new FileStream(Polku + @"\players\Players.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                    // use binary formatted                    
                    IFormatter formatter = new BinaryFormatter();
                    // kirjoitetaan pelaajat Players.bin:iin
                    formatter.Serialize(writeStream, pelaajat);
                    writeStream.Close();
                }
                else  // jos tiedosto on jo olemassa, luetaan sieltä pelaajat
                {
                    // luodaan stream pelaajien lataamiseen levyltä
                    Stream readStream = new FileStream(Polku + "\\players\\Players.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                    IFormatter formatter = new BinaryFormatter();
                    pelaajat = (ObservableCollection<Pelaajat>)formatter.Deserialize(readStream);
                    readStream.Close();
                }
            }           // jos levytoiminnot ei onnistu, näytetään poikkeus
            catch (Exception e)
            {
                MessageBox.Show("Disk read error: " + e.ToString() + "\n" + Polku +"\\players\\Players.bin");
            }

            Pelaajat = pelaajat;
        }


        
        public void TalletaPelaajat(string nimi, int pisteet)       // uusien pelaajien tallennus levylle
        {
            ObservableCollection<Pelaajat> pelaajat = new ObservableCollection<Pelaajat>();

            try
            {
                // luodaan stream pelaajien lataamiseen levyltä
                Stream readStream = new FileStream(Polku + "\\players\\Players.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                IFormatter formatter = new BinaryFormatter();
                pelaajat = (ObservableCollection<Pelaajat>)formatter.Deserialize(readStream);
                readStream.Close();

                // lisätään alkuvalikossa tehty uusi pelaaja listaan
                pelaajat.Add(new Pelaajat { PlayerName = nimi, PlayerPoints = pisteet });
                               
                Stream writeStream = new FileStream(Polku + @"\players\Players.bin", FileMode.Create, FileAccess.Write, FileShare.None);

                // kirjoitetaan pelaajat Players.bin:iin
                formatter.Serialize(writeStream, pelaajat);
                writeStream.Close();
            }
            catch (Exception e) // jos levytoiminnot ei onnistu, näytetään poikkeus
            {
                MessageBox.Show("Disk read error: " + e.ToString());
            }

        }


        public void PoistaPelaajat(int indeksi)       // pelaajan poisto
        {
            ObservableCollection<Pelaajat> pelaajat = new ObservableCollection<Pelaajat>();

            try
            {
                // luodaan stream pelaajien lataamiseen levyltä
                Stream readStream = new FileStream(Polku + "\\players\\Players.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                IFormatter formatter = new BinaryFormatter();
                pelaajat = (ObservableCollection<Pelaajat>)formatter.Deserialize(readStream);
                readStream.Close();

                // poistetaan pelaaja indeksi mukaan
                pelaajat.RemoveAt(indeksi);

                // uuden kokoelman tallennus
                Stream writeStream = new FileStream(Polku + @"\players\Players.bin", FileMode.Create, FileAccess.Write, FileShare.None);

                // kirjoitetaan pelaajat Players.bin:iin
                formatter.Serialize(writeStream, pelaajat);
                writeStream.Close();
            }
            catch (Exception e) // jos levytoiminnot ei onnistu, näytetään poikkeus
            {
                MessageBox.Show("Disk read error: " + e.ToString());
            }

        }

    }
}