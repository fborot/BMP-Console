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

using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace BMP_Console {
    public partial class ShowMembers : Form {
        public ShowMembers() {
            InitializeComponent();
        }

        public Notes MNotes = null;
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
            if (!DipIntoDB())
            {
                MessageBox.Show("An error ocurred!. The record could not be found, please contact Support", "Error searching for user", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowMembers_FormClosing(object sender, FormClosingEventArgs e) {
            if (ToBeUpdated.Count > 0) {
                MessageBox.Show("There were some pending changes. Please notice highlighted rows.", "Closing form", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ToBeUpdated.Clear();             
            }
            Form1.ShowMembers = null;
        }

        private void dgmembers_CellValueChanged(object sender, DataGridViewCellEventArgs e) {

            string mid = dgmembers.Rows[e.RowIndex].Cells[0].Value.ToString();
            string mbmp_id = dgmembers.Rows[e.RowIndex].Cells[1].Value.ToString();
            string mname = dgmembers.Rows[e.RowIndex].Cells[2].Value.ToString();
            string mmi = dgmembers.Rows[e.RowIndex].Cells[3].Value.ToString();
            string mlastname = dgmembers.Rows[e.RowIndex].Cells[4].Value.ToString();
            string mpol_holder = dgmembers.Rows[e.RowIndex].Cells[5].Value.ToString();
            string mrelation = dgmembers.Rows[e.RowIndex].Cells[6].Value.ToString();
            int mdob = FormatDoBToInt(dgmembers.Rows[e.RowIndex].Cells[7].Value.ToString());
            string mhome_ph = dgmembers.Rows[e.RowIndex].Cells[8].Value.ToString();
            string mmobile_ph = dgmembers.Rows[e.RowIndex].Cells[9].Value.ToString();
            string memail = dgmembers.Rows[e.RowIndex].Cells[10].Value.ToString();
            int mstart_d = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[11].Value.ToString());
            int mactive = dgmembers.Rows[e.RowIndex].Cells[12].Value.ToString() == "Active" ? 1 : 0;
            short mrec = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[33].Value.ToString());
            string mlanguage = dgmembers.Rows[e.RowIndex].Cells[13].Value.ToString();
            string mmarital_status = dgmembers.Rows[e.RowIndex].Cells[14].Value.ToString();
            string mgender = dgmembers.Rows[e.RowIndex].Cells[15].Value.ToString();   
            string mother_ph = dgmembers.Rows[e.RowIndex].Cells[16].Value.ToString();
            string maddress = dgmembers.Rows[e.RowIndex].Cells[17].Value.ToString();
            string maddress2 = dgmembers.Rows[e.RowIndex].Cells[18].Value.ToString();
            string mcity = dgmembers.Rows[e.RowIndex].Cells[19].Value.ToString();
            string mstate = dgmembers.Rows[e.RowIndex].Cells[20].Value.ToString();

            string mpostal_code = dgmembers.Rows[e.RowIndex].Cells[21].Value.ToString();
            string mshipping_address = dgmembers.Rows[e.RowIndex].Cells[22].Value.ToString();
            string mshipping_address2 = dgmembers.Rows[e.RowIndex].Cells[23].Value.ToString();
            string mshipping_city = dgmembers.Rows[e.RowIndex].Cells[24].Value.ToString();
            string mshipping_state = dgmembers.Rows[e.RowIndex].Cells[25].Value.ToString();
            string mshipping_pcode = dgmembers.Rows[e.RowIndex].Cells[26].Value.ToString();
            short muse_home = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[27].Value.ToString());
            string mpname = dgmembers.Rows[e.RowIndex].Cells[28].Value.ToString();
            string mptype = dgmembers.Rows[e.RowIndex].Cells[29].Value.ToString();            
            float mrec_total = float.Parse(dgmembers.Rows[e.RowIndex].Cells[30].Value.ToString());
            
            int mend_d = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[31].Value.ToString());
            short mnum = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[32].Value.ToString());

            string p_status = dgmembers.Rows[e.RowIndex].Cells[34].Value.ToString();
            string mpay_status = "";
            if (p_status.ToLower() == "current" || p_status.ToLower() == "overdue")
                mpay_status = dgmembers.Rows[e.RowIndex].Cells[34].Value.ToString();
            else {
                MessageBox.Show("Invalid value of: [" + p_status + "] for Payment Status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string magcy_id = dgmembers.Rows[e.RowIndex].Cells[35].Value.ToString();
            string mbranch_id = dgmembers.Rows[e.RowIndex].Cells[36].Value.ToString();            
            string mcc_info = dgmembers.Rows[e.RowIndex].Cells[37].Value.ToString();
            string mcc_type = dgmembers.Rows[e.RowIndex].Cells[38].Value.ToString();
            string mcc_exp_date = dgmembers.Rows[e.RowIndex].Cells[39].Value.ToString();
            short mcc_auto = Int16.Parse(dgmembers.Rows[e.RowIndex].Cells[40].Value.ToString());
            int mdateadded = Int32.Parse(dgmembers.Rows[e.RowIndex].Cells[41].Value.ToString());
            
            string mpbmp_id = dgmembers.Rows[e.RowIndex].Cells[42].Value.ToString();
            //string mnotes = dgmembers.Rows[e.RowIndex].Cells[43].Value.ToString();

            member tempM = new member(mid, mbmp_id, mname, mmi, mlastname, memail, mlanguage, mmarital_status, mgender, mdob, mhome_ph, mmobile_ph, mother_ph, maddress, maddress2, mcity, mstate, mpostal_code,
                    mshipping_address, mshipping_address2, mshipping_city, mshipping_state, mshipping_pcode, muse_home, mptype, mpname, mrec_total, mstart_d, mend_d, mnum, magcy_id, mbranch_id, mrec, mcc_info, mcc_type,
                    mcc_exp_date, mcc_auto, mdateadded, mpol_holder, mrelation, mactive, mpbmp_id, mpay_status);

            ToBeUpdated.Add(tempM);
            btCancel.Enabled = true;
            btSave.Enabled = true;

            dgmembers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
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
                    ", dateadded=" + m.dateadded.ToString() + ",policy_holder='" + m.policy_holder  + "',relationship='" + m.relationship + "',active=" + m.active + ",parent_bmp_id='" + m.parent_bmp_id + "',payment_status='" + m.payment_status + "' where member_id = " + m.member_id;

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
            string t_mlist = "";

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
                        //MessageBox.Show("Member: " + ((member)ToBeUpdated[i]).ToString() + " was updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (!t_mlist.Contains(((member)ToBeUpdated[i]).bmp_id))
                            t_mlist += ((member)ToBeUpdated[i]).bmp_id + Environment.NewLine;

                        //Form1.ShowMembers = null;
                        //this.Close();
                        //DipIntoDB();
                        //btSave.Enabled = false;
                        //ToBeUpdated.Clear();
                    }
                }
                if(t_mlist != "") {
                    t_mlist += Environment.NewLine;
                    MessageBox.Show("Member[s]: " + Environment.NewLine + t_mlist + " were updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } else {
                Form1.ShowMembers = null;
                this.Close();
            }
            DipIntoDB();
            btSave.Enabled = false;
            ToBeUpdated.Clear();
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
                    cmd = new MySqlCommand("update members_deleted set active = 0 where member_id = " + id.ToString(), conn);
                    ret = cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
                conn.Close();
            } catch (Exception e) {
                //MessageBox.Show("An error ocurred!. The record could not be deleted, please contact Support", "Error deleting user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

            return res;
        }

        private void dgmembers_MouseUp(object sender, MouseEventArgs e) {
           if (e.Button == MouseButtons.Right) {
                //int rIndex = dgmembers.HitTest(e.X, e.Y).RowIndex;
                int rIndex = Int32.Parse(dgmembers.Rows[dgmembers.SelectedRows[0].Index].Cells[0].Value.ToString());
                //MessageBox.Show("MemberID " + rIndex + " has been selected", "Hey");
                contextMenuStrip1.Show(dgmembers, new Point(e.X, e.Y));
            }
            
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            if (!DipIntoDB())
            {
                MessageBox.Show("An error ocurred!. The record could not be found, please contact Support", "Error searching for user", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool DipIntoDB()
        {
            bool res = false;

            string strTemp1 = string.Empty;
            string strTemp2 = string.Empty;
            string strTemp3 = string.Empty;

            string strSelect = "select * from members";
            string strWhere = " where";
            string strOrderBy = " order by member_id";

            if (!PingDB())
            {
                MessageBox.Show("Can not connect to the database", "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();


                if (tbMID.Text.Length > 0 || tbLN.Text.Length > 0 || tbPN.Text.Length > 0)
                {
                    strSelect += strWhere;
                    int b = strSelect.Length;

                    if (tbMID.Text.Length > 0)
                    {
                        if (strSelect.Length == 27)
                            strTemp1 = " bmp_id='" + tbMID.Text + "'";
                        else
                            strTemp1 = " and bmp_id='" + tbMID.Text + "'";

                        strSelect += strTemp1;
                    }
                    if (tbLN.Text.Length > 0)
                    {
                        if (strSelect.Length == 27)
                            strTemp2 = " last_name = '" + tbLN.Text + "'";
                        else
                            strTemp2 = " and last_name = '" + tbLN.Text + "'";

                        strSelect += strTemp2;
                    }
                    if (tbPN.Text.Length > 0)
                    {
                        if (strSelect.Length == 27)
                            strTemp3 = " (home_phone_number = '" + tbPN.Text + "' or mobile_phone_number='" + tbPN.Text + "')";
                        else
                            strTemp3 = " and (home_phone_number = '" + tbPN.Text + "' or mobile_phone_number='" + tbPN.Text + "')";

                        strSelect += strTemp3;
                    }
                }
                strSelect += strOrderBy;

                MySqlCommand cmd = new MySqlCommand(strSelect, conn);
                MySqlDataReader ret = cmd.ExecuteReader();

                MembersContainer.Clear();
                while (ret.Read())
                {

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
                    string shipping_pcode = ret.GetString(22);
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
                    int activ = ret.GetInt32(40);
                    string p_bmp_id = ret.GetString(41);
                    string payment_status = ret.GetString(42);
                    //string notes = ret.GetString(43);

                    MembersContainer.Add(new member(id.ToString(), bmp_id, name, mi, lastname, email, language, marital_status, gender, dob, home_ph, mobile_ph, other_ph, address, address2, city_, state, postal_code,
                        shipping_address, shipping_address2, shipping_city, shipping_state, shipping_pcode, use_home, ptype, pname, rec_total, start_d, end_d, num, agcy_id, branch_id, recurrency, cc_info, cc_type, cc_exp_date, cc_auto, dateadded, policy_holder, relationship, activ, p_bmp_id, payment_status));

                }
                ret.Close();
                cmd.Dispose();
                conn.Close();
                
                res = true;
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error searching for user", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }
            
            dgmembers.Columns.Clear();

            dgmembers.Columns.Add("Member Id", "Member ID");                                //00
            dgmembers.Columns.Add("BMP Member Id", "BMP Member ID");                        //01
            dgmembers.Columns.Add("Member Name", "Member Name");                            //02
            dgmembers.Columns.Add("Middle Initial", "Middle Initial");                      //03
            dgmembers.Columns.Add("Last Name", "Last Name");                                //04
            dgmembers.Columns.Add("Policy Holder", "Policy Holder");                        //05
            dgmembers.Columns.Add("Relationship", "Relationship");                          //06
            dgmembers.Columns.Add("DoB", "Date of Birth");                                  //07
            dgmembers.Columns.Add("Home Phone Name", "Home Phone Number");                  //08
            dgmembers.Columns.Add("Mobile Phone Number", "Mobile Phone Number");            //09
            dgmembers.Columns.Add("Email", "Email");                                        //10
            dgmembers.Columns.Add("Effective Date", "Effective Date");                      //11
            dgmembers.Columns.Add("Member Status", "Member Status");                        //12
            dgmembers.Columns.Add("Language", "Language");                                  //13
            dgmembers.Columns.Add("Marital Status", "Marital Status");                      //14
            dgmembers.Columns.Add("Gender", "Gender");                                      //15
            dgmembers.Columns.Add("Other Phone NUmber", "Other Phone Number");              //16
            dgmembers.Columns.Add("Address", "Address");                                    //17
            dgmembers.Columns.Add("Address2", "Address2");                                  //18
            dgmembers.Columns.Add("City", "City");                                          //19
            dgmembers.Columns.Add("State", "State");                                        //20
            dgmembers.Columns.Add("Postal Code", "Postal Code");                            //21
            dgmembers.Columns.Add("Shipping Address", "Shipping Address");                  //22
            dgmembers.Columns.Add("Shipping Address2", "Shipping Address2");                //23
            dgmembers.Columns.Add("Shipping City", "Shipping City");                        //24
            dgmembers.Columns.Add("Shipping State", "Shipping State");                      //25
            dgmembers.Columns.Add("Shipping Postal Code", "Shipping Postal Code");          //26
            dgmembers.Columns.Add("Use Home Address", "Use Home Address");                  //27
            dgmembers.Columns.Add("Plan Name", "Plan Name");                                //28
            dgmembers.Columns.Add("Plan Type", "Plan Type");                                //29
            dgmembers.Columns.Add("Recurring Charges", "Recurring Charges");                //30            
            dgmembers.Columns.Add("End Date", "End Date");                                  //31
            dgmembers.Columns.Add("Num of Members", "Num of Members");                      //32
            dgmembers.Columns.Add("Recurrency", "Recurrency");                              //33
            dgmembers.Columns.Add("Payment Status", "Payment Status");                      //34
            dgmembers.Columns.Add("Agency", "Agency");                                      //35
            dgmembers.Columns.Add("Branch", "Branch");                                      //36            
            dgmembers.Columns.Add("CC Info", "CC Info");                                    //37
            dgmembers.Columns.Add("CC Type", "CC Type");                                    //38
            dgmembers.Columns.Add("CC Exp Date", "CC Exp Date");                            //39
            dgmembers.Columns.Add("CC Auto Pay", "CC Auto Pay");                            //40
            dgmembers.Columns.Add("Date Added", "Date Added");                              //41
            dgmembers.Columns.Add("Parent BMP ID", "Parent BMP ID");                        //42
            //dgmembers.Columns.Add("Notes", "Notes");                                        //43

            dgmembers.Rows.Clear();

            dgmembers.Columns[0].Visible = false;

            dgmembers.Columns[13].Visible = false;
            dgmembers.Columns[14].Visible = false;
            dgmembers.Columns[15].Visible = false;
            dgmembers.Columns[16].Visible = false;
            dgmembers.Columns[17].Visible = false;
            dgmembers.Columns[18].Visible = false;
            dgmembers.Columns[19].Visible = false;
            dgmembers.Columns[20].Visible = false;
            dgmembers.Columns[21].Visible = false;
            dgmembers.Columns[22].Visible = false;
            dgmembers.Columns[23].Visible = false;
            dgmembers.Columns[24].Visible = false;
            dgmembers.Columns[25].Visible = false;
            dgmembers.Columns[26].Visible = false;
            dgmembers.Columns[27].Visible = false;
            dgmembers.Columns[28].Visible = false;
            dgmembers.Columns[29].Visible = false;

            dgmembers.Columns[31].Visible = false;
            dgmembers.Columns[32].Visible = false;
            dgmembers.Columns[33].Visible = false;

            dgmembers.Columns[37].Visible = false;
            dgmembers.Columns[38].Visible = false;
            dgmembers.Columns[39].Visible = false;
            dgmembers.Columns[40].Visible = false;
            dgmembers.Columns[41].Visible = false;

            dgmembers.Columns[12].ReadOnly = true;  //member_status is set from the cloud
            dgmembers.Columns[34].ReadOnly = true;  // payment_status is set from the cloud

            for (int i = 0; i < MembersContainer.Count; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();

                newRow.CreateCells(dgmembers);

                member m = (member)(MembersContainer[i]);

                newRow.Cells[0].Value = m.member_id.ToString();
                newRow.Cells[1].Value = m.bmp_id;
                newRow.Cells[2].Value = m.name;
                newRow.Cells[3].Value = m.mi;
                newRow.Cells[4].Value = m.last_name;                
                newRow.Cells[5].Value = m.policy_holder;
                newRow.Cells[6].Value = m.relationship;
                newRow.Cells[7].Value = FormatDoB(m.dob); 
                newRow.Cells[8].Value = m.home_phone_number; 
                newRow.Cells[9].Value = m.mobile_phone_number; 
                newRow.Cells[10].Value = m.email;

                newRow.Cells[11].Value = m.start_date;
                newRow.Cells[12].Value = (m.active == 1)? "Active":"Cancelled" ;
                newRow.Cells[13].Value = m.language;
                newRow.Cells[14].Value = m.marital_status; 
                newRow.Cells[15].Value = m.gender;
                newRow.Cells[16].Value = m.other_phone_number;
                newRow.Cells[17].Value = m.address;
                newRow.Cells[18].Value = m.address2;
                newRow.Cells[19].Value = m.city;
                newRow.Cells[20].Value = m.state;

                newRow.Cells[21].Value = m.postal_code;
                newRow.Cells[22].Value = m.shipping_address;
                newRow.Cells[23].Value = m.shipping_address2;
                newRow.Cells[24].Value = m.shipping_city;
                newRow.Cells[25].Value = m.shipping_state;
                newRow.Cells[26].Value = m.shipping_postal_code;
                newRow.Cells[27].Value = m.use_home_as_shipping_address;
                newRow.Cells[28].Value = m.plan_name;
                newRow.Cells[29].Value = m.plan_type;
                newRow.Cells[30].Value = m.recurring_total;                

                newRow.Cells[31].Value = m.end_date;
                newRow.Cells[32].Value = m.number_members;
                newRow.Cells[33].Value = m.recurrency;
                newRow.Cells[34].Value = m.payment_status;
                newRow.Cells[35].Value = m.agencyID;
                newRow.Cells[36].Value = m.branchID;
                newRow.Cells[37].Value = m.cc_info;
                newRow.Cells[38].Value = m.cc_type;
                newRow.Cells[39].Value = m.cc_expiration_date;
                newRow.Cells[40].Value = m.cc_auto_pay;

                newRow.Cells[41].Value = m.dateadded;
                newRow.Cells[42].Value = m.parent_bmp_id;
                
                //newRow.Cells[43].Value = m.notes;

                dgmembers.Rows.Add(newRow);
            }

            return res;
        }

        private void showTransactionsHistoryToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("show x history", "context menu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void showPaymentStatusToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("show pay status", "context menu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e) {
            bool APIres = false;
            DialogResult res = MessageBox.Show("This action is irreversible. Are you sure you want to Disable this member?", "Disable Member", MessageBoxButtons.YesNo);
            if(res == DialogResult.Yes) {                                
                int m_id = Int32.Parse(dgmembers.Rows[dgmembers.SelectedRows[0].Index].Cells[0].Value.ToString());
                string bmpid = dgmembers.Rows[dgmembers.SelectedRows[0].Index].Cells[1].Value.ToString();                
                string subs = GetSubscriptionFromMmber(bmpid);
                //subs = "7778028";
                APIres = CancelSubscription(Form1.APILoginID, Form1.APITransactionKey, subs);
                if (APIres) {
                    string db_query = string.Empty;
                    db_query = "update members set active = 0 where bmp_id = '" + bmpid + "' OR parent_bmp_id = '" + bmpid + "'";
                    if (GenericUpdateMembersInDB(db_query)) {
                        MessageBox.Show("Disable Member -> " + bmpid + "  was Successful.", "Disable Member", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else {
                        MessageBox.Show("Disable Member -> " + bmpid + " failed.", "Disable Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                }
            } 
            DipIntoDB();
        }

        private bool GenericUpdateMembersInDB(string strQuery) {
            bool res = false;
            int ret = -1;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                
                cmd = new MySqlCommand(strQuery, conn);
                ret = cmd.ExecuteNonQuery();
                res = (ret > 0) ? true : false;
                cmd.Dispose();
                conn.Close();
            } catch (Exception e) {
                MessageBox.Show("An error ocurred!. The record could not be updated, please contact Support", "Error updating user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmd.Dispose();
                conn.Close();
            }
            return res;
        }

        private bool CancelSubscription(String ApiLoginID, String ApiTransactionKey, string subs) {
            bool res = false;
            if (!Form1.bLive)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            else
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            logger.Instance.write("CancelSubscription::SubscriptionID: " + subs);

            //Please change the subscriptionId according to your request
            var request = new ARBCancelSubscriptionRequest { subscriptionId = subs };
            var controller = new ARBCancelSubscriptionController(request);                          // instantiate the controller that will call the service
            controller.Execute();

            ARBCancelSubscriptionResponse response = controller.GetApiResponse();                   // get the response from the service (errors contained if any)

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {
                if (response != null && response.messages.message != null) {
                    if (response.messages.message[0].code == "I00001" || response.messages.message[0].text == "Successful") {                  
                        logger.Instance.write("Success, Subscription Cancelled With RefID : " + response.refId);
                        res = true;
                    }                    
                }
            } else if (response != null) {
                logger.Instance.write("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return res;
        }


        private string GetSubscriptionFromMmber(string bmp_id) {
            string resSubs = "";
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            try {
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                string strQuery = "select an_subscription_id from bmp.subscriptions where bmp_id='" + bmp_id + "'";
                //Console.WriteLine(s);
                MySqlCommand cmd = new MySqlCommand(strQuery, conn);
                MySqlDataReader ret = cmd.ExecuteReader();
                while (ret.Read()) {
                    resSubs = ret.GetString(0);
                }
                conn.Close();
            } catch (Exception e) {
                MessageBox.Show("An error ocurred!. The record could not be updated, please contact Support", "Error updating user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            return resSubs;
        }
        
        private string FormatDoB(int dob) {
            string res = string.Empty;
            string temp = dob.ToString();
            res = temp.Substring(0, 4) + "-" + temp.Substring(4,2) + "-" + temp.Substring(6,2); 

            return res;
        }

        private int FormatDoBToInt(string dob) {
            int res = -1;
            string[] temp = dob.Split('-');
            res = Int32.Parse(temp[0] + temp[1] + temp[2]);
            return res;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            //MessageBox.Show("Edit Notes", "context menu", MessageBoxButtons.OK, MessageBoxIcon.Error);

            int m_id = Int32.Parse(dgmembers.Rows[dgmembers.SelectedRows[0].Index].Cells[0].Value.ToString());
            string bmpid = dgmembers.Rows[dgmembers.SelectedRows[0].Index].Cells[1].Value.ToString();

            if (MNotes == null) {
                MNotes = new Notes();
                //MNotes.MdiParent = this;
                MNotes.StartPosition = FormStartPosition.CenterScreen;
                MNotes.Text = "Notes for member " + bmpid;
                MNotes.Show();

            } else {
                this.MNotes.Dispose();
                Notes MNotes2 = new Notes();
                //MNotes.MdiParent = this;
                MNotes2.StartPosition = FormStartPosition.CenterScreen;
                MNotes2.Text = "Notes for member " + bmpid;
                MNotes2.Show();

                MNotes2.Focus();
            }
        }

        private void dgmembers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (this.dgmembers.Columns[e.ColumnIndex].Name == "Member Status") {
                if (e.Value != null) {
                    // Check for the string "pink" in the cell.
                    string stringValue = (string)e.Value;
                    stringValue = stringValue.ToLower();
                    if ((stringValue.IndexOf("cancelled") > -1)) {
                        //e.CellStyle.BackColor = Color.Red;
                        this.dgmembers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightSalmon;
                        this.dgmembers.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;                        
                    }
                }
            }
        }
    }
}
