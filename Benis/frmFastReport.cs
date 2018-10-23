using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//using ClassLibrary;
using System.Threading;

namespace Benis
{
    public partial class frmFastReport : Form
    {
        #region Global Members
        //ReportType rptType = ReportType.None;
        List<string> sortTexts, sortColumns;
        CLSDataAccess da = new CLSDataAccess();
        DataTable dtReport;
        frmMain mainForm;
        DataTable dtUsage = new DataTable();
        int Term_No = 0;
        string BillDesc = "";
        #endregion
        #region Constructors
        public frmFastReport(frmMain MainForm)
        {
            InitializeComponent();
            mainForm = MainForm;
        }
        #endregion

        #region Methods
        /*
        private void EnableSorting()
        {
            label1.Visible = cmbSort.Visible = radioAns.Visible = radioDesc.Visible = true;
            sortTexts = new List<string>();
            sortColumns = new List<string>();
            if (rptType == ReportType.ServiceTicket)
            {
                sortTexts.Add("نام");
                sortColumns.Add("FName");
                sortTexts.Add("نام خانوادگی");
                sortColumns.Add("LName");
                sortTexts.Add("شماره بلیط");
                sortColumns.Add("TicketCodeFull");
                sortTexts.Add("FirstName");
                sortColumns.Add("EngFName");
                sortTexts.Add("SurName");
                sortColumns.Add("EngLName");
                sortTexts.Add("رده سنی");
                sortColumns.Add("AgeGroup");
                sortTexts.Add("شماره قرارداد");
                sortColumns.Add("ContractCode");
            }
            else if (rptType == ReportType.ServicePassports)
            {
                sortTexts.Add("نام(فارسی)");
                sortColumns.Add("PassportOwnerFullName");
                sortTexts.Add("نام(لاتین)");
                sortColumns.Add("PassportOwneEngFullName");
                sortTexts.Add("شماره پاسپورت");
                sortColumns.Add("PassportNo");
                sortTexts.Add("نوع عضویت");
                sortColumns.Add("MembershipType");
                sortTexts.Add("تاریخ اعتبار");
                sortColumns.Add("ValidityDate");
                sortTexts.Add("توضیحات");
                sortColumns.Add("PassengerDescription");
            }
            foreach (string str in sortTexts)
                cmbSort.Items.Add(str);
        }
         */
        /*
        private void DisableSorting()
        {
            label1.Visible = cmbSort.Visible = radioAns.Visible = radioDesc.Visible = false;
            previewControl.Dock = DockStyle.Fill;
        }
        */
        /*
        private string GetSortStr()
        {
            if (!cmbSort.Visible) return "";
            string sortStr = sortColumns[cmbSort.SelectedIndex].ToString();
            if (radioDesc.Checked) sortStr += " DESC";
            return sortStr;
        }
        */
        /*
        private void SortDtReport()
        {
            DataTable dtSorted = new DataTable();
            foreach (DataColumn dc in dtReport.Columns)
                dtSorted.Columns.Add(dc.ColumnName, dc.DataType);
            foreach (DataRow dr in dtReport.Select("", GetSortStr()))
            {
                if (rptType == ReportType.ServiceTicket)
                    dtSorted.Rows.Add(dr["TripServiceID"], dr["FName"], dr["LName"], dr["TicketCodeL"], dr["TicketCodeR"], dr["TicketCodeFull"], dr["EngFName"],
                        dr["EngLName"], dr["FromDate"], dr["ToDate"], dr["AgeGroup"], dr["ContractCode"]);
                else if (rptType == ReportType.ServicePassports)
                    dtSorted.Rows.Add(dr["PassportNo"], dr["ExportPlace"], dr["ExportDate"], dr["ValidityDate"], dr["MembershipType"], dr["PassportOwnerFullName"],
                        dr["PassportOwneEngFullName"], dr["TripName"], dr["TripCode"], dr["FromDate"], dr["ToDate"], dr["AgeGroup"], dr["PassengerDescription"], dr["TripServiceID"]);
            }
            if (rptType == ReportType.ServiceTicket) report.RegisterData(dtSorted, "ViewServiceTickets");
            else if (rptType == ReportType.ServicePassports) report.RegisterData(dtSorted, "ViewServicePassports");

            report.Show();
            dtReport = dtSorted;
        }
        */

        public void FastReportBill(int Term_No, string Cntr_No, string BillDesc)
        {
            //DisableSorting();
            report.Load(Application.StartupPath + @"/FastReport/Bill.frx");
            DataTable dt = mainForm.dataAccess.GetAccessDataSetByQuery("select * from Tbl_Usage inner join Tbl_Cust on Tbl_Usage.Cntr_No = Tbl_Cust.Cntr_No where Term_No=" 
                + Term_No.ToString() + " AND Tbl_Usage.Cntr_No = " + Cntr_No).Tables[0];
            dt.Columns.Add("Debt", typeof(double));
            dt.Columns.Add("Credit", typeof(double));
            dt.Columns.Add("FullName", typeof(string));
            //dt.Columns.Add("Addr", typeof(string));
            dt.Columns.Add("Cntr_Liter_Pre", typeof(int));
            dt.Columns.Add("TermSum", typeof(double));
            dt.Columns.Add("Sub500", typeof(double));
            //dt.Columns.Add("BillSum", typeof(double));
            dt.Columns.Add("TermStart", typeof(string));
            dt.Columns.Add("TermStart_Pre", typeof(string));
            dt.Columns.Add("TermPayable", typeof(double));

            DataRow dr = dt.Rows[0];
            double debt = 0, credit = 0;
            mainForm.CalcTermDebtCredit(out credit, out debt, Term_No, Cntr_No);
            dr["Debt"] = debt;
            dr["Credit"] = credit;
            dr["FullName"] = dr["FName"].ToString() + " " + dr["LName"].ToString();
            DataTable dtPreUsage = da.GetAccessDataSetByQuery("select cntr_liter from tbl_usage where cntr_no =" + Cntr_No +
                " and term_no = " + (Term_No - 1).ToString()).Tables[0];
            if (dtPreUsage.Rows.Count > 0)
                dr["Cntr_Liter_Pre"] = Convert.ToInt16(dtPreUsage.Rows[0]["cntr_liter"]);
            else
                dr["Cntr_Liter_Pre"] = 0;
            dr["TermSum"] = CalcTermSum(dr);
            dr["TermPayable"] = mainForm .CalcTermInstalment(Term_No ,Cntr_No) + Convert.ToDouble(dr["Debt"]) - Convert.ToDouble(dr["Credit"]);
            dr["Sub500"] = CalcSub500(Convert.ToDouble(dr["TermPayable"]));
            dr["TermPayable"] = Convert.ToDouble(dr["TermPayable"]) - Convert.ToDouble(dr["Sub500"]);
            dr["TermStart"] = frmMain.GetTermStart(Term_No+1);
            dr["TermStart_Pre"] = frmMain.GetTermStart(Term_No );
            report.Parameters[0].Value = dr["TermStart"].ToString().Substring(0, 2);
            report.Parameters[1].Value = frmMain.GetToDay("/");
            report.Parameters[2].Value = frmMain.GetPayLimit("/");
            //report.Parameters[2].Value = frmMain.GetTermStart(Term_No + 1);
            report.Parameters[3].Value = BillDesc;
            report.RegisterData(dt, "TblBill");
            Text = "پیش نمایش قبوض";
            mainForm.ProgressValue = 0;
        }
        public void FastReportBill(int Bill_Term_No, string GivenBillDesc)
        {
            BillDesc = GivenBillDesc;
            Term_No = Bill_Term_No;
            CheckForIllegalCrossThreadCalls = false;
            //ThreadStart ts = new ThreadStart(GenerateUsageReport);
            //Thread thrd = new System.Threading.Thread(ts);
            //thrd.Start();
            GenerateUsageReport();
        }
        private void GenerateUsageReport()
        {
            Text = "پیش نمایش قبوض";
            report.Load(Application.StartupPath + @"/FastReport/Bill.frx");
            dtUsage = mainForm.dataAccess.GetAccessDataSetByQuery("select * from Tbl_Usage inner join Tbl_Cust on Tbl_Usage.Cntr_No = Tbl_Cust.Cntr_No where Term_No=" + Term_No.ToString()).Tables[0];
            dtUsage.Columns.Add("Debt", typeof(double));
            dtUsage.Columns.Add("Credit", typeof(double));
            dtUsage.Columns.Add("FullName", typeof(string));
            //dtUsage.Columns.Add("Addr", typeof(string));
            dtUsage.Columns.Add("Cntr_Liter_Pre", typeof(int));
            dtUsage.Columns.Add("TermSum", typeof(double));
            dtUsage.Columns.Add("Sub500", typeof(double));
            //dtUsage.Columns.Add("BillSum", typeof(double));
            dtUsage.Columns.Add("TermStart", typeof(string));
            dtUsage.Columns.Add("TermStart_Pre", typeof(string));
            dtUsage.Columns.Add("TermPayable", typeof(double));
            //int i = 1;
            foreach (DataRow dr in dtUsage.Rows)
            {
                double debt = 0, credit = 0;
                mainForm.CalcTermDebtCredit(out credit, out debt, Term_No, dr["Tbl_Usage.Cntr_No"].ToString());
                dr["Debt"] = debt;
                dr["Credit"] = credit;
                dr["FullName"] = dr["FName"].ToString() + " " + dr["LName"].ToString();
                DataTable dtPreUsage = da.GetAccessDataSetByQuery("select cntr_liter from tbl_usage where cntr_no =" + dr["Tbl_Usage.Cntr_No"].ToString() +
                    " and term_no = " + (Term_No - 1).ToString()).Tables[0];
                if (dtPreUsage.Rows.Count > 0)
                    dr["Cntr_Liter_Pre"] = Convert.ToInt16(dtPreUsage.Rows[0]["cntr_liter"]);
                else
                    dr["Cntr_Liter_Pre"] = 0;
                dr["TermSum"] = CalcTermSum(dr);
                dr["TermPayable"] = mainForm.CalcTermInstalment(Term_No, dr["Tbl_Usage.Cntr_No"].ToString()) + Convert.ToDouble(dr["Debt"]) - Convert.ToDouble(dr["Credit"]);
                //dr["BillSum"] = Convert.ToDouble(dr["TermSum"]) + Convert.ToDouble(dr["Debt"]) - Convert.ToDouble(dr["Credit"]);
                dr["Sub500"] = CalcSub500(Convert.ToDouble(dr["TermPayable"]));
                dr["TermPayable"] = Convert.ToDouble(dr["TermPayable"]) - Convert.ToDouble(dr["Sub500"]);
                dr["TermStart"] = frmMain.GetTermStart(Term_No + 1);
                dr["TermStart_Pre"] = frmMain.GetTermStart(Term_No);
                //backgroundWorker.ReportProgress((int)(i));
                //backgroundWorkerBill.ReportProgress((int)((100 * i / dtUsage.Rows.Count)));
                //i++;
                mainForm.ProgressValue = (dtUsage .Rows .IndexOf (dr)*100) / dtUsage.Rows.Count;
            }
            report.Parameters[0].Value = frmMain.GetTermStart(Term_No).ToString().Substring(0, 2);
            report.Parameters[1].Value = frmMain.GetToDay("/");
            report.Parameters[2].Value = frmMain.GetPayLimit("/");
            report.Parameters[3].Value = BillDesc;
            report.RegisterData(dtUsage, "TblBill");
            mainForm.EnableReporting();
        }
        public void FastReportDebtorList(int Term_No)
        {
            report.Load(Application.StartupPath + @"/FastReport/DebtorList.frx");
            DataTable dtCust = mainForm.dataAccess.GetAccessDataSetByQuery("select FName+' '+LName AS FullName,Cntr_No,Cust_No from Tbl_Cust order by cust_no").Tables[0];
            dtCust.Columns.Add("Bed", typeof(double));
            int i = 1;
            foreach (DataRow dr in dtCust.Rows)
            {
                double debt = 0, credit = 0;
                mainForm.CalcTermDebtCredit(out credit, out debt, Term_No+1, dr["Cntr_No"].ToString());
                dr["bed"] = debt;
                //backgroundWorker.ReportProgress((int)(i));
                backgroundWorkerBill.ReportProgress((int)((100 * i / dtCust.Rows.Count)));
                i++;
            }
            DataTable dtReport = dtCust.Copy();
            dtReport.Clear();
            foreach (DataRow dr in dtCust.Rows)
            {
                if (Convert.ToDouble(dr["Bed"]) > 0) dtReport.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }
            report.Parameters[0].Value = Term_No;
            report.Parameters[1].Value = frmMain.GetTermStart(Term_No + 1);
            report.RegisterData(dtReport, "TblDebtorList");
            Text = "لیست بدهکاران";
            mainForm.ProgressValue = 0;
        }
        public void FastReportFinancialList(string Cntr_No,string CustomerFullName)
        {
            report.Load(Application.StartupPath + @"/FastReport/CustomerFinancialList.frx");
            DataTable dt = mainForm.GetCustomerFinancialList(Cntr_No);

            DataTable dtReportUnsorted = new DataTable();
            dtReportUnsorted.Columns.Add("Date", typeof(string));
            dtReportUnsorted.Columns.Add("Description", typeof(string));
            dtReportUnsorted.Columns.Add("Bed", typeof(double));
            dtReportUnsorted.Columns.Add("Bes", typeof(double));
            dtReportUnsorted.Columns.Add("Remaining", typeof(double));
            dtReportUnsorted.Rows.Add("1386/11/30", "Hello", 1000, 3000, 2000);
            report.RegisterData(dt, "TblCustomerFinancialList");
            report.Parameters[0].Value = CustomerFullName;
            Text = "صورت وضعیت مالی مشتری";
        }
        public void FastReportCustomerlList()
        {
            report.Load(Application.StartupPath + @"/FastReport/CustomerList1.frx");
            DataTable dtCust = mainForm.GetCustomerList();

            //DataTable dtReportUnsorted = new DataTable();
            //dtReportUnsorted.Columns.Add("Date", typeof(string));
            //dtReportUnsorted.Columns.Add("Description", typeof(string));
            //dtReportUnsorted.Columns.Add("Bed", typeof(double));
            //dtReportUnsorted.Columns.Add("Bes", typeof(double));
            //dtReportUnsorted.Columns.Add("Remaining", typeof(double));
            //dtReportUnsorted.Rows.Add("1386/11/30", "Hello", 1000, 3000, 2000);
            report.RegisterData(dtCust, "Table");
            //report.Parameters[0].Value = CustomerFullName;
            //dtCust.WriteXmlSchema("e:\\schema.scm");
            //dtCust.WriteXml("e:\\schema.xml");
            Text = "لیست مشترکین";
        }
        public double CalcTermSum(DataRow DtRow)
        {
            double sum = 0;
            sum = Convert.ToDouble(DtRow["Subscription"]) + Convert.ToDouble(DtRow["Garbage"]) + Convert.ToDouble(DtRow["Wtr_Price"]) +
                Convert.ToDouble(DtRow["Partnership"]) + Convert.ToDouble(DtRow["Renovation"]) + Convert.ToDouble(DtRow["Communion"]) +
                Convert.ToDouble(DtRow["Others"]) - Convert.ToDouble(DtRow["Discount"]);
            return sum;
        }
        public double CalcSub500(double Price)
        {
            return Price % 500;
        }
        /*
        #region Service

        

        


        public void FastReportServicePassengersPassports(string ServiceID)
        {
            report.Load(Application.StartupPath + @"/FastReport/ServicePassengersPassports.frx");
            dtReport = da.GetSQLDataSetByQuery("select * from ViewServicePassports where TripServiceID=" + ServiceID + " ORDER BY PassportNo").Tables[0];
            report.RegisterData(dtReport, "ViewServicePassports");
            Text = "لیست پاسپورت";
        }

        public void FastReportServicePassengersBeds(string ServiceID,string HotelName)
        {
            DisableSorting();
            report.Load(Application.StartupPath + @"/FastReport/ServicePassengersBeds.frx");
            DataTable dt = da.GetSQLDataSetByQuery("select * from ViewServiceHotelBedCounts where TripServiceID=" + ServiceID + " AND HotelName = N'" + HotelName + "' ORDER BY RoomNo").Tables[0];
            report.RegisterData(dt, "ViewServiceHotelBedCounts");
            Text = "لیست اتاقهای مورد نیاز";
        }

        public void FastReportServiceBusSeats(string ServiceID,string BusNo)
        {
            //DisableSorting();
            report.Load(Application.StartupPath + @"/FastReport/ServiceBusSeats.frx");
            DataTable dt = da.GetSQLDataSetByQuery("select * from ViewServiceBusSeats where id=" + ServiceID + " and busno=" + BusNo).Tables[0];
            report.RegisterData(dt, "ViewServiceBusSeats");
            Text = "لیست سرنشینان اتوبوس";
        }

        

        public void FastReportServicePassengersAll(string ServiceID)
        {
            DisableSorting();
            report.Load(Application.StartupPath + @"/FastReport/ServicePassengers.frx");
            DataTable dt = da.GetSQLDataSetByQuery("select * from ViewServicePassengersAll where TripServiceID=" + ServiceID+" ORDER BY AgeGroup").Tables[0];
            report.RegisterData(dt, "ViewServicePassengersAll");
            Text = "لیست اسامی مسافرین";
        }

        public void FastReportServiceFinancial(DataTable dtServiceFinancial)
        {
            DisableSorting();
            report.Load(Application.StartupPath + @"/FastReport/ServiceFinancial.frx");
            //DataTable dt = da.GetSQLDataSetByQuery("select * from ViewServicePassengersAll where TripServiceID=" + ServiceID + " ORDER BY AgeGroup").Tables[0];
            report.RegisterData(dtServiceFinancial, "ServiceFinancial");
            Text = "صورت وضعیت مالی تور";
        }

        #endregion 
       */
        #endregion

        private void frmFastReport_Load(object sender, EventArgs e)
        {
            previewControl.Buttons = environmentSettings.PreviewSettings.Buttons;
            report.Preview = previewControl;
            report.Show();
        }
        
        private void frmFastReport_Shown(object sender, EventArgs e)
        {
            if (previewControl.PageCount > 0)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
                Close();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (e.ProgressPercentage % 3 == 0)
                mainForm.RefreshStatusBar();
            mainForm.ProgressValue = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        /*
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortDtReport();
            if (rptType == ReportType.ServiceTicket)
            {
 
            }
        }
        private void radioAns_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAns.Checked)
                SortDtReport();
        }

        private void radioDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDesc.Checked)
                SortDtReport();
        }
        */
    }
}
