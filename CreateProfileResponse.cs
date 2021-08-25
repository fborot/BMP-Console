using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
     class CreateProfileResponse {
        public string CustomerProfileID = string.Empty;
        public string CustomerPayProfileID = string.Empty;
        public string CustomerShProfileID = string.Empty;
        public bool res = false;

        public CreateProfileResponse(string cpid, string cppid, string cspid, bool r) {
            CustomerProfileID = cpid;
            CustomerPayProfileID = cppid;
            CustomerShProfileID = cspid;
            res = r;
        }
    }
}
