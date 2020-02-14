using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVP_PROJEKAT_2
{
    public class Grupa
    {
        int id_grupe;
        string naziv;
        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }
        public int Id_grupe
        {
            get { return id_grupe; }
            set { id_grupe = value; }
        }
        public Grupa(int id, string naziv)
        {
            id_grupe = id;
            this.naziv = naziv;
        }
        public override string ToString()
        {
            return naziv;
        }
    }
}
