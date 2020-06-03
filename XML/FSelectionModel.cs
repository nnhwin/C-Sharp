using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.Collections;

namespace IRXMLThesis
{
    class FSelectionModel
    {
        String xmlFileName, keyfileName, keyTerms, queryString, algorithmName, kNumber;
        XmlDocument dom = new XmlDocument();
        static int space_count = 0;
        static string[] nodeNames = new string[10];
        string outputText = "";
        ArrayList booklist = new ArrayList();
        static int array_index = 0;
        ArrayList books = new ArrayList();
        List<KeyValuePair<string, string>> attributeList = new List<KeyValuePair<string, string>>();
        string[] keyterms_pair;
        double[] keyterms_value;

        List<KeyValuePair<string, double>> first_query_Ktop = new List<KeyValuePair<string, double>>();
        List<KeyValuePair<string, double>> second_query_Ktop = new List<KeyValuePair<string, double>>();
        List<KeyValuePair<string, string>> listed_node_names_value_pair = new List<KeyValuePair<string, string>>();
        string[] combinational_pair;
        ArrayList node_set = new ArrayList();
        ArrayList Q_set = new ArrayList();
        List<KeyValuePair<string, double>> query_value = new List<KeyValuePair<string, double>>();
        ArrayList parent_node_list = new ArrayList();
        HashSet<string> final_node_name_set = new HashSet<string>();
        List<KeyValuePair<HashSet<string>, double>> node_value_pair = new List<KeyValuePair<HashSet<string>, double>>();
        

        public FSelectionModel(String xmlFileName,String keyfileName, String keyterms,String queryString,String algorithmName,String kNumber)
        {
            this.xmlFileName = xmlFileName;
            this.keyTerms = keyterms;
            this.queryString = queryString;
            this.algorithmName = algorithmName;
            this.kNumber = kNumber;
            this.keyfileName = keyfileName;
            outputText = "";
            XmlDocument mDocument = new XmlDocument();
            XmlNode mCurrentNode;
            //mDocument.Load("C:\\Users\\USER\\Desktop\\samplexml1.xml");
            //Console.WriteLine(xmlFileName);
            mDocument.Load(xmlFileName);
            mCurrentNode = mDocument.DocumentElement;

            XmlNodeList nodeList = mCurrentNode.SelectNodes("*");
            DisplayList(nodeList);

            //step 1
            FeatureSpaceCalculation();

            //step 2
            CalculateMutualInformation();

            //step 3
            algorithmName = algorithmName.Trim().ToLower();
            Console.WriteLine(algorithmName);
            GettingKTop();
            PairTopElements();
            AddingIndextoNode();

            if (algorithmName.Equals("baseline algorithm"))
            {
                BaseLineAlgorithm();
            }
            else
            {
                AnchorPruningAlgorithm();
            }

            ////step4


        }

        public void AnchorPruningAlgorithm()
        {
            outputText += "\n\n\n############Anchor Pruning Algorithm########################\n";
            //step 4
            for (int i = 0; i < combinational_pair.Length; i++)
            {
                outputText += "///////////////////////////////////////////\n";
                outputText += "\nFor \"" + combinational_pair[i] + "\"";
                Console.WriteLine("For " + combinational_pair[i] + "........................");
                node_set.Add(combinational_pair[i]);
                ArrayList new_pair_list = GettingNewQueryPair(combinational_pair[i]);
                //showPairList(new_pair_list);

                foreach (string str in new_pair_list)
                {
                    string temp_var = str;
                    temp_var = temp_var.Replace(",", " ");
                    foreach (var node in listed_node_names_value_pair)
                    {
                        string node_val = node.Value;
                        if (node_val.Contains(temp_var))
                        {
                            Q_set.Add(node.Key);
                        }

                    }// node for
                }//str for


                Q_set = RemoveDuplicateItemInSet(Q_set);
                //outputText += "\n\nStep 5............";
                ShowDataForeachPair();

                //Calculate Step 5
                double prob_value = CalcualteStep5(combinational_pair[i], queryString);
                prob_value = Math.Round(prob_value, 3);
                // query_value.Add(new KeyValuePair<string, double>(combinational_pair[i], prob_value));

                //Step 6 Smallest Lowest Common Ancestor
                HashSet<string> slcancestor_list = FindSmallestLowestCommonAncestor(Q_set);

                outputText += "\n\nSmallest Lowest Common Ancestor.......................\n";
                outputText += "SLAC = { ";
                foreach (string tt in slcancestor_list)
                    outputText += tt + ",";
                outputText = outputText.Substring(0, outputText.Length - 1) + " }";
                //    Console.WriteLine(tt);

                Q_set.Clear();

                if (final_node_name_set.Count == 0)
                {
                    final_node_name_set = slcancestor_list;

                    //step 7
                    prob_value = FindNewValue(prob_value, final_node_name_set);
                    prob_value = Math.Round(prob_value, 3);
                    outputText += "\n\nProb_value =" + prob_value;
                    query_value.Add(new KeyValuePair<string, double>(combinational_pair[i], prob_value));
                    node_value_pair.Add(new KeyValuePair<HashSet<string>, double>(slcancestor_list, prob_value));

                    //sort the value 
                    SortedList<double, string> sorted_value = SortingbyValue(query_value);
                    // outputText += "\n\n Step 20..............." ;
                    Console.WriteLine("\nKey Value Pair.....................");
                    for (int k = 0; k < sorted_value.Count; k++)
                    {
                        outputText += " \t\n Q  = {" + sorted_value.Values[k] + "}\n\n";
                        outputText += "prob_value = " + sorted_value.Keys[k] + "\n";
                        Console.WriteLine("key: {0}, value: {1}", sorted_value.Keys[k], sorted_value.Values[k]);
                    }

                    Console.WriteLine("\nNode..............................................................");
                    outputText += "\n\nNode ={ ";
                    foreach (string ss in final_node_name_set)
                    {
                        outputText += ss + ","; ;

                        Console.WriteLine(ss);
                    }
                    outputText = outputText.Substring(0, outputText.Length - 1) + " } \n";
                }
                else //else 1
                {
                    Console.WriteLine("IN ELSE>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    foreach (string ss in final_node_name_set)
                    {
                        Console.WriteLine(ss);
                    }

                }//end of else 1
            }//end i         


            ////sort the value 
            //SortedList<double, string> sorted_value = SortingbyValue(query_value);
            //// outputText += "\n\n Step 20..............." ;
            //Console.WriteLine("\nKey Value Pair.....................");
            //for (int i = 0; i < sorted_value.Count; i++)
            //{
            //    outputText += " \t\n Q  = {" + sorted_value.Values[i] + "}\n\n";
            //    outputText += "prob_value = " + sorted_value.Keys[i] + "\n";
            //    Console.WriteLine("key: {0}, value: {1}", sorted_value.Keys[i], sorted_value.Values[i]);
            //}

            //Console.WriteLine("\nNode..............................................................");
            //outputText += "\n\nNode ={ ";
            //foreach (string ss in final_node_name_set)
            //{
            //    outputText += ss + ","; ;

            //    Console.WriteLine(ss);
            //}
            //outputText = outputText.Substring(0, outputText.Length - 1) + " } \n";

        }
        /// <summary>
        /// ///////////////////////////////////////end of Anchor////////////////////////////////////
        /// </summary>

        public void AddingIndextoNode()
        {
            foreach (InfoBk obj in booklist)
            {
                string id_num = obj.getNodeValue();
                //Console.WriteLine("ID = " + id_num);
                string[] nodename = obj.getIndivNodeName();
                string[] nodevalue = obj.getIndivNodeValue();

                parent_node_list.Add(obj.getNodeName() + id_num);
                //listed_node_names_value_pair.Add(new KeyValuePair<string, string>(obj.getNodeName() + id_num, obj.getNodeValue()));
                for(int j = 1; j < nodename.Length; j++)
                {
                    listed_node_names_value_pair.Add(new KeyValuePair<string, string>(nodename[j]+id_num,nodevalue[j]));
                }
            }
        }

        public ArrayList GettingNewQueryPair(string inputQuery)
        {
            string[] query_name = queryString.Split(' ');
            ArrayList temp_pairs = new ArrayList();
            //for (int i = 0; i < combinational_pair.Length; i++)
            //{
            String[] pairs = inputQuery.Trim().ToLower().Split(' ');

                for (int j = 0; j < pairs.Length; j++)
                {
                    string temp1 = pairs[j].Trim().ToLower();
                    for (int k = j + 1; k < pairs.Length; k++)
                    {
                        string temp2 = pairs[k].Trim().ToLower();
                        temp_pairs.Add(temp1 + "," + temp2);
                    }
                }//end j
            //}//for i

            return temp_pairs;
        }
        public void BaseLineAlgorithm()
        {
            outputText += "\n\n\n############BaseLine Algorithm########################\n";
            //step 4
            for (int i = 0; i < combinational_pair.Length; i++)
            {
                outputText += "///////////////////////////////////////////\n";
                outputText+= "\nFor \"" + combinational_pair[i] + "\"";
                Console.WriteLine("For " + combinational_pair[i] + "........................");
                node_set.Add(combinational_pair[i]);
                ArrayList new_pair_list=GettingNewQueryPair(combinational_pair[i]);
                //showPairList(new_pair_list);

                foreach (string str in new_pair_list)
                {
                    string temp_var = str;
                    temp_var = temp_var.Replace(",", " ");
                    foreach (var node in listed_node_names_value_pair)
                    {
                        string node_val = node.Value;
                        if (node_val.Contains(temp_var))
                        {
                            Q_set.Add(node.Key);                           
                        }

                    }// node for
                }//str for


                Q_set=RemoveDuplicateItemInSet(Q_set);
                //outputText += "\n\nStep 5............";
                ShowDataForeachPair();

                //Calculate Step 5
                double prob_value=CalcualteStep5(combinational_pair[i], queryString);
                prob_value = Math.Round(prob_value, 3);
               // query_value.Add(new KeyValuePair<string, double>(combinational_pair[i], prob_value));

                //Step 6 Smallest Lowest Common Ancestor
                HashSet<string> slcancestor_list=FindSmallestLowestCommonAncestor(Q_set);

                outputText += "\n\nSmallest Lowest Common Ancestor.......................\n";
                outputText += "SLAC = { ";
                foreach (string tt in slcancestor_list)
                    outputText += tt+",";
                outputText = outputText.Substring(0, outputText.Length - 1) + " }";
                //    Console.WriteLine(tt);

                Q_set.Clear();

                if (final_node_name_set.Count == 0)
                {
                    final_node_name_set = slcancestor_list;

                    //step 7
                    prob_value=FindNewValue(prob_value, final_node_name_set);
                    prob_value = Math.Round(prob_value, 3);
                    outputText += "\n\nProb_value =" + prob_value;
                    query_value.Add(new KeyValuePair<string, double>(combinational_pair[i], prob_value));
                    node_value_pair.Add(new KeyValuePair<HashSet<string>, double>(slcancestor_list, prob_value));
                }
                else
                {
                    string[] str = slcancestor_list.ToArray<string>();

                    //outputText += "\n\n Step4.............";
                    outputText += "\n\t l(ix,jy) = { ";
                    foreach (string pp in slcancestor_list)
                        outputText += pp + ",";
                  
                    outputText = outputText.Substring(0, outputText.Length - 1) + " } \n";

                    foreach (string pp in final_node_name_set)
                    {
                        slcancestor_list.Remove(pp);
                    }

                    
                    //step 7
                    int num = final_node_name_set.Count;
                    prob_value = num * prob_value;
                    outputText += "\n\n Probability = " + prob_value;

                    //step 17
                    int subset_count = slcancestor_list.Count;
                    int super_count = final_node_name_set.Count;

                    //show step 4
                    outputText += "\n\n Step 14.............";
                    outputText += "\n\t{ ";
                    foreach (string strp in slcancestor_list)
                    {
                        outputText += strp + ",";
                    }
                    outputText = outputText.Substring(0, outputText.Length - 1) + " } \n";

                    prob_value = (double)prob_value * subset_count * (double)(subset_count/(double)(subset_count+super_count));
                    prob_value = Math.Round(prob_value, 3);
                    outputText += "\n\n After removing identical items \nProbability = " + prob_value;

                    Console.WriteLine(combinational_pair[i] + ".........value=" + prob_value);

                    query_value.Add(new KeyValuePair<string, double>(combinational_pair[i], prob_value));
                    node_value_pair.Add(new KeyValuePair<HashSet<string>,double>(slcancestor_list, prob_value));

                    //merge                    
                    foreach (string ss in slcancestor_list)
                    {
                        final_node_name_set.Add(ss);
                    }                        
                }
            }//end i         
            //sort the value 
            SortedList<double, string> sorted_value =SortingbyValue(query_value);
           // outputText += "\n\n Step 20..............." ;
            Console.WriteLine("\nKey Value Pair.....................");
            for (int i = 0; i < sorted_value.Count; i++)
            {
                outputText+= " \t\n Q  = {"+ sorted_value.Values[i] +"}\n\n";
                outputText += "prob_value = " + sorted_value.Keys[i] + "\n";
                Console.WriteLine("key: {0}, value: {1}", sorted_value.Keys[i], sorted_value.Values[i]);
            }
        
            Console.WriteLine("\nNode..............................................................");
            outputText += "\n\nNode ={ ";
            foreach (string ss in final_node_name_set)
            {
                outputText+= ss+  ","; ;           
                Console.WriteLine(ss);
            }
            outputText = outputText.Substring(0, outputText.Length - 1) + " } \n";        
        }//end function

        public SortedList<double, string> SortingbyValue(List<KeyValuePair<string, double>> node_value)
        {
            var descendingComparer = Comparer<double>.Create((x, y) => y.CompareTo(x));
            SortedList<double, string> sortedList1 = new SortedList<double, string>(descendingComparer);
            
            foreach (var tt in node_value)
            {
                sortedList1.Add(tt.Value, tt.Key);
            }

            return sortedList1;
        }
        public double FindNewValue(double inputvalue,HashSet<string> inputset)
        {
            double value = inputvalue;

            value *= inputset.Count;

            return value;
        }
        public ArrayList RemoveDuplicateItemInSet(ArrayList arr)
        {
            string[] newList = new string[arr.Count];
            int count = 0;
            foreach (string str in arr)
                newList[count++] = str.ToString();

            HashSet<string> hash = new HashSet<string>(newList);

            string[] array2 = hash.ToArray();
            ArrayList nn = new ArrayList();
            foreach (string str in array2)
                nn.Add(str);

            return nn;
        }

        public HashSet<string> FindSmallestLowestCommonAncestor(ArrayList Q_set)
        {
            string last_name="";
            HashSet<string> new_list = new HashSet<string>();
            new_list.Clear();
            int count = 0;

            for (int i= 0;i< Q_set.Count;i++)
            {
                count = 0;
                string indiv_node = Q_set[i].ToString();
                //Console.WriteLine(indiv_node);
                if (indiv_node.Length > 1)
                    last_name = indiv_node.Substring(indiv_node.Length - 1);

                foreach(string str in Q_set)
                {
                    if (str.Contains(last_name))
                        ++count;
                }

                if (count > 1)
                {
                    foreach(string strr in parent_node_list)
                    {
                        if (strr.Contains(last_name))
                            new_list.Add(strr);
                    }
                }
                else
                    new_list.Add(indiv_node);                           
            }
            return new_list;
        }
        public double CalcualteStep5(String all_word,string query_word)
        {
           // Console.WriteLine("All word = " + all_word);
           // Console.WriteLine("Query word =" + query_word);
            String[] a_words = all_word.Split(' ');
            String[] q_words = query_word.Split(' ');

            string pair1 = a_words[0].Trim() + " " + a_words[1].Trim();
            string pair2= a_words[2].Trim() + " " + a_words[3].Trim();
            pair1 = ReplaceSpace(pair1);
            pair2 = ReplaceSpace(pair2);

            all_word = all_word.Replace(q_words[0], "").Trim();
            all_word = all_word.Replace(q_words[1], "").Trim();
           // Console.WriteLine(" all word before replace =" + all_word);
            all_word = ReplaceSpace(all_word);
            // Console.WriteLine("Pair 1 =" + pair1);
           //  Console.WriteLine("Pair 2 =" + pair2);
          //  Console.WriteLine("New all word =" + all_word);
            string[] one_word = all_word.Split(' ');
            
            //  Console.WriteLine("Word 1= "+one_word[0]);
            //  Console.WriteLine("Word 2= "+one_word[1]);

            //Find Count
            int one_word_count=CheckContain(one_word[0]);
            int two_word_count = CheckContain(one_word[1]);

            //Console.WriteLine(one_word[0] + "\t\t" + one_word_count);
            //Console.WriteLine(one_word[1] + "\t\t" + two_word_count);

            int pair_one_count = CheckContainPair(pair1);
            int pair_two_count = CheckContainPair(pair2);

            outputText += "\n set of Pair -> { " + pair1 + "-> "+pair_one_count+" , " + pair2 + " -> "+pair_two_count+" }";
            outputText += "\n set of each word -> { " + one_word[0] + " -> "+one_word_count+" , "+ one_word[1] + "-> "+two_word_count+" } \n";
            double value1=0, value2=0;
            double prob_value = 0;
            outputText += "\n\nProbability = ";
            string output = "Probability = ";
            if (pair1.Contains(one_word[0].Trim().ToLower()))
            {
                value1 = (double)pair_one_count / (double)one_word_count;
                outputText += "( "+pair_one_count + "/" + one_word_count + ") *";
                output += pair_one_count + "/" + one_word_count + "*" ;
            }
            else if (pair1.Contains(one_word[1].Trim().ToLower()))
            {

                value1 = (double)pair_one_count /(double) two_word_count;
                outputText +="( "+ pair_one_count + "/" + two_word_count + ") *";
                output += pair_one_count + "/" + two_word_count + "*";
            }

            if (pair2.Contains(one_word[0].Trim().ToLower()))
            {

                value2 = (double)pair_two_count / one_word_count;
                outputText += "( "+pair_two_count + "/" + one_word_count + " ) = ";
                output += pair_two_count + "/" + one_word_count + "=";
            }
            else if (pair2.Contains(one_word[1].Trim().ToLower()))
            {

                value2 = (double)pair_two_count / two_word_count;
                outputText +="("+ pair_two_count + "/" + two_word_count + ") = ";
                output += pair_two_count + "/" + two_word_count + "=";
            }

            //output += "= " + value2 + "*" + value2 + "= " + (value1 * value2);
            prob_value = value1 * value2;
            prob_value = Math.Round(prob_value, 3);
            outputText += prob_value;
            Console.WriteLine(output + prob_value);

            return prob_value;
        }


        public string ReplaceSpace(string inputstring)
        {
            string all_word = inputstring.Replace(" ", "@");
            Console.WriteLine(all_word);

            if (all_word.Contains("@@@@"))
                all_word = all_word.Replace("@@@@", " ");
            else if (all_word.Contains("@@@"))
                all_word = all_word.Replace("@@@", " ");
            else if (all_word.Contains("@@"))
                all_word = all_word.Replace("@@", " ");
            else if (all_word.Contains("@"))
                all_word = all_word.Replace("@", " ");

            return all_word;
        }
        public int CheckContain(String str)
        {
            int count = 0;
            Boolean found = false;

            foreach (var node in listed_node_names_value_pair)
            {
                found = false;
                string node_value = node.Value;
                string[] temp = node_value.Split(' ');
                for(int i = 0; i <temp.Length && found != true; i++)
                {
                    string t_var = temp[i].Trim().ToLower();
                    if (str.Trim().ToLower().Equals(t_var))
                    {
                        ++count;
                        found = true;
                    }
                }
            }

            return count;
        }

        public int CheckContainPair(string string_pair)
        {
            int count = 0;
            string[] words = string_pair.Split(' ');
            string oneword = words[0].Trim().ToLower();
            string twoword = words[1].Trim().ToLower();
            Boolean firstFound = false;
            Boolean secondFound = false;

            foreach (var node in listed_node_names_value_pair)
            {
                string node_value = node.Value.Trim().ToLower();
                firstFound = false;
                secondFound = false;
                string[] temp = node_value.Split(' ');

                for (int j = 0; j < temp.Length; j++)
                {
                    string value_word = temp[j].Trim().ToLower();
                    
                    if(!value_word.Equals("") && value_word != null)
                    {
                        if (value_word.Equals(oneword))
                            firstFound = true;
                        else if (value_word.Equals(twoword))
                            secondFound = true;
                    }
                 
                }//j
                if (firstFound == true && secondFound == true)
                    ++count;
            }//var for 
            return count;                    
          }
              
        public void ShowDataForeachPair()
        {
            outputText += "\n\t L(ix,jy) ={";
            //Show all step4 set
            foreach (string str in Q_set)
            {
                outputText += str+",";
                Console.WriteLine(str);
            }
            outputText += "}\n";
        }
        public void showPairList(ArrayList new_pair_list)
        {
            //temp combined pairs
            for (int k = 0; k < new_pair_list.Count; k++)
            {
                Console.WriteLine(new_pair_list[k]);
            }
        }
        public void PairTopElements()
        {
            //Pair two elements from query and elements
            int count = first_query_Ktop.Count * second_query_Ktop.Count;
            combinational_pair = new string[count];

            int tempvar = 0;
            foreach (var pair in first_query_Ktop)
            {
                foreach(var pair2 in second_query_Ktop)
                {
                    combinational_pair[tempvar++] = pair.Key.Trim().ToLower() + " " + pair2.Key.Trim().ToLower();
                }
            }

            //show pair
            outputText += "\n\n............................Combined Pair.............";
            outputText += "\n\t\t{ ";
            Console.WriteLine("Combined pair........................");
            for(int i = 0; i < combinational_pair.Length; i++)
            {
                outputText +="\n\t\t  "+ combinational_pair[i] + "\n";
                Console.WriteLine("$$"+combinational_pair[i]);
            }
            outputText += "\t\t}\n";
        }

        public void GettingKTop()
        {
            String[] q_name = queryString.Split(' ');
            kNumber = kNumber.Trim().ToString();
            int k_num = Int32.Parse(kNumber);

            //Order the values
            string[] pair_name = keyterms_pair;
            double[] pair_value = keyterms_value;
            var list = new List<KeyValuePair<string, double>>();

            for (int i = 0; i < pair_name.Length; i++)
            {
                list.Add(new KeyValuePair<string, double>(pair_name[i], pair_value[i]));
            }


            list.Sort(Compare2);
            Console.WriteLine("After sorting...............");
            foreach (var pair in list)
            {
                Console.WriteLine(pair.Key + ">.............." + pair.Value);
            }

           // qname1 k elements
            int temp = 0;
            foreach (var pair in list)
            {
                string tmp = pair.Key;
                String[] pair_name_pair = tmp.Trim().Split(' ');
                String p_name1, p_name2, q_name1;

                p_name1 = pair_name_pair[0].Trim().ToLower();
                p_name2 = pair_name_pair[1].Trim().ToLower();
                q_name1 = q_name[0].Trim().ToLower();
                
                if (q_name1.Equals(p_name1) || q_name1.Equals(p_name2))
                {
                    Console.WriteLine(p_name1 + "..........." + p_name2 + "........" + q_name1);

                    if (temp < k_num)
                    {
                        first_query_Ktop.Add(new KeyValuePair<string, double>(pair.Key, pair.Value));
                        ++temp;
                    }
                }
            }

            //qname2
            temp = 0;
            foreach (var pair in list)
            {
                string tmp = pair.Key;
                String[] pair_name_pair = tmp.Trim().Split(' ');
                String p_name1, p_name2, q_name2;

                p_name1 = pair_name_pair[0].Trim().ToLower();
                p_name2 = pair_name_pair[1].Trim().ToLower();
                q_name2 = q_name[1].Trim().ToLower();

                if (q_name2.Equals(p_name1) || q_name2.Equals(p_name2))
                {
                    if (temp < k_num)
                    {
                        second_query_Ktop.Add(new KeyValuePair<string, double>(pair.Key, pair.Value));
                        ++temp;
                    }
                }
            }

            outputText += "\n\n ......................Top Feature Selection...................";
            //show only Top-K pairs of first
            outputText += "\nTotal " + first_query_Ktop.Count + " of query term -> " + q_name[0];
            Console.WriteLine("First query pair count= " + first_query_Ktop.Count);
            foreach (var pair in first_query_Ktop)
            {
                outputText += "\n\t" + pair.Key + "=>" + pair.Value;
                Console.WriteLine("\t" + pair.Key + "=>" + pair.Value);
            }

            Console.WriteLine("Second query pair count= " + second_query_Ktop.Count);
            outputText += "\n\nTotal " + second_query_Ktop.Count + " of query term -> " + q_name[1];
            foreach (var pair in second_query_Ktop)
            {
                outputText += "\n\t" + pair.Key + "=>" + pair.Value;
                Console.WriteLine("\t" + pair.Key + "=>" + pair.Value);
            }
        }
        int Compare2(KeyValuePair<string,double> a, KeyValuePair<string,double> b)
        {
            return b.Value.CompareTo(a.Value);
        }
        void FeatureSpaceCalculation() //sample space
        {
            //step 1 Sample space
            foreach (InfoBk obj in booklist)
            {
                ++space_count;
                InfoBk BookNode = obj;
                string[] nodename = BookNode.getIndivNodeName();
                string[] nodevalue = BookNode.getIndivNodeValue();
                List<KeyValuePair<string, string>> book_list = BookNode.getNodeList();
                //Console.WriteLine(BookNode.getNodeName() + ".........." + BookNode.getNodeValue() + "......." + BookNode.getKey());

                for (int i = 0; i < nodename.Length; i++)
                {
                    ++space_count;
                    //Console.WriteLine(nodename[i] + "->" + nodevalue[i]);
                }
               
            }
            Console.WriteLine(space_count);
            outputText += "\nSample Space = " + space_count;
        }
        public void CalculateMutualInformation()
        {
            keyterms_pair = keyTerms.Split(',');
            keyterms_value = new double[keyterms_pair.Length];
            string[] querystring_pair = queryString.Trim().Split(' ');
            int foundAll = 0;
            int found_first_one = 0;
            int found_second_one = 0;
            outputText+= "\n\nMutual Information Calculation.......................";            
            Console.WriteLine("\n\nMutual Information Calculation.......................");

            for (int i = 0; i < keyterms_pair.Length; i++)
            {
                string[] key_pair = keyterms_pair[i].ToLower().Trim().Split(' ');
                //outputText += "\n................"+keyterms_pair[i].Trim()+".....................";
                
                Console.WriteLine(keyterms_pair[i].Trim());
                //for loop for tag information
                foreach (InfoBk obj in booklist)
                {
                    InfoBk BookNode = obj;
                    string[] nodename = BookNode.getIndivNodeName();
                    string[] nodevalue = BookNode.getIndivNodeValue();
                    string first_char = key_pair[0].ToLower().Trim();
                    string second_char = key_pair[1].ToLower().Trim();
                    
                    Boolean found = false;
                    
                    for (int j = 0; j <nodename.Length; j++)
                    {
                       // Console.WriteLine(BookNode.getNodeValue() + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                        string n_value = nodevalue[j].Trim().ToLower();
                        string[] each_value_word = n_value.Split(' ');
                        // Console.WriteLine(nodename[j] + "................");
                        found = false;
                        //first 
                      //  Console.WriteLine("First Char.....................");
                        first_char = PreProcessWord(first_char).Trim().ToLower();
                        for (int k = 0; k < each_value_word.Length && found==false; k++)
                        {
                            string indiv_word = each_value_word[k].Trim().ToLower();
                            indiv_word = PreProcessWord(indiv_word);
                            if (first_char.Equals(indiv_word))
                            {
                                ++found_first_one;
                                found = true;
                            }
                        }

                        found = false;
                        //second
                        second_char = PreProcessWord(second_char).Trim().ToLower();
                        for (int k = 0; k < each_value_word.Length && found==false; k++)
                        {
                            string indiv_word = each_value_word[k].Trim().ToLower();
                            indiv_word = PreProcessWord(indiv_word);
                            if (second_char.Equals(indiv_word))
                            {
                                ++found_second_one;
                                found = true;
                            }
                                
                        }
                        found = false;

                        for (int k = 0; k < each_value_word.Length && found == false; k++)
                        {
                            if (k < each_value_word.Length-1)
                            {
                                string f_word = each_value_word[k];
                                f_word = PreProcessWord(f_word);
                                string s_word = each_value_word[k + 1];
                                s_word = PreProcessWord(s_word);
                                if(f_word.Equals(first_char) && s_word.Equals(second_char))
                                {
                                    found = true;
                                    ++foundAll;
                                }
                            }                         
                        }
                        }//for j
                   
                }//end of loop for each tag

                
                //Console.WriteLine("Found both " + foundAll);
                //Console.WriteLine("Found first " + found_first_one);
                //Console.WriteLine("Found second " + found_second_one);

                // double prob_all = Math.Round(foundAll / (double)space_count,3);
                double prob_all = (double) foundAll / (double)space_count;
               //  Console.WriteLine(prob_all);
                //double prob_each_first = Math.Round(found_first_one / (double)space_count,3);
                double prob_each_first =(double) found_first_one / (double)space_count;
                //  Console.WriteLine("First one " + prob_each_first);
                //double prob_each_second = Math.Round(found_second_one / (double)space_count,3);
                double prob_each_second =(double) found_second_one / (double)space_count;
                double v1 = (double) prob_each_first * prob_each_second;
                v1 = (double)prob_all / v1;
                double temp= (double)prob_all * Math.Log10(v1);
                keyterms_value[i] = Math.Round(temp, 3);

                outputText+= "\nMI(" + key_pair[0] + ", " + key_pair[1] + ") = BothFound(" + foundAll + "),FirstFound(" + found_first_one + "), SecondFound(" + found_second_one + ")";
                outputText+= "\nThe value = " + keyterms_value[i]+"\n\n";
               // Console.WriteLine("MI(" + key_pair[0] + "," + key_pair[1] + ")=BothFound(" + foundAll + "),FirstFound(" + found_first_one + "), SecondFound(" + found_second_one + ")");
               // Console.WriteLine("The value = " + keyterms_value[i]);


                foundAll = 0;
                found_first_one = 0;
                found_second_one = 0;

            }//loop end i
        }//function end

        public string getOutputText()
        {
            return this.outputText;
        }
        public string PreProcessWord(string inputWord)
        {
            string processedWord = null;
            inputWord = inputWord.Trim().ToLower();
            if (inputWord.EndsWith(",") || inputWord.EndsWith("."))
                inputWord = inputWord.Substring(0, inputWord.Length - 1);
            
                   
            if (inputWord.ToLower().Trim().EndsWith("s") && inputWord.Length > 3 && !inputWord.EndsWith("ss"))
            {
                processedWord = inputWord.Substring(0, inputWord.Length - 1).ToLower().Trim();
            }
            else
            {
                processedWord = inputWord.Trim().ToLower();
            }
            return processedWord;
        }
        void DisplayList(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                string rootKey = ""; ;
                string rootValue = "";
                string rootname = node.Name;

                foreach (XmlAttribute attr in node.Attributes)
                {
                    rootKey = attr.Name;
                    rootValue = attr.Value;
                }

                RecurseXmlDocumentNoSiblings(node);
                int count1 = 0;
                List<KeyValuePair<string, string>> arrlist = attributeList;
                string[] temp_name = new string[arrlist.Count];
                string[] temp_value = new string[arrlist.Count];
                foreach (KeyValuePair<string, string> acct in arrlist)
                {
                    temp_name[count1] = acct.Key;
                    temp_value[count1] = acct.Value;
                    ++count1;
                }

                InfoBk inbk = new InfoBk(rootname, rootKey, rootValue, temp_name, temp_value);

                //add all node to arraylist
                booklist.Add(inbk);
                attributeList.Clear();
            }
        }

        void RecurseXmlDocumentNoSiblings(XmlNode root)
        {
            if (root is XmlElement)
            {
                nodeNames[array_index] = root.Name;
                String name = root.Name;
                if (root.HasChildNodes)
                {
                    RecurseXmlDocument(root.FirstChild);
                }
                else if (root is XmlText)
                {
                    string text = ((XmlText)root).Value;
                    text = RemoveStopWord(text);
                    attributeList.Add(new KeyValuePair<string, string>(nodeNames[array_index], text));
                }
                else if (root is XmlComment)
                {
                    string text = root.Value;
                    if (root.HasChildNodes)
                        RecurseXmlDocument(root.FirstChild);
                }
            }
        }
        string RemoveStopWord(string inputword)
        {
            string text = inputword;
            String[] stopwords = {"of","in","at","which","where","that","this","on","an","a","to","for","and",",",".","is","are","they","we","them","us"};
            string[] inputText = text.Split(' ');
            Boolean found = false;
            string outputText = "";
            for(int j = 0; j < inputText.Length; j++)
            {       
                string str1 = inputText[j].Trim().ToLower();
                if (!str1.Equals(""))
                {
                    for (int i = 0; i < stopwords.Length && found==false; i++)
                    {
                        string stop_str = stopwords[i];
                        if (str1.Equals(stop_str))
                        {
                            found = true;
                        }                      
                    }
                    if (found == false)
                    {
                        str1=PreProcessWord(str1);
                        outputText += str1 + " ";
                    }
                    found = false;
                }
            }//end of j
            return outputText;
        }
        void RecurseXmlDocument(XmlNode root)
        {
            string name = "";
            if (root is XmlElement)
            {
                nodeNames[array_index] = root.Name;
                name = root.Name;
                if (root.HasChildNodes)
                {
                    RecurseXmlDocument(root.FirstChild);
                }
                if (root.NextSibling != null)
                    RecurseXmlDocument(root.NextSibling);
            }
            else if (root is XmlText)
            {
                string text = ((XmlText)root).Value;
                text = RemoveStopWord(text);
                Console.WriteLine("TEXT>>>>>>>>>>>>>>>>." + text);
                name = ((XmlNode)root).Name;
                attributeList.Add(new KeyValuePair<string, string>(nodeNames[array_index], text));           
            }
            else if (root is XmlComment)
            {
                string text = root.Value;
                if (root.HasChildNodes)
                    RecurseXmlDocument(root.FirstChild);
                if (root.NextSibling != null)
                    RecurseXmlDocument(root.NextSibling);
            }
        }

    }
}
