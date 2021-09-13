using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
    class branch {
        public int branch_id;
        public string branch_name;
        public string branch_address; 
        public string branch_address2;
        public string branch_postal_code;
        public string branch_contact_name;
        public string branch_email;
        public string branch_phone;
        public string branch_agency;
        public string name_for_checks = string.Empty;

        public branch(int id, string name, string addr, string addr2, string p_code, string c_name, string email, string ph_number, string agency, string name_checks) {
            branch_id = id;
            branch_name = name;
            branch_address = addr;
            branch_address2 = addr2;
            branch_postal_code = p_code;
            branch_contact_name = c_name;
            branch_email = email;
            branch_phone = ph_number;
            branch_agency = agency;
            name_for_checks = name_checks;            
        }
    }
}
