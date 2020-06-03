using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamDetection
{
    class WordVector
    {
        Dictionary<int, String> data = new Dictionary<int, string>();
        Dictionary<int, String> label = new Dictionary<int, string>();
        public WordVector(Dictionary<int, String> data, Dictionary<int, String> label)
        {
            this.data = data;
            this.label = label;
        }
        public Dictionary<String,String> ExtractWordVector()
        {
            Dictionary<int, String> vector = new Dictionary<int, String>();
            Dictionary<String, String> dataLabel = new Dictionary<string, string>();
            foreach (KeyValuePair<int, string> item in data)
            {
                String ss = item.Value;
                String pp = "";
                foreach (KeyValuePair<int, string> item1 in label)
                {
                    pp = "";
                    if (item.Key == item1.Key)
                    {
                        String[] words = item.Value.ToString().Trim().Split(' ');
                        foreach (String str in words)
                        {
                            pp += str + ",";
                        }
                        pp = pp.Substring(0, pp.Length - 1);
                        Console.WriteLine(pp + "," + item1.Value);
                        if (dataLabel.ContainsKey(pp))
                        {
                            pp += ","+item1 ;         
                            dataLabel.Add(pp, item1.Value);
                        }
                        else
                            dataLabel.Add(pp, item1.Value);
                    }
                }
            }

            return dataLabel;
        }
    }
}
