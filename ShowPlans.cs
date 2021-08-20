using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BMP_Console {
    public partial class ShowPlans : Form {

        private ArrayList ToBeUpdated = new ArrayList();
        private ArrayList PlansContainer = new ArrayList();
        public ShowPlans() {
            InitializeComponent();
        }

        private void ShowPlans_Load(object sender, EventArgs e) {
            //https://docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/walkthrough-creating-an-unbound-windows-forms-datagridview-control?view=netframeworkdesktop-4.8
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from plans order by plan_id", conn);
            MySqlDataReader ret = cmd.ExecuteReader();

            PlansContainer.Clear();
            while (ret.Read()) {
                short id = ret.GetInt16(0);
                string plan_name = ret.GetString(1);
                string pt1_name = ret.GetString(2);
                string pt1_cost = ret.GetFloat(3).ToString();
                string pt2_name = ret.GetString(4);
                string pt2_cost = ret.GetFloat(5).ToString();
                string pt3_name = ret.GetString(6);
                string pt3_cost = ret.GetFloat(7).ToString();
                PlansContainer.Add(new plan(id, plan_name, pt1_name, pt1_cost, pt2_name, pt2_cost, pt3_name, pt3_cost));
            }
            ret.Close();
            conn.Close();

            dgPlans.Columns.Add("Plan Id", "Plan ID");
            dgPlans.Columns.Add("Plan Name", "Plan Name");
            dgPlans.Columns.Add("Instance 1 Name", "PlanType 1 Name");
            dgPlans.Columns.Add("Instance 1 Cost", "PlanType 1 Cost");
            dgPlans.Columns.Add("Instance 2 Name", "PlanType 2 Name");
            dgPlans.Columns.Add("Instance 2 Cost", "PlanType 2 Cost");
            dgPlans.Columns.Add("Instance 3 Name", "PlanType 3 Name");
            dgPlans.Columns.Add("Instance 3 Cost", "PlanType 3 Cost");

            dgPlans.Rows.Clear();

            for (int i = 0; i < PlansContainer.Count; i++) {
                DataGridViewRow newRow = new DataGridViewRow();

                newRow.CreateCells(dgPlans);
                plan t = (plan)(PlansContainer[i]);
                newRow.Cells[0].Value = t.p_id.ToString();
                newRow.Cells[1].Value = t.p_name;
                newRow.Cells[2].Value = t.p_ins1_name;
                newRow.Cells[3].Value = t.p_ins1_cost;
                newRow.Cells[4].Value = t.p_ins2_name;
                newRow.Cells[5].Value = t.p_ins2_cost;
                newRow.Cells[6].Value = t.p_ins3_name;
                newRow.Cells[7].Value = t.p_ins3_cost;

                dgPlans.Rows.Add(newRow);
            }
        }

        private void button1_Click(object sender, EventArgs e) {//Cancel
            ToBeUpdated.Clear();
            Form1.ShowPlans = null;
            this.Close();

        }

        private void ShowPlans_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.ShowPlans = null;
        }

        private void button2_Click(object sender, EventArgs e) {//Delete
            
            int pid = Int16.Parse(dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[0].Value.ToString());
            string pname = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[1].Value.ToString();
            string pt1name = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[2].Value.ToString();
            string pt1cost = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[3].Value.ToString();
            string pt2name = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[4].Value.ToString();
            string pt2cost = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[5].Value.ToString();
            string pt3name = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[6].Value.ToString();
            string pt3cost = dgPlans.Rows[dgPlans.SelectedRows[0].Index].Cells[7].Value.ToString();

            string message = "You are about to delete a plan and this action can not be undone. Do you want to continue?";
            string title = "Delete Plan Confirmation";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons,MessageBoxIcon.Warning);
            if (result == DialogResult.No) {
                //this.Close();
            } else {
                if (dgPlans.SelectedRows.Count > 0) {
                    plan tempP = new plan(pid, pname, pt1name, pt1cost, pt2name, pt2cost, pt3name, pt3cost);
                    if(DeletePlanFromDB(tempP))
                        dgPlans.Rows.RemoveAt(dgPlans.SelectedRows[0].Index);
                    else
                        MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgPlans_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            //string s = e.RowIndex.ToString() + "-" + e.ColumnIndex.ToString();
            //string s = "";
            //s += dgPlans.Rows[e.RowIndex].Cells[0].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[1].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[2].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[3].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[4].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[5].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[6].Value.ToString() + "-";
            //s += dgPlans.Rows[e.RowIndex].Cells[7].Value.ToString();
            //MessageBox.Show(s);

            short pid = Int16.Parse(dgPlans.Rows[e.RowIndex].Cells[0].Value.ToString());
            string pname = dgPlans.Rows[e.RowIndex].Cells[1].Value.ToString();
            string pt1name = dgPlans.Rows[e.RowIndex].Cells[2].Value.ToString(); 
            string pt1cost = dgPlans.Rows[e.RowIndex].Cells[3].Value.ToString();
            string pt2name = dgPlans.Rows[e.RowIndex].Cells[4].Value.ToString();
            string pt2cost = dgPlans.Rows[e.RowIndex].Cells[5].Value.ToString();
            string pt3name = dgPlans.Rows[e.RowIndex].Cells[6].Value.ToString();
            string pt3cost = dgPlans.Rows[e.RowIndex].Cells[7].Value.ToString();

            plan tempP = new plan(pid, pname, pt1name, pt1cost, pt2name, pt2cost, pt3name, pt3cost);
            ToBeUpdated.Add(tempP);
            button1.Enabled = true;//cancel
            button3.Enabled = true;//save

        }

        private bool DeletePlanFromDB(plan p) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {

                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("delete from plans where plan_id = " + p.p_id.ToString(), conn);
                ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ?true:false;
                conn.Close();
            } catch (Exception e) {
                MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

            return res;
        }

        private void button3_Click(object sender, EventArgs e) {//Save
            if(ToBeUpdated.Count > 0) {
                //MessageBox.Show("There are " + ToBeUpdated.Count + " plans to be updated", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                for(int i = 0; i < ToBeUpdated.Count; i++) {
                    bool temp_res = UpdatePlan((plan)ToBeUpdated[i]);
                    if (!temp_res) {
                        MessageBox.Show("There was an error trying to update the plans. Please try again later or contact Support if the issue persists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ToBeUpdated.Clear();
                        //Form1.ShowPlans = null;
                        //this.Close();
                        break;
                    } else {
                        MessageBox.Show("Plan: " + ((plan)ToBeUpdated[i]).ToString() +  " was updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1.ShowPlans = null;
                        this.Close();
                    }
                }
            } else {
                Form1.ShowPlans = null;
                this.Close();
            }
        }

        bool UpdatePlan(plan p) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                string s = "update plans set plan_name='" + p.p_name + "',plan_instance1_name=" + p.p_ins1_name + ",plan_instance1_cost=" + p.p_ins1_cost + ",plan_instance2_name=" + p.p_ins2_name +
                    ",plan_instance2_cost=" + p.p_ins2_cost + ",plan_instance3_name=" + p.p_ins3_name + ",plan_instance3_cost=" + p.p_ins3_cost + " where plan_id = " + p.p_id.ToString();
                Console.WriteLine(s);
                MySqlCommand cmd = new MySqlCommand("update plans set plan_name='" + p.p_name + "',plan_instance1_name='" + p.p_ins1_name + "',plan_instance1_cost=" + p.p_ins1_cost + ",plan_instance2_name='" + p.p_ins2_name + 
                    "',plan_instance2_cost=" + p.p_ins2_cost + ",plan_instance3_name='" + p.p_ins3_name + "',plan_instance3_cost=" + p.p_ins3_cost + " where plan_id = " + p.p_id.ToString(), conn);
                ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;
                conn.Close();
            } catch (Exception e) {
                MessageBox.Show("An error ocurred!. The record could not be updated, please contact Support", "Error updating user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }


            return res;
        }
    }
}
