using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpamDetection
{
    public partial class AccuracyForm : Form
    {
        ArrayList al = new ArrayList();
        public AccuracyForm()
        {
            InitializeComponent();
        }

        private void AccuracyForm_Load(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        public void StartAccuracy(ArrayList finalResult)
        {
        }
        public DataTable ReadCsv(string fileName)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
