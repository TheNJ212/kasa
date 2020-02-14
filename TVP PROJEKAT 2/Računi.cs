using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVP_PROJEKAT_2
{

    public partial class Računi : Form
    {
        OleDbDataAdapter da;
        DataTable dt;
        DataTable dtPom;
        public Računi()
        {
            
            InitializeComponent();
            da = new OleDbDataAdapter("select * from racun", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Prodavnica.accdb");
            dt = new DataTable("Racun");
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[1].DefaultCellStyle.Format = "F";
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Format = "HH:mm:ss";
            dataGridView1.ReadOnly = true;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dateTimePicker1.Value = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dateTimePicker2.Value = DateTime.Parse(dateTimePicker2.Value.ToShortDateString());
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value>dateTimePicker2.Value)
            {
                MessageBox.Show("Datumi nisu korektno podešeni!", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dataGridView1.DataSource = dt;
            DataTable dtRez = new DataTable();
            IEnumerable<DataRow> rows = dt.Rows.Cast<DataRow>();
            IEnumerable<DataRow> rez = rows.Where(x =>    (  dateTimePicker1.Value<= DateTime.Parse(((DateTime)x["datum"]).ToShortDateString()+" "+((DateTime)x["vreme"]).ToLongTimeString()) ) && (dateTimePicker2.Value >= DateTime.Parse(((DateTime)x["datum"]).ToShortDateString() + " " + ((DateTime)x["vreme"]).ToLongTimeString()))) ;
            if (rez.Count() > 0)
            {
                dtPom = rez.CopyToDataTable();
                dataGridView1.DataSource = dtPom;
               
            }
            else
            {
                MessageBox.Show("Nema rezultata!");
            }

        }
    }
}
