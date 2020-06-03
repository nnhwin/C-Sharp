using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace IRXMLThesis
{
    public partial class Form2 : Form
    {
        static int space_count = 0;
        static string[] nodeNames = new string[200];
        static String[] nodeTexts = new string[200];
        ArrayList booklist = new ArrayList();
        static int array_index = 0;
        XmlDocument dom = new XmlDocument();
        ArrayList books = new ArrayList();
        List<KeyValuePair<string, string>> attributeList = new List<KeyValuePair<string, string>>();

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlNode myNode = dom.DocumentElement;
            XmlNodeList nodeList = myNode.SelectNodes("*");
            DisplayList(nodeList);
            int space_count = 0;

            foreach (InfoBk obj in booklist)
            {
                ++space_count;
                InfoBk BookNode = obj;
                string[] nodename = BookNode.getIndivNodeName();
                string[] nodevalue = BookNode.getIndivNodeValue();
                List<KeyValuePair<string, string>> book_list = BookNode.getNodeList();
                Console.WriteLine(BookNode.getNodeName() + ".........." + BookNode.getNodeValue() + "......." + BookNode.getKey());

                for(int i=0;i<nodename.Length;i++)
                {
                    ++space_count;
                    Console.WriteLine(nodename[i] + "->" + nodevalue[i]);
                }
                Console.WriteLine("########################################\n");

            }
            Console.WriteLine(space_count);

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

                    //Console.WriteLine(rootname+"........."+attr.Name+".........."+attr.Value);
                }


                ++array_index;
                RecurseXmlDocumentNoSiblings(node);
                int count1 = 0;
                List<KeyValuePair<string, string>> arrlist = attributeList;
                string[] temp_name = new string[arrlist.Count];
                string[] temp_value = new string[arrlist.Count];
                foreach (KeyValuePair<string, string> acct in arrlist)
                {
                    temp_name[count1]=acct.Key;
                    temp_value[count1]=acct.Value;
                    ++count1;
                }


                InfoBk inbk = new InfoBk(rootname, rootKey, rootValue, temp_name,temp_value);


                //add all node to arraylist
                booklist.Add(inbk);

                attributeList.Clear();


            }


        }

        void RecurseXmlDocumentNoSiblings(XmlNode root)
        {
            if (root is XmlElement)
            {
                ++space_count;
                nodeNames[array_index] = root.Name;
                String name = root.Name;
                if (root.HasChildNodes)
                {
                    RecurseXmlDocument(root.FirstChild);
                }
                else if (root is XmlText)
                {
                    string text = ((XmlText)root).Value;
                    nodeTexts[array_index] = text;
                    attributeList.Add(new KeyValuePair<string, string>(nodeNames[array_index], text));
                    ++array_index;
                }
                else if (root is XmlComment)
                {
                    string text = root.Value;
                    // Console.WriteLine(text);
                    if (root.HasChildNodes)
                        RecurseXmlDocument(root.FirstChild);
                }
            }
        }
        
        void RecurseXmlDocument(XmlNode root)
        {
            string name = "";
            if (root is XmlElement)
            {
                ++space_count;
                // Console.WriteLine(root.Name);
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
                name = ((XmlNode)root).Name;
                attributeList.Add(new KeyValuePair<string, string>(nodeNames[array_index], text));
                nodeTexts[array_index] = text;
                ++array_index;
            }
            else if (root is XmlComment)
            {
                string text = root.Value;
                // Console.WriteLine(text);
                if (root.HasChildNodes)
                    RecurseXmlDocument(root.FirstChild);
                if (root.NextSibling != null)
                    RecurseXmlDocument(root.NextSibling);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                // SECTION 1. Create a DOM Document and load the XML data into it.

                dom.Load("C:\\Users\\USER\\Desktop\\sampleXML.xml");

                // SECTION 2. Initialize the TreeView control.
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode(dom.DocumentElement.Name));
                TreeNode tNode = new TreeNode();
                tNode = treeView1.Nodes[0];

                // SECTION 3. Populate the TreeView with the DOM nodes.
                AddNode(dom.DocumentElement, tNode);
                treeView1.ExpandAll();
            }
            catch (XmlException xmlEx)
            {
                MessageBox.Show(xmlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        static string GetAttributeText(XmlNode inXmlNode, string name)
        {
            XmlAttribute attr = (inXmlNode.Attributes == null ? null : inXmlNode.Attributes[name]);
            return attr == null ? null : attr.Value;
        }
        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else
            {
                // Here you need to pull the data from the XmlNode based on the
                // type of node, whether attribute values are required, and so forth.
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
        }
    }
}
