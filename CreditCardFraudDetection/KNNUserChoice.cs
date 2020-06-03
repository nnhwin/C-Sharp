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
    public partial class KNNUserChoice : Form
    {
        int similarCount = 0;
        String output = "Finding most similar Cluster in accordance with user's needs..............\n\n";
        Dictionary<String, Double> finalList = new Dictionary<string, double>();
        ArrayList allData = new ArrayList();
        int topSimilarCount = 0;
        public int topKItem = 0;
        public KNNUserChoice()
        {
            InitializeComponent();
        }

        private void KNNUserChoice_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            cbDecoration.SelectedIndex = 0;
            cbFabricType.SelectedIndex = 0;
            cbMaterial.SelectedIndex = 0;
            cbNeckLine.SelectedIndex = 0;
            cbPatternType.SelectedIndex = 0;
            cbPrice.SelectedIndex = 0;
            textBox1.Text = "";
            cbSeason.SelectedIndex = 0;
            cbSleeveLength.SelectedIndex = 0;
            cbStyle.SelectedIndex = 0;
            cbWaiseLine.SelectedIndex = 0;
            cbSize.SelectedIndex = 0;
            k_combo.SelectedIndex = 8;
            button4.Enabled = false;
        }

        String NormalizedRating(String value)
        {
            Double intValue = Convert.ToDouble(value);
            Double min = 0.0f;
            Double max = 5.0f;
            Double newmin = 0.0f;
            Double newmax = 1.0f;
            Double newValue = ((intValue - min) / (max - min) * (newmax - newmin)) + newmin;
            newValue = Math.Abs(newValue);
            //Min-max normaliztion
            return Convert.ToString(newValue);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            richTextBox1.Text = "";
            String v_decor = cbDecoration.SelectedItem.ToString();
            String v_size = cbSize.SelectedItem.ToString();
            String v_fabric = cbFabricType.SelectedItem.ToString();
            String v_material = cbMaterial.SelectedItem.ToString();
            String v_neckline = cbNeckLine.SelectedItem.ToString();
            String v_pattern = cbPatternType.SelectedItem.ToString();
            String v_price = cbPrice.SelectedItem.ToString();
            String v_season = cbSeason.SelectedItem.ToString();
            String v_sleevelength = cbSleeveLength.SelectedItem.ToString();
            String v_style = cbStyle.SelectedItem.ToString();
            String v_waistline = cbWaiseLine.SelectedItem.ToString();
            String v_rating = textBox1.Text;

            int i = Convert.ToInt32(v_rating);
            if (i < 0 && i > 5)
            {
                MessageBox.Show("Invalid data in Rating, Fill correct one", "Invalid Data");
                return;
            }

            //Change rating to normalized rating
             v_rating=NormalizedRating(v_rating);

            String[] data = new String[13];

            data[1] = v_style;
            data[2] = v_price;
            data[3] = v_rating;
            data[4] = v_size;
            data[5] = v_season;
            data[6] = v_neckline;
            data[7] = v_sleevelength;
            data[8] = v_waistline;
            data[9] = v_material;
            data[10] = v_fabric;
            data[11] = v_decor;
            data[12] = v_pattern;


            //Load Data From CSV file and dive them according to cluster
            allData = LoadCSVDataWithTheirCluster();

            //Get ClusterCount
            int clusterCount = GetClusterCount(allData);
            output+= "Cluster count " + clusterCount+"\n";
            //Console.WriteLine("Cluster count " + clusterCount);

            Dictionary<String, ArrayList> hashCluster = GroupCluster(allData, clusterCount);
            // PrintCluster(hashCluster);

            Dictionary<String, Double> similarityPerCluster = FindDistanceCluster(hashCluster,data);
            var min = similarityPerCluster.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            output += "\nTherefore, Most Similar Cluster Group = "+min+"\n";
           // Console.WriteLine("Max Cluster - " + max);

            ArrayList maxSimiClusterData = hashCluster[min];
            // Console.WriteLine("Max data size " + maxSimiClusterData.Count);

            Dictionary<String, Double> list =DistanceBetweenInputAndFinalCluster(maxSimiClusterData,data);
            output+= "\n****************Most Similar Dress Item**************************\n";
            //Console.WriteLine("*****************************Most Similar Dress Item **********************************");
            var sortedDict = from entry in list orderby entry.Value ascending select entry;
            finalList = new Dictionary<string, double>();
            int count = 0;
            foreach (KeyValuePair<String, Double> pair in sortedDict)
            {
                //Console.WriteLine("ID-{0} has Distance Value {1} with the Input.",
                //          pair.Key, pair.Value);
                int kkvalue = Convert.ToInt32(pair.Key);
                output += "Dis(ID-" +(--kkvalue)+ ",Input)= " + pair.Value +"\n";
                ++count;
                finalList.Add(pair.Key, pair.Value);
            }

            similarCount = count;
            output += "\n\nThe most similar cluster is " + min;
            output +="\nThe elements of " + min + " is ...............";
            output += "\nTotal Similar Items =" + count+"\n";
            topSimilarCount = count;
            label4.Text += count;
            label4.Visible = true;
            richTextBox1.Text = output;


            //Write Data into DataGridView
            //First produce csv file
            ExtractData(allData, finalList);
        }

        void ExtractData(ArrayList data, Dictionary<String,Double> list)
        {
            ArrayList dataWrite = new ArrayList();
            String title_str = "Object-ID,Dress_ID,Style,Price,Rating,Size,Season,NeckLine,SleeveLength,WaiseLine,Material,FabricType,Decoration,PatternType,Cluster,DistanceValue";
            String[] title = title_str.Split(',');
            for(int i = 0; i < data.Count; i++)
            {
                String pp = Convert.ToString(i + 1);               
               
                //String id = pp.Substring(0, pp.IndexOf(','));
                   
              foreach (KeyValuePair<String, Double> pair in list)
              {
                   if(pp.Equals(pair.Key))
                   {
                        String ss = pair.Key+","+data[i] + "," + pair.Value;
                        String[] arr = ss.Split(',');
                        dataWrite.Add(arr);
                   }
              }
               
            }//for i

            String filename = @"D:\UserDataCSV.csv";
            StreamWriter file = new StreamWriter(@filename);
            
            //my2darray  is my 2d array created.
            for (int i = 0; i < dataWrite.Count; i++)
            {
                if (i == 0)
                {

                    for (int j = 0; j < title.Length; j++)
                    {
                        file.Write(title[j]);
                        if(j!=title.Length-1)
                            file.Write(",");
                    }
                    file.Write("\n");
                }

                String[] dd =(String[]) dataWrite[i];
                for (int j =0; j < dd.Length; j++)
                {
                    file.Write(dd[j]);
                    if (j != title.Length - 1)
                        file.Write(",");
                  
                 
                }
                if(i!=dataWrite.Count-1)
                //go to next line
                    file.Write("\n");

            }
            file.Close();
            this.dataGridView1.Refresh();
            dataGridView1.DataSource = ReadCsv(@filename);
            dataGridView1.Columns.Remove("Object-ID");
            this.dataGridView1.Sort(this.dataGridView1.Columns["DistanceValue"], ListSortDirection.Ascending);
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
        Dictionary<String, Double> DistanceBetweenInputAndFinalCluster(ArrayList ClusterData,String[] input)
        {
            String[] obj = new String[ClusterData.Count];
            double[] value = new double[ClusterData.Count];
            //SortedList sortedList = new SortedList();
            Dictionary<String, Double> finalValue = new Dictionary<string, double>();

            for (int i = 0; i < ClusterData.Count; i++)
            {
                String dd = ClusterData[i].ToString();
                double disValue = 0;
                String idName= dd.Substring(0, dd.IndexOf(","));
                String str1 = "", str2 = "";
                //Cal Distance
                String temp = dd.Substring(dd.IndexOf(",")+1);
                    temp = temp.Substring(0, dd.LastIndexOf(",")-2);
                   // Console.WriteLine(temp);
                    String[] data = temp.Split(',');
                    for(int j = 0; j < 12; j++)
                    {
                        if (j != 2)
                        {
                            String temp1 = data[j].ToLower();
                            String temp2 = input[j + 1].ToLower();
                            str1 += temp1 + ",";
                            str2 += temp2 + ",";
                            if (!temp1.Equals(temp2))
                            {
                                ++disValue;
                            }
                              
                        }
                        else
                        {
                            str1 += data[j] + ",";
                            str2 += input[j+1] + ",";
                            if (data[j] != input[j + 1])
                            {
                            double dd1 =Convert.ToDouble(data[j]);
                            double dd2 = Convert.ToDouble(input[j + 1]);
                            dd1 = dd1 - dd2;
                            dd1 = dd1 * dd1;
                            dd1 = Math.Sqrt(dd1);
                            disValue += dd1;
                            }
                        }
                    }//for j
                //output+= "\nObject -" + obj[i] + " and Input .................................\n";
                //output += str1 + "\n";
                //output +=str2+"\n";
                //output += "___________________________________________________________\n";
                //output += "Distance Value = " + disValue + "\n";
                //Console.WriteLine("\nObject -" + obj[i] + " and Input .................................");
                //Console.WriteLine(str1);
                //Console.WriteLine(str2);
                //Console.WriteLine("___________________________________________________");
                //Console.WriteLine("Distance Value = " + disValue);
                finalValue.Add(idName, disValue);
                //sortedList.Add(idName, simiValue);
            }
            // return sortedList;
            return finalValue;
        }


        Dictionary<String,Double> FindDistanceCluster(Dictionary<String, ArrayList> hashCluster,String[] data)
        {
            Dictionary<String, Double> similarityPerCluster = new Dictionary<string, double>();
            output+= "\nDistance Value Per Cluster.............................................\n";
            //Console.WriteLine("\n\nSimilarity Value Per Cluster.............................................");
            foreach (KeyValuePair<string, ArrayList> pair in hashCluster)
            {
                ArrayList al = pair.Value;
                //output+= "Seed Value of -" + pair.Key + "...............................\n";
                
                String[] seed_data = FindSeed(al);
                double distance = 0;
                String str1 = "", str2 = "";
                //Find Distance with INput
                for (int j = 1; j < 13; j++)
                {
                    if (j != 3)
                    {
                        String temp1 = seed_data[j].ToLower().Trim();
                        String temp2 = data[j].ToLower().Trim();
                        str1 += temp1 + ",";
                        str2 += temp2 + ",";
                        if (!temp1.Equals(temp2))
                        {
                            ++distance;
                        }
                    }
                    else
                    {
                        str1 += seed_data[j] + ",";
                        str2 += data[j] + ",";
                        if (seed_data[j] != data[j])
                        {
                            double temp_var1 = Convert.ToDouble(seed_data[j]);
                            double temp_var2 = Convert.ToDouble(data[j]);
                            temp_var1 = temp_var2 - temp_var1;
                            temp_var1 = temp_var1 * temp_var1;
                            temp_var1 = Math.Sqrt(temp_var1);
                            distance += temp_var1;
                        }
                           
                    }
                }//j
                 //output += str1 + "\n";
                 //output += str2 + "\n";
                 // output += "_______________________________________________________\n";
                 // output += "Distance Value = " + distance + "\n";

                //Console.WriteLine(str1);
                //Console.WriteLine(str2);
                //Console.WriteLine("______________________________________________________");
                //Console.WriteLine("Distance value = " + distance);
                distance=Convert.ToDouble(String.Format("{0:.###}", distance));
                output += "Dis(" + pair.Key + ", Input ) =" + distance + "\n";
                similarityPerCluster.Add(pair.Key, distance);
            }//cluster

            return similarityPerCluster;
        }

        String[] FindSeed(ArrayList al)
        {
            String[] dataStr = new String[13];
            //AssignNull
            for (int i = 1; i < 13; i++)
            {
                String str = "";
                dataStr[i] = str;
            }

            for(int pp = 0; pp < al.Count; pp++)
            {
                String[] objData = al[pp].ToString().Split(',');
                //output += al[pp].ToString() + "\n";
                //Console.WriteLine(al[pp].ToString());
                String temp_str = "";
                for (int i = 1; i < 13; i++)
                {
                    temp_str = dataStr[i];
                    temp_str += objData[i] + ",";
                    dataStr[i] = temp_str;
                }
                temp_str = temp_str.Substring(0, temp_str.Length - 1);
              
            }//pp

            //print
            //    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>");
            //for(int i = 1; i < 13; i++)
            //{
            //    Console.WriteLine(dataStr[i]);
            //}
           // output += "__________________________________________________\n";
           // Console.WriteLine("_________________________________________________________________");
            String[] avg_data = FindSeedPerCluster(dataStr);
            
            //String outputStr = "Avg -->";
           // String output1 = "\t";
            for (int i = 1; i < avg_data.Length; i++)
            {
               // output1 += avg_data[i] + "   ";
                // Console.Write(avg_data[i] + " ");
            }
           // outputStr += output1 + "\n";
           // output += outputStr + "\n";
            //Console.WriteLine(outputStr+"\n");
            return avg_data;
        }

        String[] FindSeedPerCluster(String[] data)
        {
            String[] average_data = new String[data.Length];

            for (int i = 1; i < data.Length; i++)
            {
                //  Console.Write("\nDATA>>>>>>>>>>>\n\t" + data[i]);
                String str = data[i];
                if (str.EndsWith(","))
                    str = str.Substring(0, str.Length - 1);

                String[] words = str.Split(',');

                if (i != 3)
                {
                    var occrs = words.GroupBy(x => x.ToLower())
                  .ToDictionary(g => g.Key, g => g.Count());

                    int max_count = 0;
                    String max_name = "";
                    foreach (var pair in occrs)
                    {
                        int count = pair.Value;
                        String name = pair.Key;
                        if (max_count < count)
                        {
                            max_count = count;
                            max_name = name;
                        }
                    }//
                    //  Console.WriteLine(max_name + "............" + max_count);
                    average_data[i] = max_name;
                }
                else
                {
                    Double sum = 0;
                    //for rating find average
                    for (int p = 0; p < words.Length; p++)
                    {
                        String vv = words[p].Trim();
                        if (!String.IsNullOrEmpty(vv))
                        {
                            Double dd = Convert.ToDouble(vv);
                            sum += dd;
                        }//if
                    }//for p
                    double avg = sum / words.Length;
                    average_data[i] = Convert.ToString(avg);
                    //  Console.WriteLine(data[i] + "............" + avg); ;
                }//else
            }//i
            return average_data;
        }
        //void PrintCluster(Dictionary<String,ArrayList> hash)
        //{
        //    foreach (KeyValuePair<string, ArrayList> pair in hash)
        //    {
        //        ArrayList aa = pair.Value;
        //        //Console.WriteLine("Cluster =" + pair.Key);
        //        output+= "Cluster =" + pair.Key+"\n";
        //        for (int i = 0; i < aa.Count; i++)
        //        {
        //            //Console.WriteLine(aa[i]);
        //            output += aa[i] + "\n";
        //        }
                   
        //    }
        //}
        Dictionary<string, ArrayList> GroupCluster(ArrayList data,int clustercount)
        {
            Dictionary<string, ArrayList> hash = new Dictionary<string, ArrayList>();
            for (int i = 0; i < clustercount; i++)
            {
                String ss = "Cluster-" + (i + 1);
                for(int j = 0; j < data.Count; j++)
                {
                    String dd = data[j].ToString();
                    
                    if (dd.Contains(ss))
                    {
                        if (hash.ContainsKey(ss))
                        {
                            ArrayList al = hash[ss];
                            String pp = dd.Substring(dd.IndexOf(",") + 1);
                            al.Add((j+1)+","+pp);
                            hash.Remove(ss);
                            hash.Add(ss, al);
                            
                        }
                        else
                        {
                            ArrayList al = new ArrayList();
                            String pp = dd.Substring(dd.IndexOf(",") + 1);
                            al.Add((j+1)+","+pp);
                            hash.Add(ss, al);
                        }
                        
                    }//if
                }               
            }//for
            return hash;
        }
        int GetClusterCount(ArrayList al)
        {
            int count = 0;
            HashSet<String> hset = new HashSet<string>();
            
            for(int i=0;i<al.Count;i++)
            {
                //skip title
                if (i != 0) { 
                    String str = Convert.ToString(al[i]);
                    String[] ss = str.Split(',');
                    hset.Add(ss[ss.Length-1]);
                }
            }
            count = hset.Count;

            return count;
        }
        ArrayList LoadCSVDataWithTheirCluster()
        {
            String fname = "D:\\ClusterOutput.csv";            
            ArrayList al = new ArrayList();
            using (var r = new System.IO.StreamReader(fname))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if ((line != null || line != String.Empty))
                    {
                        al.Add(line);         
                    }
                }
                r.Close();
            }
            return al;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"D:\UserChoiceSimilarDress.txt"))
            {
                file.WriteLine(output);  
               
            }
        
            MessageBox.Show("Results are written into D:\\UserChoiceSimilarDress.txt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            int count = 0;
            
            int kCount =Convert.ToInt32(k_combo.SelectedItem.ToString());
            topKItem = kCount;
            if (kCount > topSimilarCount)
            {
                MessageBox.Show("K Count should not be greater than the number of elements in Group", "Invalid Number Choice");
                return;
            }

            String temp_output = "No.   Object-ID   Dress-ID   Similarity Value\n";
            ArrayList newList = new ArrayList();
            Dictionary<String, Double> limitedData = new Dictionary<string, double>();
            int idnum = 0;
            foreach (KeyValuePair<string, Double> pair in finalList)
            {
                if (count < kCount)
                {
                    int key = Convert.ToInt32(pair.Key);
                    for (int i = 0; i < allData.Count; i++)
                    {
                        if (key == (i + 1))
                        {
                            ++idnum;
                            String dressID = allData[i].ToString().Substring(0, allData[i].ToString().IndexOf(','));
                            newList.Add(allData[i] + "," + pair.Value);
                            temp_output += idnum+".   "+ pair.Key + "  " + dressID + "  " + pair.Value + "\n";
                            limitedData.Add(pair.Key, pair.Value);
                        }
                    }
                    ++count;
                }
                else
                    break;
                
            }//for each

            richTextBox1.Text = "";
            richTextBox1.Text = temp_output;
            label4.Text = "Top K-Item Count : " + kCount;

            this.dataGridView1.DataSource = null;
            ExtractData(allData, limitedData);
            this.dataGridView1.Sort(this.dataGridView1.Columns["DistanceValue"], ListSortDirection.Ascending);

        }//button click

        private void button4_Click(object sender, EventArgs e)
        {
            PerformanceEvaluationForm pf = new PerformanceEvaluationForm(finalList,topKItem);
            pf.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
