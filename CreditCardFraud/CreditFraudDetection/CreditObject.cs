using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditFraudDetection
{
    class CreditObject
    {
        string totalString = "";
        string justFinalOutput = "";
        Dictionary<float, int> valueObjectPair;
        List<String[]> pp;
        int knumber;
        public CreditObject(string a, string b, Dictionary<float, int> dd, List<String[]> pp,int k)
        {
            this.totalString = a;
            this.justFinalOutput = b;
            this.valueObjectPair = dd;
            this.pp = pp;
            this.knumber = k;
        }

        public string getTotalString()
        {
            return this.totalString;
        }
        public string getFinalOutput()
        {
            return this.justFinalOutput;
        }
        public Dictionary<float, int> getPair()
        {
            return this.valueObjectPair;
        }
        public List<String[]> getAllList()
        {
            return this.pp;
        }
        public int getKNumber()
        {
            return this.knumber;
        }

    }
}
