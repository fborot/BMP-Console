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
    public partial class AddPlan : Form {
        public AddPlan() {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) {
            Form1.AddPlan = null;
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e) {
            if (tbpid.Text.Length > 0 && tbpname.Text.Length > 0 && tbpname.Text.Length > 0 && tbpt1name.Text.Length > 0 && tbpt1cost.Text.Length > 0 && tbpt2name.Text.Length > 0 && tbpt2cost.Text.Length > 0 && tbpt3name.Text.Length > 0 && tbpt3cost.Text.Length > 0) {

                //var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                //if(!regex.IsMatch(tbpt1cost.Text) || !regex.IsMatch(tbpt2cost.Text) || !regex.IsMatch(tbpt3cost.Text)) {
                //    MessageBox.Show("Invalid values for some of the cost", "Saving new Plan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();

                float pt1cost = tbpt1cost.Text.Length > 0 ? float.Parse(tbpt1cost.Text) : 0;
                float pt2cost = tbpt2cost.Text.Length > 0 ? float.Parse(tbpt2cost.Text) : 0;
                float pt3cost = tbpt3cost.Text.Length > 0 ? float.Parse(tbpt3cost.Text) : 0;
                MySqlCommand cmd = new MySqlCommand("insert into plans (plan_name, plan_instance1_name, plan_instance1_cost, plan_instance2_name, plan_instance2_cost, plan_instance3_name, plan_instance3_cost)" +
                    "values(@plan_name, @plan_instance1_name, @plan_instance1_cost, @plan_instance2_name, @plan_instance2_cost, @plan_instance3_name, @plan_instance3_cost)", conn);

                cmd.Parameters.Add("@plan_name", MySqlDbType.VarChar, tbpname.Text.Length).Value = tbpname.Text;
                cmd.Parameters.Add("@plan_instance1_name", MySqlDbType.VarChar, tbpt1name.Text.Length).Value = tbpt1name.Text;
                //cmd.Parameters.Add("@plan_instance1_cost", MySqlDbType.Float).Value = pt1cost;
                cmd.Parameters.Add("@plan_instance1_cost", MySqlDbType.VarChar,tbpt1cost.Text.Length).Value = tbpt1cost.Text;
                cmd.Parameters.Add("@plan_instance2_name", MySqlDbType.VarChar, tbpt2name.Text.Length).Value = tbpt2name.Text;
                //cmd.Parameters.Add("@plan_instance2_cost", MySqlDbType.Float).Value = pt2cost;
                cmd.Parameters.Add("@plan_instance2_cost", MySqlDbType.VarChar, tbpt2cost.Text.Length).Value = tbpt2cost.Text;
                cmd.Parameters.Add("@plan_instance3_name", MySqlDbType.VarChar, tbpt3name.Text.Length).Value = tbpt3name.Text;
                //cmd.Parameters.Add("@plan_instance3_cost", MySqlDbType.Float).Value = pt3cost;
                cmd.Parameters.Add("@plan_instance3_cost", MySqlDbType.VarChar, tbpt3cost.Text.Length).Value = tbpt3cost.Text;

                int count = cmd.ExecuteNonQuery();
                if (count > 0) {
                    MessageBox.Show("Record successfully addded to the database;");
                    tbpname.Text = "";
                    tbpt1name.Text = "";
                    tbpt1cost.Text = "";
                    tbpt2name.Text = "";
                    tbpt2cost.Text = "";
                    tbpt3name.Text = "";
                    tbpt3cost.Text = "";

                    tbpid.Text = (Int16.Parse(tbpid.Text) + 1).ToString();
                }
                conn.Close();

            } else {
                MessageBox.Show("All values are required", "Saving new Plan", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private bool LoadInitialValues() {
            bool res = false;
            //load branch count to calculate next branch_id
            try {
                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select count(*) from plans", conn);
                int plans_count = Convert.ToInt32(cmd.ExecuteScalar());
                tbpid.Text = plans_count.ToString();
                res = true;
            } catch (Exception e) {             
                MessageBox.Show("there was an error trying to load the Plans: " + e.Message);
            }
            return res;
        }

        private void AddPlan_Load(object sender, EventArgs e) {
            if (!LoadInitialValues())
                MessageBox.Show("There was an error trying to load the Plans");
        }


    }
}
