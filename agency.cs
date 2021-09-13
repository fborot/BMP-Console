using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
    class agency {
        public int agency_id;
        public string agency_name;
        public string agency_address;
        public string agency_address2;
        public string agency_postal_code;
        public string agency_contact_name;
        public string agency_email;
        public string agency_phone_number;
        public string name_for_checks;

        public agency(int id, string name, string addr, string addr2, string pcode, string contact_name, string email, string ph_number, string name_checks) {
            agency_id = id;
            agency_name = name;
            agency_address = addr;
            agency_address2 = addr2;
            agency_postal_code = pcode;
            agency_contact_name = contact_name;
            agency_email = email;
            agency_phone_number = ph_number;
            name_for_checks = name_checks;
        }
    }
}
