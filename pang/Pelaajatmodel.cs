using System.ComponentModel;

namespace pang
{
    // # public class Pelaajat (Model)
    // # 
    // # model -pelaajien luomiseen ja lataamiseen (pisteiden tallennus myös)
    // # 

    public class Pelaajat : INotifyPropertyChanged
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
                }
            }
        }
        public int PlayerPoints
        {
            get { return playerPoints; }

            set
            {
                if (playerPoints != value)
                {
                    playerPoints = value;
                    RaisePropertyChanged("PlayerPoints");                    
                }
            }
        }
        public string FullInfo
        {
            get
            {
                return playerName + " " + playerPoints;
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