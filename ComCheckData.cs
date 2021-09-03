using System;
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
        public decimal ICPayAmount = 0;
        public string type = string.Empty;

        public ComCheckData(string cid, decimal amount, string t)
        {
            bmp_cid = cid;
            ICPayAmount = amount;
            type = t;
        }
    }
}
