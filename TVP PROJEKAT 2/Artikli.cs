using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVP_PROJEKAT_2
{
    
    public partial class Artikli : Form
    {
        bool izmena = false;
        OleDbConnection con1 = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Prodavnica.accdb");
        public Artikli()
        {
            InitializeComponent();

        }
        public Artikli(List<Grupa> grupa) : this()
        {
            cbGrupa.DataSource = grupa;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string poruka="";
            if (txtNaziv.Text.Trim().Length == 0)
            {
                poruka += "Niste uneli naziv artikla!\r\n";
            }
            if (txtCena.Text.Trim().Length == 0)
            {
                poruka += "Niste uneli cenu artikla!\r\n";
            }
            else if (Regex.Match(txtCena.Text, @"(\d+(\.\d+)?)|(\d+(\,\d+)?)").Value == "" || txtCena.Text.Contains(" ") || txtCena.Text.Contains("\t")) 
            {
                poruka += "Niste uneli cenu u korektnom formatu!";
               
            }
            if(poruka!="")
            {
                MessageBox.Show(poruka);
                return;
            }
            try
            {
                OleDbCommand cmd = con1.CreateCommand();
                cmd.CommandText = "insert into artikal (naziv,cena,popust,id_grupe) values(@naziv,@cena,@popust,@id_grupe)";
                cmd.Parameters.AddWithValue("@naziv", txtNaziv.Text);
                cmd.Parameters.AddWithValue("@cena", double.Parse(txtCena.Text.Replace(".", ",")));
                cmd.Parameters.AddWithValue("@cena", (double)numPopust.Value/100.0);
                cmd.Parameters.AddWithValue("@cena", (cbGrupa.SelectedItem as Grupa).Id_grupe);
                cmd.Connection.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Uspešno dodat artikal");
                    izmena = true;
                }
            }
            catch
            {

            }
            finally
            {
                con1.Close();
            }
        }
        public bool provera()
        {
            return izmena;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
       
    }
}
