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
    public partial class AddAgency : Form {
        public AddAgency() {
            InitializeComponent();
        }

        private void AddAgency_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.AddAgency = null;
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (tbAgcyName.Text.Length > 0 && tbaddress.Text.Length > 0 && tbcontact.Text.Length > 0 && tbemail.Text.Length > 0 && tbphone.Text.Length > 0 && tbpostalcode.Text.Length > 0 && tbNameChecks.Text.Length > 0) {

                if (!isValidEmail(tbemail.Text)) {
                    MessageBox.Show("Invalid value for the Email Address", "Saving new Agency", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!isValidPhoneNumber(tbphone.Text)) {
                    MessageBox.Show("Invalid value for the Phone Number", "Saving new Agency", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!isValidZipCode(tbpostalcode.Text)) {
                    MessageBox.Show("Invalid value for the Zip Code", "Saving new Agency", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                //string ServerIP = "127.0.0.1"; string DBUserName = "fborot"; string DBUserPassword = "Fab!anB0"; string DBName = "bmp";
                //string mySQLConnectionString = "server=" + ServerIP + ";uid=" + DBUserName + ";pwd=" + DBUserPassword + ";database=" + DBName;
                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();


                MySqlCommand cmd = new MySqlCommand("insert into agencies (agency_name,agency_address, agency_address2, agency_postal_code, agency_contact_name, agency_email, agency_phone_number, name_for_checks)" +
                    "values (@agency_name, @agency_address, @agency_address2, @agency_postal_code, @agency_contact_name, @agency_email, @agency_phone_number,@name_for_checks)", conn);

                cmd.Parameters.Add("@agency_name", MySqlDbType.VarChar, tbAgcyName.Text.Length).Value = tbAgcyName.Text;
                cmd.Parameters.Add("@agency_address", MySqlDbType.VarChar, tbaddress.Text.Length).Value = tbaddress.Text;
                cmd.Parameters.Add("@agency_address2", MySqlDbType.VarChar, tbaddress2.Text.Length).Value = tbaddress2.Text;
                cmd.Parameters.Add("@agency_contact_name", MySqlDbType.VarChar, tbcontact.Text.Length).Value = tbcontact.Text;
                cmd.Parameters.Add("@agency_postal_code", MySqlDbType.VarChar, tbpostalcode.Text.Length).Value = tbpostalcode.Text;
                cmd.Parameters.Add("@agency_email", MySqlDbType.VarChar, tbemail.Text.Length).Value = tbemail.Text;
                cmd.Parameters.Add("@agency_phone_number", MySqlDbType.VarChar, tbphone.Text.Length).Value = tbphone.Text;
                cmd.Parameters.Add("@name_for_checks", MySqlDbType.VarChar,tbNameChecks.Text.Length ).Value = tbNameChecks.Text;

                int res = cmd.ExecuteNonQuery();
                if (res > 0) {
                    MessageBox.Show(res.ToString() + " record successfully created!", "Creating new Agency",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbAgcyName.Text = "";
                    tbaddress.Text = "";
                    tbaddress2.Text = "";
                    tbcontact.Text = "";
                    tbpostalcode.Text = "";
                    tbphone.Text = "";
                    tbemail.Text = "";
                    tbNameChecks.Text = "";

                    tbAgencyID.Text = (Int16.Parse(tbAgencyID.Text) + 1).ToString();
                }

            } else {
                MessageBox.Show("All fields are required, check your values!");
            }
        }

        private void AddAgency_Load(object sender, EventArgs e) {
            //string ServerIP = "127.0.0.1"; string DBUserName = "fborot"; string DBUserPassword = "Fab!anB0"; string DBName = "bmp";
            //string mySQLConnectionString = "server=" + ServerIP + ";uid=" + DBUserName + ";pwd=" + DBUserPassword + ";database=" + DBName;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();

            //load member count to calculate next member_id
            MySqlCommand cmd = new MySqlCommand("select count(*) from agencies", conn);
            int agency_count = Convert.ToInt32(cmd.ExecuteScalar());
            tbAgencyID.Text = agency_count.ToString();
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
