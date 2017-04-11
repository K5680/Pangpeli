using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pang.Model;
using System.Collections.ObjectModel;

namespace pang.ViewModel
{
    class PelaajatViewModel
    {
            //private List<Pelaajat> pelaajat = new List<Pelaajat>();
            //public List<Pelaajat> Pelaajat { get { return pelaajat; } }

            public ObservableCollection<Pelaajat> Pelaajat
            {
                get;
                set;
            }

            public void LataaPelaajat()
            {
                ObservableCollection<Pelaajat> pelaajat = new ObservableCollection<Pelaajat>();

 //           public PelaajatViewModel()
 //           {
   
                pelaajat.Add(new Pelaajat { PlayerName = "Jokke", PlayerPoints = 0 });
                pelaajat.Add(new Pelaajat { PlayerName = "Guest Player", PlayerPoints = 0 });
                Pelaajat = pelaajat;


    //          pelaajat.Add(new Pelaajat { PlayerName = "Lokke", PlayerPoints = 0 });
    //          pelaajat.Add(new Pelaajat { PlayerName = "Quest Player", PlayerPoints = 0 });
        }
    }
}