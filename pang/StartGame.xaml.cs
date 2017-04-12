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
     
            
            /*       Bubble Sort

            public void BubbleSort(int[] intArray)

            {

            Console.WriteLine("==========UnSorted Array Input===============");

            for (int i = 0; i < intArray.Length; i++)

            {

                Console.WriteLine(intArray[i]);

            }



            for (int i = intArray.Length - 1; i > 0; i--)

            {

                for (int j = 0; j <= i - 1; j++)

                {

                    if (intArray[j] > intArray[j + 1])

                    {

                        int highValue = intArray[j];



                        intArray[j] = intArray[j + 1];

                        intArray[j + 1] = highValue;

                    }

                }

            }



            Console.WriteLine("==========Sorted Array Using BubbleSort===============");

            for (int i = 0; i < intArray.Length; i++)

            {

                Console.WriteLine(intArray[i]);

            }

        }       */


    }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
