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
    public partial class ShowMembers : Form {
        public ShowMembers() {
            InitializeComponent();
        }
        ArrayList MembersContainer = new ArrayList();
        ArrayList ToBeUpdated = new ArrayList();

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
                MessageBox.Show(e.Message, "Show Members", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }

        private void ShowMembers_Load(object sender, EventArgs e) {

            if (!PingDB())
            {
                MessageBox.Show("Can not connect to the database", "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from members order by member_id", conn);
            MySqlDataReader ret = cmd.ExecuteReader();

            MembersContainer.Clear();            
            while (ret.Read()) {

                int id = ret.GetInt32(0);
                string bmp_id = ret.GetString(1);
                string name = ret.GetString(2);
                string mi = ret.GetString(3);
                string lastname = ret.GetString(4);
                string email = ret.GetString(5).ToString();
                string language = ret.GetString(6);
                string marital_status = ret.GetString(7);
                string gender = ret.GetString(8);
                int dob = ret.GetInt32(9);
                string home_ph = ret.GetString(10);
                string mobile_ph = ret.GetString(11);
                string other_ph = ret.GetString(12);
                string address = ret.GetString(13);
                string address2 = ret.GetString(14);
                string city_ = ret.GetString(15);
                string state = ret.GetString(16);
                string postal_code = ret.GetString(17);
                string shipping_address = ret.GetString(18);
                string shipping_address2 = ret.GetString(19);
                string shipping_city = ret.GetString(20);
                string shipping_state = ret.GetString(21);
                string shipping_pcode= ret.GetString(22);
                short use_home = ret.GetInt16(23);
                string ptype = ret.GetString(24);
                string pname = ret.GetString(25);
                float rec_total = ret.GetFloat(26);
                int start_d = ret.GetInt32(27);
                int end_d = ret.GetInt32(28);
                short num = ret.GetInt16(29);
                string agcy_id = ret.GetString(30);
                string branch_id = ret.GetString(31);
                short recurrency = ret.GetInt16(32);
                string cc_info = ret.GetString(33);
                string cc_type = ret.GetString(34);
                string cc_exp_date = ret.GetString(35);
                short cc_auto = ret.GetInt16(36);
                int dateadded = ret.GetInt32(37);
                string policy_holder = ret.GetString(38);
                string relationship = ret.GetString(39);

                MembersContainer.Add(new member(id.ToString(),bmp_id,name,mi,lastname,email,language,marital_status,gender,dob,home_ph,mobile_ph,other_ph,address,address2,city_,state,postal_code,
                    shipping_address,shipping_address2,shipping_city,shipping_state,shipping_pcode,use_home,ptype,pname,rec_total,start_d,end_d,num,agcy_id,branch_id, recurrency, cc_info,cc_type,cc_exp_date,cc_auto,dateadded,policy_holder, relationship));

            }
            ret.Close();
            conn.Close();

            dgmembers.Columns.Add("Member Id", "Member ID");
            dgmembers.Columns.Add("BMP Member Id", "BMP Member ID");
            dgmembers.Columns.Add("Member Name", "Member Name");
            dgmembers.Columns.Add("Middle Initial", "Middle Initial");
            dgmembers.Columns.Add("Last Name", "Last Name");
            dgmembers.Columns.Add("Email", "Email");
            dgmembers.Columns.Add("Language", "Language");
            dgmembers.Columns.Add("Marital Status", "Marital Status");
            dgmembers.Columns.Add("Gender", "Gender");
            dgmembers.Columns.Add("DoB", "Date of Birth");
            dgmembers.Columns.Add("Home Phone Name", "Home Phone Number");
            dgmembers.Columns.Add("Mobile Phone Number", "Mobile Phone Number");
            dgmembers.Columns.Add("Other Phone NUmber", "Other Phone Number");
            dgmembers.Columns.Add("Address", "Address");
            dgmembers.Columns.Add("Address2", "Address2");
            dgmembers.Columns.Add("City", "City");
            dgmembers.Columns.Add("State", "State");
            dgmembers.Columns.Add("Postal Code", "Postal Code");
            dgmembers.Columns.Add("Shipping Address", "Shipping Address");
            dgmembers.Columns.Add("Shipping Address2", "Shipping Address2");
            dgmembers.Columns.Add("Shipping City", "Shipping City");
            dgmembers.Columns.Add("Shipping State", "Shipping State");
            dgmembers.Columns.Add("Shipping Postal Code", "Shipping Postal Code");
            dgmembers.Columns.Add("Use Home Address", "Use Home Address");
            dgmembers.Columns.Add("Plan Name", "Plan Name");
            dgmembers.Columns.Add("Plan Type", "Plan Type");
            dgmembers.Columns.Add("Recurring Charges", "Recurring Charges");
            dgmembers.Columns.Add("Start Date", "Start Date");
            dgmembers.Columns.Add("End Date", "End Date");
            dgmembers.Columns.Add("Num of Members", "Num of Members");
            dgmembers.Columns.Add("Agency", "Agency");
            dgmembers.Columns.Add("Branch", "Branch");
            dgmembers.Columns.Add("Recurrency", "Recurrency");
            dgmembers.Columns.Add("CC Info", "CC Info");
            dgmembers.Columns.Add("CC Type", "CC Type");
            dgmembers.Columns.Add("CC Exp Date", "CC Exp Date");
            dgmembers.Columns.Add("CC Auto Pay", "CC Auto Pay");
            dgmembers.Columns.Add("Date Added", "Date Added");
            dgmembers.Columns.Add("Policy Holder", "Policy Holder");
            dgmembers.Columns.Add("Relationship", "Relationship");

            dgmembers.Rows.Clear();

            for (int i = 0; i < MembersContainer.Count; i++) {
                DataGridViewRow newRow = new DataGridViewRow();

                newRow.CreateCells(dgmembers);
                member m = (member)(MembersContainer[i]);
                newRow.Cells[0].Value = m.member_id.ToString();
                newRow.Cells[1].Value = m.bmp_id;
                newRow.Cells[2].Value = m.name;
                newRow.Cells[3].Value = m.mi;
                newRow.Cells[4].Value = m.last_name;
                newRow.Cells[5].Value = m.email;
                newRow.Cells[6].Value = m.language;
                newRow.Cells[7].Value = m.marital_status;
                newRow.Cells[8].Value = m.gender;
                newRow.Cells[9].Value = m.dob;
                newRow.Cells[10].Value = m.home_phone_number;
                newRow.Cells[11].Value = m.mobile_phone_number;
                newRow.Cells[12].Value = m.other_phone_number;
                newRow.Cells[13].Value = m.address;
                newRow.Cells[14].Value = m.address2;
                newRow.Cells[15].Value = m.city;
                newRow.Cells[16].Value = m.state;
                newRow.Cells[17].Value = m.postal_code;
                newRow.Cells[18].Value = m.shipping_address;
                newRow.Cells[19].Value = m.shipping_address2;
                newRow.Cells[20].Value = m.shipping_city;
                newRow.Cells[21].Value = m.shipping_state;
                newRow.Cells[22].Value = m.shipping_postal_code;
                newRow.Cells[23].Value = m.use_home_as_shipping_address;
                newRow.Cells[24].Value = m.plan_name;
                newRow.Cells[25].Value = m.plan_type;
                newRow.Cells[26].Value = m.recurring_total;
                newRow.Cells[27].Value = m.start_date;
                newRow.Cells[28].Value = m.end_date;
                newRow.Cells[29].Value = m.number_members;
                newRow.Cells[30].Value = m.agencyID;
                newRow.Cells[31].Value = m.branchID;
                newRow.Cells[32].Value = m.recurrency;
                newRow.Cells[33].Value = m.cc_info;
                newRow.Cells[34].Value = m.cc_type;
                newRow.Cells[35].Value = m.cc_expiration_date;
                newRow.Cells[36].Value = m.cc_auto_pay;
                newRow.Cells[37].Value = m.dateadded;
                newRow.Cells[38].Value = m.policy_holder;
                newRow.Cells[39].Value = m.relationship;

                dgmembers.Rows.Add(newRow);
            }
        }

        private void ShowMembers_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.ShowMembers = null;
        }

        private void dgmembers_CellValueChanged(object sender, DataGridViewCellEventArgs e) {

            string mid = dgmembers.Rows[e.RowIndex].Cells[0].Value.ToString();
            string mbmp_id = dgmembers.Rows[e.RowIndex].Cells[1].Value.ToString();
            string mname = dgmembers.Rows[e.RowIndex].Cells[2].Value.ToString();
            string mmi = dgmembers.Rows[e.RowIndex].Cells[3].Value.ToString();
            string mlastname = dgmembers.Rows[e.RowIndex].Cells[4].Value.ToString();
            string memail = dgmembers.Rows[e.RowIndex].Cells[5].Value.ToString();
            string mlanguage = dgmembers.Rows[e.RowIndex].Cells[6].Value.ToString();
            string mmarital_status = dgmembers.Rows[e.RowIndex].Cells[7].Value.ToString();
            string mgender = dgmembers.Rows[e.RowIndex].Cells[8].Value.ToString();
            int mdob = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[9].Value.ToString());
            string mhome_ph = dgmembers.Rows[e.RowIndex].Cells[10].Value.ToString();
            string mmobile_ph = dgmembers.Rows[e.RowIndex].Cells[11].Value.ToString();
            string mother_ph = dgmembers.Rows[e.RowIndex].Cells[12].Value.ToString();
            string maddress = dgmembers.Rows[e.RowIndex].Cells[13].Value.ToString();
            string maddress2 = dgmembers.Rows[e.RowIndex].Cells[14].Value.ToString();
            string mcity = dgmembers.Rows[e.RowIndex].Cells[15].Value.ToString();
            string mstate = dgmembers.Rows[e.RowIndex].Cells[16].Value.ToString();
            string mpostal_code = dgmembers.Rows[e.RowIndex].Cells[17].Value.ToString();
            string mshipping_address = dgmembers.Rows[e.RowIndex].Cells[18].Value.ToString();
            string mshipping_address2 = dgmembers.Rows[e.RowIndex].Cells[19].Value.ToString();
            string mshipping_city = dgmembers.Rows[e.RowIndex].Cells[20].Value.ToString();
            string mshipping_state = dgmembers.Rows[e.RowIndex].Cells[21].Value.ToString();
            string mshipping_pcode = dgmembers.Rows[e.RowIndex].Cells[22].Value.ToString();
            short muse_home = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[23].Value.ToString());
            string mptype = dgmembers.Rows[e.RowIndex].Cells[24].Value.ToString();
            string mpname = dgmembers.Rows[e.RowIndex].Cells[25].Value.ToString();
            float mrec_total = float.Parse(dgmembers.Rows[e.RowIndex].Cells[26].Value.ToString());
            int mstart_d = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[27].Value.ToString());
            int mend_d = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[28].Value.ToString());
            short mnum = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[29].Value.ToString());
            string magcy_id = dgmembers.Rows[e.RowIndex].Cells[30].Value.ToString();
            string mbranch_id = dgmembers.Rows[e.RowIndex].Cells[31].Value.ToString();
            short mrec = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[32].Value.ToString());
            string mcc_info = dgmembers.Rows[e.RowIndex].Cells[33].Value.ToString();
            string mcc_type = dgmembers.Rows[e.RowIndex].Cells[34].Value.ToString();
            string mcc_exp_date = dgmembers.Rows[e.RowIndex].Cells[35].Value.ToString();
            short mcc_auto = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[36].Value.ToString());
            int mdateadded = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[37].Value.ToString());
            string mpol_holder = dgmembers.Rows[e.RowIndex].Cells[38].Value.ToString();
            string mrelation = dgmembers.Rows[e.RowIndex].Cells[39].Value.ToString();

            member tempM = new member(mid, mbmp_id, mname, mmi, mlastname, memail, mlanguage, mmarital_status, mgender, mdob, mhome_ph, mmobile_ph, mother_ph, maddress, maddress2, mcity, mstate, mpostal_code,
                    mshipping_address, mshipping_address2, mshipping_city, mshipping_state, mshipping_pcode, muse_home, mptype, mpname, mrec_total, mstart_d, mend_d, mnum, magcy_id, mbranch_id, mrec, mcc_info, mcc_type,
                    mcc_exp_date, mcc_auto, mdateadded, mpol_holder, mrelation);

            ToBeUpdated.Add(tempM);
            btCancel.Enabled = true;
            btSave.Enabled = true;
        }

        bool UpdateMember(member m) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                string strQuery = "update members set bmp_id='" + m.bmp_id + "',name='" + m.name + "',mi='" + m.mi + "',last_name='" + m.last_name + "',email='" + m.email + "',language='" + m.language + "',marital_status='" + m.marital_status + "',gender='" + m.gender + "',dob=" + m.dob.ToString()
                    + ",home_phone_number='" + m.home_phone_number + "',mobile_phone_number='" + m.mobile_phone_number + "',other_phone_number='" + m.other_phone_number + "',address='" + m.address + "',address2='" + m.address2 +
                    "',city='" + m.city + "',state='" + m.state + "',postal_code='" + m.postal_code + "',shipping_address='" + m.shipping_address + "',shipping_address2='" + m.shipping_address2 + "',shipping_city='" +
                    m.shipping_city + "',shipping_state='" + m.shipping_state + "',shipping_postal_code='" + m.shipping_postal_code + "',use_home_as_shipping_address=" + m.use_home_as_shipping_address + ",plan_name='" +
                    m.plan_name + "',plan_type='" + m.plan_type + "',recurring_total=" + m.recurring_total + ",start_date=" + m.start_date + ",end_date=" + m.end_date + ",number_members=" + m.number_members + ",agency_id='" +
                    m.agencyID + "',branch_id='" + m.branchID + "',recurrency=" + m.recurrency.ToString() + ",cc_info='" + m.cc_info + "',cc_type='" + m.cc_type + "',cc_expiration_date='" + m.cc_expiration_date + "',cc_auto_pay=" + m.cc_auto_pay.ToString() +
                    ", dateadded=" + m.dateadded.ToString() + ",policy_holder='" + m.policy_holder  + "',relationship='"+ m.relationship + "' where member_id = " + m.member_id;

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

        private void btCancel_Click(object sender, EventArgs e) {
            ToBeUpdated.Clear();
            Form1.ShowMembers = null;
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e) {
            if (ToBeUpdated.Count > 0) {
                for (int i = 0; i < ToBeUpdated.Count; i++) {
                    bool temp_res = UpdateMember((member)ToBeUpdated[i]);
                    if (!temp_res) {
                        MessageBox.Show("There was an error trying to update the members. Please try again later or contact Support if the issue persists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ToBeUpdated.Clear();
                        //Form1.ShowAgencies = null;
                        //this.Close();
                        break;
                    } else {
                        MessageBox.Show("Member: " + ((member)ToBeUpdated[i]).ToString() + " was updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1.ShowMembers = null;
                        this.Close();
                    }
                }
            } else {
                Form1.ShowMembers = null;
                this.Close();
            }
        }

        private void btDelete_Click(object sender, EventArgs e) {
            int m_id = Int32.Parse(dgmembers.Rows[dgmembers.SelectedRows[0].Index].Cells[0].Value.ToString());

            string message = "You are about to delete a member and this action can not be undone. Do you want to continue?";
            string title = "Delete Member Confirmation";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.No) {
                //this.Close();
            } else {
                if (dgmembers.SelectedRows.Count > 0) {
                    if (DeleteMemberFromDB(m_id))
                        dgmembers.Rows.RemoveAt(dgmembers.SelectedRows[0].Index);
                    else
                        MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool DeleteMemberFromDB(int id) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {

                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO members_deleted SELECT * FROM members WHERE member_id='" + id.ToString() + "'", conn);
                ret = cmd.ExecuteNonQuery();
                if (ret > 0) {
                    cmd = new MySqlCommand("delete from members where member_id = " + id.ToString(), conn);                                    
                    ret = cmd.ExecuteNonQuery();
                    res = (ret > 0) ? true : false;
                    if (res)//reseting auto_increment id
                    {
                        cmd = new MySqlCommand("SELECT MAX(member_id) FROM members",conn);
                        int num = -1;
                        num = Convert.ToInt32(cmd.ExecuteScalar());
                        if (num >= 0)
                        {
                            num++;
                            cmd = new MySqlCommand("ALTER TABLE members AUTO_INCREMENT=" + num.ToString(),conn);
                            ret = cmd.ExecuteNonQuery();
                        }
                    }
                }
                conn.Close();
            } catch (Exception e) {
                //MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

            return res;
        }

    }
}
