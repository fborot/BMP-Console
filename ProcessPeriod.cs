using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using MySql.Data.MySqlClient;


namespace BMP_Console {
    public partial class ProcessPeriod : Form {

        bool bPChecksEnabled = false;
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        Hashtable HTableAgency = new Hashtable();
        Hashtable HTableBranch = new Hashtable();

        Hashtable HTableAgencyCheck = new Hashtable();
        Hashtable HTableBranchCheck = new Hashtable();

        ArrayList ChecksPayload = new ArrayList();
        short ChecksCount = -1;

        int x_offset = -1 * Form1.X_Offset; // the printer adds 5 mm... the (0,0) coordinate is actually at (5,5) in mm with the house printer
        int y_offset = -1 * Form1.Y_Offset; // the printer adds 5 mm
        float font_size = Form1.Font_Size;
        private int checks_counter = 0;


        public ProcessPeriod() {
            InitializeComponent();
        }

        private void btExit_Click(object sender, EventArgs e) {
            Form1.PPeriod = null;
            this.Close();
        }

        private void ProcessPeriod_FormClosing(object sender, FormClosingEventArgs e) {
            Form1.PPeriod = null;
        }

        private void btCalculate_Click(object sender, EventArgs e) {
            // calculate commisions
            //startDate = DateTime.Today.Subtract(TimeSpan.FromDays(0));
            //endDate = DateTime.Today.AddDays(1);

            if (!PingDB())
            {
                MessageBox.Show("Can not connect to the database", "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bPChecksEnabled = false;
            bPrint.Enabled = bPChecksEnabled;

            btCalculate.Enabled = false;
            btExit.Enabled = false;

            startDate = dtStartDate.Value.Date;
            endDate = dtEnddate.Value.Date;

            rTBResults.Text = "";
            HTableAgency = Hashtable.Synchronized(HTableAgency);
            HTableBranch = Hashtable.Synchronized(HTableBranch);

            HTableAgency.Clear();
            HTableBranch.Clear();

            HTableAgencyCheck = Hashtable.Synchronized(HTableAgencyCheck);
            HTableBranchCheck = Hashtable.Synchronized(HTableBranchCheck);

            HTableAgencyCheck.Clear();
            HTableBranchCheck.Clear();

            ArrayList batches = GetBatchesInPeriod(Form1.APILoginID, Form1.APITransactionKey, startDate, endDate);
            ArrayList transactions = new ArrayList();
            bool res = false;
            foreach (string batch in batches) {
                res |= GetTransactionsInBatch(Form1.APILoginID, Form1.APITransactionKey, batch, ref transactions);
            }
            if (res) {
                UpdateTransactionsListFromDB(ref transactions);
            } else
            {
                MessageBox.Show("No Transactions Found during the selected period.", "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btCalculate.Enabled = true;
                btExit.Enabled = true;
                return;
            }

            //res = t_id + "_" + t_type + "_" + t_bmpCustID + "_" + t_amount + "_" + t_agency + "_" + t_branch + "_" + t_plan + "_" + t_recurrency + "_" + t_subscriptionid;
            //rTBResults.SelectionFont = new Font(rTBResults.Font, FontStyle.Bold);
            rTBResults.AppendText("Transaction#\t" + "Transaction Type\t" + "MemberID\t\t" + "Amount\t" + "Agency\t\t" + "Branch\t\t" + "Plan\t" + "Recurrency\t" + "SubscriptionID\t" + Environment.NewLine);
            //rTBResults.SelectionFont = new Font(rTBResults.Font, FontStyle.Regular);
            foreach (TransactionDetail t in transactions)
            {
                if (t.isValid()) {
                    rTBResults.Text += t.ToString() + Environment.NewLine;
                    string[] temp = t.t_bmpCustID.Split('-');
                    string tplan = temp[0];
                    if (t.t_type == "new") {
                        decimal ic = GetAgencyInitialCommisionValues(tplan, t.t_recurrency);
                        ComCheckData myobj_a = new ComCheckData(t.t_bmpCustID, ic, "Initial Sales Commission");
                        ic = GetBranchInitialCommisionValues(tplan, t.t_recurrency);
                        ComCheckData myobj_b = new ComCheckData(t.t_bmpCustID, ic, "Initial Sales Commission");
                        if (t.t_agency != "1BMP") {
                            if (HTableAgency.Contains(t.t_agency)) {
                                CheckEnvelope CE = (CheckEnvelope)HTableAgency[t.t_agency];
                                //ArrayList myAR = (ArrayList)HTableAgency[t.t_agency];
                                CE.List.Add(myobj_a);
                                //myAR.Add(myobj_a);
                                CE.Total += myobj_a.PayAmount;
                                HTableAgency[t.t_agency] = CE;
                                //HTableAgency[t.t_agency] = myAR;
                            } else {
                                CheckEnvelope CE = new CheckEnvelope("", myobj_a.PayAmount, myobj_a);
                                //ArrayList myAR = new ArrayList();
                                //myAR.Add(myobj_a);
                                HTableAgency[t.t_agency] = CE;
                                //HTableAgency[t.t_agency] = myAR;
                            }
                        }
                        if (t.t_branch != "Direct Sales") {
                            if (HTableBranch.Contains(t.t_branch)) {
                                CheckEnvelope CE = (CheckEnvelope)HTableBranch[t.t_branch];
                                //ArrayList myAR = (ArrayList)HTableBranch[t.t_branch];
                                CE.List.Add(myobj_b);
                                //myAR.Add(myobj_b);
                                CE.Total += myobj_b.PayAmount;
                                HTableBranch[t.t_branch] = CE;
                                //HTableBranch[t.t_branch] = myAR;
                            } else {
                                CheckEnvelope CE = new CheckEnvelope("", myobj_b.PayAmount, myobj_b);
                                //ArrayList myAR = new ArrayList();
                                //myAR.Add(myobj_b);
                                HTableBranch[t.t_branch] = CE;
                                //HTableBranch[t.t_branch] = myAR;
                            }
                        }
                    } else {
                        decimal rc = GetAgencyRenewalCommisionValues(tplan, t.t_recurrency);
                        ComCheckData myobj_a = new ComCheckData(t.t_bmpCustID, rc, "Renewal Commission");
                        rc = GetBranchRenewalCommisionValues(tplan, t.t_recurrency);
                        ComCheckData myobj_b = new ComCheckData(t.t_bmpCustID, rc, "Renewal Commission");
                        if (t.t_agency != "1BMP") {
                            if (HTableAgency.Contains(t.t_agency)) {
                                CheckEnvelope CE = (CheckEnvelope)HTableAgency[t.t_agency];
                                //ArrayList myAR = (ArrayList)HTableAgency[t.t_agency];
                                CE.List.Add(myobj_a);
                                //myAR.Add(myobj_a);
                                CE.Total += myobj_a.PayAmount;
                                HTableAgency[t.t_agency] = CE;
                                //HTableAgency[t.t_agency] = myAR;
                            } else {
                                CheckEnvelope CE = new CheckEnvelope("", myobj_a.PayAmount, myobj_a);
                                //ArrayList myAR = new ArrayList();
                                //myAR.Add(myobj_a);
                                HTableAgency[t.t_agency] = CE;
                                //HTableAgency[t.t_agency] = myAR;
                            }
                        }
                        if (t.t_branch != "Direct Sales") {
                            if (HTableBranch.Contains(t.t_branch)) {
                                CheckEnvelope CE = (CheckEnvelope)HTableBranch[t.t_branch];
                                //ArrayList myAR = (ArrayList)HTableBranch[t.t_branch];
                                CE.List.Add(myobj_b);
                                //myAR.Add(myobj_b);
                                CE.Total += myobj_b.PayAmount;
                                HTableBranch[t.t_branch] = CE;
                                //HTableBranch[t.t_branch] = myAR;
                            } else {
                                CheckEnvelope CE = new CheckEnvelope("", myobj_b.PayAmount, myobj_b);
                                //ArrayList myAR = new ArrayList();
                                //myAR.Add(myobj_b);
                                HTableBranch[t.t_branch] = CE;
                                //HTableBranch[t.t_branch] = myAR;
                            }
                        }
                    }
                }
            }
            
            StringBuilder check_data = new StringBuilder();

            check_data.Append(Environment.NewLine);
            check_data.Append("Agencies details" + Environment.NewLine);
            foreach (DictionaryEntry h in HTableAgency) {
                CheckEnvelope tempCE = (CheckEnvelope)h.Value;

                string check_name = GetCheckName(h.Key.ToString(),0);
                CheckEnvelope newCE = new CheckEnvelope(check_name, tempCE.Total, tempCE.List);
                HTableAgencyCheck[h.Key] = newCE;
                ChecksPayload.Add(newCE);

                check_data.Append(h.Key.ToString() + "\t\t\t\t" + "$" + newCE.Total.ToString() + "\t\t\t" + Environment.NewLine);    
                foreach(ComCheckData c in tempCE.List) {
                    check_data.Append(c.bmp_cid + "\t\t\t\t" + c.type + "\t\t\t" + "$" + c.PayAmount + Environment.NewLine);
                }
            }
            check_data.Append(Environment.NewLine);
            
            check_data.Append("Branches details" + Environment.NewLine);

            foreach (DictionaryEntry h in HTableBranch) {
                CheckEnvelope tempCE = (CheckEnvelope)h.Value;

                string check_name = GetCheckName(h.Key.ToString(), 1);
                CheckEnvelope newCE = new CheckEnvelope(check_name, tempCE.Total, tempCE.List);
                HTableBranchCheck[h.Key] = newCE;
                ChecksPayload.Add(newCE);

                check_data.Append(h.Key.ToString() + "\t\t\t" + "$" + newCE.Total.ToString() + "\t\t\t" + Environment.NewLine);
                foreach (ComCheckData c in tempCE.List) {
                    check_data.Append(c.bmp_cid + "\t\t\t" + c.type + "\t\t\t" + "$" + c.PayAmount + Environment.NewLine);
                }
            }
            

            if(HTableAgencyCheck.Count > 0 || HTableBranchCheck.Count> 0) {
                rTBResults.AppendText(Environment.NewLine);
                rTBResults.AppendText("We are going to print " + (HTableAgencyCheck.Count + HTableBranchCheck.Count).ToString() + " checks: " + HTableAgencyCheck.Count.ToString() + " for Agencies and " + HTableBranchCheck.Count.ToString() + " for Branches");
                rTBResults.AppendText(Environment.NewLine);
            } else {
                rTBResults.AppendText("No checks to print on this period");
                return;
            }

            rTBResults.AppendText(check_data.ToString());
            rTBResults.AppendText(Environment.NewLine);

            if (HTableAgencyCheck.Count > 0 || HTableBranchCheck.Count > 0) {
                if (!bPChecksEnabled) {
                    bPChecksEnabled = true;
                    bPrint.Enabled = bPChecksEnabled;
                    ChecksCount = (short)ChecksPayload.Count;
                }
            }

            btCalculate.Enabled = true;
            btExit.Enabled = true;

        }

        public ArrayList GetBatchesInPeriod(String ApiLoginID, String ApiTransactionKey, DateTime startDate, DateTime endDate) {

            ArrayList res = new ArrayList();
            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

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
            var request = new getSettledBatchListRequest();
            request.firstSettlementDate = startDate;
            request.lastSettlementDate = endDate;
            request.includeStatistics = true;

            // instantiate the controller that will call the service
            var controller = new getSettledBatchListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();


            if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {
                if (response.batchList == null)
                    return res;
                int i = 0;
                foreach (var batch in response.batchList) {
                    res.Add(batch.batchId);
                }
            } else if (response != null) {
                res.Add("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                //Console.WriteLine("Error: " + response.messages.message[0].code + "  " +  response.messages.message[0].text);
            }

            return res;
        }
   
        public bool GetTransactionsInBatch(string ApiLoginID, string ApiTransactionKey, string batchId, ref ArrayList res ) {

            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            if (!Form1.bLive)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            else
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;


            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType() {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getTransactionListRequest();
            request.batchId = batchId;
            request.paging = new Paging {
                limit = 100,
                offset = 1
            };
            request.sorting = new TransactionListSorting {
                orderBy = TransactionListOrderFieldEnum.id,
                orderDescending = true
            };

            // instantiate the controller that will call the service
            var controller = new getTransactionListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok) {

                if (response.transactions == null) {

                    res.Add(new TransactionDetail("Null: Transactions List is null.", "", "", 0, "", "", "", -1, ""));
                    return false;
                }
                   

                foreach (var transaction in response.transactions) {
                    if (transaction.transactionStatus == "declined" || transaction.transactionStatus == "refundSettledSuccessfully")
                        continue;
                    string tempID = transaction.transId; string plan = "";
                    string subs = ""; string invoice = "";
                    string bmpCID = ""; string type = "new";
                    if (transaction.invoiceNumber != null && transaction.invoiceNumber != "NOVEMBER MEMBERSHIP") {         // FB 20211201 added to process the 2 transactions made manual by Samira             
                        if( transaction.subscription == null && (transaction.transId  == "63473115201" || transaction.transId == "63473111013")) { // FB 20220110 to fix issue with double manual chanrge because CC expired
                            subs = "55724840";      // new subscription created after the 2 manual payments, old usbs was 55063658 -> terminated, we needed to charge 2 months and keep active
                        } else {
                            subs = transaction.subscription.id.ToString();
                        }
                            invoice = transaction.invoiceNumber;
                            string[] t = invoice.ToString().Split('-');
                            bmpCID = t[1] + "-" + t[2] + "-" + t[3];
                            type = "renewal";
                            plan = t[1];
                    }
                    decimal amount = transaction.settleAmount;

                    res.Add(new TransactionDetail(tempID,type, bmpCID, amount, "", "", plan, -1,subs));
                }
            } else if (response != null) {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);

                string temp = "Error: " + response.messages.message[0].code + "  " +  response.messages.message[0].text;
                res.Add(new TransactionDetail(temp, "", "", 0, "", "", "", -1,""));
                return false;
            }

            if (res.Count >= 1)
                return true;
            else
                return false;
        }

        public void UpdateTransactionsListFromDB(ref ArrayList ar) {            
            foreach (TransactionDetail td in ar) {
                if (td.t_type == "new") {
                    string data = "";
                    data = DipIntoTransDB(td.t_id);
                    if (data.Length > 0) {
                        string[] temp = data.Split('-');
                        td.t_bmpCustID = temp[0] + "-" + temp[1] + "-" + temp[2];
                        td.t_plan = temp[0];
                        td.t_amount = Decimal.Parse(temp[3]);
                        td.t_agency = temp[4];
                        td.t_branch = temp[5];
                        td.t_recurrency = Int16.Parse(temp[6]);
                    }
                } else {// renewal
                    string data = "";
                    data = DipIntoSubsDB(td.t_subscriptionid);
                    if (data.Length > 0) {
                        string[] temp = data.Split('-');
                        td.t_bmpCustID = temp[0] + "-" + temp[1] + "-" + temp[2];
                        td.t_plan = temp[0]; 
                        td.t_amount = Decimal.Parse(temp[3]);
                        td.t_agency = temp[4];
                        td.t_branch = temp[5];
                        td.t_recurrency = Int16.Parse(temp[6]);
                    }
                }
            }
            
        }

        public string DipIntoTransDB(string trans) {
            string res = "" ;

            MySqlConnection conn = null;
            MySqlDataReader ret = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from transactions where trans_id = '" + trans + "'", conn);
                ret = cmd.ExecuteReader();

                while (ret.Read())
                {
                    res = ret.GetString(3) + "-" + ret.GetDouble(4).ToString() + "-"
                        + ret.GetString(5) + "-" + ret.GetString(6) + "-" + ret.GetInt16(7);

                }
            } catch(Exception e)
            {
                MessageBox.Show(e.Message, "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally
            {
                if (ret != null)
                    ret.Close();
                if (conn != null)
                    conn.Close();
            }
            return res;

        }
        public string DipIntoSubsDB(string subscription_id) {
            string res = "";

            MySqlConnection conn = null;
            MySqlDataReader ret = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from subscriptions where an_subscription_id = '" + subscription_id + "'", conn);
                ret = cmd.ExecuteReader();

                while (ret.Read())
                {
                    res = ret.GetString(3) + "-" + ret.GetDouble(4).ToString() + "-"
                        + ret.GetString(5) + "-" + ret.GetString(6) + "-" + ret.GetInt16(7);

                }
            } catch( Exception e)
            {
                MessageBox.Show(e.Message, "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally
            {
                if(ret!= null)
                    ret.Close();
                if(conn != null)
                    conn.Close();
            }
            return res;

        }

        public decimal GetAgencyInitialCommisionValues(string plan, short rec)
        {
            //CORE:           CORS(Single)    CORF(Family)    
            //CORE PLUS:      COPS(Single)    COPF(Family)
            //COMPLETE:       COMS(Single)    COMC(Couple)    COMF(Family)
            //BMP PLUS:       BMPS(Single)    BMPC(Couple)    BMPF(Family)
            decimal cost = 0;
            switch (plan)
            {
                case "CORS":
                    if (rec == 3)
                        cost = 5;
                    if (rec == 6)
                        cost = 10;
                    if (rec == 12)
                        cost = 20;

                    break;
                case "CORF":
                    if (rec == 3)
                        cost = 10;
                    if (rec == 6)
                        cost = 20;
                    if (rec == 12)
                        cost = 40;

                    break;
                case "COPS":
                    if (rec == 3)
                        cost = 10;
                    if (rec == 6)
                        cost = 200;
                    if (rec == 12)
                        cost = 40;

                    break;
                case "COPF":
                    if (rec == 3)
                        cost = 15;
                    if (rec == 6)
                        cost = 30;
                    if (rec == 12)
                        cost = 60;

                    break;

                case "COMS":
                    cost = 5;
                    
                    break;
                case "COMC":
                    cost = 7;

                    break;
                case "COMF":
                    cost = 9;

                    break;
                case "BMPS":
                    cost = 5;

                    break;
                case "BMPC":
                    cost = 7;

                    break;
                case "BMPF":
                    cost = 9;

                    break;
            }
            return cost;
        }

        public decimal GetBranchInitialCommisionValues(string plan, short rec)
        {
            //CORE:           CORS(Single)    CORF(Family)    
            //CORE PLUS:      COPS(Single)    COPF(Family)
            //COMPLETE:       COMS(Single)    COMC(Couple)    COMF(Family)
            //BMP PLUS:       BMPS(Single)    BMPC(Couple)    BMPF(Family)
            decimal cost = 0;
            switch (plan)
            {
                case "CORS":
                    if (rec == 3)
                        cost = 35;
                    if (rec == 6)
                        cost = 50;
                    if (rec == 12)
                        cost = 75;

                    break;
                case "CORF":
                    if (rec == 3)
                        cost = 50;
                    if (rec == 6)
                        cost = 100;
                    if (rec == 12)
                        cost = 200;

                    break;
                case "COPS":
                    if (rec == 3)
                        cost = 50;
                    if (rec == 6)
                        cost = 75;
                    if (rec == 12)
                        cost = 100;

                    break;
                case "COPF":
                    if (rec == 3)
                        cost = 65;
                    if (rec == 6)
                        cost = 130;
                    if (rec == 12)
                        cost = 250;

                    break;

                case "COMS":
                    cost = 20;

                    break;
                case "COMC":
                    cost = 25;

                    break;
                case "COMF":
                    cost = 30;

                    break;
                case "BMPS":
                    cost = 20;

                    break;
                case "BMPC":
                    cost = 25;

                    break;
                case "BMPF":
                    cost = 30;

                    break;
            }
            return cost;
        }

        public decimal GetAgencyRenewalCommisionValues(string plan, short rec)
        {
            //CORE:           CORS(Single)    CORF(Family)    
            //CORE PLUS:      COPS(Single)    COPF(Family)
            //COMPLETE:       COMS(Single)    COMC(Couple)    COMF(Family)
            //BMP PLUS:       BMPS(Single)    BMPC(Couple)    BMPF(Family)
            decimal cost = 0;
            switch (plan)
            {
                case "CORS":
                    if (rec == 3)
                        cost = 5;
                    if (rec == 6)
                        cost = 10;
                    if (rec == 12)
                        cost = 20;

                    break;
                case "CORF":
                    if (rec == 3)
                        cost = 10;
                    if (rec == 6)
                        cost = 20;
                    if (rec == 12)
                        cost = 40;

                    break;
                case "COPS":
                    if (rec == 3)
                        cost = 10;
                    if (rec == 6)
                        cost = 200;
                    if (rec == 12)
                        cost = 40;

                    break;
                case "COPF":
                    if (rec == 3)
                        cost = 15;
                    if (rec == 6)
                        cost = 30;
                    if (rec == 12)
                        cost = 60;

                    break;

                case "COMS":
                    cost = 3;

                    break;
                case "COMC":
                    cost = 5;

                    break;
                case "COMF":
                    cost = 7;

                    break;
                case "BMPS":
                    cost = 3;

                    break;
                case "BMPC":
                    cost = 5;

                    break;
                case "BMPF":
                    cost = 7;

                    break;
            }
            return cost;
        }

        public decimal GetBranchRenewalCommisionValues(string plan, short rec)
        {
            //CORE:           CORS(Single)    CORF(Family)    
            //CORE PLUS:      COPS(Single)    COPF(Family)
            //COMPLETE:       COMS(Single)    COMC(Couple)    COMF(Family)
            //BMP PLUS:       BMPS(Single)    BMPC(Couple)    BMPF(Family)
            decimal cost = 0;
            switch (plan)
            {
                case "CORS":
                    if (rec == 3)
                        cost = 30;
                    if (rec == 6)
                        cost = 40;
                    if (rec == 12)
                        cost = 60;

                    break;
                case "CORF":
                    if (rec == 3)
                        cost = 35;
                    if (rec == 6)
                        cost = 75;
                    if (rec == 12)
                        cost = 150;

                    break;
                case "COPS":
                    if (rec == 3)
                        cost = 35;
                    if (rec == 6)
                        cost = 50;
                    if (rec == 12)
                        cost = 75;

                    break;
                case "COPF":
                    if (rec == 3)
                        cost = 45;
                    if (rec == 6)
                        cost = 100;
                    if (rec == 12)
                        cost = 200;

                    break;

                case "COMS":
                    cost = 5;

                    break;
                case "COMC":
                    cost = 7;

                    break;
                case "COMF":
                    cost = 9;

                    break;
                case "BMPS":
                    cost = 5;

                    break;
                case "BMPC":
                    cost = 7;

                    break;
                case "BMPF":
                    cost = 9;

                    break;
            }
            return cost;
        }

        public string GetCheckName(string in_value, short type ) {
            // in value is either an agency name or a branch name
            // type is either agency(0) or branch(1)

            string res = string.Empty;
            MySqlDataReader ret = null;
            MySqlCommand cmd = null;
            MySqlConnection conn = null;
            string query = (type == 0) ? "select * from agencies where agency_name = '" : "select * from branches where branch_name = '";
            try {
                
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = Form1.mySQLConnectionString;
                conn.Open();
                cmd = new MySqlCommand(query + in_value + "'", conn);
                ret = cmd.ExecuteReader();

                while (ret.Read()) {
                    string name_for_checks = "";
                    short id = ret.GetInt16(0);
                    string name = ret.GetString(1);
                    string addres = ret.GetString(2);
                    string address2 = ret.GetString(3);
                    string pcode = ret.GetString(4);
                    string contact = ret.GetString(5);
                    string email = ret.GetString(6);
                    string ph = ret.GetString(7);
                    if(type == 0)
                        name_for_checks = ret.GetString(8);
                    else {
                        string bagency = ret.GetString(8);
                        name_for_checks = ret.GetString(9);
                    }

                    res = name_for_checks;

                }
                ret.Close();
                conn.Close();
            } catch (Exception e) {
                ret.Close();
                conn.Close();
            }

            return res;
        }

        private void ProcessPeriod_Load(object sender, EventArgs e) {
            bPChecksEnabled = false;
        }

        //

        private  String ConvertToWords(String numb) {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "";// "Only";
            try {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0) {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0) {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Dollars " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            } catch { }
            return val;
        }

        private  String ConvertDecimals(String number) {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++) {
                digit = number[i].ToString();
                if (digit.Equals("0")) {
                    engOne = "Zero";
                } else {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        private  String ConvertWholeNumber(String Number) {
            string word = "";
            try {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0) {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits) {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone) {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0") {
                            try {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            } catch { }
                        } else {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            } catch { }
            return word.Trim();
        }

        private  String tens(String Number) {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number) {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0) {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private String ones(String Number) {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number) {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        //

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
            //++count;
            //e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            //e.Graphics.DrawString("07|05|2021", this.Font, Brushes.Black, 172 + x_offset, 26 + y_offset);//get it from date.now
            //e.Graphics.DrawString("Paola Diaz", this.Font, Brushes.Black, 30 + x_offset, 38 + y_offset);    // get it from array
            //e.Graphics.DrawString("84.00", this.Font, Brushes.Black, 175 + x_offset, 38 + y_offset);    // get it from array, total
            //e.Graphics.DrawString("Eighty four dollars and zero cents", this.Font, Brushes.Black, 30 + x_offset, 46 + y_offset);    // get it from a function that receives the total from array
            //e.Graphics.DrawString("Rec.Payments", this.Font, Brushes.Black, 25 + x_offset, 70 + y_offset);
            //// add details, member info

            //e.HasMorePages = count < 2 ? true : false; // 2 here is the number of pages

            CheckEnvelope temp = (CheckEnvelope)ChecksPayload[checks_counter]; 
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            Font font = new Font(FontFamily.GenericSansSerif, font_size);
            string tempTotal = temp.Total.ToString();
            if (!tempTotal.Contains('.'))
            {
                tempTotal += ".00";
            }

            e.Graphics.DrawString(DateTime.Now.ToString("MM|dd|yyyy"), font, Brushes.Black, 177 + x_offset, 21 + y_offset);//get it from date.now
            e.Graphics.DrawString(temp.Name, font, Brushes.Black, 35 + x_offset, 32 + y_offset);    // get it from array
            e.Graphics.DrawString(tempTotal, font, Brushes.Black, 177 + x_offset, 33 + y_offset);    // get it from array, total
            e.Graphics.DrawString(ConvertToWords(temp.Total.ToString()), font, Brushes.Black, 30 + x_offset, 41 + y_offset);    // get it from a function that receives the total from array
            //e.Graphics.DrawString(ConvertToWords(temp.Total.ToString()) + "|00--", font, Brushes.Black, 30 + x_offset, 41 + y_offset);    // get it from a function that receives the total from array
            e.Graphics.DrawString("Commission", font, Brushes.Black, 25 + x_offset, 67 + y_offset);

            e.Graphics.DrawString("Details", font, Brushes.Black, 25 + x_offset, 100 + y_offset);
            short off = 0;
            foreach(ComCheckData c in temp.List)
            {
                off += 5;
                e.Graphics.DrawString(c.bmp_cid + "\t" + c.type + "\t" + "$ " + c.PayAmount.ToString(), font, Brushes.Black, 25 + x_offset, 100 + off + y_offset);
                //e.Graphics.DrawString(c.bmp_cid + "\t" + c.type + "\t" + "$ " + c.PayAmount.ToString() + ".00", font, Brushes.Black, 25 + x_offset, 100 + off + y_offset);
            }

            checks_counter++;
            e.HasMorePages = (checks_counter < ChecksCount) ? true : false;
        }

        private void bPrint_Click(object sender, EventArgs e) {
            checks_counter = 0;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }


            //printPreviewDialog1.Document = printDocument1;
            //if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    printDocument1.Print();
            //}

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
                MySqlCommand cmd = new MySqlCommand("select COUNT(*) from plans" , conn);
                ret = Int16.Parse(cmd.ExecuteScalar().ToString());
                if (ret >= 0)
                    res = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Processing Period", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return res;
        }
    }
}
