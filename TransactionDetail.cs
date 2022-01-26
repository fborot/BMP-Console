using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
    class TransactionDetail {
        public string t_id = string.Empty;
        public string t_type = string.Empty; //new|renewal
        public string t_bmpCustID = string.Empty;
        public decimal t_amount = 0;
        public string t_agency = string.Empty;
        public string t_branch = string.Empty;
        public string t_plan = string.Empty;
        public short t_recurrency = -1;
        public string t_subscriptionid = string.Empty;

        public TransactionDetail(string id, string type, string custid, decimal amount, string agency, string branch, string plan,short recurrency, string subs) {
            t_id = id;
            t_type = type;
            t_bmpCustID = custid;
            t_amount = amount;
            t_agency = agency;
            t_branch = branch;
            t_plan = plan;
            t_recurrency = recurrency;
            t_subscriptionid = subs;
        }
        public bool isValid() {
            bool res = false;
            string[] tt = null;
            if (t_id.Length > 0 && t_type.Length > 0 && t_bmpCustID.Length > 0 && t_amount > 0 && t_agency.Length > 0 && t_branch.Length > 0 && t_plan.Length > 0 && t_recurrency >= 0) {
                if (t_type == "renewal") {
                    if (t_subscriptionid.Length < 1) {
                        res = false;
                        return res;
                    }
                }
            } else
                return res;

            tt = t_id.Split(':');
            if (tt[0] != "Error" && tt[0] != "Null") {
                res = true;
                return res;
            }

            return res;
        }
        public override string ToString()
        {
            string res = string.Empty;
            if (t_subscriptionid.Length == 0)
                t_subscriptionid = "NA";
            string padding = (t_branch.Length <= 6) ? "\t\t\t" : "\t\t";
            res = t_id + "\t" + t_type + "\t\t\t" + t_bmpCustID + "\t" + "$ " + t_amount + "\t\t" + t_agency + "\t\t" + t_branch + padding + t_plan + "\t\t" + t_recurrency + "\t\t" + t_subscriptionid;
            //res = String.Format("{0,-20}|{1,-25}|{2,30}|{3,20}|{4,20}|{5,-20}|{6,-20}|{7,-20}|{8,-20}", t_id,t_type,t_bmpCustID,t_amount,t_agency,t_branch,t_plan,t_recurrency,t_subscriptionid);


            return res;

        }
    }
}
