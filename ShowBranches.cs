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
    public partial class ShowBranches : Form {

        private ArrayList BranchesContainer = new ArrayList();
        private ArrayList ToBeUpdated = new ArrayList();
        public ShowBranches() {
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
                MessageBox.Show(e.Message, "Show Branches", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }

        private void ShowBranches_Load(object sender, EventArgs e) {

            if (!PingDB())
            {
                MessageBox.Show("Can not connect to the database", "Show Branches", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from branches order by branch_id", conn);
            MySqlDataReader ret = cmd.ExecuteReader();

            BranchesContainer.Clear();
            while (ret.Read()) {
                short id = ret.GetInt16(0);
                string branch_name = ret.GetString(1);
                string branch_address = ret.GetString(2);
                string branch_address2 = ret.GetString(3);
                string branch_pcode = ret.GetString(4);
                string branch_cname = ret.GetString(5);
                string branch_cemail = ret.GetString(6);
                string branch_ph = ret.GetString(7);
                string branch_agency = ret.GetString(8);
                string name_for_checks = ret.GetString(9);

                BranchesContainer.Add(new branch(id, branch_name, branch_address, branch_address2, branch_pcode, branch_cname, branch_cemail, branch_ph, branch_agency,name_for_checks));
            }
            ret.Close();
            conn.Close();

            dgBranches.Columns.Add("Branch Id", "Branch ID");
            dgBranches.Columns.Add("Branch Name", "Branch Name");
            dgBranches.Columns.Add("Branch Address", "Address");
            dgBranches.Columns.Add("Branch Address2", "Address2");
            dgBranches.Columns.Add("Branch Postal Code", "Postal Code");
            dgBranches.Columns.Add("Branch Contact Name", "Contact Name");
            dgBranches.Columns.Add("Branch Contact Email", "Contact Email");
            dgBranches.Columns.Add("Branch Phone Number", "Phone Number");
            dgBranches.Columns.Add("Branch Agency", "Agency");
            dgBranches.Columns.Add("Name for Checks", "Payment Name");

            dgBranches.Rows.Clear();

            dgBranches.Columns[0].Visible = false;

            for (int i = 0; i < BranchesContainer.Count; i++) {
                DataGridViewRow newRow = new DataGridViewRow();

                newRow.CreateCells(dgBranches);
                branch t = (branch)(BranchesContainer[i]);
                newRow.Cells[0].Value = t.branch_id.ToString();
                newRow.Cells[1].Value = t.branch_name;
                newRow.Cells[2].Value = t.branch_address;
                newRow.Cells[3].Value = t.branch_address2;
                newRow.Cells[4].Value = t.branch_postal_code;
                newRow.Cells[5].Value = t.branch_contact_name;
                newRow.Cells[6].Value = t.branch_email;
                newRow.Cells[7].Value = t.branch_phone;
                newRow.Cells[8].Value = t.branch_agency;
                newRow.Cells[9].Value = t.name_for_checks;

                dgBranches.Rows.Add(newRow);
            }
        }

        private void ShowBranches_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.ShowBranches = null;
        }

        private bool DeleteBranchFromDB(branch b) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {

                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("delete from branches where branch_id = " + b.branch_id.ToString(), conn);
                ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;
                if (res)//reseting auto_increment id
                {
                    cmd = new MySqlCommand("SELECT MAX(branch_id) FROM branches", conn);
                    int num = -1;
                    num = Convert.ToInt32(cmd.ExecuteScalar());
                    if (num >= 0)
                    {
                        num++;
                        cmd = new MySqlCommand("ALTER TABLE branches AUTO_INCREMENT=" + num.ToString(), conn);
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

        private void btCancel_Click(object sender, EventArgs e) {
            ToBeUpdated.Clear();
            Form1.ShowBranches = null;
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e) {
            if (ToBeUpdated.Count > 0) {                
                for (int i = 0; i < ToBeUpdated.Count; i++) {
                    bool temp_res = UpdateBranch((branch)ToBeUpdated[i]);
                    if (!temp_res) {
                        MessageBox.Show("There was an error trying to update the branchs. Please try again later or contact Support if the issue persists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ToBeUpdated.Clear();
                        //Form1.ShowPlans = null;
                        //this.Close();
                        break;
                    } else {
                        MessageBox.Show("Plan: " + ((branch)ToBeUpdated[i]).ToString() + " was updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1.ShowBranches = null;
                        this.Close();
                    }
                }
            }
            else {
                Form1.ShowBranches = null;
                this.Close();
            }
        }

        bool UpdateBranch(branch b) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                string strQuery = "update branches set branch_name='" + b.branch_name + "',branch_address='" + b.branch_address + "',branch_address2='" + b.branch_address2 + "',branch_postal_code='" + b.branch_postal_code +
                    "',branch_contact_name='" + b.branch_contact_name + "',branch_email='" + b.branch_email + "',branch_phone='" + b.branch_phone + "',branch_agency='" + b.branch_agency +"',name_for_checks='" + 
                    b.name_for_checks + "' where branch_id = " + b.branch_id.ToString();
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

        private void dgBranches_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            short bid = Int16.Parse(dgBranches.Rows[e.RowIndex].Cells[0].Value.ToString());
            string bname = dgBranches.Rows[e.RowIndex].Cells[1].Value.ToString();
            string baddr = dgBranches.Rows[e.RowIndex].Cells[2].Value.ToString();
            string baddr2 = dgBranches.Rows[e.RowIndex].Cells[3].Value.ToString();
            string bpcode = dgBranches.Rows[e.RowIndex].Cells[4].Value.ToString();
            string bcname = dgBranches.Rows[e.RowIndex].Cells[5].Value.ToString();
            string bemail = dgBranches.Rows[e.RowIndex].Cells[6].Value.ToString();
            string bphone = dgBranches.Rows[e.RowIndex].Cells[7].Value.ToString();
            string bagency = dgBranches.Rows[e.RowIndex].Cells[8].Value.ToString();
            string bchecksname = dgBranches.Rows[e.RowIndex].Cells[9].Value.ToString();

            branch tempB = new branch(bid,bname,baddr,baddr2,bpcode,bcname,bemail,bphone,bagency,bchecksname);
            ToBeUpdated.Add(tempB);
            btCancel.Enabled = true;
            btSave.Enabled = true;
        }

        private void btDelete_Click(object sender, EventArgs e) {

            int bid = Int32.Parse(dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[0].Value.ToString());
            string bname = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[1].Value.ToString();
            string baddr = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[2].Value.ToString();
            string baddr2 = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[3].Value.ToString();
            string bpcode = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[4].Value.ToString();
            string bcname = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[5].Value.ToString();
            string bemail = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[6].Value.ToString();
            string bphone = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[7].Value.ToString();
            string bagency = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[8].Value.ToString();
            string bchecksname = dgBranches.Rows[dgBranches.SelectedRows[0].Index].Cells[9].Value.ToString();

            string message = "You are about to delete a branch and this action can not be undone. Do you want to continue?";
            string title = "Delete Branch Confirmation";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.No) {
                //this.Close();
            } else {
                if (dgBranches.SelectedRows.Count > 0) {
                    branch tempB = new branch(bid, bname, baddr, baddr2, bpcode, bcname, bemail, bphone,bagency,bchecksname);
                    if (DeleteBranchFromDB(tempB))
                        dgBranches.Rows.RemoveAt(dgBranches.SelectedRows[0].Index);
                    else
                        MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
