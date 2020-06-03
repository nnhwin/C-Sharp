using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamDetection
{
    class BayesianClassification
    {
        Dictionary<String, String> data = new Dictionary<string, string>();
        public Dictionary<int, String> resultPair= new Dictionary<int, string>();
        public String output = "";

        public BayesianClassification()
        {
        }

        public void NaiveByesForInput(Dictionary<String, String> data,String inputStr)
        {

        }

        public void StartNaiveBayes(Dictionary<String, String> data)
        {
            this.data = data;

            Dictionary<String, int> classLabel = CountClassLabel(data);
            Dictionary<string, string> ResultList = new Dictionary<string, string>();

            int totalTranscations = data.Count;
            int ccCount = 0;
            //Find class label one by one
            foreach (KeyValuePair<String, string> currentItem in data)
            {
                output += (++ccCount) + ". " + currentItem.Key.ToString() + "\n";
                String[] currentArr = currentItem.Key.ToString().Trim().Split(',');
                //assign all count zero
                int[] yCount = new int[currentArr.Length];
                int[] nCount = new int[currentArr.Length];

                foreach (KeyValuePair<String, string> DataItem in data)
                {
                    String[] dataWordArr = DataItem.Key.ToString().Trim().Split(',');
                    //Console.WriteLine("............." + currentItem.Key + "............................" + DataItem.Key);
                    for (int i = 0; i < currentArr.Length; i++)
                    {
                        String inputStr = currentArr[i].Trim().ToLower();
                        for (int j = 0; j < dataWordArr.Length; j++)
                        {
                            String dataStr = dataWordArr[j].Trim().ToLower();
                            if (!dataStr.Contains("www") || !dataStr.Contains("http"))
                            {
                                //Clean
                                inputStr = CleanData(inputStr);
                                dataStr = CleanData(dataStr);
                                if (inputStr.Equals(dataStr))
                                {
                                    //yes count P(x|yes)
                                    if (DataItem.Value.Equals("1"))
                                    {
                                        int cc = yCount[i];
                                        ++cc;
                                        yCount[i] = cc;
                                    }
                                    else // for no
                                    {
                                        int cc = nCount[i];
                                        ++cc;
                                        nCount[i] = cc;
                                    }
                                    break;
                                }//Equal

                            }//!equal www
                        }//j
                    }//i

                }//inner for

                //Whole Yes and NO Value
                float wholeYesValue = 0, wholeNoValue = 0;
                int wholeYesCount = 0, wholeNoCount = 0;
                foreach (KeyValuePair<String, int> classItem in classLabel)
                {
                    if (classItem.Key.Equals("Spam"))
                    {
                        int vv = classItem.Value;
                        wholeYesCount = vv;
                        wholeYesValue = (float)vv / totalTranscations;
                    }
                    else
                    {
                        int vv = classItem.Value;
                        wholeNoCount = vv;
                        wholeNoValue = (float)vv / totalTranscations;
                    }
                }

                double yesAllValue = 1, noAllValue = 1;
                String wordStr = "";
                String yesStr = "";
                String noStr = "";
                double[] yesValuesPerWord = new double[currentArr.Length];
                double[] noValuesPerWord = new double[currentArr.Length];
                for (int i = 0; i < currentArr.Length; i++)
                {
                    //Calculate YCount
                    //classLabel                  
                    wordStr += currentArr[i] + ",";

                    //Yes
                    double value = (double)yCount[i] / wholeYesCount;
                    yesStr += yCount[i] + "/" + wholeYesCount + "* ";
                    yesValuesPerWord[i] = value;


                    //NO
                    value = (double)nCount[i] / wholeNoCount;
                    noStr += nCount[i] + "/" + wholeNoCount + "* ";
                    noValuesPerWord[i] = value;

                }

                wordStr = wordStr.Substring(0, wordStr.Length - 1);
                yesStr = yesStr.Substring(0, yesStr.Length - 2) + "=";
                noStr = noStr.Substring(0, noStr.Length - 2) + "=";
                for (int i = 0; i < currentArr.Length; i++)
                {
                    //yes
                    yesStr += yesValuesPerWord[i] + "*";
                    yesAllValue *= yesValuesPerWord[i];
                    noAllValue *= noValuesPerWord[i];
                    noStr += noValuesPerWord[i] + "*";
                }
                yesStr = yesStr.Substring(0, yesStr.Length - 1) + "=" + yesAllValue;
                noStr = noStr.Substring(0, noStr.Length - 1) + "=" + noAllValue;

                //Find max from yesAllValue
                String Spam_value = FindMax(yesAllValue, noAllValue);

                Console.WriteLine(wordStr + "\n" + yesStr + "\n" + noStr);
                output += "P(X|Spam)=" + yesStr + "\n" + "P(X|Non-Spam)=" + noStr + "\n";
                output += "Classification Result  -" + Spam_value + "\n";
                output += "____________________________________________________________________\n";
                ResultList.Add(currentItem.Key, Spam_value);
                resultPair.Add(ccCount, Spam_value);
                //
            }//outer for

            //Final Result
            output += "\n\n***********Naive Bayesian Classification Result*****************************\n";
            int count = 0;
            foreach (KeyValuePair<String, string> item in ResultList)
            {
                output += (++count) + ".  " + item.Key + "\t Label- " + item.Value + "\n";
                Console.WriteLine(item.Key + "....$$..........." + item.Value);
            }

        }

        // public BayesianClassification(Dictionary<String, String> data) {



        //}

        String FindMax(double yesValue,double noValue)
        {
            String spam = "NoSpam";
            if (yesValue > noValue)
            {
                spam = "Spam"; 
            }
            return spam;
        }

        String CleanData(String str)
        {
            String pp = str;
            if (pp.EndsWith(","))
                pp = pp.Substring(0, pp.Length - 1);

            pp = pp.Replace(":", "");

            return pp;
        }

        Dictionary<String,int> CountClassLabel(Dictionary<String,String> data)
        {
            Dictionary<String, int> labelCount = new Dictionary<string, int>();
            int spamCount = 0;
            int nonSpamCount = 0;
            foreach (KeyValuePair<String, string> item in data)
            {
                if (item.Value.Equals("1"))
                    ++spamCount;
                else
                    ++nonSpamCount;
            }
            labelCount.Add("Spam", spamCount);
            labelCount.Add("NonSpam", nonSpamCount);
            Console.WriteLine("Spam Count " + spamCount);
            Console.WriteLine("Non Spam Count " + nonSpamCount);
            return labelCount;
        }
        //static void Main()
        //{
        //    Dictionary<String, String> dataLabel = new Dictionary<string, string>();
        //    dataLabel.Add("check, tube, please,channel:, kobyoshi02", "1");
        //    dataLabel.Add("guy, check, new, channel, please, leave, like, comment, please", "0");
        //    dataLabel.Add("test,please,check,say,murdevcom", "1");
        //    //dataLabel.Add("shak,sexy,a,channel,enjoy,?", "1");
        //    //dataLabel.Add("watch,v = vtarggvgtwq,check,?", "1");
        //    //dataLabel.Add("check,new,website,site,kid,stuff,kidsmediausa,com", "1");
        //    //dataLabel.Add("subscribe,channel,?", "1");
        //    //dataLabel.Add("turn,mute,soon,came,want,check,view ?", "0");
        //    //dataLabel.Add("check,channel,funny,video,?", "1");
        //    //dataLabel.Add("shouldd,check,channel,tell,do,?", "1");
        //    //dataLabel.Add("subscribe,me ?", "1");
        //    //dataLabel.Add("start,read,do,stop,do,subscribe,one,day,youre,entire,family,will,die,want,stay,alive,subscribe,right,now ?", "1");
        //    //dataLabel.Add("https://twittercom/gbphotographygb?", "1");
        //    //dataLabel.Add("subscribe,like,comment ?", "1");
        //    //dataLabel.Add("please,like,:d,https://premiumeasypromosappcom/voteme/19924/616375350?", "1");
        //    //dataLabel.Add("hello,do,like,gam,art,video,scientific,experiment,tutorial,lyric,video,do,please,check,our,channel,subscribe,weve,start,soon,hope,will,be,able,cover,our,expectation,can,also,check,weve,got,far,?", "1");
        //    //dataLabel.Add("im,check,view ?", "0");
        //    //dataLabel.Add("http://wwwebaycom/itm/171183229277,sspagename=strk:meselx:it&amp;,trksid=p3984m1555l2649,?", "1");
        //    //dataLabel.Add("http://ubuntuonecom/40beuutvu2zkxk4utgpz8k?", "1");
        //    //dataLabel.Add("edm,apparel,company,dicat,br,music,inspir,deign,our,cloth,perfect,rave,music,festival,neon,crop,top,tank,top,t - hirt,v - neck,accessory,follow,facebook,instagraml,free,giveaway,new,visit,our,site,oncueapparel ?", "1");
        //    //dataLabel.Add("think,100,million,view,come,people,want,check,view ?", "0");
        //    //dataLabel.Add("subscribe,channel,people,:d ?", "1");
        //    //dataLabel.Add("show,auburn,pride,here:,http://wwwteespringcom/tigermeathoodie?", "1");
        //    //dataLabel.Add("check,view ?", "0");


          


        //    // new BayesianClassification(dataLabel,idData);
        //    new BayesianClassification(dataLabel);


        //}
    }
}
