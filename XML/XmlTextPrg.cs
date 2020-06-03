using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IRXMLThesis
{
    class XmlTextPrg
    {
        public static void main()
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load("samplexml1.xml");

            //Display all the book titles.
            XmlNodeList elemList = doc.GetElementsByTagName("title");
            for (int i = 0; i < elemList.Count; i++)
            {
                Console.WriteLine(elemList[i].InnerXml);
            }
        }
    }
}
