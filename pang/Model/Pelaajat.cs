using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pang.Model
{
         /// <summary>
        /// This class holds Pelaajat properties.
        /// </summary>
        public class Pelaajat : INotifyPropertyChanged  // lisätty inotify
    {
        private string playerName;
        private int playerPoints;    

            public string PlayerName
            {
                get
                {
                    return playerName;
                }
                set
                {
                    if (playerName != value)
                    {
                        playerName = value;
                        RaisePropertyChanged("PlayerName");
                        RaisePropertyChanged("FullInfo");
                    }
                }
            }
        public int PlayerPoints
        {
            get
            {
                return playerPoints;

            }
            set
            {
                if (playerPoints != value)
                {
                    playerPoints = value;
                    RaisePropertyChanged("PlayerPoints");
                    RaisePropertyChanged("FullInfo");
                }
            }
        }


        public string FullInfo
            {
                get
                {
                    return PlayerName + " " + PlayerPoints;
                }
            }



        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


    }
    }