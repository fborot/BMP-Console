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

        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        Hashtable HTableAgency = new Hashtable();
        Hashtable HTableBranch = new Hashtable();

        ArrayList Agency = new ArrayList();
        ArrayList Branch = new ArrayList();

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
            startDate = dtStartDate.Value.Date;
            endDate = dtEnddate.Value.Date;

            rTBResults.Text = "";
            HTableAgency = Hashtable.Synchronized(HTableAgency);
            HTableBranch = Hashtable.Synchronized(HTableBranch);


            ArrayList batches = GetBatchesInPeriod(Form1.APILoginID, Form1.APITransactionKey, startDate, endDate);
            ArrayList transactions = new ArrayList();
            bool res = false;
            foreach (string batch in batches) {
                res |= GetTransactionsInBatch(Form1.APILoginID, Form1.APITransactionKey, batch, ref transactions);
            }
            if (res) {
                UpdateTransactionsListFromDB(ref transactions);
            }

            //res = t_id + "_" + t_type + "_" + t_bmpCustID + "_" + t_amount + "_" + t_agency + "_" + t_branch + "_" + t_plan + "_" + t_recurrency + "_" + t_subscriptionid;
            //rTBResults.SelectionFont = new Font(rTBResults.Font, FontStyle.Bold);
            rTBResults.AppendText("Transaction#\t" + "Transaction Type\t" + "CustomerID\t\t" + "Amount\t" + "Agency\t\t" + "Branch\t\t" + "Plan\t" + "Recurrency\t" + "SubscriptionID\t" + Environment.NewLine);
            //rTBResults.SelectionFont = new Font(rTBResults.Font, FontStyle.Regular);
            foreach (TransactionDetail t in transactions)
            {                
                rTBResults.Text += t.ToString() + Environment.NewLine;
                string[] temp = t.t_bmpCustID.Split('-');
                string tplan = temp[0];
                if (t.t_type == "new")
                {
                    decimal ic = GetAgencyInitialCommisionValues(tplan, t.t_recurrency);
                    ComCheckData myobj_a = new ComCheckData(t.t_bmpCustID, ic, "Initial Commission");
                    Agency.Add(myobj_a);
                    ic = GetBranchInitialCommisionValues(tplan, t.t_recurrency);
                    ComCheckData myobj_b = new ComCheckData(t.t_bmpCustID, ic, "Initial Commission");
                    Branch.Add(myobj_b);
                    
                    if (HTableAgency.Contains(t.t_agency))
                        HTableAgency[t.t_agency] = Agency;
                    else
                        HTableAgency.Add(t.t_agency, Agency);

                    if (HTableBranch.Contains(t.t_branch))
                        HTableBranch[t.t_branch] = Branch;
                    else
                        HTableBranch.Add(t.t_branch, Branch);

                } else
                {
                    decimal rc = GetAgencyRenewalCommisionValues(tplan, t.t_recurrency);
                    ComCheckData myobj_a = new ComCheckData(t.t_bmpCustID, rc, "Recurrent Commission");
                    Agency.Add(myobj_a);
                    rc = GetBranchRenewalCommisionValues(tplan, t.t_recurrency);
                    ComCheckData myobj_b = new ComCheckData(t.t_bmpCustID, rc, "Recurrent Commission");
                    Branch.Add(myobj_b);

                    if (HTableAgency.Contains(t.t_agency))
                        HTableAgency[t.t_agency] = Agency;
                    else
                        HTableAgency.Add(t.t_agency, Agency);

                    if (HTableBranch.Contains(t.t_branch))
                        HTableBranch[t.t_branch] = Branch;
                    else
                        HTableBranch.Add(t.t_branch, Branch);

                }                
            }

            int a = 0;
            
            
        }

        public ArrayList GetBatchesInPeriod(String ApiLoginID, String ApiTransactionKey, DateTime startDate, DateTime endDate) {

            ArrayList res = new ArrayList();
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
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
            
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
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
                    string tempID = transaction.transId; string plan = "";
                    string subs = ""; string invoice = "";
                    string bmpCID = ""; string type = "new";
                    if (transaction.invoiceNumber != null) {                        
                        //if (transaction.invoiceNumber.Length > 0) {
                            subs = transaction.subscription.id.ToString();
                            invoice = transaction.invoiceNumber;
                            string[] t = invoice.ToString().Split('-');
                            bmpCID = t[1] + "-" + t[2] + "-" + t[3];
                            type = "renewal";
                            plan = t[1];
                        //}                        
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
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from transactions where trans_id = '"  + trans + "'", conn);
            MySqlDataReader ret = cmd.ExecuteReader();

            while (ret.Read()) {
                res = ret.GetString(3) + "-" + ret.GetDouble(4).ToString() + "-"
                    + ret.GetString(5) + "-" + ret.GetString(6) + "-" + ret.GetInt16(7);

            }
            ret.Close();
            conn.Close();
            return res;

        }
        public string DipIntoSubsDB(string subscription_id) {
            string res = "";

            MySqlConnection conn = null;
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = Form1.mySQLConnectionString;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from subscriptions where an_subscription_id = '" + subscription_id + "'", conn);
            MySqlDataReader ret = cmd.ExecuteReader();

            while (ret.Read()) {
                res = ret.GetString(3) + "-" + ret.GetDouble(4).ToString() + "-"
                    + ret.GetString(5) + "-" + ret.GetString(6) + "-" + ret.GetInt16(7);

            }
            ret.Close();
            conn.Close();
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


    }
}
