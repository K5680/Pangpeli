using System;
using System.Collections.Generic;
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
    /// Pelaajien luominen / lataaminen ennen peliä
    /// </summary>
    public partial class Players: Window
    {
        public Players()
        {
            InitializeComponent();
        }

        private void btnNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            // uuden pelaajan luonti    TODO
        }

        private void btnLoadPlayer_Click(object sender, RoutedEventArgs e)
        {
            // pelaajien lataus, tässä vaiheessa oikaistaan suoraan peliin  TODO
            StartGame startGame = new pang.StartGame();
            startGame.Show();
            Close();
        }

        private void btnExitPlayer_Click(object sender, RoutedEventArgs e)
        {
            // Sovelluksen sammutus
            Application.Current.Shutdown();
        }
    }
}

