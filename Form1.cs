using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Nini.Config;

namespace BMP_Console {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        static public AddMember Addmember = null;
        static public AddAgency AddAgency= null;
        static public AddBranch AddBranch = null;
        static public AddPlan AddPlan = null;
        static public ShowPlans ShowPlans = null;
        static public ShowMembers ShowMembers  = null;
        static public ShowAgencies ShowAgencies = null;
        static public ShowBranches ShowBranches = null;
        static public ProcessPeriod PPeriod = null;
        static public CalibratePrinter CPrinter = null;

        static public string db_host = string.Empty;
        static public string db_name = string.Empty;
        static public string db_user = string.Empty;
        static public string db_password = string.Empty;
        static public int db_connect_timeout = 5;

        static public string mySQLConnectionString = string.Empty;

        static public string APILoginID = string.Empty;
        static public string APITransactionKey = string.Empty;

        static public int X_Offset = 5;
        static public int Y_Offset = 5;
        static public float Font_Size = 12;

        private void versionToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("test");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void addMemberToolStripMenuItem_Click(object sender, EventArgs e) {
            //AddMember.ShowDialog();
            if (Addmember == null) {
                Addmember = new AddMember();
                Addmember.MdiParent = this;
                Addmember.StartPosition = FormStartPosition.Manual;
                Addmember.Show();

            } else {
                Addmember.Focus();
            }
        }

        private void versionToolStripMenuItem1_Click(object sender, EventArgs e) {
            MessageBox.Show("test");
        }

        private void addAgencyToolStripMenuItem_Click(object sender, EventArgs e) {
            //AddAgency.ShowDialog();
            if (AddAgency == null) {
                AddAgency = new AddAgency();
                AddAgency.MdiParent = this;
                AddAgency.StartPosition = FormStartPosition.Manual;
                AddAgency.Show();

            } else {
                AddAgency.Focus();
            }
        }

        private void addSalesmanToolStripMenuItem_Click(object sender, EventArgs e) {
            //AddBranch.ShowDialog();
            if (AddBranch == null) {
                AddBranch = new AddBranch();
                AddBranch.MdiParent = this;
                AddBranch.StartPosition = FormStartPosition.Manual;
                AddBranch.Show();

            } else {
                AddBranch.Focus();
            }
        }

        private void addPlanStripMenuItem_Click(object sender, EventArgs e) {
            //AddPlanh.ShowDialog();
            if (AddPlan == null) {
                AddPlan = new AddPlan();
                AddPlan.MdiParent = this;
                AddPlan.StartPosition = FormStartPosition.Manual;
                AddPlan.Show();

            } else {
                AddPlan.Focus();
            }
        }

        private bool LoadSettings() {
            bool res = false;
            IniConfigSource source = new IniConfigSource("./Settings/settings.ini");
            try {
                
                db_host = source.Configs["Database"].Get("Host", "127.0.0.1");
                db_user = source.Configs["Database"].Get("Username", "bmpadmin");
                db_password = source.Configs["Database"].Get("Password", "bmp@dm1n");
                db_name = source.Configs["Database"].Get("DBName", "bmp");
                db_connect_timeout = source.Configs["Database"].GetInt("ConnectTimeout", 5);

                APILoginID = source.Configs["AuthorizedNet"].Get("APILoginID", "3GQB2ens6x");
                APITransactionKey = source.Configs["AuthorizedNet"].Get("ApiTransactionKey", "9887PZn64wQ3rTxk");

                X_Offset = source.Configs["Printer"].GetInt("X_Offset", 5);
                Y_Offset = source.Configs["Printer"].GetInt("Y_Offset", 5);
                Font_Size = source.Configs["Printer"].GetInt("FontSize", 12);

                mySQLConnectionString = "server=" + db_host + ";uid=" + db_user + ";pwd=" + db_password + ";database=" + db_name + ";Connect Timeout=" + db_connect_timeout.ToString();

                res = true;
                return res;
            } catch (Nini.Ini.IniException NINI_ex) {
                source = null; //I got a nini exception
                MessageBox.Show("Error loading keywords from ini file. {0}", NINI_ex.Message);
                //Console.WriteLine("Main::Error loading keywords from ini file. {0}", NINI_ex.Message);
                //logger.Instance.write("Main::Error loading keywords from ini file. " + NINI_ex.Message);
                return res;
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            LoadSettings();
        }

        private void showPlansToolStripMenuItem_Click(object sender, EventArgs e) {
            // ShowPlans.ShowDialog();
            if (ShowPlans == null) {
                ShowPlans = new ShowPlans();
                ShowPlans.MdiParent = this;
                ShowPlans.StartPosition = FormStartPosition.Manual;
                ShowPlans.Show();

            } else {
                ShowPlans.Focus();
            }
        }

        private void showMemberToolStripMenuItem_Click(object sender, EventArgs e) {
            // ShowMembers.ShowDialog();
            if (ShowMembers == null) {
                ShowMembers = new ShowMembers();
                ShowMembers.MdiParent = this;
                ShowMembers.StartPosition = FormStartPosition.Manual;
                ShowMembers.Show();

            } else {
                ShowMembers.Focus();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            // ShowAgencies.ShowDialog();
            if (ShowAgencies == null) {
                ShowAgencies = new ShowAgencies();
                ShowAgencies.MdiParent = this;
                ShowAgencies.StartPosition = FormStartPosition.Manual;
                ShowAgencies.Show();

            } else {
                ShowAgencies.Focus();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            // ShowBranches.ShowDialog();
            if (ShowBranches == null) {
                ShowBranches = new ShowBranches();
                ShowBranches.MdiParent = this;
                ShowBranches.StartPosition = FormStartPosition.Manual;
                ShowBranches.Show();

            } else {
                ShowBranches.Focus();
            }
        }

        private void processPeriodToolStripMenuItem_Click(object sender, EventArgs e) {
            //PPeriod.ShowDialog();
            if (PPeriod == null) {
                PPeriod = new ProcessPeriod();
                PPeriod.MdiParent = this;
                PPeriod.StartPosition = FormStartPosition.Manual;
                PPeriod.Show();

            } else {
                PPeriod.Focus();
            }
        }

        private void calibratePrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CPrinter.ShowDialog();
            if (CPrinter == null)
            {
                CPrinter = new CalibratePrinter();
                CPrinter.MdiParent = this;
                CPrinter.StartPosition = FormStartPosition.Manual;
                CPrinter.Show();

            }
            else
            {
                CPrinter.Focus();
            }
        }
    }
}
