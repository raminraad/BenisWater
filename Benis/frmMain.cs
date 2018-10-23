using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using FarsiLibrary.Utils;
using System.Threading;

namespace Benis
{
    public partial class frmMain : Form
    {
        #region global members
        public CLSDataAccess dataAccess = new CLSDataAccess();
        DataTable dtCust;
        DataTable dtUsage;
        DataTable dtPayment;
        DataTable dtTermNo;
        DataTable dtSelectedTermUsage;
        DataTable dtIndicator;
        double editingIndicatorValue_UnitPrice = -1;
        double editingIndicatorValue_MaxUse = -1;
        string editingDate = "";
        bool BillDescIsInUpdateMode = false;
        bool DateSourceIsInUpdateMode = false;
        #endregion

        #region constructors
        public frmMain()
        {
            InitializeComponent();
            SetDtIndicator();
            SetDtCust();
            SetDtPayment();
            SetDtUsage();
            FillLstTermNo();
            grdIndicator.DataSource = dtIndicator.DefaultView;
            lstTermNo_SelectedIndexChanged(null, null);
            txtBillDescription.Text = System.IO.File.ReadAllText(Application.StartupPath + "\\BillDesc.txt");
            mskDateSource.Text = System.IO.File.ReadAllText(Application.StartupPath + "\\DateSource.txt");
            documentTabStrip1.SelectTab(Convert.ToInt16(System.IO.File.ReadAllText(Application.StartupPath+"\\SelectedTab.tab")));
            progressBar.Visibility = lblReporting.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
        }
        #endregion

        #region methods
        public void DisableReporting()
        {
            btnTermBillPrint.Enabled = false;
            progressBar.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            lblReporting.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            grdUsage.Columns["cmdPrintBill"].IsVisible = false;
        }
        public void EnableReporting()
        {
            btnTermBillPrint.Enabled = true;
            progressBar.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            lblReporting.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            grdUsage.Columns["cmdPrintBill"].IsVisible = true;
        }
        public void RefreshStatusBar()
        {
            status.Refresh();
        }
        static public string GetToDay(string Separator)
        {
            string year, month, day;
            PersianCalendar pc = new PersianCalendar();
            year = pc.GetYear(DateTime.Now).ToString();
            month = pc.GetMonth(DateTime.Now).ToString();
            day = pc.GetDayOfMonth(DateTime.Now).ToString();
            if (month.Length == 1)
                month = "0" + month;
            if (day.Length == 1)
                day = "0" + day;
            return (year + Separator + month + Separator + day).Substring(2);
        }
        static public string GetPayLimit(string Separator)
        {
            string year, month, day;
            PersianCalendar pc = new PersianCalendar();
            DateTime dtPayLimit = DateTime.Now.AddMonths(1);
            int persianMonthNo=pc.GetMonth(dtPayLimit);
            if (persianMonthNo > 6) 
                dtPayLimit = dtPayLimit.AddDays(-1); 
            year = pc.GetYear(dtPayLimit).ToString();
            month = pc.GetMonth(dtPayLimit).ToString();
            day = pc.GetDayOfMonth(dtPayLimit).ToString();
            if (month.Length == 1)
                month = "0" + month;
            if (day.Length == 1)
                day = "0" + day;
            return (year + Separator + month + Separator + day).Substring(2);
        }
        public static string GetTermStart(int TermNo)
        {
            string strDateSource=System.IO.File.ReadAllText(Application.StartupPath + "\\DateSource.txt");
            TermNo -= 10;
            if (TermNo == 0) return strDateSource;
            int DateSourceYear = Convert.ToInt16("13" + strDateSource.Substring(0, 2));
            int DateSourceMonth = Convert.ToInt16(strDateSource.Substring(3, 2));
            int DateSourceDay = Convert.ToInt16(strDateSource.Substring(6, 2));
            PersianCalendar pc = new FarsiLibrary.Utils.PersianCalendar();
            DateTime dt = pc.ToDateTime(DateSourceYear, DateSourceMonth, DateSourceDay, 1, 1, 1, 1);
            dt = dt.AddMonths(2 * (TermNo - 1));
            PersianDate pd = PersianDateConverter.ToPersianDate(dt);
            int daysToAdd = 0;
            if (pd.Day > 15) daysToAdd = 1;
            else if (pd.Day < 15) daysToAdd = -1;
            while (pd.Day != 1)
            {
                dt = dt.AddDays(daysToAdd);
                pd = PersianDateConverter.ToPersianDate(dt);
            }
            string year = pd.Year.ToString();
            string month = pd.Month.ToString();
            string day = pd.Day.ToString();
            if (month.Length == 1) month = "0" + month;
            if (day.Length == 1) day = "0" + day;
            return (year + "/" + month + "/" + day).Remove(0, 2);
        }
        public double CalcTermDebt(int TermNo, string Cntr_No)
        {
            double debtSum = 0;
            string query1 = "select sum(Subscription+Garbage+Wtr_Price+Partnership-Discount+Renovation+Communion+Others) as BillSum from tbl_usage where " + "cntr_no = " + Cntr_No + " and term_no < " + TermNo.ToString();
            string query2 = "select sum(Balance) as Instalment_Total from tbl_instalment where cntr_no = "+Cntr_No+" and Bill_Term_No < " + TermNo.ToString();
            string query3 = "select sum(Balance) as Instalment_Payable from tbl_instalment where cntr_no = " + Cntr_No + " and Pay_Term_No < " + TermNo.ToString();
            DataTable dt1 = dataAccess.GetAccessDataSetByQuery(query1).Tables[0];
            //DataTable dt2 = dataAccess.GetAccessDataSetByQuery(query2).Tables.Count > 0 ? dataAccess.GetAccessDataSetByQuery(query2).Tables[0] : null;
            //DataTable dt3 = dataAccess.GetAccessDataSetByQuery(query3).Tables.Count > 0 ? dataAccess.GetAccessDataSetByQuery(query3).Tables[0] : null;
            DataTable dt2 = dataAccess.GetAccessDataSetByQuery(query2).Tables[0];
            DataTable dt3 = dataAccess.GetAccessDataSetByQuery(query3).Tables[0];
            DataRow dr1 = dt1.Rows[0];
            DataRow dr2 = dt2.Rows[0];
            DataRow dr3 = dt3.Rows[0];
            double instalment_total = dr2["Instalment_Total"].ToString() == "" ? 0 : Convert.ToDouble(dr2["Instalment_Total"]);
            double instalment_payable = dr3["Instalment_Payable"].ToString() == "" ? 0 : Convert.ToDouble(dr3["Instalment_Payable"]);
            try
            {
                debtSum = Convert.ToDouble(dr1["BillSum"]) - instalment_total + instalment_payable;
                //MessageBox.Show(debtSum.ToString() + " = " + dr1["BillSum"].ToString () +"-"+instalment_total.ToString ()+"+"+instalment_payable.ToString ());
                return debtSum;
            }
            catch
            {
                return 0;
            }
        }
        public double CalcTermInstalment(int TermNo, string Cntr_No)
        {
            double BillSum = 0;
            string query1 = "select Subscription+Garbage+Wtr_Price+Partnership-Discount+Renovation+Communion+Others as BillSum from tbl_usage where " + "cntr_no = " + Cntr_No + " and term_no = " + TermNo.ToString();
            string query2 = "select sum(Balance) as Instalment_Total from tbl_instalment where cntr_no = " + Cntr_No + " and Bill_Term_No = " + TermNo.ToString();
            string query3 = "select sum(Balance) as Instalment_Payable from tbl_instalment where cntr_no = " + Cntr_No + " and Pay_Term_No = " + TermNo.ToString();
            DataTable dt1 = dataAccess.GetAccessDataSetByQuery(query1).Tables[0];
            DataTable dt2 = dataAccess.GetAccessDataSetByQuery(query2).Tables[0];
            DataTable dt3 = dataAccess.GetAccessDataSetByQuery(query3).Tables[0];
            DataRow dr1 = dt1.Rows[0];
            DataRow dr2 = dt2.Rows[0];
            DataRow dr3 = dt3.Rows[0];
            double instalment_total = dr2["Instalment_Total"].ToString() == "" ? 0 : Convert.ToDouble(dr2["Instalment_Total"]);
            double instalment_payable = dr3["Instalment_Payable"].ToString() == "" ? 0 : Convert.ToDouble(dr3["Instalment_Payable"]);
            try
            {
                BillSum = Convert.ToDouble(dr1["BillSum"]) - instalment_total + instalment_payable;
                //MessageBox.Show(debtSum.ToString() + " = " + dr1["BillSum"].ToString () +"-"+instalment_total.ToString ()+"+"+instalment_payable.ToString ());
                return BillSum;
            }
            catch
            {
                return 0;
            }
        }
        public double CalcPaySumToTerm(int TermNo, string Cntr_No)
        {
            double paySum = 0;
            string termStart = GetTermStart(TermNo);
            string query = "select sum(Pay_Balance) as Pay_Balance from tbl_payment where " +
                "cntr_no = " + Cntr_No + " and Pay_Date < '" + termStart + "'";
            DataTable dt = dataAccess.GetAccessDataSetByQuery(query).Tables[0];
            DataRow dr = dt.Rows[0];
            try
            {
                paySum = Convert.ToDouble(dr["Pay_Balance"]);
                return paySum;
            }
            catch
            {
                return 0;
            }
        }
        private void SetDtCust()
        {
            dtCust = dataAccess.GetAccessDataSetByQuery("select * from tbl_cust").Tables[0];
            dtCust.Columns.Add("cmdUpdate", typeof(string));
            dtCust.Columns.Add("cmdDelete", typeof(string));
            dtCust.Columns.Add("cmdFinancialList", typeof(string));
            foreach (DataRow dr in dtCust.Rows)
            {
                dr["cmdUpdate"] = "ویرایش";
                dr["cmdDelete"] = "حذف";
                dr["cmdFinancialList"] = "صورتحساب";
            }
            dtCust.AcceptChanges();
            grdCust.DataSource = dtCust.DefaultView;
        }
        private void SetDtUsage()
        {
            dtUsage = dataAccess.GetAccessDataSetByQuery("select * from tbl_usage inner join tbl_cust on tbl_usage.cntr_no = tbl_cust.cntr_no").Tables[0];
        }
        private void SetDtPayment()
        {
            dtPayment = dataAccess.GetAccessDataSetByQuery("select * from tbl_payment inner join tbl_cust on tbl_payment.cntr_no = tbl_cust.cntr_no").Tables[0];
            dtPayment.Columns.Add("cmdUpdate", typeof(string));
            dtPayment.Columns.Add("cmdDelete", typeof(string));
            foreach (DataRow dr in dtPayment.Rows)
            {
                dr["cmdUpdate"] = "ویرایش";
                dr["cmdDelete"] = "حذف";
            }
            dtPayment.AcceptChanges();
            grdPayment.DataSource = dtPayment.DefaultView;
        }
        private void FillLstTermNo()
        {
            //lstTermNo.Items.Clear();
            dtTermNo = dataAccess.GetAccessDataSetByQuery("select distinct term_no from Tbl_usage order by term_no desc").Tables[0];
            lstTermNo.DataSource = dtTermNo;
            lstTermNo.DisplayMember = "Term_No";
            //foreach (DataRow dr in dtTermNo.Rows)
            //    lstTermNo.Items.Add(dr["term_no"].ToString());
        }
        private void SetDtIndicator()
        {
            dtIndicator = dataAccess.GetAccessDataSetByQuery("select * from tbl_indicator order by max_Use").Tables[0];
        }
        public double CalcWaterPrice(string Term_No, string Cntr_No)
        {
            try
            {
                double currentCntr_Liter = GetCntr_Liter(Term_No, Cntr_No);
                return CalcWaterPrice(Term_No, Cntr_No, currentCntr_Liter);
            }
            catch { return 0; }
        }
        public DataTable GetCustomerFinancialList(string Cntr_No)
        {
            DataTable dtReportUnsorted = new DataTable();
            dtReportUnsorted.Columns.Add("Date", typeof(string));
            dtReportUnsorted.Columns.Add("Description", typeof(string));
            dtReportUnsorted.Columns.Add("Bed", typeof(double));
            dtReportUnsorted.Columns.Add("Bes", typeof(double));
            dtReportUnsorted.Columns.Add("Remaining", typeof(double));
            DataTable dtTerms = dataAccess.GetAccessDataSetByQuery("select Term_No from Tbl_usage where cntr_no = " + Cntr_No + " order by term_no").Tables[0];
            foreach (DataRow dr in dtTerms.Rows)
            {
                object[] values = new object[5];
                values[0] = GetTermStart(Convert.ToInt16(dr["Term_no"]) + 1);
                values[1] = "مبلغ قبض دوره " + dr["Term_No"].ToString();
                values[2] = CalcPriceSumForTerm(dr["term_no"].ToString(), Cntr_No);
                values[3] = 0;
                values[4] = 0;
                dtReportUnsorted.Rows.Add(values);
            }
            DataTable dtCustomerPayments = dataAccess.GetAccessDataSetByQuery("select * from Tbl_Payment where Cntr_no = " + Cntr_No).Tables[0];
            foreach (DataRow dr in dtCustomerPayments.Rows)
            {
                object[] values = new object[5];
                values[0] = dr["pay_date"].ToString();
                values[1] = "واریز به حساب";
                values[2] = 0;
                values[3] = dr["pay_balance"].ToString();
                values[4] = 0;
                dtReportUnsorted.Rows.Add(values);
            }
            DataTable dtReportSorted = dtReportUnsorted.Copy();
            dtReportSorted.Rows.Clear();
            foreach (DataRow dr in dtReportUnsorted.Select("", "date"))
            {
                object[] values = new object[5];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = dr[i];
                }
                dtReportSorted.Rows.Add(values);
            }
            double preRemaining = 0;
            foreach (DataRow drRep in dtReportSorted.Rows)
            {
                drRep["Remaining"] = preRemaining = preRemaining - Convert.ToDouble(drRep["bed"]) + Convert.ToDouble(drRep["bes"]);
            }
            return dtReportSorted;
        }
        public DataTable GetCustomerList()
        {
            DataTable dtReportCustomer = dataAccess.GetAccessDataSetByQuery("select Cntr_no,Cust_no,FName+' '+LName AS FullName from Tbl_cust order by cust_no").Tables[0];
            return dtReportCustomer;
        }
        public double CalcWaterPrice(string Term_No, string Cust_No, double CurrentCntr_Liter)
        {
            try
            {
                DataTable dt = dataAccess.GetAccessDataSetByQuery("select Cntr_Liter from " +
                    "tbl_usage where Term_No = " + (Convert.ToDouble(Term_No) - 1).ToString() + " and cust_No = " + Cust_No).Tables[0];
                double previousCntr_Liter = 0;
                if (dt.Rows.Count > 0) previousCntr_Liter = Convert.ToDouble(dt.Rows[0]["Cntr_Liter"]);
                double currentCntr_Liter = CurrentCntr_Liter;
                double ConsumeAmount = currentCntr_Liter - previousCntr_Liter;
                bool calculationDone = false;
                double priceSum = 0;
                int lastMaxUse = 0;
                foreach (DataRow dr in dtIndicator.Rows)
                {
                    double delta = ConsumeAmount - (Convert.ToDouble(dr["Max_Use"]) - lastMaxUse);
                    if (delta >= 0)
                    {
                        priceSum += ((Convert.ToDouble(dr["Max_Use"]) - lastMaxUse) * Convert.ToDouble(dr["Unit_Price"]));
                        lastMaxUse = Convert.ToInt16(dr["Max_Use"]);
                        ConsumeAmount = delta;
                    }
                    else if (!calculationDone)
                    {
                        priceSum += (Convert.ToDouble(dr["Unit_Price"]) * ConsumeAmount);
                        calculationDone = true;
                    }
                }
                return priceSum;
            }
            catch
            {
                return 0;
            }
        }
        public void CalcTermDebtCredit(out double Credit, out double Debt, int Term_No, string Cntr_No)
        {
            double paySum = CalcPaySumToTerm(Term_No, Cntr_No);
            double priceSum = CalcTermDebt(Term_No, Cntr_No);
            if (paySum >= priceSum)
            {
                Credit = paySum - priceSum;
                Debt = 0;
            }
            else
            {
                Debt = priceSum - paySum;
                Credit = 0;
            }
        }
        public double GetCntr_Liter(string Term_No, string Cust_No)
        {
            try
            {
                return Convert.ToDouble(dataAccess.GetAccessDataSetByQuery("select Cntr_Liter from " +
                                       "tbl_usage where Term_No = " + Term_No + " and Cust_No = " + Cust_No).Tables[0].Rows[0]["Cntr_Liter"]);
            }
            catch { return 0; }
        }
        private void SetDtSelectedTermUsage()
        {
            try
            {
                int selectedTerm_No = Convert.ToInt16(lstTermNo.Items[lstTermNo.SelectedIndex].ToString());
                dtSelectedTermUsage = dataAccess.GetAccessDataSetByQuery("select * from tbl_usage inner join tbl_cust on tbl_usage.cntr_no = tbl_cust.cntr_no where term_no = " + selectedTerm_No.ToString()).Tables[0];
                //dtSelectedTermUsage.Columns.Add("debt", typeof(double));
                //dtSelectedTermUsage.Columns.Add("credit", typeof(double));
                dtSelectedTermUsage.Columns.Add("priceSum", typeof(double));
                dtSelectedTermUsage.Columns.Add("cmdDelete", typeof(string));
                dtSelectedTermUsage.Columns.Add("cmdPrintBill", typeof(string));
                dtSelectedTermUsage.Columns.Add("cmdInstalment", typeof(string));
                //int i = 0;
                foreach (DataRow dr in dtSelectedTermUsage.Rows)
                {
                    //i++;
                    //double debt = 0, credit = 0;
                    //CalcTermDebtCredit(out credit, out debt, selectedTerm_No, dr["tbl_usage.cntr_no"].ToString());
                    //dr["debt"] = debt;
                    //dr["credit"] = credit;
                    dr["priceSum"] = CalcPriceSumForRow(dr);
                    dr["cmdDelete"] = "حذف";
                    dr["cmdPrintBill"] = "چاپ قبض";
                    dr["cmdInstalment"] = "تقسیط";
                }
                dtSelectedTermUsage.AcceptChanges();
                grdUsage.DataSource = dtSelectedTermUsage.DefaultView;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private double CalcPriceSumForTerm(string Term_No, string Cntr_No)
        {
            DataTable dtRep = dataAccess.GetAccessDataSetByQuery("select Subscription+Garbage+Wtr_Price+" +
                "Partnership+Renovation+Communion+Others-Discount from Tbl_usage where Term_No = " + Term_No + " and Cntr_No = " +
                Cntr_No).Tables[0];
            //if (dtRep.Rows.Count > 0)
            return Convert.ToDouble(dtRep.Rows[0][0]);
            //else
            //{
            //    return 0;
            //}
        }
        private double CalcPriceSumForRow(int RowIndex)
        {
            double sum = 0;
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Subscription"].Value);
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Garbage"].Value);
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Wtr_Price"].Value);
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Partnership"].Value);
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Renovation"].Value);
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Communion"].Value);
            sum += Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Others"].Value);
            sum -= Convert.ToDouble(grdUsage.Rows[RowIndex].Cells["Discount"].Value);
            return sum;
        }
        private double CalcPriceSumForRow(DataRow dataRow)
        {
            double sum = 0;
            sum += Convert.ToDouble(dataRow["Subscription"]);
            sum += Convert.ToDouble(dataRow["Garbage"]);
            sum += Convert.ToDouble(dataRow["Wtr_Price"]);
            sum += Convert.ToDouble(dataRow["Partnership"]);
            sum += Convert.ToDouble(dataRow["Renovation"]);
            sum += Convert.ToDouble(dataRow["Communion"]);
            sum += Convert.ToDouble(dataRow["Others"]);
            sum -= Convert.ToDouble(dataRow["Discount"]);
            return sum;
        }
        public static string FilterNumericString(string InputString)
        {
            try
            {
                Convert.ToInt32(InputString);
                return InputString;
            }
            catch
            {
                return "";
            }
        }
        public static bool IsNumeric(string InputString)
        {
            try
            {
                if (InputString.Trim() == "") return true;
                Convert.ToInt32(InputString);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ExistsInTable(string TableName, string ColumnName, string Value, bool ValueIsNumeric)
        {
            try
            {
                string absoluteValue = Value;
                if (!ValueIsNumeric) absoluteValue = "'" + Value + "'";
                CLSDataAccess da = new CLSDataAccess();
                if ((da.GetAccessDataSetByQuery("select * from " + TableName + " where " + ColumnName + " = " + absoluteValue)).Tables[0].Rows.Count > 0) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private void lstTermNo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                SetDtSelectedTermUsage();
                lblUsageFromDate.Text = GetTermStart(Convert.ToInt16(lstTermNo.SelectedItem.Text));
                lblUsageToDate.Text = GetTermStart(Convert.ToInt16(lstTermNo.SelectedItem.Text) + 1);
            }
            catch { }
        }

        //private void mnuCust_Click(object sender, EventArgs e)
        //{
        //    foreach (DataRow dr in dataAccess.GetAccessDataSetByQuery("select * from tbl_Cust").Tables[0].Rows)
        //    {
        //        dataAccess.ExecuteAccess("update tbl_usage set cntr_No = " + dr["cntr_No"].ToString() + " where cust_No = " + dr["cust_No"].ToString());
        //        dataAccess.ExecuteAccess("update tbl_payment set cntr_No = " + dr["cntr_No"].ToString() + " where cust_No = " + dr["cust_No"].ToString());
        //    }
        //    DataTable dtPayment = dataAccess.GetAccessDataSetByQuery("select distinct pay_date from tbl_payment").Tables[0];
        //    int payCount = dtPayment.Rows.Count;
        //    int index = 0;
        //    foreach (DataRow dr in dtPayment.Rows)
        //    {
        //        dataAccess.ExecuteAccess("update tbl_payment set Pay_DateStr = '" + dr["pay_date"].ToString().Substring(0, 2) + "/" + dr["pay_date"].ToString().Substring(2, 2) + "/" + dr["pay_date"].ToString().Substring(4, 2) + "' where pay_date = " + dr["pay_date"].ToString());
        //        index++;
        //        Text = index.ToString() + "/" + payCount.ToString();
        //    }
        //}

        //private void grdUsage_CellBeginEdit(object sender, Telerik.WinControls.UI.GridViewCellCancelEventArgs e)
        //{
        //    editingItemValue = Convert.ToDouble(grdUsage.Rows[e.RowIndex].Cells[e.Column.Name].Value);
        //}

        //private void grdUsage_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        //{
        //    if (!IsNumeric(e.Value.ToString().Trim())) grdUsage.Rows[e.RowIndex].Cells[e.Column.Name].Value=editingItemValue;
        //}

        private void grdUsage_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.Column.Name == "Cntr_Liter")
                {
                    grdUsage.Rows[e.RowIndex].Cells["Wtr_Price"].Value = CalcWaterPrice(
                        lstTermNo.Items[lstTermNo.SelectedIndex].Text,
                        grdUsage.Rows[e.RowIndex].Cells["tbl_usage.Cntr_No"].Value.ToString(),
                        Convert.ToDouble(grdUsage.Rows[e.RowIndex].Cells["Cntr_Liter"].Value));
                }
                if (e.Column.Name != "priceSum")
                    grdUsage.Rows[e.RowIndex].Cells["priceSum"].Value = CalcPriceSumForRow(e.RowIndex);
            }
        }

        private void btnInsertTerm_Click(object sender, EventArgs e)
        {
            if (lstTermNo.SelectedIndex == 0)
            {
                DataRow dr = dtTermNo.NewRow();
                dr["Term_No"] = Convert.ToInt16(lstTermNo.Items.First.Text) + 1;
                dtTermNo.Rows.InsertAt(dr, 0);
                //RadListDataItem selectedItem = new RadListDataItem((Convert.ToInt16(lstTermNo.Items.First.Text)).ToString());
                string selectedItem = lstTermNo.Items.First.Text;
                frmUsageInsert frm = new frmUsageInsert(dtCust, dtTermNo, this, selectedItem);
                frm.ShowDialog();
                FillLstTermNo();
                SetDtSelectedTermUsage();
            }
            else
            {
                //RadListDataItem item = new RadListDataItem((Convert.ToDouble(lstTermNo.SelectedItem.Text) + 1).ToString());
                int index = lstTermNo.FindString((Convert.ToDouble(lstTermNo.SelectedItem.Text) + 1).ToString());
                if (index != -1)
                {
                    RadListDataItem item = lstTermNo.Items[index];
                    frmUsageInsert frm = new frmUsageInsert(dtCust, dtTermNo, this, item.Text);
                    frm.ShowDialog();
                    lstTermNo.SelectedItem = item;
                    SetDtSelectedTermUsage();
                }
                else
                {
                    DataRow dr = dtTermNo.NewRow();
                    dr["Term_No"] = Convert.ToInt16(lstTermNo.SelectedItem.Text) + 1;
                    dtTermNo.Rows.InsertAt(dr, lstTermNo.SelectedIndex);
                    string selectedItem = lstTermNo.Items[lstTermNo.FindString((Convert.ToDouble(lstTermNo.SelectedItem.Text)).ToString())].Text;
                    frmUsageInsert frm = new frmUsageInsert(dtCust, dtTermNo, this, selectedItem);
                    frm.ShowDialog();
                    FillLstTermNo();
                    SetDtSelectedTermUsage();
                }
            }
        }
        private void btnRefreshUsage_Click(object sender, EventArgs e)
        {
            SetDtSelectedTermUsage();
        }


        private void btnRefreshCust_Click(object sender, EventArgs e)
        {
            SetDtCust();
        }

        #region events
        #endregion

        public static Color InvalidColor { get { return Color.Salmon; } }
        public static Color ValidColor { get { return Color.White; } }

        private void btnDeleteTerm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("تمامی کارکردهای مربوط به دوره " + lstTermNo.SelectedItem.Text + " حذف خواهند گردید. آیا تأیید می نمایید؟", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataAccess.ExecuteAccess("delete from tbl_usage where term_no = " + lstTermNo.SelectedItem.Text);
                FillLstTermNo();
                //SetDtSelectedTermUsage();
            }
        }


        private void btnInsertCust_Click(object sender, EventArgs e)
        {
            if ((new frmCustInsertUpdate()).ShowDialog() == DialogResult.OK) btnRefreshCust_Click(null, null);
        }

        private void grdUsage_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (dtSelectedTermUsage.GetChanges() != null)
            {
                string query = "update tbl_usage set ";
                query += "Subscription = " + Convert.ToDouble(e.OldRow.Cells["Subscription"].Value) + ",";
                query += "Garbage = " + Convert.ToDouble(e.OldRow.Cells["Garbage"].Value) + ",";
                query += "Wtr_Price = " + Convert.ToDouble(e.OldRow.Cells["Wtr_Price"].Value) + ",";
                query += "Partnership = " + Convert.ToDouble(e.OldRow.Cells["Partnership"].Value) + ",";
                query += "Discount = " + Convert.ToDouble(e.OldRow.Cells["Discount"].Value) + ",";
                query += "Renovation = " + Convert.ToDouble(e.OldRow.Cells["Renovation"].Value) + ",";
                query += "Communion = " + Convert.ToDouble(e.OldRow.Cells["Communion"].Value) + ",";
                query += "Others = " + Convert.ToDouble(e.OldRow.Cells["Others"].Value) + ",";
                query += "Cntr_Liter = " + Convert.ToDouble(e.OldRow.Cells["Cntr_Liter"].Value);
                query += " where Term_No = " + Convert.ToDouble(e.OldRow.Cells["Term_No"].Value);
                query += " and Cntr_No = " + Convert.ToDouble(e.OldRow.Cells["tbl_usage.Cntr_No"].Value);
                dataAccess.ExecuteAccess(query);
                dtSelectedTermUsage.AcceptChanges();
            }
        }

        private void grdUsage_CommandCellClick(object sender, EventArgs e)
        {
            string cntrNo = grdUsage.CurrentRow.Cells["tbl_usage.cntr_No"].Value.ToString();
            string payBalance= grdUsage.CurrentRow.Cells["priceSum"].Value.ToString();
            string custNo= grdUsage.CurrentRow.Cells["Cust_No"].Value.ToString();
            string custName = grdUsage.CurrentRow.Cells["FName"].Value.ToString()+grdUsage.CurrentRow.Cells["LName"].Value.ToString();
            int termNo = Convert.ToInt16(lstTermNo.SelectedItem.Text);
            if (grdUsage.CurrentColumn.Name == "cmdDelete")
            {
                if (MessageBox.Show("اطلاعات مربوط به این قبض حذف خواهند گردید. آیا تأیید می نمایید؟", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataAccess.ExecuteAccess("delete from tbl_Usage where term_No = " + lstTermNo.SelectedItem.Text +
                        " and cntr_No = " + cntrNo);
                    lstTermNo_SelectedIndexChanged(null, null);
                }
            }
            else if (grdUsage.CurrentColumn.Name == "cmdPrintBill")
            {
                frmFastReport frmRep = new frmFastReport(this);
                frmRep.FastReportBill(termNo, cntrNo, txtBillDescription.Text);
                //frmRep.MdiParent = this;
                frmRep.Show();
            }
            else if (grdUsage.CurrentColumn.Name == "cmdInstalment")
            {
                frmInstalmentInsertUpdate frmIns = new frmInstalmentInsertUpdate(cntrNo, termNo.ToString(), custNo, custName, payBalance);
                frmIns.ShowDialog();
            }
        }

        private void grdCust_CommandCellClick(object sender, EventArgs e)
        {
            if (grdCust.CurrentColumn.Name == "cmdDelete")
            {
                if (MessageBox.Show("تمامی اطلاعات مربوط به " + grdCust.SelectedRows[0].Cells["FName"].Value.ToString() + " " + grdCust.SelectedRows[0].Cells["LName"].Value.ToString() + " حذف خواهند گردید. آیا تأیید می نمایید؟", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataAccess.ExecuteAccess("delete from tbl_usage where cntr_no = " + grdCust.SelectedRows[0].Cells["Cntr_No"].Value.ToString()
                        + " and cust_no = " + grdCust.SelectedRows[0].Cells["Cust_No"].Value.ToString());
                    dataAccess.ExecuteAccess("delete from tbl_cust where cntr_no = " + grdCust.SelectedRows[0].Cells["Cntr_No"].Value.ToString()
                        + " and cust_no = " + grdCust.SelectedRows[0].Cells["Cust_No"].Value.ToString());
                    dataAccess.ExecuteAccess("delete from tbl_payment where cntr_no = " + grdCust.SelectedRows[0].Cells["Cntr_No"].Value.ToString()
                        + " and cust_no = " + grdCust.SelectedRows[0].Cells["Cust_No"].Value.ToString());
                    btnRefreshCust_Click(null, null);
                    FillLstTermNo();
                    lstTermNo_SelectedIndexChanged(null, null);
                    btnRefreshUsage_Click(null, null);
                    btnRefreshPayment_Click(null, null);
                }
            }
            else if (grdCust.CurrentColumn.Name == "cmdUpdate")
            {
                frmCustInsertUpdate frm = new frmCustInsertUpdate(grdCust.SelectedRows[0].Cells["Cntr_No"].Value.ToString());
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    btnRefreshCust_Click(null, null);
                    btnRefreshUsage_Click(null, null);
                    btnRefreshPayment_Click(null, null);
                }
            }
            else if (grdCust.CurrentColumn.Name == "cmdFinancialList")
            {
                frmFastReport frmRep = new frmFastReport(this);
                frmRep.FastReportFinancialList(grdCust.SelectedRows[0].Cells["Cntr_No"].Value.ToString(),
                    grdCust.SelectedRows[0].Cells["FName"].Value.ToString() + " " +
                    grdCust.SelectedRows[0].Cells["LName"].Value.ToString());
                //frmRep.MdiParent = this;
                frmRep.Show();
            }
        }

        private void grdCust_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (dtCust.GetChanges() != null)
            {
                string query = "update tbl_cust set ";
                query += "fname = '" + (e.OldRow.Cells["fname"].Value) + "',";
                query += "lname = '" + (e.OldRow.Cells["lname"].Value) + "',";
                query += "addr = '" + (e.OldRow.Cells["addr"].Value);
                query += "' where Cntr_No = " + (e.OldRow.Cells["Cntr_No"].Value);
                dataAccess.ExecuteAccess(query);
                dtCust.AcceptChanges();
            }
        }

        private void grdIndicator_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (dtIndicator.GetChanges() != null)
            {
                string query = "update tbl_indicator set ";
                query += "Max_Use = " + Convert.ToDouble(e.OldRow.Cells["Max_Use"].Value) + ",";
                query += "Unit_Price = " + Convert.ToDouble(e.OldRow.Cells["Unit_Price"].Value);
                query += " where Max_Use = " + editingIndicatorValue_MaxUse.ToString();
                query += " and Unit_Price = " + editingIndicatorValue_UnitPrice.ToString();
                dataAccess.ExecuteAccess(query);
                editingIndicatorValue_MaxUse = editingIndicatorValue_UnitPrice = -1;
                dtSelectedTermUsage.AcceptChanges();
            }
            else
            {
                editingIndicatorValue_MaxUse = editingIndicatorValue_UnitPrice = -1;
            }
        }

        private void grdIndicator_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (editingIndicatorValue_MaxUse == -1 && editingIndicatorValue_UnitPrice == -1)
            {
                editingIndicatorValue_MaxUse = Convert.ToDouble(grdIndicator.Rows[e.RowIndex].Cells["Max_Use"].Value);
                editingIndicatorValue_UnitPrice = Convert.ToDouble(grdIndicator.Rows[e.RowIndex].Cells["Unit_Price"].Value);
            }
        }

        private void grdPayment_CommandCellClick(object sender, EventArgs e)
        {
            if (grdPayment.CurrentColumn.Name == "cmdDelete")
            {
                if (MessageBox.Show("تمامی اطلاعات مربوط به این پرداخت حذف خواهند گردید. آیا تأیید می نمایید؟", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataAccess.ExecuteAccess("delete from tbl_Payment where cntr_no = " + grdPayment.SelectedRows[0].Cells["Tbl_Payment.Cntr_No"].Value.ToString()+
                        " and Pay_Date = '" + grdPayment.SelectedRows[0].Cells["Pay_Date"].Value.ToString()+
                        "' and Pay_Balance = " + grdPayment.SelectedRows[0].Cells["Pay_Balance"].Value.ToString());
                    btnRefreshPayment_Click(null, null);
                }
            }
            else if (grdPayment.CurrentColumn.Name == "cmdUpdate")
            {
                frmPaymentInsertUpdate frm = new frmPaymentInsertUpdate(dtCust, this,
                    grdPayment.CurrentRow.Cells["Cust_No"].Value.ToString(),
                    grdPayment.CurrentRow.Cells["Pay_Balance"].Value.ToString(),
                    grdPayment.CurrentRow.Cells["Pay_Date"].Value.ToString());
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    btnRefreshPayment_Click(null, null);
                }
            }
        }

        private void btnRefreshPayment_Click(object sender, EventArgs e)
        {
            SetDtPayment();
        }

        private void grdPayment_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (dtPayment.GetChanges() != null)
            {
                string query = "update tbl_payment set ";
                query += "pay_date = '" + (e.OldRow.Cells["pay_date"].Value) + "',";
                query += "pay_balance = " + (e.OldRow.Cells["pay_balance"].Value);
                query += " where Cntr_No = " + (e.OldRow.Cells["tbl_payment.Cntr_No"].Value);
                dataAccess.ExecuteAccess(query);
                dtPayment.AcceptChanges();
                btnRefreshPayment_Click(null, null);
            }
        }

        private void btnInsertPayment_Click(object sender, EventArgs e)
        {
            frmPaymentInsertUpdate frm = new frmPaymentInsertUpdate(dtCust, this);
            frm.ShowDialog();
            btnRefreshPayment_Click(null, null);
        }

        private void grdPayment_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
                if (e.Column.Name == "Pay_Date")
                {
                    if (!CLSValidityCheck.IsDate("13" + e.Value.ToString()))
                    {
                        grdPayment.Rows[e.RowIndex].Cells["Pay_Date"].Value = editingDate;
                        MessageBox.Show("تاریخ وارد شده صحیح نیست");
                    }
                }
        }

        private void grdPayment_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.RowIndex != -1)
                if (e.Column.Name == "Pay_Date")
                    editingDate = grdPayment.Rows[e.RowIndex].Cells["Pay_Date"].Value.ToString();
        }

        private void btnBillDescUpdate_Click(object sender, EventArgs e)
        {
            if (!BillDescIsInUpdateMode)
            {
                txtBillDescription.ReadOnly = false;
                BillDescIsInUpdateMode = true;
                btnBillDescUpdate.Image = Benis.Properties.Resources.OK_42;
                btnBillDescCancel.Visible = true;
                txtBillDescription.Select();
                txtBillDescription.SelectAll();
            }
            else
            {
                System.IO.File.WriteAllText(Application.StartupPath + "\\BillDesc.txt", txtBillDescription.Text.Trim());
                txtBillDescription.ReadOnly = true;
                BillDescIsInUpdateMode = false;
                btnBillDescCancel.Visible = false;
                btnBillDescUpdate.Image = Benis.Properties.Resources.Update_42;
            }
        }

        private void btnBillDescCancel_Click(object sender, EventArgs e)
        {
            txtBillDescription.Text = System.IO.File.ReadAllText(Application.StartupPath + "\\BillDesc.txt");
            txtBillDescription.ReadOnly = true;
            btnBillDescCancel.Visible = false;
            BillDescIsInUpdateMode = false;
            btnBillDescUpdate.Image = Benis.Properties.Resources.Update_42;
        }

        private void btnTermBillPrint_Click(object sender, EventArgs e)
        {
            //DisableReporting();
            frmFastReport frmRep = new frmFastReport(this);
            frmRep.FastReportBill(Convert.ToInt16(lstTermNo.SelectedItem.Text), txtBillDescription.Text);

            frmRep.Show();


            #region Old Comment
            //frmRep.MdiParent = this;
            //string min = (DateTime.Now.Minute - tm.Minute).ToString();
            //string sec = (DateTime.Now.Second - tm.Second).ToString();
            //string msg = "محاسبات قبوض انجام گردید" + "\n";
            //msg += "تعداد کل رکوردها " + billCount.ToString() + "\n";
            //msg += "زمان محاسبات برای هر رکورد" + ((float)(DateTime.Now.Subtract(tm).TotalSeconds) / billCount).ToString().Substring(0, 4) + " ثانیه" + "\n";
            //msg += "زمان کل محاسبات" + ((float)DateTime.Now.Subtract(tm).TotalMinutes).ToString().Substring(0,4) + " دقیقه و " +
            //    (DateTime.Now.Subtract(tm).TotalSeconds - DateTime.Now.Subtract(tm).TotalMinutes * 60).ToString().Substring(0, 4) + " ثانیه";
            //MessageBox.Show(msg,"گزارش محاسبات",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.RightAlign,false); 
            #endregion
        }


        public int ProgressValue
        {
            set { progressBar.Value1 = value; }
            get { return progressBar.Value1; }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "لطفاً مسیر پشتیبان گیری را مشخص فرمایید";
            folderDialog.ShowNewFolderButton = true;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.Copy(Application.StartupPath + @"\Benis.mdb", folderDialog.SelectedPath + "\\" + BackupFileName, true);
                    MessageBox.Show("عملیات پشتیبان گیری با موفقیت انجام شد", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                }
                catch
                {
                    MessageBox.Show("خطا در پشتیبان گیری. لطفاً مسیر انتخاب شده را بررسی و مجدداً سعی نمایید", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Backup Files (*.bak)|*.bak";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                bool doIt = false;
                if (System.IO.File.Exists(Application.StartupPath + @"\Benis.mdb"))
                {
                    if (MessageBox.Show("اطلاعات فایل پشتیبان با اطلاعات جاری جایگزین خواهد گردید. تأیید می نمایید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false) == DialogResult.Yes)
                    {
                        doIt = true;
                    }
                }
                else doIt = true;
                if (doIt)
                    try
                    {
                        System.IO.File.Copy(openDialog.FileName, Application.StartupPath + @"\Benis.mdb", true);
                        MessageBox.Show("عملیات بازگردانی اطلاعات با موفقیت انجام شد. لطفاً تا راه اندازی مجدد برنامه شکیبا باشید","",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.RightAlign);
                        Application.Restart();
                    }
                    catch
                    {
                        MessageBox.Show("خطا در بازگردانی اطلاعات. لطفاً مسیر انتخاب شده را بررسی و مجدداً سعی نمایید", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.File.WriteAllText(Application.StartupPath + "\\SelectedTab.tab", documentTabStrip1.SelectedIndex.ToString());
            if (!System .IO .Directory .Exists (@"D:\WtrAutoBackup")) System.IO .Directory .CreateDirectory (@"D:\WtrAutoBackup");
            System.IO.File.Copy(Application.StartupPath + @"\Benis.mdb", @"D:\WtrAutoBackup\" + BackupFileName, true);
        }

        private void btnDateSourceUpdate_Click(object sender, EventArgs e)
        {
            if (!DateSourceIsInUpdateMode)
            {
                mskDateSource.ReadOnly = false;
                DateSourceIsInUpdateMode = true;
                btnDateSourceUpdate.Image = Benis.Properties.Resources.OK_42;
                btnDateSourceCancel.Visible = true;
                mskDateSource.Select();
                mskDateSource.SelectAll();
            }
            else
            {
                if (CLSValidityCheck.IsDate(mskDateSource) && mskDateSource.Value.ToString()!="")
                {
                    if (Convert.ToInt16(mskDateSource.Text.Substring(6, 2)) == 1)
                    {
                        System.IO.File.WriteAllText(Application.StartupPath + "\\DateSource.txt", mskDateSource.Text.Trim());
                        mskDateSource.ReadOnly = true;
                        DateSourceIsInUpdateMode = false;
                        btnDateSourceCancel.Visible = false;
                        btnDateSourceUpdate.Image = Benis.Properties.Resources.Update_42;
                        lblUsageFromDate.Text = GetTermStart(Convert.ToInt16(lstTermNo.SelectedItem.Text));
                        lblUsageToDate.Text = GetTermStart(Convert.ToInt16(lstTermNo.SelectedItem.Text) + 1);
                    }
                    else
                    {
                        MessageBox.Show("مبدأ دوره بایستی از ابتدای ماه شروع گردد", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    }
                }
                else
                {
                    MessageBox.Show("لطفاً یک مقدار معتبر برای تاریخ وارد نمایید", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                }
            }
        }

        private void btnDateSourceCancel_Click(object sender, EventArgs e)
        {
            mskDateSource.Text = System.IO.File.ReadAllText(Application.StartupPath + "\\DateSource.txt");
            CLSValidityCheck.IsDate(mskDateSource);
            mskDateSource.ReadOnly = true;
            btnDateSourceCancel.Visible = false;
            DateSourceIsInUpdateMode = false;
            btnDateSourceUpdate.Image = Benis.Properties.Resources.Update_42;
        }

        private void btnTermDebtorList_Click(object sender, EventArgs e)
        {
            frmFastReport frmRep = new frmFastReport(this);
            frmRep.FastReportDebtorList(Convert.ToInt16(lstTermNo.SelectedItem.Text));
            //frmRep.MdiParent = this;
            frmRep.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnPrintCustomerList_Click(object sender, EventArgs e)
        {
            frmFastReport frmRep = new frmFastReport(this);
            frmRep.FastReportCustomerlList();
                //frmRep.MdiParent = this;
                frmRep.Show();
        }

        //private void mskDateSource_Leave(object sender, EventArgs e)
        //{
        //    CLSValidityCheck.IsDate(mskDateSource);
        //}
        public double debtSum1 { get; set; }
        public string BackupFileName
        {
            get
            {
                return "Backup-" + GetToDay(".") + "-" + DateTime.Now.Hour.ToString() + "." +
                    DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + ".bak";
            }
        }

    }
}
