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

            //First Save in CSV File
            var file = @"D:\FinalResult.csv";

            using (var stream = File.CreateText(file))
            {
                for (int i = 0; i < finalResult.Count; i++)
                {
                    string first = finalResult[i].ToString();
                    string csvRow = string.Format("{0}", first);
                    stream.WriteLine(csvRow);
                }
                Console.WriteLine("Write.........CSV File");
            }
            this.dataGridView1.Refresh();
            dataGridView1.DataSource = ReadCsv(@file);
            al = finalResult;
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

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;

            //Calculate TF FN 
            int TPCount = 0;
            int TNCount = 0;
            int FPCount = 0;
            int FNCount = 0;
            foreach (String str in al)
            {

                String[] arr = str.Split(',');
                String num = arr[0].Trim();
                String text = arr[1].Trim();
                String label = arr[2].Trim();
                String mylabel = arr[3].Trim();


                if (label.Equals("1") || label.Equals("Spam"))
                {
                    if (mylabel.Equals("Spam"))
                    {
                        ++TPCount;
                    }
                    else
                        ++FNCount;
                }
                else
                {
                    if (mylabel.Equals("Spam"))
                        ++FNCount;
                    else
                        ++FPCount;
                }

                String output= "Confusion Matrix of Classification Result...................\n";
                output += "True Positive=" + TPCount;
                output += "\nTrue Negative=" + TNCount;
                output += "\nFalse Positive=" + FPCount;
                output += "\nFalse Negative=" + FNCount;

                double accuracy =(double) (TPCount + TNCount) / (TPCount + TNCount + FPCount + FNCount);

                output+="\nAccuracy = (TP+TN)/(TP+TN+FP+FN) ="+ "("+TPCount +"+"+ TNCount+") / ("+TPCount+" + "+TNCount+" + "+FPCount+" + "+FNCount+")\n\t\t="+accuracy+"\n";
                output += "\n\nAccuracy value of Classification Result =" + accuracy * 100 + "%";

                richTextBox1.Text = output;
                Console.WriteLine(num + ".." + text + "......." + label + "...." + mylabel);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
