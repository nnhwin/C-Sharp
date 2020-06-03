using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisCode
{
    class RandomClass
    {
        public RandomClass()
        {
            Dictionary<string, ArrayList> hash = new Dictionary<string, ArrayList>();

            for(int i = 0; i < 2; i++)
            {
                if (hash.ContainsKey("1"))
                {
                    ArrayList al = hash["1"];
                    al.Add("ewe");
                    hash.Remove("1");
                    hash.Add("1", al);
                }
                else
                {
                    ArrayList al = new ArrayList();
                    al.Add("344,213,5,22,43,1");
                    hash.Add("1", al);
                }
            }

            Console.WriteLine(hash.Count);

            foreach (KeyValuePair<string, ArrayList> pair in hash)
            {
                ArrayList aa = pair.Value;
                Console.WriteLine(pair.Key);
                for (int i = 0; i < aa.Count; i++)
                    Console.Write(aa[i]+" ");
            }




            //String[] words = new String[] { "animal", "animal", "dot", "print", "solid" };
            //var occrs = words.GroupBy(x => x.ToLower())
            //   .ToDictionary(g => g.Key, g => g.Count());
            //foreach (var pair in occrs)
            //    Console.WriteLine(pair.Key + " " + pair.Value);

            //    List<List<int>> list = new List<List<int>>();
            //    var rand = new Random();
            //    for (int i = 0; i < 10; i++)
            //    {
            //        //
            //        // Put some integers in the inner lists.
            //        //
            //        List<int> sublist = new List<int>();
            //        int top = rand.Next(1, 15);
            //        for (int v = 0; v < top; v++)
            //        {
            //            sublist.Add(rand.Next(1, 5));
            //        }
            //        //
            //        // Add the sublist to the top-level List reference.
            //        //
            //        list.Add(sublist);
            //    }
            //    //
            //    // Display the List.
            //    //
            //    Display(list);
            //    //Dictionary<int, int> hash = new Dictionary<int, int>();
            //    //hash.Add(1, 1);
            //    //hash.Add(2, 1);
            //    //foreach(KeyValuePair<int,int> data in hash)
            //    //{
            //    //    Console.WriteLine(data.Key + "............" + data.Value);
            //    //}

            //    //Dictionary<int, String[]> seedPair = new Dictionary<int, String[]>();
            //    //String[] arr = new String[]{"skirt","blue","Chiffon"};
            //    //seedPair.Add(1, arr);
            //    //arr = new String[]{"trouser","white","jean"};
            //    //seedPair.Add(2,arr);
            //    //foreach (KeyValuePair<int, String[]> data in seedPair)
            //    //{
            //    //    String[] pp = data.Value;
            //    //    Console.WriteLine(data.Key);
            //    //    foreach (String s in pp)
            //    //    {
            //    //        Console.Write("\t"+ s);
            //    //    }
            //    //    Console.WriteLine();
            //    //}
            //}
            //static void Display(List<List<int>> list)
            //{
            //    //
            //    // Display everything in the List.
            //    //
            //    Console.WriteLine("Elements:");
            //    foreach (var sublist in list)
            //    {
            //        foreach (var value in sublist)
            //        {
            //            Console.Write(value);
            //            Console.Write(' ');
            //        }
            //        Console.WriteLine();
            //    }
            //    //
            //    // Display element at 3, 3.
            //    //
            //    if (list.Count > 3 &&
            //        list[3].Count > 3)
            //    {
            //        Console.WriteLine("Element at 3, 3:");
            //        Console.WriteLine(list[3][3]);
            //    }
            //    //
            //    // Display total count.
            //    //
            //    int count = 0;
            //    foreach (var sublist in list)
            //    {
            //        count += sublist.Count;
            //    }
            //    Console.WriteLine("Count:");
            //    Console.WriteLine(count);
            //}
        }
    }
}

