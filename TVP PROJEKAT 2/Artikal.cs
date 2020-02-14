using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVP_PROJEKAT_2
{
    class Artikal
    {
        int id;
        string naziv;
        double cena;
        int id_grupe;

        public string Naziv {
            get { return naziv; }
            set { naziv = value; }
        }
        public double Cena {
            get { return cena; }
            set { cena = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Id_grupe
        {
            get { return id_grupe; }
            set { id_grupe = value; }
        }

        public Artikal(int id, string naziv,double cena,double popust,int id_grupe)
        {
            this.id = id;
            this.naziv = naziv;
            this.cena = cena *(1 - popust);
            this.id_grupe = id_grupe;
        }

        public override string ToString()
        {
            return naziv;
        }
    }
}
