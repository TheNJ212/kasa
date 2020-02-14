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
    public partial class Form1 : Form
    {
        OleDbConnection con1=new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Prodavnica.accdb");
        Artikal a=new Artikal(0,"",0,0,0);
        List<Artikal> artikli = new List<Artikal>();
        List<Grupa> grupe = new List<Grupa>();
        List<Racun> artNaRacunu = new List<Racun>();
        double ukupno = 0;
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            try
            {
                OleDbCommand cmd = con1.CreateCommand();
                cmd.CommandText = "select naziv,id_grupe from grupa";
                cmd.Connection.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string naziv = (string)reader["naziv"];
                    int id_grupe = (int)reader["id_grupe"];
                    grupe.Add(new Grupa(id_grupe, naziv));
                    //dugmici grupe
                    Button b = new Button();
                    b.Name = "btnGrupa" + id_grupe;
                    b.Text = naziv;
                    b.Tag = id_grupe;
                    b.Height = 70;
                    b.Width = 155;
                    b.Font = new Font(b.Font.FontFamily, 12);
                    b.Click += B_Click2;
                    flowLayoutPanel1.Controls.Add(b);
                }
            }
            catch
            {


            }
            finally
            {
                con1.Close();
            }
            try
            {
                OleDbCommand cmd = con1.CreateCommand();
                cmd.CommandText = "select artikal.id_artikla,artikal.naziv,artikal.cena,artikal.popust,artikal.id_grupe from artikal";
                cmd.Connection.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["id_artikla"];
                    string naziv = (string)reader["naziv"];
                    double cena = (double)reader["cena"];
                    double popust = (double)reader["popust"];
                    int id_grupe = (int)reader["id_grupe"];
                    Artikal a = new Artikal(id, naziv, cena, popust, id_grupe);
                    artikli.Add(a);
                }
            }
            catch
            {

            }
            finally
            {
                con1.Close();
            }
            numKolicina.Enabled = false;
            Task task = new Task(sortirajListu);
            task.Start();
        }
        private void sortirajListu()
        {
            artikli.Sort((x, y) => string.Compare(x.Naziv, y.Naziv));
        }
        private void btnRacuni_Click(object sender, EventArgs e)
        {
            Računi r = new Računi();
            r.Show();
        }

        private void btnArtikli_Click(object sender, EventArgs e)
        {
            Artikli aForm = new Artikli(grupe);
            aForm.Show();
            aForm.FormClosed += AForm_FormClosed;
        }

        private void AForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((sender as Artikli).provera())
            {
                Task task = new Task(osveziBazu);
                task.Start();
            }
         
        }
        private void osveziBazu()
        {
            try
            {
                artikli.Clear();
                OleDbCommand cmd = con1.CreateCommand();
                cmd.CommandText = "select artikal.id_artikla,artikal.naziv,artikal.cena,artikal.popust,artikal.id_grupe from artikal";
                cmd.Connection.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["id_artikla"];
                    string naziv = (string)reader["naziv"];
                    double cena = (double)reader["cena"];
                    double popust = (double)reader["popust"];
                    int id_grupe = (int)reader["id_grupe"];
                    Artikal a = new Artikal(id, naziv, cena, popust, id_grupe);
                    artikli.Add(a);

                }
            }
            catch
            {

            }
            finally
            {
                con1.Close();
            }
            sortirajListu();
        }

        private void B_Click(object sender, EventArgs e)
        {
            Button dgm = sender as Button;
            foreach(Artikal art in artikli)
            {
                if (dgm.Tag.ToString() == art.Id.ToString())
                {
                    a = art;
                    txtArtNaziv.Text = a.Naziv;
                    txtArtCenaSaPopustom.Text = (a.Cena * (double)numKolicina.Value).ToString("F");
                }
            }
            numKolicina.Enabled = true;
            numKolicina.Focus();
            AcceptButton = btnDodaj;
            numKolicina.Value = 1;
            a = artikli.First(x => x.Id == (int)dgm.Tag);


        }
        private void B_Click2(object sender, EventArgs e)
        {
            groupBox1.Text = "Artikli";
            flowLayoutPanel1.Controls.Clear();
            Button dgm = sender as Button;
            foreach (Artikal art in artikli)
            {
                if (dgm.Tag.ToString() == art.Id_grupe.ToString())
                {
                    //dugmici artikli
                    Button b = new Button();
                    b.Name = "btnArtikal" + art.Id;
                    b.Text = art.Naziv;
                    b.Tag = art.Id;
                    b.Height = 70;
                    b.Width = 155;
                    b.Font = new Font(b.Font.FontFamily, 12);
                    b.Click += B_Click;
                    flowLayoutPanel1.Controls.Add(b);
                }
            }
            numKolicina.Value = 1;
            txtArtCenaSaPopustom.Text = "";
            txtArtNaziv.Text = "";
            txtArtNaziv.Focus();
        }

        private void numKolicina_ValueChanged(object sender, EventArgs e)
        {
            txtArtCenaSaPopustom.Text = (a.Cena * (double)numKolicina.Value).ToString("F");
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            
            if (txtArtNaziv.Text == "") return;
            if (txtTotal.Text == "")
            {
                txtRacun.Text = "";
                txtPovracaj.Text = "";
                txtUplaceno.Text = "";
            } 
            if ( txtRacun.Text == "")
            {
                txtRacun.AppendText("================================\r\n");
                txtRacun.AppendText("\t         Racun\r\n");
                txtRacun.AppendText("================================\r\n");
            }

            if (txtArtNaziv.Text != "")
            {
                txtRacun.AppendText(txtArtNaziv.Text + "\r\n");
                txtRacun.AppendText(numKolicina.Value.ToString() + "x "+ a.Cena.ToString("F")+"\t\t" + txtArtCenaSaPopustom.Text + "\r\n");
            }
            if (txtArtCenaSaPopustom.Text != "")
            {
                ukupno += double.Parse(txtArtCenaSaPopustom.Text);
                txtTotal.Text = ukupno.ToString("F");
            }
            artNaRacunu.Add(new Racun(a,(int)numKolicina.Value));

        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            try
            {
                if (ukupno > 0)
                {
                    string poruka = "";
                    if (txtUplaceno.Text.Trim().Length == 0)
                    {
                        poruka += "Niste uneli iznos!";
                    }
                    else if (Regex.Match(txtUplaceno.Text, @"(\d+(\.\d+)?)|(\d+(\,\d+)?)").Value == "" || txtUplaceno.Text.Contains(" ") || txtUplaceno.Text.Contains("\t"))
                    {
                        poruka += "Niste uneli iznos u korektnom formatu!";

                    }
                    if (poruka != "")
                    {
                        MessageBox.Show(poruka);
                        return;
                    }
                    double razlika =  double.Parse(txtUplaceno.Text.Replace(".", ","))- ukupno;
                    if (razlika<0)
                    {
                        MessageBox.Show("Iznos za uplatu je manji od ukupnog iznosa!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    txtPovracaj.Text = razlika.ToString("F");
                    OleDbCommand cmd = con1.CreateCommand();
                    cmd.CommandText = "insert into racun(cena,datum,vreme) values(" + ukupno + "," + "#" + DateTime.Now.ToShortDateString().Replace(".", "-").TrimEnd('-') + "#,'" + DateTime.Now.ToLongTimeString() + "')";
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    txtRacun.AppendText("---------------------------------------------------------\r\n");
                    txtRacun.AppendText("TOTAL:\t\t\t" + ukupno.ToString("F") + "\r\n");
                    txtRacun.AppendText("Uplaćeno:\t\t" + double.Parse(txtUplaceno.Text.Replace(".", ",")).ToString("F") + "\r\n");
                    txtRacun.AppendText("Povraćaj:\t\t\t" + razlika.ToString("F") + "\r\n");
                    txtRacun.AppendText(DateTime.Now.ToString() + "\r\n");
                    txtRacun.AppendText("================================\r\n");
                }
                else MessageBox.Show("Niste dodali ni jedan artikal!");
            }
            catch
            {


            }
            finally
            {
                con1.Close();
            }
            ukupno = 0;
            txtArtNaziv.Text = "";
            txtArtCenaSaPopustom.Text = "";
            numKolicina.Value = 1;
            txtTotal.Text = "";
        }

        private void btnPonisti_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da želite da poništite sve?", "Upozorenje", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                txtTotal.Text = "";
                txtRacun.Text = "";
                txtArtNaziv.Text = "";
                txtArtCenaSaPopustom.Text = "";
                numKolicina.Value = 1;
                artNaRacunu.Clear();
                ukupno = 0;
                btnGrupe.PerformClick();
            }
            
        }

        private void btnStorniraj_Click(object sender, EventArgs e)
        {
            if (artNaRacunu.Count == 0) {
                MessageBox.Show("Nema artikala za storniranje!");
                return;
            }
            Racun r = artNaRacunu.Find(x => x.Art.Id == a.Id);
            if (r == null) return;
            ukupno -= r.Art.Cena * r.Kolicina;
            txtRacun.AppendText("STORNO " + r.Art.Naziv + "\r\n");
            txtRacun.AppendText(r.Kolicina + "x " + (-r.Art.Cena).ToString("F") + "\t\t" + (-r.Art.Cena*r.Kolicina).ToString("F") + "\r\n");
            artNaRacunu.Remove(r);
            txtTotal.Text = ukupno.ToString("F");
        }

        private void btnGrupe_Click(object sender, EventArgs e)
        {
            if(groupBox1.Text != "Grupe"){
                flowLayoutPanel1.Controls.Clear();
                groupBox1.Text = "Grupe";
                numKolicina.Enabled = false;
                foreach (Grupa g in grupe)
                {
                    //dugmici grupe
                    Button b2 = new Button();
                    b2.Name = "btnGrupa" + g.Id_grupe;
                    b2.Text = g.Naziv;
                    b2.Tag = g.Id_grupe;
                    b2.Height = 70;
                    b2.Width = 155;
                    b2.Font = new Font(b2.Font.FontFamily, 12);
                    b2.Click += B_Click2;
                    flowLayoutPanel1.Controls.Add(b2);
                }
                txtArtCenaSaPopustom.Text = "";
                txtArtNaziv.Text = "";
            }

        }


        private void komandeTastature(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                btnGrupe.PerformClick();
            }
            
        }

        private void txtUplaceno_TextChanged(object sender, EventArgs e)
        {
            AcceptButton = btnTotal;
        }
    }
}
