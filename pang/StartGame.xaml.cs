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
    /// P?elin käynnistysruutu
    /// </summary>
    public partial class StartGame : Window
    {
        public StartGame()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            // Käynnistetään itse peli
            MainWindow mainWindow = new pang.MainWindow();
            mainWindow.Show();
        }

        private void btnHighScores_Click(object sender, RoutedEventArgs e)
        {
            // tässä näytetään lista parhaista pelaajista   TODO


        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

