using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamDetection
{
    class SpamDetectionAlgo
    {
        public List<string> dataListWithID = null;
        public List<string> dataListWithRating = null;
        //public List<string> newData = null;
        public Dictionary<int, string> dataWithId = new Dictionary<int, string>();
        public Dictionary<int, string> classWithId = new Dictionary<int, string>();

        public Dictionary<int, string> StopWordRemovalFun(Dictionary<int, string> dataListwithID)
        {
            //Change lowercase and remove marks
            Dictionary<int, string> pre_data = new Dictionary<int, string>();
            Dictionary<int, string> dd = new Dictionary<int, string>();
            foreach (KeyValuePair<int, string> item in dataListwithID)
            {
                String temp_var = item.Value.ToLower().Trim();
                temp_var = CleanSymbol(item.Value);
                dd.Add(item.Key, temp_var);
            }

            dataListwithID = ClearData(dd);

            //Remove Stopwords
            Dictionary<int, string> preprocess_dataList = RemoveStopWord(dd);
            
            // preprocess_dataList = RemoveExpletives(preprocess_dataList);
            foreach (KeyValuePair<int, string> item in preprocess_dataList)
            {
               String st= CleanSymbol(item.Value);
                pre_data.Add(item.Key, st);
                //Console.WriteLine(item.Key+">>>>" + item.Value);
                //preprocess_dataList.Add(item.Key, item.Value);
            }
               

            return pre_data;
        }

        Dictionary<int, String> ClearData(Dictionary<int,String> d)
        {
            Dictionary<int, String> temp = new Dictionary<int, String>();
            String str = "";
            foreach (KeyValuePair<int, string> item in d)
            {
                str = item.Value.ToLower();
                str = str.Replace(".,", "").Trim();
                str = str.Replace("!!!!", "").Trim();
                str = str.Replace("^_^", "").Trim();
                str = str.Replace("!_!", "").Trim();
                str = str.Replace("\"", "").Trim();
                str = str.Replace(",", "").Trim();
                str = str.Replace("'", "").Trim();
                str = str.Replace(".", "").Trim();
                str = str.Replace("!", "").Trim();
                str = str.Replace("?", "").Trim();
                str = str.Replace("\"","").Trim();
                str = checkFullStop(str);
                temp.Add(item.Key, str);
                
            }

           
            return temp;
        }

        String checkFullStop(String str)
        {
            String pp = str;
            while (pp.Contains("."))
            {
                pp = pp.Replace(".", "");
            }
            return pp;
        }
        public float FindTFValue(String findWord,String[] ownLine)
        {
            float vv = 0.0f;
            int count = 0;
            foreach(string str in ownLine)
            {
                if (str.ToLower().Equals(findWord.ToLower()))
                    ++count;
            }
            vv = count / (float)ownLine.Length;
            return vv;
        }
        public int getCount(String findWord,String[] ownLine)
        {
            int count = 0;
            foreach (string str in ownLine)
            {
                if (str.ToLower().Equals(findWord.ToLower()))
                    ++count;
            }
            return count;
        }

        public double FindIDFValue(List<String> dataList,String findWord)
        {
            double vv = 0.0f;
            int count = 0;
            foreach(string d in dataList)
            {
                string p = d;
                if (p.Contains(findWord))
                {
                    ++count;
                }
            }
            // IDF = Log(100 / 1 + 20) = Log(4.7619) = 0.67778
            vv=Math.Log10(dataList.Count /(float) (count));
            return vv;
        }

        public Dictionary<int, String> StemmingProcess(Dictionary<int,String> dataList)
        {
            Dictionary<int, String> stemmedData = new Dictionary<int, String>();
            stemmedData =DoProcess1(dataList);

            return stemmedData;
        }

        Dictionary<int, String> DoProcess1(Dictionary<int,String> data)
        {
            Dictionary<int, String> list = new Dictionary<int, String>();
            String temp_str = "";
            foreach (KeyValuePair<int, string> item in data)
            {
                temp_str = "";
                string d = item.Value;
                string[] arr = d.Split(' ');
                for(int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].EndsWith("mming"))
                        arr[i] = arr[i].Replace("mming", "m");
                    if (arr[i].EndsWith("ing"))
                        arr[i] = arr[i].Replace("ing","");
                    if (arr[i].EndsWith("ies"))
                        arr[i] = arr[i].Replace("ies", "y");
                    if (arr[i].EndsWith("ed"))
                        arr[i] = arr[i].Replace("ed", "");
                    if (arr[i].EndsWith("es"))
                        arr[i] = arr[i].Replace("es", "");
                    if (arr[i].EndsWith("s"))
                        arr[i] = arr[i].Replace("s", "");
                }

                foreach(string s in arr)
                {
                    temp_str += s + " ";
                }
                list.Add(item.Key, temp_str);
              
            }
            return list;
        }

        List<String> RemoveExpletives(List<String> dataList)
        {
            List<String> removeWord = new List<string>();

            List<String> words = GetExpletiveList();
            for (int i = 0; i < dataList.Count; i++)
            {
                string data = dataList[i].Trim();
                string after_process_temp = "";
                String[] temp_str = data.Split(' ');

                bool b = false;
                foreach (string st in temp_str)
                {
                    String ss1 = st.Trim().ToLower();
                    foreach (String str in words)
                    {
                        String ss2 = str.Trim().ToLower();

                        if (ss1.Equals(ss2))
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b == false)
                        after_process_temp += ss1 + " ";
                    b = false;
                }
                after_process_temp = after_process_temp.Trim();
                removeWord.Add(after_process_temp);
            }
            return removeWord;
        }

        public List<String> ExtractFeatuers(List<String> dataList)
        {
            List<String> removeFeature = new List<string>();

            List<String> featurelist = GetFeatureList(); 
            for (int i = 0; i < dataList.Count; i++)
            {
                string data = dataList[i].Trim();
                string after_process_temp = "";
                String[] temp_str = data.Split(' ');
                bool b = false;
               
                foreach (string st in temp_str)
                {
                    String ss1 = st.Trim().ToLower();
                    foreach (String str in featurelist)
                    {
                        String ss2 = str.Trim().ToLower();

                        if (ss1.Equals(ss2))
                        {                           
                            b = true;
                            after_process_temp += ss2 + " ";
                        }
                        if (ss1.Contains('@') && b==false)
                        {
                            b = true;
                            after_process_temp += ss1 + " ";
                        }
                           
                    }
                    
                    
                }
                b = false;
                after_process_temp = after_process_temp.Trim();
                removeFeature.Add(after_process_temp);
            }
            return removeFeature; 
        }
       
       Dictionary<int,String> RemoveStopWord(Dictionary<int, String> dataList)
        {
            Dictionary<int, String> removeStopWord = new Dictionary<int, String>();
            List<String> stopword=GetStopWordList();
            foreach (KeyValuePair<int, string> item in dataList)
            {
                string data = item.Value.Trim();
                string after_process_temp = "";
                String[] temp_str = data.Split(' ');
                bool b = false;
                foreach (string st in temp_str)                   
                {
                    String ss1 = st.Trim().ToLower();
                    ss1=CleanSymbol(ss1);
                    ss1 = ss1.Trim();
                    foreach (String str in stopword)
                    {                       
                        String ss2 = str.Trim().ToLower();
                       // Console.WriteLine("Data -" + ss1 + "@..............stopword -" + ss2+"@");
                        if (ss1.Equals(ss2))
                        {
                            b = true;
                            break;                          
                        }
                    }
                    if (b == false)
                        after_process_temp += ss1 + " ";
                    b = false;
                }

                //Remove Link
                CleanSymbol(after_process_temp);

                //Remove Emotionicon
                after_process_temp = after_process_temp.Trim();
                removeStopWord.Add(item.Key,after_process_temp);
            }            
            return removeStopWord;
        }

        String CleanSymbol(String str)
        {
            String pp = "";
            String[] qq = str.Split(',');
            foreach(String ss in qq)
            {
                pp += ss + " ";
            }


            qq = pp.Split('?');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('[');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split(']');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }


            qq = pp.Split('(');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split(')');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('{');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('}');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('^');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('_');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('"');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }


            qq = pp.Split(',');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('?');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split('!');
            pp = "";
            foreach (String ss in qq)
            {
                pp += ss + " ";
            }

            qq = pp.Split(' ');
            pp = "";
            foreach (String ss in qq)
            {
                String kk = ss.Trim();
                kk = kk.Replace("?", "").Trim();
                kk = kk.Replace(".,", "").Trim();
                kk = kk.Replace("!!!!", "").Trim();
                kk = kk.Replace("^_^", "").Trim();
                kk = kk.Replace("!_!", "").Trim();
                kk = kk.Replace("\"", "").Trim();
                kk = kk.Replace(",", "").Trim();
                kk = kk.Replace("'", "").Trim();
                kk = kk.Replace(".", "").Trim();
                kk = kk.Replace("!", "").Trim();
                kk = kk.Replace("?", "").Trim();
                kk = kk.Replace("\"", "").Trim();

                if (!String.IsNullOrEmpty(kk))
                    pp += kk + " ";
            }

           
            return pp;
        }

        List<String> GetFeatureList()
        {
            String root = Directory.GetCurrentDirectory();
            string f = root + "\\features.txt";
            List<String> list = new List<string>();

            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }
        List<String> GetExpletiveList()
        {
            String root = Directory.GetCurrentDirectory();
            string f = root + "\\expletives.txt";
            List<String> list = new List<string>();

            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;
        }
        List<String> GetStopWordList()
        {
            String root = Directory.GetCurrentDirectory();
            string f = root+"\\StopWordList.txt";
            List<String> list = new List<string>();

            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    list.Add(line.ToLower());
                }
            }
            return list;
        }
        public void ExtractSelectedPortion(List<String[]> inputData,int index)
        {
            dataWithId = new Dictionary<int, string>();
            classWithId = new Dictionary<int, string>();
            //List <String> newData = new List<String>();
            //Console.WriteLine("The index is " + index + "......data size " + inputData.Count);
            int count = 0;
            foreach (String[] str in inputData)
            {
                String content = "";
                String label = "";
                for(int i = 0; i < str.Length; i++)
                {
                    if (i == index)
                        content = str[i];
                    else if (i == index + 1)
                        label = str[i];
       
                }
                dataWithId.Add(++count, content);
                classWithId.Add(count, label);
            }
            
        }

      
        List<String> InsertIDnumbertoAllList(List<String> aa)
        {
            List<String> listWithID = new List<String>();
            string str = "";
            int id = 0;
            foreach (var row in aa)
            {
                if (id == 0)
                {
                    ++id;
                    continue;
                }
                str = (id) + "@ " + row;
                ++id;
                
                Console.WriteLine(str);
                //String[] newstr = str.Split('@');
                listWithID.Add(str);
            }
            return listWithID;
        }


    }
}
