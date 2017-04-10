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
        PelaajatViewModel svmo = new PelaajatViewModel();

        PelaajatViewModel evm;

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

            svmo.LataaPelaajat();

            this.DataContext = this;
            CustomWidth = 300;  // ikkunan leveyden määrittely

            // textboxin tyhjennys, kun laatikko valitaan
            txtPlayerName.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(txtPlayerName_MouseLeftButtonDown), true); 


        }

        public event PropertyChangedEventHandler PropertyChanged;       

        private void btnNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            // uuden pelaajan luonti    TODO
            CustomWidth = 600;          // levennetään ikkuna, pelaajien luomisen kenttiä varten
            lblNewOrLoad.Content = "Create / Select player";
        }


        private void btnLoadPlayer_Click(object sender, RoutedEventArgs e)
        {
            // pelaajien lataus, tässä vaiheessa oikaistaan suoraan peliin  TODO
            CustomWidth = 600;          // levennetään ikkuna, pelaajien luomisen kenttiä varten
            lblNewOrLoad.Content = "Load new player";

            StartGame startGame = new pang.StartGame();
            startGame.Show();
            Close();
        }

        private void btnExitPlayer_Click(object sender, RoutedEventArgs e)
        {
            // Sovelluksen sammutus
            Application.Current.Shutdown();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            StartGame startGame = new pang.StartGame(); // aloita peli
            startGame.Show();
            Close();                                    // sulje tämä ikkuna
        }


        private void PelaajatViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            PelaajatViewControl.DataContext = svmo;
        }

        private void dgPelaajat_Loaded(object sender, RoutedEventArgs e)
        {
            //dgPelaajat.DataContext = svmo.Pelaajat;
            
        }

        private void lsvPelaajat_Loaded(object sender, RoutedEventArgs e)
        {
            //lsvPelaajat.DataContext = svmo.Pelaajat;
        }


        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlayerName.Text != "")       // ei lisätä tyhjää pelaajaa
            { 
                Pelaajat uusi = new Pelaajat();
                uusi.PlayerName = txtPlayerName.Text;
                uusi.PlayerPoints = 0; // pelaajaa luodessa pisteet 0, eikä tulosteta vielä mihinkään
                svmo.Pelaajat.Add(uusi);
                txtPlayerName.Text = "";
                //txtPlayerPoints.Text = "";
            }
        }


        private void txtPlayerName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtPlayerName.Text = "";    // tyhjennä nimi ruutu klikatessa
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
//            Pelaajat valittu = (Pelaajat)lsvPelaajat.SelectedItem;
//            spData.DataContext = valittu;
        }
    }
}


// TODO   TODO   TODO   TODO   TODO   TODO   TODO   vaihda tiedonsidonta demo 2:een pelaajien valinta
