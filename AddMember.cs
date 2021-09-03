using System;
using System.Collections;
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
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace BMP_Console {
    public partial class AddMember : Form {

        ArrayList PlansContainer = new ArrayList();
        ArrayList AgenciesContainer = new ArrayList();
        ArrayList BranchesContainer = new ArrayList();
        ArrayList DependentsContainer = new ArrayList();
        bool ResettingForm = false;
       
        public AddMember() {
            InitializeComponent();
        }

        private void AddMember_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.Addmember = null;            
        }

        private void groupBox1_Enter(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            
            string strBMPID = "";
            strBMPID=  CreateMemberID() + "-" + tbMemberID.Text;
            if (strBMPID.Length == 0)
            {
                MessageBox.Show("An error occured creating the Memebr ID", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(ckbUseHome.Enabled == true) {
                tbShAddress.Text = tbAddres.Text;
                tbShAddress2.Text = tbAddress2.Text;
                tbShCity.Text = tbCity.Text;
                tbShPostalCode.Text = tbPostalCode.Text;
                cbShState.SelectedIndex = cbState.SelectedIndex;
            }
            try {
                
                if (!isValidCost(tbRecurringTotal.Text)) {
                    MessageBox.Show("Invalid values for the recurring cost", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!isValidEmail(tbEmail.Text)) {
                    MessageBox.Show("Invalid value for the Email Address", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!isValidPhoneNumber(tbHomePh.Text) ) {
                    MessageBox.Show("Invalid value for the Home Phone Number", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!isValidPhoneNumber(tbMobilePH.Text) ) {
                    //MessageBox.Show("Invalid value for the Mobile Phone Number", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                    tbMobilePH.Text = tbHomePh.Text;
                }
                if ( !isValidPhoneNumber(tbOtherPh.Text)) {
                    //MessageBox.Show("Invalid value for some of the Other Phone Number", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                    tbOtherPh.Text = tbHomePh.Text;
                }

                if (!isValidZipCode(tbPostalCode.Text) || !(isValidZipCode(tbShPostalCode.Text))) {
                    MessageBox.Show("Invalid value for the Zip Code", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!isValidCCNumber(tbCCInfo.Text) || !(isValidCCExpDate(tbCCExpDate.Text))) {
                    MessageBox.Show("Invalid value for the Credit Card", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!(tbANetTID.Text.Length > 0)) {
                    MessageBox.Show("Invalid value for the Authorize.Net Transaction ID", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!(tbANAuthCode.Text.Length > 0)) {
                    MessageBox.Show("Invalid value for the Authorize.Net Authorization Code", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                short NumberMembers = tbNumberMembers.Text.Length > 0 ? Int16.Parse(tbNumberMembers.Text) : (Int16)0;
                float RecurringTotal = tbRecurringTotal.Text.Length > 0 ? float.Parse(tbRecurringTotal.Text) : (float)0;
                if (NumberMembers > 0) {
                    if (!(isALL_LDDoBValid(NumberMembers))) {
                        MessageBox.Show("Invalid value for the Legal Dependents Date of Birth", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }                    
                    if (!CreateDependeList(NumberMembers)) {
                        MessageBox.Show("Invalid values for Legal Dependents", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                }
                int ndob = Int32.Parse(dtDOB.Value.ToString("yyyyMMdd"));
                int nStart = Int32.Parse(dtStart.Value.ToString("yyyyMMdd"));
                int nEnd = Int32.Parse(dtEnd.Value.ToString("yyyyMMdd"));
                int nDAdded = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
                
                member temp_member = new member(tbMemberID.Text,strBMPID, tbName.Text, tbMI.Text, tbLName.Text, tbEmail.Text, cbLanguage.SelectedItem.ToString(), cbMarital.SelectedItem.ToString(), cbGender.SelectedItem.ToString(),ndob, tbHomePh.Text,
                    tbMobilePH.Text, tbOtherPh.Text, tbAddres.Text, tbAddress2.Text, tbCity.Text, cbState.SelectedItem.ToString(), tbPostalCode.Text, tbShAddress.Text, tbShAddress2.Text, tbShCity.Text,
                    cbShState.SelectedItem.ToString(),tbShPostalCode.Text, ckbUseHome.Checked?(short)1:(short)0, cbPlanType.SelectedItem.ToString(), cbPlanName.SelectedItem.ToString(),
                    RecurringTotal, nStart, nEnd, NumberMembers,cbAgencyID.SelectedItem.ToString(), cbBranchID.SelectedItem.ToString(), Int16.Parse(cbRecurrency.SelectedItem.ToString()),tbCCInfo.Text, 
                    cbCCType.SelectedItem.ToString(), tbCCExpDate.Text, ckbCCAuto.Checked?(short)1:(short)0, nDAdded, "Yes", "Self");

                if (temp_member.validate_member_info()) {
                    CreateProfileResponse tempProfile = CreateCustomerProfileFromTransaction(Form1.APILoginID, Form1.APITransactionKey, tbANetTID.Text, temp_member);
                    //CreateProfileResponse tempProfile = new CreateProfileResponse("901406126", "901201859", "903700265", true);
                    if (tempProfile.res == true) {
                        string CCExpDate = string.Empty;
                        CCExpDate = GetCustomerPayProfileDetails(Form1.APILoginID, Form1.APITransactionKey, tempProfile.CustomerProfileID, tempProfile.CustomerPayProfileID);
                        //CCExpDate = "2024-01";
                        if(IsValidANetExpDate(CCExpDate).Length > 0) {
                            short interval = Int16.Parse(cbRecurrency.SelectedItem.ToString());
                            temp_member.cc_expiration_date = IsValidANetExpDate(CCExpDate);
                            string subscriptionID = "";
                            if (CreateSubscriptionFromProfile(Form1.APILoginID, Form1.APITransactionKey, interval, temp_member.bmp_id, tempProfile.CustomerProfileID, tempProfile.CustomerPayProfileID, tempProfile.CustomerShProfileID, tbRecurringTotal.Text, ref subscriptionID)) {
                                if (SaveMemberInDB(temp_member)) {
                                    MessageBox.Show("Member Successfully Created", "Saving member in Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    if (NumberMembers > 0) {
                                        member seed_member = temp_member;
                                        for (int n = 0; n < DependentsContainer.Count; n++) {
                                            seed_member = CreateMemberBasedOnDependent((Dependent)DependentsContainer[n], seed_member);
                                            SaveMemberInDB(seed_member);
                                        }
                                    }
                                    SaveTransactionInDB(temp_member, tbANetTID.Text, tbANAuthCode.Text);
                                    SaveSubscriptionInDB(temp_member, tbANetTID.Text, subscriptionID);
                                    ResetForm();
                                }                                
                            }
                        }                        
                    }
                }

            } catch (Exception excep) {
                MessageBox.Show(excep.ToString());
            }
        }

        private bool SaveMemberInDB(member m) {

            bool res = false;
            try
            {
                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("insert into members (bmp_id, name, mi, last_name, email, language, marital_status, gender ,dob, home_phone_number, mobile_phone_number, other_phone_number, address, address2, city, state," +
                    " postal_code, shipping_address, shipping_address2, shipping_city, shipping_state, shipping_postal_code, use_home_as_shipping_address, plan_name, plan_type, recurring_total, start_date, end_date, number_members, " +
                    "agency_id, branch_id, recurrency, cc_info, cc_type, cc_expiration_date, cc_auto_pay, dateadded, policy_holder, relationship) values (" +
                    "@bmp_id, @name, @mi, @last_name, @email, @language, @marital_status, @gender, @dob, @home_phone_number, @mobile_phone_number, @other_phone_number, @address, @address2, @city, @state," +
                    " @postal_code, @shipping_address, @shipping_address2, @shipping_city, @shipping_state, @shipping_postal_code, @use_home_as_shipping_address, @plan_name, @plan_type, @recurring_total, @start_date, @end_date," +
                    " @number_members, @agency_id, @branch_id, @recurrency, @cc_info, @cc_type, @cc_expiration_date, @cc_auto_pay, @dateadded, @policy_holder, @relationship)", conn);

                cmd.Parameters.Add("@bmp_id", MySqlDbType.VarChar, m.bmp_id.Length).Value = m.bmp_id;
                cmd.Parameters.Add("@name", MySqlDbType.VarChar, m.name.Length).Value = m.name;
                cmd.Parameters.Add("@mi", MySqlDbType.VarChar, m.mi.Length).Value = m.mi;
                cmd.Parameters.Add("@last_name", MySqlDbType.VarChar, m.last_name.Length).Value = m.last_name;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar, m.email.Length).Value = m.email;
                cmd.Parameters.Add("@language", MySqlDbType.VarChar, m.language.Length).Value = m.language;
                cmd.Parameters.Add("@marital_status", MySqlDbType.VarChar, m.marital_status.Length).Value = m.marital_status;
                cmd.Parameters.Add("@gender", MySqlDbType.VarChar, m.gender.Length).Value = m.gender;
                cmd.Parameters.Add("@dob", MySqlDbType.Int32).Value = m.dob;
                cmd.Parameters.Add("@home_phone_number", MySqlDbType.VarChar, m.home_phone_number.Length).Value = m.home_phone_number;
                cmd.Parameters.Add("@mobile_phone_number", MySqlDbType.VarChar, m.mobile_phone_number.Length).Value = m.mobile_phone_number;
                cmd.Parameters.Add("@other_phone_number", MySqlDbType.VarChar, m.other_phone_number.Length).Value = m.other_phone_number;
                cmd.Parameters.Add("@address", MySqlDbType.VarChar, m.address.Length).Value = m.address;
                cmd.Parameters.Add("@address2", MySqlDbType.VarChar, m.address2.Length).Value = m.address2;
                cmd.Parameters.Add("@city", MySqlDbType.VarChar, m.city.Length).Value = m.city;
                cmd.Parameters.Add("@state", MySqlDbType.VarChar, m.state.Length).Value = m.state;
                cmd.Parameters.Add("@postal_code", MySqlDbType.VarChar, m.postal_code.Length).Value = m.postal_code;
                cmd.Parameters.Add("@shipping_address", MySqlDbType.VarChar, m.shipping_address.Length).Value = m.shipping_address;
                cmd.Parameters.Add("@shipping_address2", MySqlDbType.VarChar, m.shipping_address2.Length).Value = m.shipping_address2;
                cmd.Parameters.Add("@shipping_city", MySqlDbType.VarChar, m.shipping_city.Length).Value = m.shipping_city;
                cmd.Parameters.Add("@shipping_state", MySqlDbType.VarChar, m.shipping_state.Length).Value = m.shipping_state;
                cmd.Parameters.Add("@shipping_postal_code", MySqlDbType.VarChar, m.shipping_postal_code.Length).Value = m.shipping_postal_code;
                cmd.Parameters.Add("@use_home_as_shipping_address", MySqlDbType.Int16).Value = m.use_home_as_shipping_address;
                cmd.Parameters.Add("@plan_name", MySqlDbType.VarChar, m.plan_name.Length).Value = m.plan_name;
                cmd.Parameters.Add("@plan_type", MySqlDbType.VarChar, m.plan_type.Length).Value = m.plan_type;
                cmd.Parameters.Add("@recurring_total", MySqlDbType.Float).Value = m.recurring_total;
                cmd.Parameters.Add("@start_date", MySqlDbType.Int32).Value = m.start_date;
                cmd.Parameters.Add("@end_date", MySqlDbType.Int32).Value = m.end_date;
                cmd.Parameters.Add("@number_members", MySqlDbType.Int16).Value = m.number_members;
                cmd.Parameters.Add("@agency_id", MySqlDbType.VarChar, m.agencyID.Length).Value = m.agencyID;
                cmd.Parameters.Add("@branch_id", MySqlDbType.VarChar, m.branchID.Length).Value = m.branchID;
                cmd.Parameters.Add("@recurrency", MySqlDbType.Int16).Value = m.recurrency;
                cmd.Parameters.Add("@cc_info", MySqlDbType.VarChar, m.cc_info.Length).Value = m.cc_info;
                cmd.Parameters.Add("@cc_type", MySqlDbType.VarChar, m.cc_type.Length).Value = m.cc_type;
                cmd.Parameters.Add("@cc_expiration_date", MySqlDbType.VarChar, m.cc_expiration_date.Length).Value = m.cc_expiration_date;
                cmd.Parameters.Add("@cc_auto_pay", MySqlDbType.Int16).Value = m.cc_auto_pay;
                cmd.Parameters.Add("@dateadded", MySqlDbType.Int32).Value = m.dateadded;
                cmd.Parameters.Add("@policy_holder", MySqlDbType.VarChar, m.policy_holder.Length).Value = m.policy_holder;
                cmd.Parameters.Add("@relationship", MySqlDbType.VarChar, m.relationship.Length).Value = m.relationship;

                int result = cmd.ExecuteNonQuery();
                conn.Close();
                res = (result == 1) ? true : false;
            } catch (Exception e)
            {
                MessageBox.Show("Error Saving Member in the Database.", "Saving member in Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return res;
        }

        private bool SaveTransactionInDB(member m, string t_id, string a_code)
        {
            bool res = false;
            try
            {
                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("insert into transactions (trans_id, auth_code, bmp_id, recurring_total, agency_id,branch_id, recurrency, dateadded ) values (" +
                    "@trans_id, @auth_code, @bmp_id, @recurring_total, @agency_id, @branch_id, @recurrency, @dateadded)", conn);

                cmd.Parameters.Add("@trans_id", MySqlDbType.VarChar, t_id.Length).Value = t_id;
                cmd.Parameters.Add("@auth_code", MySqlDbType.VarChar, a_code.Length).Value = a_code;
                cmd.Parameters.Add("@bmp_id", MySqlDbType.VarChar, m.bmp_id.Length).Value = m.bmp_id;
                cmd.Parameters.Add("@recurring_total", MySqlDbType.Float).Value = m.recurring_total;
                cmd.Parameters.Add("@agency_id", MySqlDbType.VarChar, m.agencyID.Length).Value = m.agencyID;
                cmd.Parameters.Add("@branch_id", MySqlDbType.VarChar, m.branchID.Length).Value = m.branchID;
                cmd.Parameters.Add("@recurrency", MySqlDbType.Int16, m.recurrency).Value = m.recurrency;
                cmd.Parameters.Add("@dateadded", MySqlDbType.Int32).Value = m.dateadded;

                int result = cmd.ExecuteNonQuery();
                conn.Close();
                res = (result == 1) ? true : false;
            } catch (Exception e)
            {
                MessageBox.Show("Error Saving Transaction in the Database.", "Saving member in Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return res;
        }

        private bool SaveSubscriptionInDB(member m, string t_id, string subscription) {
            bool res = false;
            try {
                MySqlConnection conn = null;
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("insert into subscriptions (an_subscription_id, an_orig_trans_id, bmp_id, recurring_total, agency_id,branch_id, recurrency, dateadded ) values (" +
                    "@an_subscription_id, @an_original_trans_id, @bmp_id, @recurring_total, @agency_id, @branch_id, @recurrency, @dateadded)", conn);

                cmd.Parameters.Add("@an_subscription_id", MySqlDbType.VarChar, subscription.Length).Value = subscription;
                cmd.Parameters.Add("@an_original_trans_id", MySqlDbType.VarChar, t_id.Length).Value = t_id;
                cmd.Parameters.Add("@bmp_id", MySqlDbType.VarChar, m.bmp_id.Length).Value = m.bmp_id;
                cmd.Parameters.Add("@recurring_total", MySqlDbType.Float).Value = m.recurring_total;
                cmd.Parameters.Add("@agency_id", MySqlDbType.VarChar, m.agencyID.Length).Value = m.agencyID;
                cmd.Parameters.Add("@branch_id", MySqlDbType.VarChar, m.branchID.Length).Value = m.branchID;
                cmd.Parameters.Add("@recurrency", MySqlDbType.Int16).Value = m.recurrency;
                cmd.Parameters.Add("@dateadded", MySqlDbType.Int32).Value = m.dateadded;

                int result = cmd.ExecuteNonQuery();
                conn.Close();
                res = (result == 1) ? true : false;
            } catch (Exception e) {
                MessageBox.Show("Error Saving Subscription in the Database.", "Saving member in Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return res;
        }

        private bool ChargeMember(member m) {
            return true;
        }

        private void AddMember_Load(object sender, EventArgs e) {

            LoadInitialInfoFromDB();

            this.gbLD.Enabled = false;


            this.Size = new Size(1300, 680);
            cbLanguage.Items.Add("English");
            cbLanguage.Items.Add("Spanish");
            cbLanguage.Items.Add("Creole");
            cbLanguage.Items.Add("Other");
            //cbLanguage.SelectedValue = cbLanguage.Items[0];

            cbMarital.Items.Add("Single");
            cbMarital.Items.Add("Married");
            //cbMarital.SelectedValue = cbMarital.Items[0];

            int count = PlansContainer.Count;
            for (int i = 0; i < count; i++) {
                plan p = (plan)PlansContainer[i];
                cbPlanName.Items.Add(p.p_name);
            }
            count = 0;
            count = AgenciesContainer.Count;
            for(int i = 0; i < count; i++) {
                agency a = (agency)AgenciesContainer[i];
                cbAgencyID.Items.Add(a.agency_name);
            }

            cbState.Items.Add("Alabama");
            cbState.Items.Add("Alaska");
            cbState.Items.Add("Arizona");
            cbState.Items.Add("Arkansas");
            cbState.Items.Add("California");
            cbState.Items.Add("Colorado");
            cbState.Items.Add("Connecticut");
            cbState.Items.Add("Delaware");
            cbState.Items.Add("Florida");
            cbState.Items.Add("Georgia");
            cbState.Items.Add("Hawaii");
            cbState.Items.Add("Idaho");
            cbState.Items.Add("Illinois");
            cbState.Items.Add("Indiana");
            cbState.Items.Add("Iowa");
            cbState.Items.Add("Kansas");
            cbState.Items.Add("Kentucky");
            cbState.Items.Add("Louisiana");
            cbState.Items.Add("Maine");
            cbState.Items.Add("Maryland");
            cbState.Items.Add("Massachusetts");
            cbState.Items.Add("Michigan");
            cbState.Items.Add("Minnesota");
            cbState.Items.Add("Mississippi");
            cbState.Items.Add("Missouri");
            cbState.Items.Add("Montana");
            cbState.Items.Add("Nebraska");
            cbState.Items.Add("Nevada");
            cbState.Items.Add("New Hampshire");
            cbState.Items.Add("New Jersey");
            cbState.Items.Add("New Mexico");
            cbState.Items.Add("New York");
            cbState.Items.Add("North Carolina");
            cbState.Items.Add("North Dakota");
            cbState.Items.Add("Ohio");
            cbState.Items.Add("Oklahoma");
            cbState.Items.Add("Oregon");
            cbState.Items.Add("Pennsylvania");
            cbState.Items.Add("Rhode Island");
            cbState.Items.Add("South Carolina");
            cbState.Items.Add("South Dakota");
            cbState.Items.Add("Tennessee");
            cbState.Items.Add("Texas");
            cbState.Items.Add("Utah");
            cbState.Items.Add("Vermont");
            cbState.Items.Add("Virginia");
            cbState.Items.Add("Washington");
            cbState.Items.Add("West Virginia");
            cbState.Items.Add("Wisconsin");
            cbState.Items.Add("Wyoming");

            cbShState.Items.Add("Alabama");
            cbShState.Items.Add("Alaska");
            cbShState.Items.Add("Arizona");
            cbShState.Items.Add("Arkansas");
            cbShState.Items.Add("California");
            cbShState.Items.Add("Colorado");
            cbShState.Items.Add("Connecticut");
            cbShState.Items.Add("Delaware");
            cbShState.Items.Add("Florida");
            cbShState.Items.Add("Georgia");
            cbShState.Items.Add("Hawaii");
            cbShState.Items.Add("Idaho");
            cbShState.Items.Add("Illinois");
            cbShState.Items.Add("Indiana");
            cbShState.Items.Add("Iowa");
            cbShState.Items.Add("Kansas");
            cbShState.Items.Add("Kentucky");
            cbShState.Items.Add("Louisiana");
            cbShState.Items.Add("Maine");
            cbShState.Items.Add("Maryland");
            cbShState.Items.Add("Massachusetts");
            cbShState.Items.Add("Michigan");
            cbShState.Items.Add("Minnesota");
            cbShState.Items.Add("Mississippi");
            cbShState.Items.Add("Missouri");
            cbShState.Items.Add("Montana");
            cbShState.Items.Add("Nebraska");
            cbShState.Items.Add("Nevada");
            cbShState.Items.Add("New Hampshire");
            cbShState.Items.Add("New Jersey");
            cbShState.Items.Add("New Mexico");
            cbShState.Items.Add("New York");
            cbShState.Items.Add("North Carolina");
            cbShState.Items.Add("North Dakota");
            cbShState.Items.Add("Ohio");
            cbShState.Items.Add("Oklahoma");
            cbShState.Items.Add("Oregon");
            cbShState.Items.Add("Pennsylvania");
            cbShState.Items.Add("Rhode Island");
            cbShState.Items.Add("South Carolina");
            cbShState.Items.Add("South Dakota");
            cbShState.Items.Add("Tennessee");
            cbShState.Items.Add("Texas");
            cbShState.Items.Add("Utah");
            cbShState.Items.Add("Vermont");
            cbShState.Items.Add("Virginia");
            cbShState.Items.Add("Washington");
            cbShState.Items.Add("West Virginia");
            cbShState.Items.Add("Wisconsin");
            cbShState.Items.Add("Wyoming");
            //cbShState.SelectedValue = cbShState.Items[0];

            cbCCType.Items.Add("Visa");
            cbCCType.Items.Add("MasterCard");
            cbCCType.Items.Add("Amex");

            cbLD1Gender.Items.Add("Male");
            cbLD1Gender.Items.Add("Female");
            cbLD2Gender.Items.Add("Male");
            cbLD2Gender.Items.Add("Female");
            cbLD3Gender.Items.Add("Male");
            cbLD3Gender.Items.Add("Female");
            cbLD4Gender.Items.Add("Male");
            cbLD4Gender.Items.Add("Female");

            cbLD1Rel.Items.Add("Child");
            cbLD1Rel.Items.Add("Spouse");
            cbLD2Rel.Items.Add("Child");
            cbLD2Rel.Items.Add("Spouse");
            cbLD3Rel.Items.Add("Child");
            cbLD3Rel.Items.Add("Spouse");
            cbLD4Rel.Items.Add("Child");
            cbLD4Rel.Items.Add("Spouse");

            cbGender.Items.Add("Male");
            cbGender.Items.Add("Female");
        }

        private bool LoadInitialInfoFromDB() {
            bool res1 = false; bool res2 = false; bool res3 = true;
            //string ServerIP = "127.0.0.1"; string DBUserName = "fborot"; string DBUserPassword = "Fab!anB0"; string DBName = "bmp";
            //string mySQLConnectionString = "server=" + ServerIP + ";uid=" + DBUserName + ";pwd=" + DBUserPassword + ";database=" + DBName;
            
            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString; 
            conn.Open();
            
            //load plans
            MySqlCommand cmd = new MySqlCommand("select * from plans order by plan_id asc",conn);
            MySqlDataReader ret = cmd.ExecuteReader();            
            while (ret.Read()) {
                int id = ret.GetInt32(0);
                string name = ret.GetString(1);
                string i1name = ret.GetString(2);
                string i1cost = ret.GetFloat(3).ToString();
                string i2name = ret.GetString(4);
                string i2cost = ret.GetFloat(5).ToString();
                string i3name = ret.GetString(6);
                string i3cost = ret.GetFloat(7).ToString();

                PlansContainer.Add(new plan(id, name, i1name, i1cost, i2name, i2cost, i3name, i3cost));
                res1 = true;
            }
            ret.Close();
            
            //load member count to calculate next member_id
            cmd = new MySqlCommand("SELECT COUNT(*),max(member_id) FROM members", conn);
            //int members_count = Convert.ToInt32(cmd.ExecuteScalar());
            //tbMemberID.Text = members_count.ToString();
            ret = cmd.ExecuteReader();
            string strID = string.Empty;
            int count = 0;
            int incremental_value = 0;
            while (ret.Read()){
                count = ret.GetInt32(0);
                incremental_value = ret.GetInt32(1);
                incremental_value++;
            }
            if(count == 0)
            {
                strID = "000000";
            } else
            {
                strID = incremental_value.ToString("D6");
            }
            tbMemberID.Text = strID;
            ret.Close();

            //load agencies
            cmd = new MySqlCommand("select * from agencies order by agency_id asc", conn);
            ret = cmd.ExecuteReader();
            while (ret.Read()) {
                int a_id = ret.GetInt32(0);
                string a_name = ret.GetString(1);
                string a_address = ret.GetString(2);
                string a_address2 = ret.GetString(3);
                string a_postal_code = ret.GetString(4);
                string a_contact_name = ret.GetString(5);
                string a_email = ret.GetString(6);
                string a_ph_number = ret.GetString(7);
                string a_use_agency_name = ret.GetString(8);

                AgenciesContainer.Add(new agency(a_id, a_name, a_address, a_address2, a_postal_code, a_contact_name, a_email, a_ph_number, a_use_agency_name));
                res2 = true;
            }
            ret.Close();

            //load branches
            cmd = new MySqlCommand("select * from branches order by branch_id asc", conn);
            ret = cmd.ExecuteReader();
            while (ret.Read()) {
                int b_id = ret.GetInt32(0);
                string b_name = ret.GetString(1);
                string b_address = ret.GetString(2);
                string b_address2 = ret.GetString(3);
                string b_postal_code = ret.GetString(4);
                string b_contact_name = ret.GetString(5);
                string b_email = ret.GetString(6);
                string b_ph_number = ret.GetString(7);
                string b_agency = ret.GetString(8);
                string b_use_bname = ret.GetString(9);

                BranchesContainer.Add(new branch(b_id, b_name, b_address, b_address2, b_postal_code, b_contact_name, b_email, b_ph_number, b_agency, b_use_bname));
                res3 = true;
            }
            ret.Close();

            conn.Close();
            return (res1 && res2 && res3);
        }

        private void cbPlanName_TextChanged(object sender, EventArgs e) {
            if (!ResettingForm) {
                string pname = cbPlanName.SelectedItem.ToString();
                int c = PlansContainer.Count;
                cbPlanType.Items.Clear();
                cbRecurrency.Items.Clear();
                for (int i = 0; i < c; i++) {
                    plan t_plan = (plan)PlansContainer[i];
                    string t_pname = t_plan.p_name;
                    if (t_pname == pname) {
                        for (int j = 0; j < 3; j++) {
                            if (t_plan.p_ins1_name != "NA")
                                cbPlanType.Items.Add(t_plan.p_ins1_name);
                            if (t_plan.p_ins2_name != "NA")
                                cbPlanType.Items.Add(t_plan.p_ins2_name);
                            if (t_plan.p_ins3_name != "NA")
                                cbPlanType.Items.Add(t_plan.p_ins3_name);

                            break;
                        }
                        if (pname.ToLower().Contains("core")) {
                            cbRecurrency.Items.Add("3");
                            cbRecurrency.Items.Add("6");
                            cbRecurrency.Items.Add("12");
                        } else {
                            cbRecurrency.Items.Add("1");
                        }

                    }
                }
            }
        }

        private void cbAgencyID_TextChanged(object sender, EventArgs e) {
            if (!ResettingForm) {
                string aname = cbAgencyID.SelectedItem.ToString();
                int c = AgenciesContainer.Count;
                cbBranchID.Items.Clear();
                int p_id = -1;
                for (int i = 0; i < c; i++) {
                    agency t_agency = (agency)AgenciesContainer[i];
                    string t_aname = t_agency.agency_name;
                    if (t_aname == aname) {
                        p_id = t_agency.agency_id;
                        break;
                    }
                }
                c = BranchesContainer.Count;
                for (int j = 0; j < c; j++) {
                    branch t_branch = (branch)BranchesContainer[j];
                    if (t_branch.branch_agency == aname) {
                        cbBranchID.Items.Add(t_branch.branch_name);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e) {
            Form1.Addmember = null;
            this.Close();
        }

        private void ResetForm() {
            ResettingForm = true;

            tbName.Text = "";
            tbLName.Text = "";
            tbEmail.Text = "";
            cbLanguage.SelectedIndex = -1;
            cbMarital.SelectedIndex = -1;
            dtDOB.Value = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            tbHomePh.Text = "";
            tbMobilePH.Text = "";
            tbOtherPh.Text = "";
            
            tbAddres.Text = "";
            tbAddress2.Text = "";
            tbCity.Text = "";
            cbState.SelectedIndex = -1;
            tbPostalCode.Text = "";

            tbShAddress.Text = "";
            tbShAddress2.Text = "";
            tbShCity.Text = "";
            cbShState.SelectedIndex = -1;
            tbShPostalCode.Text = "";
            
            ckbUseHome.Checked = false;

            cbPlanType.SelectedIndex = -1;
            cbPlanName.SelectedIndex = -1;
            
            tbRecurringTotal.Text = "";
            dtStart.Value = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dtEnd.Value = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            tbNumberMembers.Text = "";
            cbAgencyID.SelectedIndex = -1;
            cbBranchID.SelectedIndex = -1;
            tbCCInfo.Text = "";
            cbCCType.SelectedIndex = -1;
            tbCCExpDate.Text = "";
            ckbCCAuto.Checked = false;

            tbANetTID.Text = "";
            tbANAuthCode.Text = "";

            tbMemberID.Text = (Int32.Parse(tbMemberID.Text) + 1).ToString();
            DependentsContainer.Clear();

            ResettingForm = false;

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

        //
        bool isValidCost(string cost) {
            bool res = false;
            Regex regexCost = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

            Match matchCost = regexCost.Match(cost);
            if (matchCost.Success)
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

        bool isValidCCNumber(string cc) {
            bool res = false;
            Regex rCC = new Regex(@"(^\d{4}$)");
            Match matchCC = rCC.Match(cc);
            if (matchCC.Success)
                res = true;

            return res;
        }

        bool isValidCCExpDate(string exp_date) {
            bool res = false;
            Regex rexpDate = new Regex(@"(^\d{4}$)");
            Match matchExpDate = rexpDate.Match(exp_date);
            if (matchExpDate.Success)
                res = true;

            return res;
        }
        string IsValidANetExpDate(string exp) {
            string res = string.Empty;
            string[] temp = exp.Split('-');
            string year = temp[0].Substring(2, 2);
            string month = temp[1];
            Regex rexp = new Regex(@"(^\d{2}$)");
            Match matchYear = rexp.Match(year);
            Match matchMonth = rexp.Match(month);
            if (matchYear.Success && matchMonth.Success) {
                res = month + year;
            }
            return res;
        }

        bool isValidLDDoB(string dobdate) {
            bool res = false;
            //mm/dd/yyyy
            string[] d = dobdate.Split('/');
            if(d.Length == 3) {
                for(int n = 0; n< 3; n++) {
                    try {
                        int v = Int32.Parse(d[n]);
                        switch (n){
                            case 0:
                                res = (v <= 12 || v >= 1) ? true : false;
                                 break;
                            case 1:
                                res = (v <= 31 || v >= 1) ? true : false;
                                break;
                            case 2:
                                res = (v <= 2100 || v >=1900) ? true : false;
                                break;
                        }
                    } catch (Exception ev) {
                        res = false;
                    }
                }
                res = true;
            }
            return res;
        }

        bool isALL_LDDoBValid(int num) {
            bool res = false;
            switch (num) {
                case 1:
                    res = (isValidLDDoB(tbLD1DOB.Text) == true) ? true : false;
                    break;
                case 2:
                    res = (isValidLDDoB(tbLD1DOB.Text) == true) ? true : false;
                    if(res)
                        res = (isValidLDDoB(tbLD2DOB.Text) == true) ? true : false;
                    break;
                case 3:
                    res = (isValidLDDoB(tbLD1DOB.Text) == true) ? true : false;
                    if (res) {
                        res = (isValidLDDoB(tbLD2DOB.Text) == true) ? true : false;
                        if (res)
                            res = (isValidLDDoB(tbLD3DOB.Text) == true) ? true : false;
                    }
                    break;
                case 4:
                    res = (isValidLDDoB(tbLD1DOB.Text) == true) ? true : false;
                    if (res) {
                        res = (isValidLDDoB(tbLD2DOB.Text) == true) ? true : false;
                        if (res) {
                            res = (isValidLDDoB(tbLD3DOB.Text) == true) ? true : false;
                            if (res)
                                res = (isValidLDDoB(tbLD4DOB.Text) == true) ? true : false;
                        }
                    }
                    break;
                default:
                    res = true;
                    break;
            }
            return res;
        }
        private void ckbUseHome_Click(object sender, EventArgs e) {
            
            if (!ckbUseHome.Checked) {
                tbShAddress.Enabled = true;
                tbShAddress2.Enabled = true;
                tbShCity.Enabled = true;
                cbShState.Enabled = true;
                tbShPostalCode.Enabled = true;                
            } else {
                tbShAddress.Enabled = false;
                tbShAddress2.Enabled = false;
                tbShCity.Enabled = false;
                cbShState.Enabled = false;
                tbShPostalCode.Enabled = false;
            }
        }

        private bool ChargeCC(String ApiLoginID, String ApiTransactionKey, member m) {
            bool res = false;
            return true;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            customerAddressType billingAddress = new customerAddressType {
                firstName = tbName.Text,//"John",
                lastName = tbLName.Text,//"Doe",
                address = tbAddres.Text,//"123 My St",
                city = tbCity.Text,//"OurTown",
                zip = tbPostalCode.Text,//"98004"
                phoneNumber = tbHomePh.Text,
                email = tbEmail.Text
            };

            var creditCard = new creditCardType {
                cardNumber = tbCCInfo.Text,         // "4111111111111111",
                expirationDate = tbCCExpDate.Text   //"0718"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            //"transactionSettings": {
            //    "setting": {
            //        "settingName": "testRequest",
            //        "settingValue": "false"
            //    }
            //}
            // Add line Items
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = 2, unitPrice = new Decimal(15.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };


            var transactionRequest = new transactionRequestType {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),   // charge the card
                amount = Convert.ToDecimal(tbRecurringTotal.Text),// 133.45m,
                payment = paymentType,
                billTo = billingAddress,
                shipTo = billingAddress
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response.messages.resultCode == messageTypeEnum.Ok) {
                if (response.transactionResponse != null) {
                    Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    res = true;
                }
            } else {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                if (response.transactionResponse != null) {
                    Console.WriteLine("Transaction Error : " + response.transactionResponse.errors[0].errorCode + " " + response.transactionResponse.errors[0].errorText);
                }
                res = false;
            }

            return res;
        }

        private CreateProfileResponse CreateCustomerProfileFromTransaction(string ApiLoginID, string ApiTransactionKey, string transactionId, member m) {
            bool res = false;
            //Console.WriteLine("CreateCustomerProfileFromTransaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var customerProfile = new customerProfileBaseType {
                merchantCustomerId = m.bmp_id,//"CORS-2108-002006",
                email = m.email,//"hello@castleblack.com",
                description = "Customer Profile for BMP CustomerID: " + m.bmp_id
            };

            var request = new createCustomerProfileFromTransactionRequest {
                transId = transactionId,
                // You can either specify the customer information in form of customerProfileBaseType object
                customer = customerProfile
                //  OR   
                // You can just provide the customer Profile ID
                // customerProfileId = "123343"                
            };

            var controller = new createCustomerProfileFromTransactionController(request);
            controller.Execute();

            createCustomerProfileResponse response = controller.GetApiResponse();

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {
                if (response != null && response.messages.message != null) {
                    Console.WriteLine("Success, CustomerProfileID : " + response.customerProfileId);
                    Console.WriteLine("Success, CustomerPaymentProfileID : " + response.customerPaymentProfileIdList[0]);
                    Console.WriteLine("Success, CustomerShippingProfileID : " + response.customerShippingAddressIdList[0]);
                    res = true;
                }
            } else if (response != null) {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }
            CreateProfileResponse prof = new CreateProfileResponse(response.customerProfileId, response.customerPaymentProfileIdList[0], response.customerShippingAddressIdList[0], res);
            return prof;            
        }

        public bool CreateSubscriptionFromProfile(String ApiLoginID, String ApiTransactionKey, short intervalLength,string bmp_id, string customerProfileId, string customerPaymentProfileId, string customerAddressId, string str_amount, ref string subs_id) {
            bool res = false;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey
            };

            paymentScheduleTypeInterval interval = new paymentScheduleTypeInterval();

            interval.length = intervalLength;         // months can be indicated between 1 and 12
            interval.unit = ARBSubscriptionUnitEnum.months;

            //testing
            //interval.length = 7;
            //interval.unit = ARBSubscriptionUnitEnum.days;


            paymentScheduleType schedule = new paymentScheduleType {
                interval = interval,
                startDate = DateTime.Now.AddMonths(intervalLength),      // start date should be tomorrow
                //testing
                //startDate = DateTime.Now.AddDays(7),      // start date should be tomorrow
                totalOccurrences = 9999,                          // 999 indicates no end date
                trialOccurrences = 0
            };
           
            customerProfileIdType customerProfile = new customerProfileIdType() {
                customerProfileId = customerProfileId,
                customerPaymentProfileId = customerPaymentProfileId,
                customerAddressId = customerAddressId
            };
            orderType myOrder = new orderType()
            {
                invoiceNumber = "INV-" + bmp_id,
                description = "This is an ARB transaction for " + bmp_id
            };
            ARBSubscriptionType subscriptionType = new ARBSubscriptionType() {
                amount = Decimal.Parse(str_amount),
                trialAmount = 0.00m,
                paymentSchedule = schedule,
                profile = customerProfile,
                order = myOrder
            };

            var request = new ARBCreateSubscriptionRequest { subscription = subscriptionType };
            var controller = new ARBCreateSubscriptionController(request);          // instantiate the controller that will call the service
            controller.Execute();

            ARBCreateSubscriptionResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {
                if (response != null && response.messages.message != null) {
                    //Console.WriteLine("Success, Subscription ID : " + response.subscriptionId.ToString());
                    subs_id = response.subscriptionId.ToString();
                    res = true;
                }
            } else if (response != null) {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return res;
        }

        //public ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId,string customerPaymentProfileId) {
            
        //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
        //    // define the merchant information (authentication / transaction id)
        //    ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
        //        name = ApiLoginID,
        //        ItemElementName = ItemChoiceType.transactionKey,
        //        Item = ApiTransactionKey,
        //    };

        //    var request = new getCustomerPaymentProfileRequest();
        //    request.customerProfileId = customerProfileId;
        //    request.customerPaymentProfileId = customerPaymentProfileId;

        //    // Set this optional property to true to return an unmasked expiration date
        //    //request.unmaskExpirationDateSpecified = true;
        //    //request.unmaskExpirationDate = true;


        //    // instantiate the controller that will call the service
        //    var controller = new getCustomerPaymentProfileController(request);
        //    controller.Execute();

        //    // get the response from the service (errors contained if any)
        //    var response = controller.GetApiResponse();

        //    if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {
        //        Console.WriteLine(response.messages.message[0].text);
        //        Console.WriteLine("Customer Payment Profile Id: " + response.paymentProfile.customerPaymentProfileId);
        //        if (response.paymentProfile.payment.Item is creditCardMaskedType) {
        //            Console.WriteLine("Customer Payment Profile Last 4: " + (response.paymentProfile.payment.Item as creditCardMaskedType).cardNumber);
        //            Console.WriteLine("Customer Payment Profile Expiration Date: " + (response.paymentProfile.payment.Item as creditCardMaskedType).expirationDate);

        //            if (response.paymentProfile.subscriptionIds != null && response.paymentProfile.subscriptionIds.Length > 0) {
        //                Console.WriteLine("List of subscriptions : ");
        //                for (int i = 0; i < response.paymentProfile.subscriptionIds.Length; i++)
        //                    Console.WriteLine(response.paymentProfile.subscriptionIds[i]);
        //            }
        //        }
        //    } else if (response != null) {
        //        Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
        //                          response.messages.message[0].text);
        //    }

        //    return response;
        //}

        public string GetCustomerPayProfileDetails(String ApiLoginID, String ApiTransactionKey, string customerProfileId, string customerPaymentProfileId) {
            string CCExpDate = string.Empty;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getCustomerPaymentProfileRequest();
            request.customerProfileId = customerProfileId;
            request.customerPaymentProfileId = customerPaymentProfileId;

            // Set this optional property to true to return an unmasked expiration date
            request.unmaskExpirationDateSpecified = true;
            request.unmaskExpirationDate = true;


            // instantiate the controller that will call the service
            var controller = new getCustomerPaymentProfileController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {
                //Console.WriteLine(response.messages.message[0].text);
                //Console.WriteLine("Customer Payment Profile Id: " + response.paymentProfile.customerPaymentProfileId);
                if (response.paymentProfile.payment.Item is creditCardMaskedType) {
                    //Console.WriteLine("Customer Payment Profile Last 4: " + (response.paymentProfile.payment.Item as creditCardMaskedType).cardNumber);
                    //Console.WriteLine("Customer Payment Profile Expiration Date: " + (response.paymentProfile.payment.Item as creditCardMaskedType).expirationDate);
                    CCExpDate = (response.paymentProfile.payment.Item as creditCardMaskedType).expirationDate;
                    //if (response.paymentProfile.subscriptionIds != null && response.paymentProfile.subscriptionIds.Length > 0) {
                    //    Console.WriteLine("List of subscriptions : ");
                    //    for (int i = 0; i < response.paymentProfile.subscriptionIds.Length; i++)
                    //        Console.WriteLine(response.paymentProfile.subscriptionIds[i]);
                    //}
                }
            } else if (response != null) {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return CCExpDate;
        }
        
        private void tbNumberMembers_TextChanged(object sender, EventArgs e) {
            short nmem = 0;
            if (tbNumberMembers.Text.Length > 0) {
                try {
                    nmem = Int16.Parse(tbNumberMembers.Text);
                    if (nmem < 0 || nmem > 4) {
                        MessageBox.Show("Invalid value for the Number of Members (1-4)", "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Invalid value for the Number of Members: " + ex.Message, "Saving new Member", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.gbLD.Enabled = true;
                
                switch (nmem) {
                    case 1:
                        //this.cbLD1.Enabled = true;
                        this.tbLD1Name.Enabled = true;
                        this.tbLD1MI.Enabled = true;
                        this.tbLD1LN.Enabled = true;
                        this.cbLD1Gender.Enabled = true;
                        this.tbLD1DOB.Enabled = true;
                        this.cbLD1Rel.Enabled = true;
                        this.tbLD1Name.Text = "";
                        this.tbLD1MI.Text = "";
                        this.tbLD1LN.Text = "";
                        this.tbLD1DOB.Text = "";
                        this.cbLD1Gender.SelectedIndex = -1;
                        this.cbLD1Rel.SelectedIndex = -1;

                        //this.cbLD2.Enabled = false;
                        this.tbLD2Name.Enabled = false;
                        this.tbLD2MI.Enabled = false;
                        this.tbLD2LN.Enabled = false;
                        this.cbLD2Gender.Enabled = false;
                        this.tbLD2DOB.Enabled = false;
                        this.cbLD2Rel.Enabled = false;
                        this.tbLD2Name.Text = "";
                        this.tbLD2MI.Text = "";
                        this.tbLD2LN.Text = "";
                        this.tbLD2DOB.Text = "";
                        this.cbLD2Gender.SelectedIndex = -1;
                        this.cbLD2Rel.SelectedIndex = -1;

                        //this.cbLD3.Enabled = false;
                        this.tbLD3Name.Enabled = false;
                        this.tbLD3MI.Enabled = false;
                        this.tbLD3LN.Enabled = false;
                        this.cbLD3Gender.Enabled = false;
                        this.tbLD3DOB.Enabled = false;
                        this.cbLD3Rel.Enabled = false;
                        this.tbLD3Name.Text = "";
                        this.tbLD3MI.Text = "";
                        this.tbLD3LN.Text = "";
                        this.tbLD3DOB.Text = "";
                        this.cbLD3Gender.SelectedIndex = -1;
                        this.cbLD3Rel.SelectedIndex = -1;

                        //this.cbLD4.Enabled = false;
                        this.tbLD4Name.Enabled = false;
                        this.tbLD4MI.Enabled = false;
                        this.tbLD4LN.Enabled = false;
                        this.cbLD4Gender.Enabled = false;
                        this.tbLD4DOB.Enabled = false;
                        this.cbLD4Rel.Enabled = false;
                        this.tbLD4Name.Text = "";
                        this.tbLD4MI.Text = "";
                        this.tbLD4LN.Text = "";
                        this.tbLD4DOB.Text = "";
                        this.cbLD4Gender.SelectedIndex = -1;
                        this.cbLD4Rel.SelectedIndex = -1;

                        break;

                    case 2:
                        //this.cbLD1.Enabled = true;
                        this.tbLD1Name.Enabled = true;
                        this.tbLD1MI.Enabled = true;
                        this.tbLD1LN.Enabled = true;
                        this.cbLD1Gender.Enabled = true;
                        this.tbLD1DOB.Enabled = true;
                        this.cbLD1Rel.Enabled = true;

                        //this.cbLD2.Enabled = true;
                        this.tbLD2Name.Enabled = true;
                        this.tbLD2MI.Enabled = true;
                        this.tbLD2LN.Enabled = true;
                        this.cbLD2Gender.Enabled = true;
                        this.tbLD2DOB.Enabled = true;
                        this.cbLD2Rel.Enabled = true;

                        //this.cbLD3.Enabled = false;
                        this.tbLD3Name.Enabled = false;
                        this.tbLD3MI.Enabled = false;
                        this.tbLD3LN.Enabled = false;
                        this.cbLD3Gender.Enabled = false;
                        this.tbLD3DOB.Enabled = false;
                        this.cbLD3Rel.Enabled = false;
                        this.tbLD3Name.Text = "";
                        this.tbLD3MI.Text = "";
                        this.tbLD3LN.Text = "";
                        this.tbLD3DOB.Text = "";
                        this.cbLD3Gender.SelectedIndex = -1;
                        this.cbLD3Rel.SelectedIndex = -1;

                        //this.cbLD4.Enabled = false;
                        this.tbLD4Name.Enabled = false;
                        this.tbLD4MI.Enabled = false;
                        this.tbLD4LN.Enabled = false;
                        this.cbLD4Gender.Enabled = false;
                        this.tbLD4DOB.Enabled = false;
                        this.cbLD4Rel.Enabled = false;
                        this.tbLD4Name.Text = "";
                        this.tbLD4MI.Text = "";
                        this.tbLD4LN.Text = "";
                        this.tbLD4DOB.Text = "";
                        this.cbLD4Gender.SelectedIndex = -1;
                        this.cbLD4Rel.SelectedIndex = -1;

                        break;

                    case 3:
                        //this.cbLD1.Enabled = true;
                        this.tbLD1Name.Enabled = true;
                        this.tbLD1MI.Enabled = true;
                        this.tbLD1LN.Enabled = true;
                        this.cbLD1Gender.Enabled = true;
                        this.tbLD1DOB.Enabled = true;
                        this.cbLD1Rel.Enabled = true;

                        //this.cbLD2.Enabled = true;
                        this.tbLD2Name.Enabled = true;
                        this.tbLD2MI.Enabled = true;
                        this.tbLD2LN.Enabled = true;
                        this.cbLD2Gender.Enabled = true;
                        this.tbLD2DOB.Enabled = true;
                        this.cbLD2Rel.Enabled = true;

                        //this.cbLD3.Enabled = true;
                        this.tbLD3Name.Enabled = true;
                        this.tbLD3MI.Enabled = true;
                        this.tbLD3LN.Enabled = true;
                        this.cbLD3Gender.Enabled = true;
                        this.tbLD3DOB.Enabled = true;
                        this.cbLD3Rel.Enabled = true;

                        //this.cbLD4.Enabled = false;
                        this.tbLD4Name.Enabled = false;
                        this.tbLD4MI.Enabled = false;
                        this.tbLD4LN.Enabled = false;
                        this.cbLD4Gender.Enabled = false;
                        this.tbLD4DOB.Enabled = false;
                        this.cbLD4Rel.Enabled = false;
                        this.tbLD4Name.Text = "";
                        this.tbLD4MI.Text = "";
                        this.tbLD4LN.Text = "";
                        this.tbLD4DOB.Text = "";
                        this.cbLD4Gender.SelectedIndex = -1;
                        this.cbLD4Rel.SelectedIndex = -1;

                        break;

                    case 4:
                        //this.cbLD1.Enabled = true;
                        this.tbLD1Name.Enabled = true;
                        this.tbLD1MI.Enabled = true;
                        this.tbLD1LN.Enabled = true;
                        this.cbLD1Gender.Enabled = true;
                        this.tbLD1DOB.Enabled = true;
                        this.cbLD1Rel.Enabled = true;

                        //this.cbLD2.Enabled = true;
                        this.tbLD2Name.Enabled = true;
                        this.tbLD2MI.Enabled = true;
                        this.tbLD2LN.Enabled = true;
                        this.cbLD2Gender.Enabled = true;
                        this.tbLD2DOB.Enabled = true;
                        this.cbLD2Rel.Enabled = true;

                        //this.cbLD3.Enabled = true;
                        this.tbLD3Name.Enabled = true;
                        this.tbLD3MI.Enabled = true;
                        this.tbLD3LN.Enabled = true;
                        this.cbLD3Gender.Enabled = true;
                        this.tbLD3DOB.Enabled = true;
                        this.cbLD3Rel.Enabled = true;

                        //this.cbLD4.Enabled = true;
                        this.tbLD4Name.Enabled = true;
                        this.tbLD4MI.Enabled = true;
                        this.tbLD4LN.Enabled = true;
                        this.cbLD4Gender.Enabled = true;
                        this.tbLD4DOB.Enabled = true;
                        this.cbLD4Rel.Enabled = true;

                        break;

                    default:

                        //this.cbLD1.Enabled = false;
                        this.tbLD1Name.Enabled = false;
                        this.tbLD1MI.Enabled = false;
                        this.tbLD1LN.Enabled = false;
                        this.cbLD1Gender.Enabled = false;
                        this.tbLD1DOB.Enabled = false;
                        this.cbLD1Rel.Enabled = false;
                        this.tbLD1Name.Text = "";
                        this.tbLD1MI.Text = "";
                        this.tbLD1LN.Text = "";
                        this.tbLD1DOB.Text = "";
                        this.cbLD1Gender.SelectedIndex = -1;
                        this.cbLD1Rel.SelectedIndex = -1;

                        //this.cbLD2.Enabled = false;
                        this.tbLD2Name.Enabled = false;
                        this.tbLD2MI.Enabled = false;
                        this.tbLD2LN.Enabled = false;
                        this.cbLD2Gender.Enabled = false;
                        this.tbLD2DOB.Enabled = false;
                        this.cbLD2Rel.Enabled = false;
                        this.tbLD2Name.Text = "";
                        this.tbLD2MI.Text = "";
                        this.tbLD2LN.Text = "";
                        this.tbLD2DOB.Text = "";
                        this.cbLD2Gender.SelectedIndex = -1;
                        this.cbLD2Rel.SelectedIndex = -1;

                        //this.cbLD3.Enabled = false;
                        this.tbLD3Name.Enabled = false;
                        this.tbLD3MI.Enabled = false;
                        this.tbLD3LN.Enabled = false;
                        this.cbLD3Gender.Enabled = false;
                        this.tbLD3DOB.Enabled = false;
                        this.cbLD3Rel.Enabled = false;
                        this.tbLD3Name.Text = "";
                        this.tbLD3MI.Text = "";
                        this.tbLD3LN.Text = "";
                        this.tbLD3DOB.Text = "";
                        this.cbLD3Gender.SelectedIndex = -1;
                        this.cbLD3Rel.SelectedIndex = -1;

                        //this.cbLD4.Enabled = false;
                        this.tbLD4Name.Enabled = false;
                        this.tbLD4MI.Enabled = false;
                        this.tbLD4LN.Enabled = false;
                        this.cbLD4Gender.Enabled = false;
                        this.tbLD4DOB.Enabled = false;
                        this.cbLD4Rel.Enabled = false;
                        this.tbLD4Name.Text = "";
                        this.tbLD4MI.Text = "";
                        this.tbLD4LN.Text = "";
                        this.tbLD4DOB.Text = "";
                        this.cbLD4Gender.SelectedIndex = -1;
                        this.cbLD4Rel.SelectedIndex = -1;

                        break;
                }
            }
        }

        private bool isLDInfoValid(short num_members) {
            bool res = false;
            //this.gbLD.Controls.Count.
            return res;
        }

        private string CreateMemberID()
        {
            //logic to create memberid
            string strYYMM = string.Empty;
            string strMemberID = string.Empty;

            if (cbPlanName.SelectedItem.ToString() == "Core")
            {
                strMemberID += "COR";
                if (cbPlanType.SelectedItem.ToString() == "Single")
                    strMemberID += "S";
                else
                    strMemberID += "F";
            }
            if (cbPlanName.SelectedItem.ToString() == "Core Plus")
            {
                strMemberID += "COP";
                if (cbPlanType.SelectedItem.ToString() == "Single")
                    strMemberID += "S";
                else
                    strMemberID += "F";
            }
            if (cbPlanName.SelectedItem.ToString() == "Complete")
            {
                strMemberID += "COM";
                if (cbPlanType.SelectedItem.ToString() == "Single")
                    strMemberID += "S";
                else if(cbPlanType.SelectedItem.ToString() == "Couples")
                    strMemberID += "C";
                else if (cbPlanType.SelectedItem.ToString() == "Family")
                    strMemberID += "F";
            }
            if (cbPlanName.SelectedItem.ToString() == "BMP Plus")
            {
                strMemberID += "BMP";
                if (cbPlanType.SelectedItem.ToString() == "Single")
                    strMemberID += "S";
                else if (cbPlanType.SelectedItem.ToString() == "Couples")
                    strMemberID += "C";
                else if (cbPlanType.SelectedItem.ToString() == "Family")
                    strMemberID += "F";
            }

            strYYMM = dtStart.Value.ToString("yyMM");
            strMemberID += "-";
            strMemberID += strYYMM;

            return strMemberID;
        }

        bool CreateDependeList(int number_deps) {
            bool res = false;
            try {
                switch (number_deps) {
                    case 1:
                        Dependent d1 = new Dependent(tbLD1Name.Text, tbLD1MI.Text, tbLD1LN.Text, cbLD1Gender.SelectedItem.ToString(), tbLD1DOB.Text, cbLD1Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d1);
                        break;
                    case 2:
                        Dependent d21 = new Dependent(tbLD1Name.Text, tbLD1MI.Text, tbLD1LN.Text, cbLD1Gender.SelectedItem.ToString(), tbLD1DOB.Text, cbLD1Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d21);
                        Dependent d22 = new Dependent(tbLD2Name.Text, tbLD2MI.Text, tbLD2LN.Text, cbLD2Gender.SelectedItem.ToString(), tbLD2DOB.Text, cbLD2Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d22);
                        break;
                    case 3:
                        Dependent d31 = new Dependent(tbLD1Name.Text, tbLD1MI.Text, tbLD1LN.Text, cbLD1Gender.SelectedItem.ToString(), tbLD1DOB.Text, cbLD1Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d31);
                        Dependent d32 = new Dependent(tbLD2Name.Text, tbLD2MI.Text, tbLD2LN.Text, cbLD2Gender.SelectedItem.ToString(), tbLD2DOB.Text, cbLD2Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d32);
                        Dependent d33 = new Dependent(tbLD3Name.Text, tbLD3MI.Text, tbLD3LN.Text, cbLD3Gender.SelectedItem.ToString(), tbLD3DOB.Text, cbLD3Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d33);
                        break;
                    case 4:
                        Dependent d41 = new Dependent(tbLD1Name.Text, tbLD1MI.Text, tbLD1LN.Text, cbLD1Gender.SelectedItem.ToString(), tbLD1DOB.Text, cbLD1Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d41);
                        Dependent d42 = new Dependent(tbLD2Name.Text, tbLD2MI.Text, tbLD2LN.Text, cbLD2Gender.SelectedItem.ToString(), tbLD2DOB.Text, cbLD2Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d42);
                        Dependent d43 = new Dependent(tbLD3Name.Text, tbLD3MI.Text, tbLD3LN.Text, cbLD3Gender.SelectedItem.ToString(), tbLD3DOB.Text, cbLD3Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d43);
                        Dependent d44 = new Dependent(tbLD4Name.Text, tbLD4MI.Text, tbLD4LN.Text, cbLD4Gender.SelectedItem.ToString(), tbLD4DOB.Text, cbLD4Rel.SelectedItem.ToString());
                        DependentsContainer.Add(d44);
                        break;
                    default:
                        res = false;
                        break;
                }

                res = true;
            } catch(Exception e) {
                res = false;
            }
            return res;
        }

        member CreateMemberBasedOnDependent(Dependent dep, member m) {
            string new_id = (Int32.Parse(m.member_id) + 1).ToString("D6");
            string new_bmp_id = m.bmp_id.Substring(0, 10) + new_id;
            int new_dob = FormatLDDoB(dep.dob);
            member new_member = new member(new_id, new_bmp_id, dep.name, dep.mi, dep.lname, m.email, m.language, m.marital_status, dep.gender, new_dob, m.home_phone_number, m.mobile_phone_number, m.other_phone_number,
                m.address, m.address2, m.city, m.state, m.postal_code, m.shipping_address, m.shipping_address2, m.shipping_city, m.shipping_state, m.shipping_postal_code,
                m.use_home_as_shipping_address, m.plan_name, m.plan_type, m.recurring_total, m.start_date, m.end_date, 0, m.agencyID, m.branchID, m.recurrency, m.cc_info, m.cc_type, m.cc_expiration_date, m.cc_auto_pay, m.dateadded, "No", dep.relationship);
            return new_member;
        }

        int FormatLDDoB(string dob) {
            int res = 0;
            string[] temp = dob.Split('/');
            res = Int32.Parse(temp[2] + temp[1] + temp[0]);
            return res;
        }

    }
}
