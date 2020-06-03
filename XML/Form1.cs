using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace IRXMLThesis
{
    public partial class Form1 : Form
    {
        String fileData = "";
        String fileName1 = "";
        String fileName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                fileName1 = openFileDialog1.FileName;
                fileNamelabel.Text = openFileDialog1.FileName;
                fileData=sr.ReadToEnd();
                loadKeyTerms.Enabled = true;
                sr.Close();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            xmlReaderForm xmlViewForm = new xmlReaderForm(fileData);
            xmlViewForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // SECTION 1. Create a DOM Document and load the XML data into it.
                XmlDocument dom = new XmlDocument();
                dom.Load(fileName1);

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
        //static string GetAttributeText(XmlNode inXmlNode, string name)
        //{
        //    XmlAttribute attr = (inXmlNode.Attributes == null ? null : inXmlNode.Attributes[name]);
        //    return attr == null ? null : attr.Value;
        //}
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

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void loadKeyTerms_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                fileName = openFileDialog1.FileName;
                richTextBox1.Text = sr.ReadToEnd();
                retrievedBut.Enabled = true;
                sr.Close();
            }
        }

        private void retrievedBut_Click(object sender, EventArgs e)
        {
            String xmlFileName = fileName;
            String keytermsText = richTextBox1.Text;
            String queryString = textBox1.Text;
            String algorithmName = comboBox1.Text;
            String kNumber = comboBox2.Text;

            if (queryString != null && !queryString.Equals(""))
            {
                FSelectionModel fsmodel = new FSelectionModel(fileName1, xmlFileName, richTextBox1.Text, textBox1.Text, comboBox1.Text, comboBox2.Text);
                richTextBox2.Text = fsmodel.getOutputText();
            }
            else
                MessageBox.Show("You should fill your query in Query Box");
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
