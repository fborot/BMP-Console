using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
    class member {
        public string member_id = string.Empty;
        public string name = string.Empty;
        public string mi = string.Empty;
        public string last_name = string.Empty;
        public string email = string.Empty;
        public string language = string.Empty;
        public string marital_status = string.Empty;
        public string gender = string.Empty;
        public int dob = 0;
        public string home_phone_number = string.Empty;
        public string mobile_phone_number = string.Empty;
        public string other_phone_number = string.Empty;
        public string address = string.Empty;
        public string address2 = string.Empty;
        public string city = string.Empty;
        public string state = string.Empty;
        public string postal_code = string.Empty;
        public string shipping_address = string.Empty;
        public string shipping_address2= string.Empty;
        public string shipping_city = string.Empty;
        public string shipping_state = string.Empty;
        public string shipping_postal_code = string.Empty;
        public short use_home_as_shipping_address = 0;
        public string plan_type = string.Empty;
        public string plan_name = string.Empty;
        public float recurring_total = 0;
        public int start_date = 0;
        public int end_date = 0;
        public short number_members = 0;
        public string agencyID = string.Empty;
        public string branchID = string.Empty;
        public short recurrency = 0;
        public string cc_info = string.Empty;
        public string cc_type = string.Empty;
        public string cc_expiration_date = string.Empty;
        public short cc_auto_pay = 0;
        public int dateadded = 0;
        public string policy_holder = string.Empty;
        public string relationship = string.Empty;

        public member(string m_id,string n, string m_i, string ln, string em, string lang, string m_st, string gen, int db, string h_ph_num, string mob_ph_num, string other_ph_num,
            string addr, string addr2, string cty, string st, string pos_code, string sh_addr, string sh_addr2, string sh_city, string sh_state, string sh_pos_code,
            short use_home_addr, string ptype, string pname, float rec_total, int start, int end, short num_members, string agcyID,string bID,short rec,string cc_inf, string cc_typ, string cc_exp_date, short a_pay, int dadded, string pol_holder, string relation) {
            member_id = m_id;
            name = n;
            mi = m_i;
            last_name = ln;
            email = em;
            language = lang;
            marital_status = m_st;
            gender = gen;
            dob = db;
            home_phone_number = h_ph_num;
            mobile_phone_number = mob_ph_num;
            other_phone_number = other_ph_num;
            address = addr;
            address2 = addr2;
            city = cty;
            state = st;
            postal_code = pos_code;
            shipping_address = sh_addr;
            shipping_address2 = sh_addr2;
            shipping_city = sh_city;
            shipping_state = sh_state;
            shipping_postal_code = sh_pos_code;
            use_home_as_shipping_address = use_home_addr;
            plan_type = ptype;
            plan_name = pname;
            recurring_total = rec_total;
            start_date = start;
            end_date = end;
            number_members = num_members;
            agencyID = agcyID;
            branchID = bID;
            recurrency = rec;
            cc_info = cc_inf;
            cc_type = cc_typ;
            cc_expiration_date = cc_exp_date;
            cc_auto_pay = a_pay;
            dateadded = dadded;
            policy_holder = pol_holder;
            relationship = relation;
        }
        public bool validate_member_info() {
            return true;//for now until we add the validation logic
        }
    }
}
