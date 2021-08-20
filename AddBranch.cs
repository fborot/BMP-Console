using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BMP_Console {
    public partial class AddBranch : Form {
        public AddBranch() {
            InitializeComponent();
        }

        private void btcancel_Click(object sender, EventArgs e) {
            Form1.AddBranch = null;
            this.Close();
        }

        private void btsave_Click(object sender, EventArgs e) {
            if(tbBranchID.Text.Length > 0 && tbaddress.Text.Length > 0 && tbaddress2.Text.Length > 0 && tbpostalcode.Text.Length > 0 && tbcontactname.Text.Length > 0 && tbcontactemail.Text.Length > 0 && tbcontactphone.Text.Length > 0) {
                //string ServerIP = "127.0.0.1"; string DBUserName = "fborot"; string DBUserPassword = "Fab!anB0"; string DBName = "bmp";
                //string mySQLConnectionString = "server=" + ServerIP + ";uid=" + DBUserName + ";pwd=" + DBUserPassword + ";database=" + DBName;

                if (!isValidEmail(tbcontactemail.Text)) {
                    MessageBox.Show("Invalid value for the Email Address", "Saving new Branch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!isValidPhoneNumber(tbcontactphone.Text)) {
                    MessageBox.Show("Invalid value for the Phone Number", "Saving new Branch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(!isValidZipCode(tbpostalcode.Text)) {
                    MessageBox.Show("Invalid value for the Zip Code", "Saving new Branch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();

                                MySqlCommand cmd = new MySqlCommand("insert into branches (branch_name,branch_address, branch_address2, branch_postal_code, branch_contact_name, branch_email, branch_phone, branch_agency)" +
                    "values (@branch_name, @branch_address, @branch_address2, @branch_postal_code, @branch_contact_name, @branch_email, @branch_phone_number, @branch_agency)", conn);

                cmd.Parameters.Add("@branch_name", MySqlDbType.VarChar, tbbranchname.Text.Length).Value = tbbranchname.Text;
                cmd.Parameters.Add("@branch_address", MySqlDbType.VarChar, tbaddress.Text.Length).Value = tbaddress.Text;
                cmd.Parameters.Add("@branch_address2", MySqlDbType.VarChar, tbaddress2.Text.Length).Value = tbaddress2.Text;
                cmd.Parameters.Add("@branch_contact_name", MySqlDbType.VarChar, tbcontactname.Text.Length).Value = tbcontactname.Text;
                cmd.Parameters.Add("@branch_postal_code", MySqlDbType.VarChar, tbpostalcode.Text.Length).Value = tbpostalcode.Text;
                cmd.Parameters.Add("@branch_email", MySqlDbType.VarChar, tbcontactemail.Text.Length).Value = tbcontactemail.Text;
                cmd.Parameters.Add("@branch_phone_number", MySqlDbType.VarChar, tbcontactphone.Text.Length).Value = tbcontactphone.Text;
                cmd.Parameters.Add("@branch_agency", MySqlDbType.VarChar, cbagencies.SelectedItem.ToString().Length).Value = cbagencies.SelectedItem.ToString();

                int res = cmd.ExecuteNonQuery();
                if(res > 0) {
                    MessageBox.Show(res.ToString() + " record successfully created!", "Creating new Branch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbbranchname.Text = "";
                    tbaddress.Text = "";
                    tbaddress2.Text = "";
                    tbcontactemail.Text = "";
                    tbcontactname.Text = "";
                    tbcontactphone.Text = "";
                    tbpostalcode.Text = "";

                    tbBranchID.Text = (Int16.Parse(tbBranchID.Text) + 1).ToString();
                }
                
            } else {
                MessageBox.Show("All fields are required, check your values!");
            }
        }

        private void AddBranch_Load(object sender, EventArgs e) {
            //load initial values
            bool res = LoadInitialValues();
            if (!res)
                MessageBox.Show("There was an error trying to connect tothe database to load some values");

        }

        private bool LoadInitialValues() {
            bool res1 = false; bool res2 = false;
            //string ServerIP = "127.0.0.1"; string DBUserName = "fborot"; string DBUserPassword = "Fab!anB0"; string DBName = "bmp";
            //string mySQLConnectionString = "server=" + ServerIP + ";uid=" + DBUserName + ";pwd=" + DBUserPassword + ";database=" + DBName;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();

            //load agencies
            try {
                MySqlCommand cmd = new MySqlCommand("select * from agencies order by agency_id asc", conn);
                MySqlDataReader ret = cmd.ExecuteReader();
                while (ret.Read()) {
                    int a_id = ret.GetInt32(0);
                    string a_name = ret.GetString(1);
                    string a_address = ret.GetString(2);
                    string a_address2 = ret.GetString(3);
                    string a_postal_code = ret.GetString(4);
                    string a_contact_name = ret.GetString(5);
                    string a_email = ret.GetString(6);
                    string a_ph_number = ret.GetString(7);

                    this.cbagencies.Items.Add(a_name);

                }
                ret.Close();
            } catch (Exception e) {
                res1 = false;
                MessageBox.Show("there was an error trying to load the Agencies: " + e.Message);
            }
            res1 = true;

            //load branch count to calculate next branch_id
            try {
                MySqlCommand cmd = new MySqlCommand("select count(*) from branches", conn);
                int branches_count = Convert.ToInt32(cmd.ExecuteScalar());
                tbBranchID.Text = branches_count.ToString();
                res2 = true;
            } catch (Exception e) {
                res2 = false;
                MessageBox.Show("there was an error trying to load the Branches: " + e.Message,"Error adding Branch.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            res2 = true;
            
            return res1 && res2;

        }

        private void AddBranch_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.AddBranch = null;
        }

        bool isValidEmail(string email) {
            bool res = false;
            string[] part1 = email.Split('@');
            if (part1.Length == 2)
                if (part1[1].Split('.').Length == 2)
                    res = true;


            return res;
        }

        bool isValidPhoneNumber(string phone) {
            bool res = false;
            //Regex rph = new Regex(@"\d{3}-\d{3}-\d{4}$");
            Regex rph = new Regex(@"\d{3}-?\d{3}-?\d{4}$");

            Match matchPh = rph.Match(phone);
            if (matchPh.Success)
                res = true;

            return res;
        }

        bool isValidZipCode(string zip_code) {
            bool res = false;
            Regex rZip = new Regex(@"(^\d{5}$)|(^\d{9}$)|(^\d{5}-\d{4}$)");
            Match matchZip = rZip.Match(zip_code);
            if (matchZip.Success)
                res = true;

            return res;
        }

    }
}
