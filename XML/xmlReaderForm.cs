using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRXMLThesis
{
    public partial class xmlReaderForm : Form
    {
        String fileData = "";
        public xmlReaderForm(String fileData)
        {
            this.fileData = fileData;
            InitializeComponent();
        }

        private void xmlReaderForm_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = fileData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
