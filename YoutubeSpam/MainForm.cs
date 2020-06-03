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

namespace SpamDetection
{
    public partial class MainForm : Form
    {
        String fileName = "";
        public MainForm()
        {
            InitializeComponent();
        }

        private void datasetSpamDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var dForm = new Form1();
            dForm.Closed += (s, args) => this.Close();
            dForm.Show();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(label1);
            Controls.Remove(label2);
           // Controls.Add(groupBox1);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataSetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Controls.Remove(button1);
            Controls.Remove(dataGridView1);
            Controls.Add(label1);
            Controls.Add(label2);
        }

        private void button1_Click(object sender, EventArgs e)
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
        public DataTable ReadCsv(string fileName)
        {
            DataTable dt = new DataTable("Data");
            using (OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" +Path.GetDirectoryName(fileName) + "\";Extended Properties='text;HDR=yes;FMT=Delimited(,)';"))
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

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void inputSpamDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////Indiv Input
            //InputSpamDetectForm inputForm = new InputSpamDetectForm();
            //inputForm.Show();
            
        }

        private void performanceEvaluationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
