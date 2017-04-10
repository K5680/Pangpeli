using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pang.Model;

namespace pang.ViewModel
{
    class PelaajatViewModel
    {
            private List<Pelaajat> pelaajat = new List<Pelaajat>();
            public List<Pelaajat> Pelaajat { get { return pelaajat; } }

            public PelaajatViewModel()
            {
            // generate some dummy data for testing purposes
            pelaajat.Add(new Pelaajat { PlayerName = "Jokke", PlayerPoints = 0 });
            pelaajat.Add(new Pelaajat { PlayerName = "Guest Player", PlayerPoints = 0 });

        }
    }
}