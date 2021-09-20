using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMP_Console {
    public partial class EditMember : Form {
        public EditMember() {
            InitializeComponent();
        }

        private void EditMember_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.EMember = null;            
        }
    }
}
