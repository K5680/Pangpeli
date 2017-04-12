using pang.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pang
{
    /// <summary>
    ///           Players.cs        Pelaajien luominen / lataaminen ennen peliä
    /// </summary>
    /// 
    public partial class Players: Window, INotifyPropertyChanged
    {
        ViewModel.PelaajatViewModel pvm = new ViewModel.PelaajatViewModel();

        private int width;
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

        public Players()
        {
            InitializeComponent();
            
            pvm.LataaPelaajat(); // lataa pelaajat
            myGrid.DataContext = pvm.Pelaajat;

            this.DataContext = this;
            CustomWidth = 300;  // ikkunan leveyden määrittely

            // textboxin tyhjennys, kun laatikko valitaan
            txtPlayerName.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(txtPlayerName_MouseLeftButtonDown), true);

            txtMessage.Text = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;       


        private void btnExitPlayer_Click(object sender, RoutedEventArgs e)
        {
            // Sovelluksen sammutus
            Application.Current.Shutdown();
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // OK -napilla kuitataan valittu pelaaja, ja jatketaan, ellei pelaaja ole tyhjä, MUTTA
            // Jos ollaan Delete player -moodissa, poistetaan pelaaja listasta
            Model.Pelaajat valittu = (Model.Pelaajat)lsvPelaajat.SelectedItem;

            if (valittu != null)
            {
                String sisus = btnOK.Content.ToString();

                if (sisus == "OK")
                {
                    StartGame startGame = new pang.StartGame(); // aloita peli
                    startGame.Show();
                    Close();                                    // sulje tämä ikkuna
                }
                else
                {
                    Pelaajat poisto = new Pelaajat();
                    poisto.PlayerName = Ukko.NykyinenPelaaja;   // listasta valittu pelaajanimi
                   // pvm.PoistaPelaajat(poisto.PlayerName);                            // PITÄÄKÖ TEHDÄ INDEKSI?

                }

            }
        }


        private void btnAddNew_Click_1(object sender, RoutedEventArgs e)    // pelaajan lisäys -nappi
        {
            if (txtPlayerName.Text != "")       // ei lisätä tyhjää pelaajaa
            {
                Pelaajat uusi = new Pelaajat();
                uusi.PlayerName = txtPlayerName.Text;
                uusi.PlayerPoints = 0; // pelaajaa luodessa pisteet 0, eikä tulosteta vielä mihinkään
                pvm.Pelaajat.Add(uusi);

                pvm.TalletaPelaajat(uusi.PlayerName, uusi.PlayerPoints);    // tallennetaan pelaajalistaan muutokset

                txtPlayerName.Text = "";
                txtMessage.Text = "Player Added.";
            }
        }


        private void btnNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            txtPlayerName.Visibility = Visibility.Visible;          // valitun moodin mukaan näytetään ja piilotetaan valintoja
            btnAddNew.Visibility = Visibility.Visible;
            lsvPelaajat.Visibility = Visibility.Hidden;
            btnOK.Visibility = Visibility.Hidden;
            scrLista.Visibility = Visibility.Hidden;

            // uuden pelaajan luonti    TODO
            CustomWidth = 600;          // levennetään ikkuna, pelaajien luomisen kenttiä varten
            lblLoad.Content = "Create Player";
        }


        private void btnLoadPlayer_Click(object sender, RoutedEventArgs e)
        {
            txtPlayerName.Visibility = Visibility.Hidden;           // valitun moodin mukaan näytetään ja piilotetaan valintoja
            btnAddNew.Visibility = Visibility.Hidden;
            lsvPelaajat.Visibility = Visibility.Visible;
            btnOK.Visibility = Visibility.Visible;
            scrLista.Visibility = Visibility.Visible;

            CustomWidth = 600;          // levennetään ikkuna, pelaajien luomisen kenttiä varten
            lblLoad.Content = "Load Player";
            btnOK.Content = "OK";
        }


        private void txtPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtPlayerName.Text = "";    // tyhjennä nimi ruutua klikatessa
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.Pelaajat valittu = (Model.Pelaajat)lsvPelaajat.SelectedItem;
            spData.DataContext = valittu;
            Ukko.NykyinenPelaaja = valittu.PlayerName;
        }
 

        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            txtPlayerName.Visibility = Visibility.Hidden;           // valitun moodin mukaan näytetään ja piilotetaan valintoja
            btnAddNew.Visibility = Visibility.Hidden;
            lsvPelaajat.Visibility = Visibility.Visible;
            btnOK.Visibility = Visibility.Visible;
            scrLista.Visibility = Visibility.Visible;
            
            CustomWidth = 600;          // levennetään ikkuna, pelaajien luomisen kenttiä varten
            lblLoad.Content = "Delete Player";
            btnOK.Content = "Delete";
        }
    }
}



