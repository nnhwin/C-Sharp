using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CreditFraudDetection
{
   
    public partial class MainForm : Form
    {
        String fileData = "";
        String fileName1 = "";
        String fileName = "";
        int rowCount = 0, colCount = 0;
        TextBox txtBox;
        public MainForm()
        {
            InitializeComponent();
           // this.textBox1.Size = new System.Drawing.Size(442, 50);
        }

        private void dataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //RemoveForm1();
            //Label lbl = new Label();
            //lbl.Text = "hello";
            //lbl.Location = new Point(40, 120);
            //Controls.Add(lbl);
            this.Dispose();
            MainForm mm = new MainForm();
            mm.Show();
        }

        private void detectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateDataSetForm updateForm = new UpdateDataSetForm();
            updateForm.Show();
        }
        void RemoveForm1()
        {
            //Controls.Remove(dataGridView1);
            //Controls.Remove(button1);
            //Controls.Remove(groupBox1);
            this.Hide();
            var dForm = new DetectionForm(fileName);
            dForm.Closed += (s, args) => this.Close();
            dForm.Show();

        }

       
        public DataTable ReadCsv(string fileName)
        {
            DataTable dt = new DataTable("Data");
            using (OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" +
                Path.GetDirectoryName(fileName) + "\";Extended Properties='text;HDR=yes;FMT=Delimited(,)';"))
            {
                //Execute select query
                using (OleDbCommand cmd = new OleDbCommand(string.Format("select *from [{0}]", new FileInfo(fileName).Name), cn))
                {
                    cn.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.DataSource = ReadCsv(ofd.FileName);
                    fileName = ofd.FileName;

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var dForm = new DetectionForm(fileName);
            dForm.Closed += (s, args) => this.Close();
            dForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }


}
