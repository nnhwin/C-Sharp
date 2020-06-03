using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace SpamDetection
{
    public partial class Form1 : Form
    {
        string fileName = null;
        RadioButton[] rb;
        List<string[]> table = new List<string[]>();
        int mySelectedIndex = 0;
        int rowCount = 0, colCount = 0;
        SpamDetectionAlgo detectAlgo;
        List<String> lastData;
        List<String> dataListWithValue;
        IDictionary<int, string> rateValue;
       // List<String> rateValueList;
        List<String> finalResultForDataset=new List<String>();
        Dictionary<int, String> dataWithID;
        Dictionary<int, String> dataWithLabel;
        Dictionary<int, String> preData1=new Dictionary<int, string>();
        String outputString = "";
        Dictionary<int, String> result = new Dictionary<int, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void spamDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    fileName = ofd.FileName;
                    textBox1.Text = fileName;

                }

            }

            string[] colNames = StartDetection();
            rb = new RadioButton[colNames.Length];

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
               // Console.WriteLine(i + "......." + colNames[i]);
                rb[i] = new RadioButton();
                rb[i].Text = colNames[i];
                rb[i].AutoSize = true;
                rb[i].Location = new Point(10, i * 40);
                panel1.Controls.Add(rb[i]);
            }
        }//button 1 click
        string[] StartDetection()
        {
            String str = "";
            int cint = 0;
            string firstline = null;

            using (var r = new System.IO.StreamReader(fileName))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if ((line != null || line != String.Empty) && cint == 0)
                    {
                        firstline = line;
                        ++cint;
                    }
                    else
                    {
                        String[] ss = new String[5];
                        String str1 = line.Substring(0, line.IndexOf(',') + 1);
                        line = line.Replace(str1, "").Trim();
                        String str2 = line.Substring(0, line.IndexOf(",") + 1);
                        line = line.Replace(str2, "").Trim();
                        String str3 = line.Substring(0, line.IndexOf(',') + 1);
                        line = line.Replace(str3, "").Trim();
                        String str4 = line.Substring(0, line.LastIndexOf(",") + 1);
                        line = line.Replace(str4, "").Trim();
                        String str5 = line;

                        ss[0] = str1;
                        ss[1] = str2;
                        ss[2] = str3;
                        ss[3] = str4;
                        ss[4] = str5;
                        table.Add(ss);
                        ++rowCount;
                        ++cint;
                    }                  
                }
                r.Close();
            }
            string[] colNames = GetAttributeCount(firstline);
            colCount = colNames.Length;
            return colNames;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            mySelectedIndex = CheckSelectedBox();
            detectAlgo = new SpamDetectionAlgo();
            detectAlgo.ExtractSelectedPortion(table, mySelectedIndex);
            dataWithID = detectAlgo.dataWithId;
            dataWithLabel = detectAlgo.classWithId;

            richTextBox1.Text = "";
            int count = 0;

            String str = "";
            foreach (KeyValuePair<int, string> item in dataWithID)
            {
                foreach (KeyValuePair<int, string> item2 in dataWithID)
                {
                    if(item.Key==item2.Key)
                    {
                        str += item.Key + ".   " + item.Value+"\n";
                        break;
                    }
                }
            }
            richTextBox1.Text = str;
        }
        int CheckSelectedBox()
        {
            int index = 0;
            
            for (int i = 0; i < rb.Length; i++)
            {
                if (rb[i].Checked == true)
                {
                    index = i;
                }
              
            }
            return index;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<int,String> preproData = detectAlgo.StopWordRemovalFun(dataWithID);
            dataWithID = preproData;
            richTextBox1.Text = "";

            preData1 = detectAlgo.StemmingProcess(dataWithID);
            richTextBox1.Text = "";
            foreach (KeyValuePair<int, string> item in preData1)
            {
                richTextBox1.AppendText(item.Key + ". " + item.Value + "\n");
            }              
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //  Dictionary<int, String> dataWithID;
            //Dictionary<int, String> dataWithLabel;
            //Result
            ArrayList finalResult = new ArrayList();
            finalResult.Add("No,Comment Text,DatasetClass Label, Our Class Label");
            foreach (KeyValuePair<int, string> dataID in dataWithID)
            {
                //Console.WriteLine(dataID.Key + "......@@...." + dataID.Value);
                foreach (KeyValuePair<int, String> dataLabel in dataWithLabel)
                {
                   // Console.WriteLine(dataLabel.Key + "...!!......." + dataLabel.Value);
                    if (dataID.Key == dataLabel.Key)
                    {
                        String text = dataID.Value;
                        String classLabel = dataLabel.Value;
                        foreach (KeyValuePair<int, String> resultPair in result)
                        {
                            int idkey = resultPair.Key;
                          //  Console.WriteLine(text + "..........." + text1);
                            if (dataID.Key==idkey)
                            {
                                String str = "";
                                if (classLabel.Equals("1"))
                                    str = "Spam";
                                else
                                    str = "NoSpam";
                                String ss = dataID.Key + "," + text + "," + str + "," + resultPair.Value;
                                finalResult.Add(ss);
                                Console.WriteLine(dataID.Key + "," + text + "," + str + "," + resultPair.Value);
                            }
                        }//resultPair
                    }//==
                }//dataLabel
            }//foreach dataID


            AccuracyForm af = new AccuracyForm();
            af.StartAccuracy(finalResult);
            af.Show();
            
        }
        /// <summary>
        /// ////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            dataListWithValue = new List<String>();
            richTextBox1.Text = "";
            foreach(String str in lastData)
            {
                String[] eachWord = str.Split(' ');
                string pp = str.Replace("@", ".  ");
                richTextBox1.AppendText(pp + "\n");
                //float[] valueWord = new float[eachWord.Length];
                for(int i = 0; i < eachWord.Length; i++)
                {
                    if (!eachWord[i].Contains("@"))
                    {
                        float tfvalue = detectAlgo.FindTFValue(eachWord[i], eachWord);
                        double idfvalue = detectAlgo.FindIDFValue(lastData, eachWord[i]);
                        int numberOfWord = detectAlgo.getCount(eachWord[i], eachWord);
                        double tfidfvalue = tfvalue * idfvalue;
                        string tfidfstr = tfidfvalue.ToString("F");
                        string tfvaluestr = tfvalue.ToString("0.000");
                        string idfvaluestr = idfvalue.ToString("0.000");
                        
                        dataListWithValue.Add(str+","+eachWord[i] + ","+ numberOfWord+"," + tfvaluestr + "," + idfvaluestr + "," + tfidfstr);
                        Console.WriteLine(eachWord[i] + "--> TF=" + tfvaluestr + " IDF=" + idfvaluestr + " TF*IDF=" + tfidfstr);
                        richTextBox1.AppendText(String.Format("{0,20}", eachWord[i]) + "--> TF=" + String.Format("{0,5}",tfvaluestr) + "\tIDF=" + String.Format("{0,5}", idfvaluestr) + "\tTF*IDF=" + String.Format("{0,5}",tfidfstr) +"\n");
                    }
                    
                }
                richTextBox1.AppendText("\n...............................................\n");
                Console.WriteLine("\n\n");
            }
           
        }

        /// <summary>
        /// ////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            WordVector wv = new WordVector(preData1, dataWithLabel);
            //data,data,data --> 0
            Dictionary<String,String> dataClass= wv.ExtractWordVector();
            BayesianClassification bayes = new BayesianClassification();
            bayes.StartNaiveBayes(dataClass);
            richTextBox1.Text = "";
            richTextBox1.Text = bayes.output;
            outputString = bayes.output;
            result = bayes.resultPair;
        }     

        private void button8_Click(object sender, EventArgs e)
        {
            
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\YouTubeSpam.txt"))
                {
                    file.Write(outputString);
                }
                MessageBox.Show("Results are succesfully writen in a file under D:\\YouTubeSpamDetection.txt","Write Result to Disk");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

      



        /// <summary>
        /// ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        string[] GetAttributeCount(string str)
        {
            String[] names = str.Split(',');
            return names;
        }
    }
}
