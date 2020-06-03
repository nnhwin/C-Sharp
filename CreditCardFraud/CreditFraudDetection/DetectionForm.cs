using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditFraudDetection
{
    public partial class DetectionForm : Form
    {
        String fileName;
        int rowCount = 0, colCount = 0;
        CheckBox[] rb;
        List<string[]> table = new List<string[]>();
        CreditObject dataObject;
        String writeData = "";
        public Dictionary<float, int> finalFraudList;
        String[] stepCal;
        public DetectionForm()
        {
            InitializeComponent();
        }
        public DetectionForm(String fileName)
        {
            InitializeComponent();
            this.fileName = fileName;
            comboBox1.SelectedIndex = 0;
        }

        private void DetectionForm_Load(object sender, EventArgs e)
        {
            
            string[] colNames=StartDetection();
            rb = new CheckBox[colNames.Length];
            
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
                rb[i] = new CheckBox();
                rb[i].Text = colNames[i];
                rb[i].AutoSize = true;
                rb[i].Location = new Point(10, i * 40);
                panel1.Controls.Add(rb[i]);
            }
        }
        string[] StartDetection()
        {          
            String str = "";
            int cint = 0;
            string firstline =null;
            
            using (var r = new System.IO.StreamReader(fileName))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if ((line != null || line != String.Empty) && cint==0)
                    {
                        firstline = line;
                        ++cint;
                    }
                       
                    table.Add(Regex.Split(line, @"\s|[;]|[,]"));
                    //Console.WriteLine(line);
                    ++rowCount;
                    ++cint;
                }
                r.Close();
            }
            string[] colNames= GetAttributeCount(firstline);
            colCount = colNames.Length;      
            //MessageBox.Show("Row Count =" + rowCount + "............colCount=" + colCount);
            return colNames;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rb.Length; i++)
                rb[i].Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rb.Length; i++)
                rb[i].Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            
            MessageBox.Show("Processing...,Please Wait","Algorithm Running");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int knumber = Int32.Parse(comboBox1.SelectedItem.ToString());
            Dictionary<int, bool> checkUnCheckAttributes = CheckSelectedBox();
            int checkCount = 0;
            foreach (KeyValuePair<int, bool> item in checkUnCheckAttributes)
            {
                if (item.Value == true)
                    ++checkCount;
            }

            
           GettingOnlyNecessaryAttributesValues detectAlgo= new GettingOnlyNecessaryAttributesValues(table, checkUnCheckAttributes,knumber);
           stepCal = detectAlgo.stepsCal;
           dataObject =detectAlgo.dataObject;
           richTextBox1.Text = dataObject.getFinalOutput();
           finalFraudList = detectAlgo.finalFraudList;
           
            watch.Stop();
          
            var elapsedMs = watch.ElapsedMilliseconds;
            var elspasedsecond = watch.Elapsed.TotalSeconds;
            var minute = watch.Elapsed.TotalMinutes;
            button4.Enabled = true;
            
            String strr="Total Elapsed Time: " + elspasedsecond.ToString("#.###") + " seconds.";
            label2.Text = strr+"\nTotal Elapsed Time: "+ minute.ToString("#.###") + " minutes.";
           
            //button5.Enabled = true;
        }

        public async void WaitSomeTime(Form item,double milis)
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(milis);
            await Task.Delay(interval);
            item.Close();
        }
        string[] GetAttributeCount(string str)
        {
            String[] names = str.Split(',');
            return names;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string allOutput = dataObject.getTotalString();
            //richTextBox1.Text = "";
            //richTextBox1.Text = allOutput;
            writeData = allOutput;
            button7.Enabled = true;
            button5.Enabled = true;
            ProgressForm pgfrm = new ProgressForm();
            pgfrm.ShowProces(stepCal,allOutput);
            pgfrm.Show();
            pgfrm.BringToFront();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Check Performance
            List<String[]> allList = dataObject.getAllList();
            // Dictionary<float, int> objectValuePair = dataObject.getPair();
            Dictionary<float, int> allDataPair = dataObject.getPair();
            var ll = allDataPair.Keys.ToList();
            Dictionary<String, String> idClass = new Dictionary<String, String>();
            int flag = 0;
            foreach (String[] key in allList)
            {
                if (flag == 0)
                    flag = 1;
                else
                {
                    String[] ss = key;
                    String id = ss[0];
                    String classlabel = ss[key.Length - 1];
                    int tempint = Convert.ToInt32(id);
                    --tempint;
                   // Console.WriteLine(id + "......." + classlabel);
                    idClass.Add(Convert.ToString(tempint), classlabel);
                }             
            }

            var allData = idClass.Keys.ToList();
            Dictionary<float, int> objectValuePair = finalFraudList;
            int knumber = dataObject.getKNumber();
            //Dictionary<float, int> fraudPair=dataObject.
            //Print out
            var list = objectValuePair.Keys.ToList();
            list.Reverse();
            int fraudCount = 0;
            int totalCount = 0;
            string output = "";
            int count = 1;
            int truePostive = 0, trueNegative = 0, falsePositive = 0, falseNegative = 0;
            output += "Class label 1 -> Non-Fraud   \t Class label 2 -> Fraud\n\n";

            output += "Fraud Object Detected by Algorithm........................\n";
            foreach (var kk in list)
            {
                int id = objectValuePair[kk] + 1;
                float fraudValue = kk;
                output += "Object-" + id + " => Fraud Value =" + fraudValue + "\n";
            }
            output += "\n**********************************\nDetailed Caclulation of TN,TP, FN,FP.................\n";
            foreach (var key in allData)
            {
               // Console.WriteLine(key + ".....#####....." + idClass[key]);
                int classlabelByDataset = Convert.ToInt32(idClass[key]);
                int k_id = Convert.ToInt32(key);
                int found = 0;          
                foreach (var kk in list)
                {
                    int id = objectValuePair[kk] + 1;
                    if (id == k_id)
                    {                        
                        //output += "\nDeclear Fraud by Algorithm................\n";
                        //Fraud by algorithm
                        if (classlabelByDataset == 1)
                        {
                            //1 is non-fraud by db
                            ++falsePositive;
                            output += "Object-" + id + " is FalsePositive.\n";
                            output += "Dataset --> Non-Fraud\n";
                            output+="Algorithm --> Fraud\n\n";
                            ++totalCount;
                            found = 1;                        
                        }
                        else
                        {
                            ++truePostive;                           
                            output += "Object-" + id + " is TruePositive.\n";
                            output += "Dataset --> Fraud\n";
                            output += "Algorithm -->Fraud\n\n";
                            ++fraudCount;                       
                            ++totalCount;
                            found = 1;                       
                        }
                        
                    }
                   

                }//for each

                if (found == 0)
                {
                    //Console.WriteLine("non-equal.........");
                    //Non-fraud by algorithm
                    if (classlabelByDataset == 1)
                    {
                        //1 is non-fraud by db
                        ++trueNegative;
                        output += "Object-" + k_id + " is TrueNegative.\n";
                        output += "Dataset --> Non-Fraud\n";
                        output += "Algorithm --> Non-Fraud\n\n";
                    }
                    else
                    {
                        ++falseNegative;
                        output += "Object-" + k_id + " is FalseNegative.\n";
                        output += "Dataset --> Fraud\n";
                        output += "Algorithm --> Non-Fraud\n\n";
                        ++fraudCount;
                    }
                    found = 0;
                }

            }


            //Check All fraud from dataset
            //fraud(dataset) fruad(system) =truepostive
            //fraud(dataset) non-fraud(system)=falsenegative
            //non-fraud fraud = falsepositive
            //nonfraud nonfraud=truenegative
          
            output += "\t\tConfusion Matrix\n\t\t*************************\n";
            output += "\t\tFraud\t\t   Non-Fraud\n";
            output += "Fraud\t\t" + truePostive + " (TP)\t\t\t" + falseNegative + "(FN) \n";
            output += "Non-Fraud\t " + falsePositive + "(FP)\t\t\t" + trueNegative + "(TN)\n\n\t\t**********************\n\n";
            output += "\n*******************Performance Result***********************************\n";
            output += "Accuracy = TP + TN / (TP + TN + FN + FP)\n";
            float pineChay = (truePostive + trueNegative + falsePositive + falseNegative);
            float accuracy = (truePostive + trueNegative) / pineChay;
            output += "Accuracy = (" + truePostive + "+" + trueNegative + ")/" + "(" + truePostive + "+" + trueNegative + "+" + falsePositive + "+" + falseNegative + ") =" + Math.Round(accuracy,2) + "\n";
            double vv = Math.Round(accuracy, 2);
            output += "Accuracy = " + vv * 100 + "%\n\n";

            //Precision
            
           /* output += "Precision=TP/(TP+FP)= " + truePostive+"/("+truePostive+"+"+falsePositive+")=";
            pineChay = truePostive + falsePositive;
            float precisionresult =(float) truePostive / pineChay;
            output += precisionresult + "\n";
            double pre = Math.Round(precisionresult,2);
            output += "Precision = " + pre * 100 + "%\n\n";

            //Recall
            output += "Recall=TP/(TP+FN)= " + truePostive + "/(" + truePostive + "+" + falseNegative + ")=";
            pineChay = truePostive + falseNegative;
            float recallresult = (float)truePostive / pineChay;
            output += recallresult + "\n";
            double recall = Math.Round(recallresult, 2);
            output += "Recall = " + recall * 100 + "%\n\n";*/

            //Error rate
            output += "Error Rate=(FP+FN)/(TP+TN+FN+FP)\n";
            pineChay = truePostive + trueNegative + falsePositive + falseNegative;
            float errorRate = (falsePositive + falseNegative) / pineChay;
            output += "Error Rate = (" + falsePositive + "+" + falseNegative + ")/" + "(" + truePostive + "+" + trueNegative + "+" + falsePositive + "+" + falseNegative + ") =" + Math.Round(errorRate,2) + "\n";
            double nn = Math.Round(errorRate, 2);
            output += "Error Rate = " + nn * 100 + "%\n\n";
            
            richTextBox1.Text = "";
            richTextBox1.Text = output;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"D:\CreditCardFraud.txt"))
            {
                
                if (!String.IsNullOrEmpty(writeData))
                    file.WriteLine(writeData);
            }
            MessageBox.Show("Credit Card Detailed Calculation Steps are stored at D:\\CreditCardFraud.txt");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateDataSetForm updateForm = new UpdateDataSetForm();
            updateForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Dictionary<int, bool> CheckSelectedBox()
        {
            int count=0;
            Dictionary<int, bool> checkBoxList = new Dictionary<int, bool>();
            for (int i=0;i<rb.Length;i++)
            {
                if (rb[i].Checked == true)
                {
                    ++count;
                    checkBoxList.Add(i + 1, true);
                }
                else
                    checkBoxList.Add(i + 1, false);                    
            }
            return checkBoxList;
        }
    }
}
