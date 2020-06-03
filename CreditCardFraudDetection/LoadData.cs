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

namespace ThesisCode
{

    public partial class LoadData : Form
    {
        Label[] rb;
        int rowCount=0, colCount = 0;
        ArrayList al = new ArrayList();
        String fname = "";
        String[,] dataLine;
        public String[,] pre_processedData;

        public LoadData()
        {
            InitializeComponent();
        }

        string[] StartDetection(String fileName)
        {
            
            int cint = 0;
            int attCount = 0;
            string firstline = null;
           
            using (var r = new System.IO.StreamReader(fileName))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if ((line != null || line != String.Empty) && cint == 0)
                    {
                        firstline = line;
                        attCount = line.Split(',').Length;
                        ++cint;
                        
                    }
                    //Console.WriteLine(line);
                    al.Add(line);
                    ++cint;
                }
                rowCount = cint;
                colCount = attCount;
              
                r.Close();
            }

            string[] colNames = GetAttributeCount(firstline);
            return colNames;
        }


        string[] GetAttributeCount(string str)
        {
            String[] names = str.Split(',');
            return names;
        }

        private void AddAttributeToList(String fname)
        {
            this.fname = fname;
            string[] colNames = StartDetection(fname);
            rb = new Label[colNames.Length];
            ScrollBar vScrollBar1 = new VScrollBar();
            panel1.AutoScroll = false;
            panel1.HorizontalScroll.Enabled = false;
            panel1.HorizontalScroll.Visible = true;
            panel1.HorizontalScroll.Maximum = 0;
            panel1.AutoScroll = true;
            panel1.VerticalScroll.Enabled = true;
            panel1.VerticalScroll.Visible = true;

            for (int i = 0; i < colNames.Length; i++)
            {
                //Console.WriteLine(i + "......." + colNames[i]);
                rb[i] = new Label();
                rb[i].Text = colNames[i];
                rb[i].AutoSize = true;
                rb[i].Location = new Point(10, i * 40);
                panel1.Controls.Add(rb[i]);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.DataSource = ReadCsv(ofd.FileName);
                    String ffname = ofd.FileName;
                    AddAttributeToList(ffname);
                    fname = ffname;

                }

            }
            
        }
        public DataTable ReadCsv(string fileName)
        {
            DataTable dt = new DataTable("Data");
            DataTable dt1 = new DataTable("Data");
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
            dt1 = dt;
            

            dataGridView1.DataSource = dt1;
            return dt;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PreProcess pp = new PreProcess();
            pre_processedData=pp.DoPreProcess(al,rowCount,colCount);
            MessageBox.Show("Preprocessing is done.");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Don't save if no data is returned
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            // Column headers
            string columnsHeader = "";
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if(i==dataGridView1.Columns.Count-1)
                    columnsHeader += dataGridView1.Columns[i].Name;
                else
                    columnsHeader += dataGridView1.Columns[i].Name + ",";
            }
            //sb.Append(columnsHeader + Environment.NewLine);
            sb.Append(columnsHeader);
            // Go through each cell in the datagridview
            foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
            {
                // Make sure it's not an empty row.
                if (!dgvRow.IsNewRow)
                {
                    sb.Append(Environment.NewLine);
                    for (int c = 0; c < dgvRow.Cells.Count; c++)
                    {
                        // Append the cells data followed by a comma to delimit.
                        if(c==dgvRow.Cells.Count-1)
                            sb.Append(dgvRow.Cells[c].Value);
                        else
                            sb.Append(dgvRow.Cells[c].Value + ",");
                    }
                    // Add a new line in the text file.

                    
                   
                }
            }
            
           
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fname, false))
            {
                sw.WriteLine(sb.ToString());
            }
        
            MessageBox.Show("CSV file saved.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String filename= @"D:\processedData.csv";
            StreamWriter file = new StreamWriter(@filename);
            //my2darray  is my 2d array created.
            for (int i = 0; i < rowCount; i++)
            {
                //for (int j = 1; j < colCount-1; j++)
                for (int j = 0; j < colCount ; j++)
                {
                    file.Write(pre_processedData[i, j]);
                    //it is comman and not a tab
                    if(j!=colCount-1)
                        file.Write(",");
                }
                //go to next line
                if(i!=rowCount-1)
                file.Write("\n");

            }
            file.Close();
            this.dataGridView1.Refresh();
            dataGridView1.DataSource = ReadCsv(@filename);        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            KPrototype kp = new KPrototype(pre_processedData, rowCount, colCount,al);
            kp.Show();
        }

        private void LoadData_Load(object sender, EventArgs e)
        {
        }
    }
}
