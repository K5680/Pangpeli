using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace pang
{
    /// <summary>
    /// Pelin käynnistysruutu
    /// </summary>
    public partial class StartGame : Window, INotifyPropertyChanged
    {

        private int width;      // ikkunan leveys muuttuu -> muuttuja
        public int CustomWidth
        {
            get { return width; }
            set
            {
                if (value != width)
                {
                    width = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("CustomWidth"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public StartGame()
        {
            InitializeComponent();
            this.DataContext = this;
            CustomWidth = 300;  // ikkunan leveyden määrittely            
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            // Käynnistetään itse peli
            CustomWidth = 300;  // ikkunan leveyden palautus            

            MainWindow mainWindow = new MainWindow();            
            mainWindow.Show();
        }


        // Highscore -nappi, lataa tulokset
        private void btnHighScores_Click(object sender, RoutedEventArgs e)
        {
            CustomWidth = 600;          // levennetään ikkuna highscoren näyttämistä varten

            string polku = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\players";
            string[] lista;
            string EnkatNimet = "";
            string EnkatPisteet = "";
            int rivi = 0;

            try
            {
                Highscore hig = new Highscore();
                hig.LataaTiedot();  // tietojen alustus, luo tiedoston ja tallennuskansion jos niitä ei ole

                lista = System.IO.File.ReadAllLines(polku + @"\Highscores.bin");    // ladataan ennätykset levyltä

                foreach (string line in lista)
                {
                    if (rivi % 2 == 0)
                    {
                        EnkatNimet = EnkatNimet + line + "\n";      // toiseen tauluun nimet (joka toinen rivi tiedostosta)
                    }
                    else
                    {
                        EnkatPisteet = EnkatPisteet + line + "\n";  // joka toiseen pisteet
                    }
                    rivi++;
                }
            }
            catch (Exception ex)     // jos levytoiminnot ei onnistu, näytetään poikkeus
            {
                MessageBox.Show("Disk error: " + ex);
            }

            txtHighscoreNames.Text = EnkatNimet;      // enkat ruutuun
            txtHighscorePoints.Text = EnkatPisteet;      // enkat ruutuun
        }
    }
}

