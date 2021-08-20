using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP_Console {
    class plan {

        public int p_id;
        public string p_name;
        public string p_ins1_name;
        public string p_ins1_cost;
        public string p_ins2_name;
        public string p_ins2_cost;
        public string p_ins3_name;
        public string p_ins3_cost;
        public plan(int id, string name, string instance1_name, string instance1_cost, string instance2_name, string instance2_cost, string instance3_name, string instance3_cost) {
            p_id = id;
            p_name = name;
            p_ins1_name = instance1_name;
            p_ins1_cost = instance1_cost;
            p_ins2_name = instance2_name;
            p_ins2_cost = instance2_cost;
            p_ins3_name = instance3_name;
            p_ins3_cost = instance3_cost;
        }
        override public string ToString() {
            string temp = p_id.ToString() + "," + p_name + "," + p_ins1_name + "," + p_ins1_cost + "," + p_ins1_cost + "," + p_ins2_name + "," + p_ins2_cost + "," + p_ins3_name + "," + p_ins3_cost;
            return temp;
        }
    }
}
