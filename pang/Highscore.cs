using System;
using System.IO;
using System.Windows;

namespace pang
{
    class Highscore
    {
        private string polku = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\players";
        private string[] text;
        // tallentaa nykyisen pelaajan pisteet Highscoreen
        public Highscore()
        {
        }


        public void LataaTiedot()
        {
            try
            {
                // jollei players-kansiota ole, luodaan se
                if (!Directory.Exists(polku)) Directory.CreateDirectory(polku);

                // jollei Players.bin:iä ole, luodaan se, laitetaan jotain tuloksia
                if (!File.Exists(polku + @"\Highscores.bin"))
                {
                    string[] lines = { "AAA", "100", "BBB", "200", "CCC", "300", "DDD", "400", "EEE", "500", "FFF", "600", "GGG", "700", "HHH", "800", "III", "900", "JJJ", "1000" };
                    File.WriteAllLines(polku + @"\Highscores.bin", lines);
                }

                text = System.IO.File.ReadAllLines(polku + @"\Highscores.bin");    // ladataan ennätykset levyltä
            }
            catch (Exception e)     // jos levytoiminnot ei onnistu, näytetään poikkeus
            {
                MessageBox.Show("Disk read error: " + e.ToString() + "\n" + polku + "\\players\\Highscores.bin");
            }
            
        }


        public void TallennaPisteet(int Pistemäärä)
        {
            LataaTiedot();  // ladataan ensin aiemmat tiedot

            try
                {
                    string[,] nronimi;           // Taulukot ennätyslistaa varten    (1)   Nro & Nimi -taulukko -string tyyppinen
                    int[,] nropisteet;           // Taulukot ennätyslistaa varten    (2)   Nro & Pisteet -taulukko  -int tyyppinen
                    nronimi = new string[11, 2];
                    nropisteet = new int[11, 2];

                    int tt = 0;
                    int indeksinro = 0;          // Erityyppisten (int & string) taulukoiden tiedot yhdistetään lopuksi indeksinumeron avulla
                    foreach (string line in text)
                    {
                        if (tt % 2 == 0)    //  pariton / parillinen
                        {
                            nronimi[indeksinro, 0] = line;              // nimi tähän soluun
                            nronimi[indeksinro, 1] = indeksinro.ToString();     // indeksinumero tähän soluun
                            nropisteet[indeksinro, 1] = indeksinro;  // indeksinumero tähän soluun
                            indeksinro++;
                            //                                System.Diagnostics.Debug.WriteLine(line + " line"); // debuggia
                        }
                        else
                        {
                            nropisteet[indeksinro - 1, 0] = Int32.Parse(line);  // pisteet tähän soluun
                        }
                        tt++;
                    }

                    nronimi[indeksinro, 0] = Ukko.NykyinenPelaaja;   // lisätään uusi tulos
                    nronimi[indeksinro, 1] = indeksinro.ToString();     // indeksinumero tähän soluun
                    nropisteet[indeksinro, 1] = indeksinro;  // indeksinumero tähän soluun
                    nropisteet[indeksinro, 0] = Pistemäärä; //heebo.Pisteet;

                    // lajitellaan ja tallennetaan 10 parasta
                    BubbleSort(nropisteet, nronimi, polku);

                }
                catch (FileNotFoundException)   // jos tiedostoa ei kuitenkaan löydy, näytetään poikkeus
                {
                    Console.WriteLine("File not found (FileNotFoundException)");
                }


        }


        // Bubblesortilla ennätyslistan sorttaus + lopussa tallennus levylle
        public void BubbleSort(int[,] intArray, string[,] stringArray, string hakemistoPolku)
        {
            for (int i = intArray.Length / 2 - 1; i > 0; i--)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    if (intArray[j, 0] > intArray[(j + 1), 0])
                    {
                        int highValue = intArray[j, 0];
                        int highValue2 = intArray[j, 1];

                        intArray[j, 0] = intArray[(j + 1), 0];
                        intArray[j, 1] = intArray[(j + 1), 1];
                        intArray[(j + 1), 0] = highValue;
                        intArray[(j + 1), 1] = highValue2;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("sorted:----------------------  "); // debuggia
            string[,] aputaulu;
            aputaulu = new string[11, 2];

            // kopioidaan tuloslista oikeassa järjestyksessä
            for (int i = 1; i < intArray.Length / 2; i++)
            {                
                aputaulu[i, 0] = intArray[i, 0].ToString();     // koostetaan tulostaulu int- ja string -tauluista -> tässä talteen pisteet          

                // Etsitään indeksiä vastaava nimi string -taulukosta
                for (int k = intArray.Length / 2 - 1; k > -1; k--)
                {
                    try
                    {
                        int jk;
                        if (Int32.TryParse(stringArray[k, 1], out jk))
                        {
                            if (jk == intArray[i, 1])
                            {

                                aputaulu[i, 1] = stringArray[k, 0];     // koostetaan tulostaulu int- ja string -tauluista  -> nimi talteen
                                System.Diagnostics.Debug.WriteLine("  ** nimi: " + aputaulu[i, 1] + "indeksi k " + k);
                            }
                        }
                    }
                    catch (FormatException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }

            try
            {
                // LOPULLISEN LISTAN TULOSTUS -> LEVYLLE
                System.IO.StreamWriter outputFile = new System.IO.StreamWriter(hakemistoPolku + @"\Highscores.bin");
                for (int im = 10; im > 0; im--)
                {
                    System.Diagnostics.Debug.WriteLine("indeksi: " + im + "tulos:  " + aputaulu[im, 0] + "   " + aputaulu[im, 1]);
                    outputFile.WriteLine(aputaulu[im, 1]);
                    outputFile.WriteLine(aputaulu[im, 0]);
                }
                outputFile.Close();
            }
            catch (Exception e)     // jos levytoiminnot ei onnistu, näytetään poikkeus
            {
                MessageBox.Show("Disk write error: " + e.ToString() + "\n" + hakemistoPolku + "\\players\\Highscores.bin");
            }
        }

    }
}

