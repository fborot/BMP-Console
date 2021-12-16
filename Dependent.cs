using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
    class Dependent {
        public string name = string.Empty;
        public string mi = string.Empty;
        public string lname = string.Empty;
        public string gender = string.Empty;
        public string dob = string.Empty;
        public string relationship = string.Empty;
        public Dependent(string n, string _mi, string ln, string gen, string d, string rel) {
            name = n;
            mi = _mi;
            lname = ln;
            gender = gen;
            dob = d;
            relationship = rel;
        }

        override public string ToString() {
            string sb = string.Empty;
            sb = name + "," + mi + "," + lname + "," + gender + "," + dob + "," + relationship;

            return sb;
        }
    }
}
