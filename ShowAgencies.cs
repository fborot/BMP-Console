using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace BMP_Console {
    public partial class ShowAgencies : Form {

        private ArrayList AgenciesContainer = new ArrayList();
        private ArrayList ToBeUpdated = new ArrayList();
        public ShowAgencies() {
            InitializeComponent();
        }

        public bool PingDB()
        {
            bool res = false;

            MySqlConnection conn = null;
            var ret = -1;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select COUNT(*) from plans", conn);
                ret = Int16.Parse(cmd.ExecuteScalar().ToString());
                if (ret >= 0)
                    res = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Show Agencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }

        private void ShowAgencies_Load(object sender, EventArgs e) {

            if (!PingDB())
            {
                MessageBox.Show("Can not connect to the database", "Show Agencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from agencies order by agency_id", conn);
            MySqlDataReader ret = cmd.ExecuteReader();

            AgenciesContainer.Clear();
            while (ret.Read()) {
                short id = ret.GetInt16(0);
                string agency_name = ret.GetString(1);
                string agency_address= ret.GetString(2);
                string agency_address2 = ret.GetString(3);
                string agency_pcode = ret.GetString(4);
                string agency_cname = ret.GetString(5);
                string agency_cemail = ret.GetString(6);
                string agency_ph = ret.GetString(7);
                string name_for_checks = ret.GetString(8);

                AgenciesContainer.Add(new agency(id, agency_name, agency_address, agency_address2, agency_pcode, agency_cname, agency_cemail, agency_ph, name_for_checks));
            }
            ret.Close();
            conn.Close();

            dgAgencies.Columns.Add("Agency Id", "Agency ID");
            dgAgencies.Columns.Add("Agency Name", "Agency Name");
            dgAgencies.Columns.Add("Agency Address", "Address");
            dgAgencies.Columns.Add("Agency Address2", "Address2");
            dgAgencies.Columns.Add("Agency Postal Code", "Postal Code");
            dgAgencies.Columns.Add("Agency Contact Name", "Contact Name");
            dgAgencies.Columns.Add("Agency Contact Email", "Contact Email");
            dgAgencies.Columns.Add("Agency Phone Number", "Phone Number");
            dgAgencies.Columns.Add("Name For Checks", "Name For Checks");

            dgAgencies.Columns[0].Visible = false;

            dgAgencies.Rows.Clear();

            for (int i = 0; i < AgenciesContainer.Count; i++) {
                DataGridViewRow newRow = new DataGridViewRow();

                newRow.CreateCells(dgAgencies);
                agency t = (agency)(AgenciesContainer[i]);
                newRow.Cells[0].Value = t.agency_id.ToString();
                newRow.Cells[1].Value = t.agency_name;
                newRow.Cells[2].Value = t.agency_address;
                newRow.Cells[3].Value = t.agency_address2;
                newRow.Cells[4].Value = t.agency_postal_code;
                newRow.Cells[5].Value = t.agency_contact_name;
                newRow.Cells[6].Value = t.agency_email;
                newRow.Cells[7].Value = t.agency_phone_number;
                newRow.Cells[8].Value = t.name_for_checks;

                dgAgencies.Rows.Add(newRow);
            }
        }

        private void ShowAgencies_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.ShowAgencies = null;
        }

        private void btDelete_Click(object sender, EventArgs e) {
            int aid = Int32.Parse(dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[0].Value.ToString());
            string aname = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[1].Value.ToString();
            string aaddr = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[2].Value.ToString();
            string aaddr2 = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[3].Value.ToString();
            string apcode = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[4].Value.ToString();
            string acname = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[5].Value.ToString();
            string aemail = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[6].Value.ToString();
            string aphone = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[7].Value.ToString();
            string ause = dgAgencies.Rows[dgAgencies.SelectedRows[0].Index].Cells[8].Value.ToString();

            string message = "You are about to delete anagency and this action can not be undone. Do you want to continue?";
            string title = "Delete Agency Confirmation";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.No) {
                //this.Close();
            } else {
                if (dgAgencies.SelectedRows.Count > 0) {
                    agency tempA = new agency(aid, aname, aaddr, aaddr2, apcode, acname, aemail, aphone,ause);
                    if (DeleteAgencyFromDB(tempA))
                        dgAgencies.Rows.RemoveAt(dgAgencies.SelectedRows[0].Index);
                    else
                        MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btCancel_Click(object sender, EventArgs e) {
            ToBeUpdated.Clear();
            Form1.ShowAgencies = null;
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e) {
            if (ToBeUpdated.Count > 0) {
                for (int i = 0; i < ToBeUpdated.Count; i++) {
                    bool temp_res = UpdateAgency((agency)ToBeUpdated[i]);
                    if (!temp_res) {
                        MessageBox.Show("There was an error trying to update the agencies. Please try again later or contact Support if the issue persists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ToBeUpdated.Clear();
                        //Form1.ShowAgencies = null;
                        //this.Close();
                        break;
                    } else {
                        MessageBox.Show("Agency: " + ((agency)ToBeUpdated[i]).ToString() + " was updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1.ShowAgencies = null;
                        this.Close();
                    }
                }
            } else {
                Form1.ShowAgencies = null;
                this.Close();
            }
        }

        private bool DeleteAgencyFromDB(agency a) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {

                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("delete from agencies where agency_id = " + a.agency_id.ToString(), conn);
                ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;
                if (res)//reseting auto_increment id
                {
                    cmd = new MySqlCommand("SELECT MAX(agency_id) FROM agencies", conn);
                    int num = -1;
                    num = Convert.ToInt32(cmd.ExecuteScalar());
                    if (num >= 0)
                    {
                        num++;
                        cmd = new MySqlCommand("ALTER TABLE agencies AUTO_INCREMENT=" + num.ToString(), conn);
                        ret = cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            } catch (Exception e) {
                MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

            return res;
        }

        bool UpdateAgency(agency a) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                string strQuery = "update agencies set agency_name='" + a.agency_name + "',agency_address='" + a.agency_address + "',agency_address2='" + a.agency_address2 + "',agency_postal_code='" + a.agency_postal_code +
                "',agency_contact_name='" + a.agency_contact_name + "',agency_email='" + a.agency_email + "',agency_phone_number='" + a.agency_phone_number + "',name_for_checks='" + a.name_for_checks + "' where agency_id = " + a.agency_id.ToString();
                
                //Console.WriteLine(s);
                MySqlCommand cmd = new MySqlCommand(strQuery, conn);
                ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;
                conn.Close();
            } catch (Exception e) {
                MessageBox.Show("An error ocurred!. The record could not be updated, please contact Support", "Error updating user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }


            return res;
        }

        private void dgAgencies_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            short aid = Int16.Parse(dgAgencies.Rows[e.RowIndex].Cells[0].Value.ToString());
            string aname = dgAgencies.Rows[e.RowIndex].Cells[1].Value.ToString();
            string aaddr = dgAgencies.Rows[e.RowIndex].Cells[2].Value.ToString();
            string aaddr2 = dgAgencies.Rows[e.RowIndex].Cells[3].Value.ToString();
            string apcode = dgAgencies.Rows[e.RowIndex].Cells[4].Value.ToString();
            string acname = dgAgencies.Rows[e.RowIndex].Cells[5].Value.ToString();
            string aemail = dgAgencies.Rows[e.RowIndex].Cells[6].Value.ToString();
            string aphone = dgAgencies.Rows[e.RowIndex].Cells[7].Value.ToString();
            string checks_name = dgAgencies.Rows[e.RowIndex].Cells[8].Value.ToString();


            agency tempA = new agency(aid, aname, aaddr, aaddr2, apcode, acname, aemail, aphone, checks_name);
            ToBeUpdated.Add(tempA);
            btCancel.Enabled = true;
            btSave.Enabled = true;
        }
    }
}
