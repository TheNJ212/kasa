using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVP_PROJEKAT_2
{
    class Racun
    {
        int kolicina;
        Artikal art;
        public Artikal Art
        {
            get { return art; }
            set { art = value; }
        }

        public int Kolicina
        {
            get { return kolicina; }
            set { kolicina = value; }
        }
        public Racun(Artikal a,int kol)
        {
            art = a;
            kolicina = kol;
        }
    }
}
