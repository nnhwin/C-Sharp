using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRXMLThesis
{
    class InfoBk
    {
        string nodename;
        string nodekey;
        Hashtable bklist = new Hashtable();
        string nodevalue;
        string[] indiv_node_name;
        string[] indiv_node_value;
        List<KeyValuePair<string, string>> nodeList = new List<KeyValuePair<string, string>>();

        public InfoBk()
        {

        }
        public InfoBk(string name, string key, string value, List<KeyValuePair<string, string>> list )
        {
            this.nodekey = key;
            this.nodename = name;
            this.nodevalue = value;
            this.nodeList = list;
        }
        public InfoBk(string name, string key, string value, string[] node_name,string[] node_value)
        {
            this.nodekey = key;
            this.nodename = name;
            this.nodevalue = value;
            this.indiv_node_name = node_name;
            this.indiv_node_value = node_value;

        }
        public List<KeyValuePair<string, string>> getNodeList()
        {
            return this.nodeList;
        }
        public void setNodeList(List<KeyValuePair<string, string>> list)
        {
            this.nodeList = list;
        }
        public InfoBk(string name,string key,string value, Hashtable htable)
        {
            this.nodekey = key;
            this.nodename = name;
            this.bklist = htable;
            this.nodevalue = value;
        }
        public string[] getIndivNodeName()
        {
            return this.indiv_node_name;
        }
        public void setIndivNdoeName(string[] nn)
        {
            this.indiv_node_name = nn;
        }
        public string[] getIndivNodeValue()
        {
            return this.indiv_node_value;
        }
        public void setIndivNodeValue(string[] vv)
        {
            this.indiv_node_value = vv;
        }
        public string getNodeName()
        {
            return this.nodename;
        }
        public void setNodeName(string name)
        {
            this.nodename = name;
        }

        public void setKey(string key)
        {
            this.nodekey = key;
        }
        public string getKey()
        {
            return this.nodekey;
        }
        public void setInfoList(Hashtable infolist)
        {
            this.bklist = infolist;
        }
        public Hashtable getInfoList()
        {
            return this.bklist;
        }
        public string getNodeValue()
        {
            return this.nodevalue;
        }
        public void setNodeValue(string value)
        {
            this.nodevalue = value;
        }

    }
}
