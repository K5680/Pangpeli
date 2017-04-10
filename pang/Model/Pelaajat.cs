using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pang.Model
{
         /// <summary>
        /// This class holds Pelaajat properties.
        /// </summary>
        public class Pelaajat
        {            
            public string PlayerName { get; set; }
            public int PlayerPoints { get; set; }
            

            public string FullInfo
            {
                get
                {
                return PlayerName + " " + PlayerPoints;
                }
            }
        }
    }