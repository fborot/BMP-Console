using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console
{
    class ComCheckData
    {
        //Commisions Check Data
        public string bmp_cid = string.Empty;
        public decimal PayAmount = 0;
        public string type = string.Empty;

        public ComCheckData(string cid, decimal amount, string t)
        {
            bmp_cid = cid;
            PayAmount = amount;
            type = t;
        }
    }

    class CheckEnvelope {

        public string Name = string.Empty;
        public decimal Total = 0;
        public ArrayList List = null;
        public CheckEnvelope(string nam, decimal tot, ComCheckData data) {
            Name = nam;
            Total = tot;
            if (List == null)
                List = new ArrayList();

            List.Add(data);
        }

        public CheckEnvelope(string nam, decimal tot, ArrayList ar) {
            Name = nam;
            Total = tot;
            List = ar;
        }
    }

}
