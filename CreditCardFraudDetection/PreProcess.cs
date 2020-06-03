using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThesisCode
{
    class PreProcess
    {
        public PreProcess()
        {
            
        }
        public String[,] DoPreProcess(ArrayList al,int rowCount,int colCount)
        {
            String[,] dataLine = new String[rowCount, colCount];
            String[] newData = new String[rowCount];

            for (int i = 0; i < al.Count; i++)
            {
                String str = al[i].ToString();
                //Console.WriteLine(str);
                if (!String.IsNullOrEmpty(str))
                {

                    str = str.Trim();
                String[] temparr = str.Split(',');
                    //ignore col 1 and last column
                    //  String temp = "";

                    //for (int j = 1; j < colCount-1 ; j++)
                    for (int j = 0; j < colCount; j++)
                    {
                    //Console.WriteLine(temparr[j] + "NNNNNNNNNNNNNN"+"။။။။။"+j);
                    if (!String.IsNullOrEmpty(temparr[j]))
                    {
                        if (IsValidDecimal(temparr[j]) == true && j!=colCount-1 && j!=0 && j!=1)
                        {
                            //Normalize here
                            //Console.WriteLine(" Decimal " + temparr[j]);
                            Double nor_value = NormalizedRating(temparr[j]);
                            // Console.WriteLine("Data " + temparr[j] + "...to noramlized " + nor_value);
                           // temp += nor_value + ",";
                            dataLine[i, j] = Convert.ToString(nor_value);
                        }
                        else
                        {
                            dataLine[i, j] = temparr[j];
                        }
                    }
                    
                    else
                    {
                        dataLine[i, j] = temparr[j];
                    }
                      
                }

                }
            }
            return dataLine;
        }

        Double NormalizedRating(String value)
        {
            Double intValue = Convert.ToDouble(value);
            Double min = 0.0f;
            Double max =5.0f;
            Double newmin = 0.0f;
            Double newmax = 1.0f;
            Double newValue = ((intValue - min) / (max - min)*(newmax-newmin))+newmin;
            newValue = Math.Abs(newValue);
            //Min-max normaliztion
            return newValue;

        }
        public static bool IsValidDecimal(String input)
        {
            input = input.Trim();
            if (!string.IsNullOrEmpty(input) && input != null)
            {
                Match m = Regex.Match(input, @"^-?\d*\.?\d+");
                return m.Success && m.Value != "";
            }
            else
                return false;
           
        }
    }
}
