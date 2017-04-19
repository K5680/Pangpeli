using pang.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace pang
{
    /// <summary>
    ///           Players.cs        Pelaajien luominen / lataaminen ennen peliä
    /// </summary>
    /// 
    public partial class Players: Window, INotifyPropertyChanged
    {
        ViewModel.PelaajatViewModel pvm = new ViewModel.PelaajatViewModel();

        private int width;      // ikkunan leveys muuttuu -> muuttuja
        private int indeksi;    // pelaajan poistamiseen indeksi perusteella collectionista
        private int poistotehty;    // jotta lista ei jumitu, sitä ei saa päivittää kun valinta on poistetulla Itemillä

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

            lblLoad.Content = "Load Player";

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
                else // jos nappi on "Delete"
                {
                    System.Diagnostics.Debug.WriteLine("indeksi 1: " + indeksi);
                    poistotehty = 1;    // ListViewSelectionChanged on NULL, koska valittu item poistetaan, sen takia estetään sen lukeminen poiston ajaksi
                    pvm.PoistaPelaajat(indeksi);
                    pvm.Pelaajat.RemoveAt(indeksi);
                    lsvPelaajat.SelectedItem = 0;
                    poistotehty = 0;
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

            txtMessage.Text = "Create a new player.";
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
            txtMessage.Text = "Please, load a player.";

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
            if (poistotehty == 0)
            {
                Model.Pelaajat valittu = (Model.Pelaajat)lsvPelaajat.SelectedItem;
                Ukko.NykyinenPelaaja = valittu.PlayerName;
                indeksi = pvm.Pelaajat.IndexOf(valittu);    // valitun nimen indeksi collectionissa
            }
            else
            {
                indeksi = 0;
            }
            //spData.DataContext = valittu;
        }


        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            txtPlayerName.Visibility = Visibility.Hidden;           // valitun moodin mukaan näytetään ja piilotetaan valintoja
            btnAddNew.Visibility = Visibility.Hidden;
            lsvPelaajat.Visibility = Visibility.Visible;
            btnOK.Visibility = Visibility.Visible;
            scrLista.Visibility = Visibility.Visible;

            txtMessage.Text = "Choose a player to delete.";
            CustomWidth = 600;          // levennetään ikkuna, pelaajien luomisen kenttiä varten
            lblLoad.Content = "Delete Player";
            btnOK.Content = "Delete";
        }


        // tällä koodilla enter nappi lisää pelaajan
        private void txtPlayerName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true;
                btnAddNew_Click_1(sender, e);
            }
        }
    }
}



