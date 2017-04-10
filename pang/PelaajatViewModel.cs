
using System.Collections.ObjectModel;
using pang;
using System.Collections.Generic;

namespace pang
{
    // # Pelaajat ViewModel
    // # 
    // # model -pelaajien luomiseen ja lataamiseen (pisteiden tallennus myös)
    // # 
    public class PelaajatViewModel
    {


        public ObservableCollection<Pelaajat> Pelaajat
        {
            get;
            set;
        }
        public void LataaPelaajat()
        {
            ObservableCollection<Pelaajat> pelaajat = new ObservableCollection<Pelaajat>();
            pelaajat.Add(new Pelaajat { PlayerName = "Guest Player", PlayerPoints = 1000 });
            Pelaajat = pelaajat;
        }


    }

}