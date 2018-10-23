using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Benis
{
    public partial class frmInstalmentInsertUpdate : Form
    {
        CLSDataAccess dataAccess = new CLSDataAccess();
        double instalmentSum = 0;
        double remainBalance = 0;
        double billBalance = 0;
        string cntrNo = "";
        string termNo = "";
        //DataTable dtCust;
        bool modeIsUpdate = false;
        bool dataModified = false;
        DataTable dtInstalment;
        //frmMain mainForm;

        public frmInstalmentInsertUpdate(string CntrNo, string TermNo, string Cust_No, string Cust_Name, string Pay_Balance) // Update Mode
        {
            InitializeComponent();
            Text = "مدیریت اطلاعات تقسیط";
            numTermNo.Minimum = int.Parse(TermNo);
            cntrNo = CntrNo;
            termNo = TermNo;
            modeIsUpdate = (dtInstalment = dataAccess.GetAccessDataSetByQuery("select *,'حذف' as cmdDelete from tbl_instalment where Cntr_No = " + cntrNo + " and Bill_Term_No = " + termNo + " order by Pay_Term_No").Tables[0]).Rows.Count > 0;
            grdInstalment.DataSource = dtInstalment.DefaultView;
            lblCustName.Text = Cust_Name;
            lblBillBalance.Text = Pay_Balance;
            lblTermNo.Text = TermNo;
            lblCntrNo.Text = CntrNo;
            billBalance = double.Parse(Pay_Balance);
            CalcFormBalances();
        }

        private void CalcFormBalances()
        {
            instalmentSum = 0;
            foreach (DataRow dr in dtInstalment.Rows)
            {
                instalmentSum += double.Parse(dr["Balance"].ToString());
            }
            remainBalance = billBalance - instalmentSum;
            lblBillBalance .Text = billBalance.ToString();
            lblInstalmentSum.Text  = instalmentSum.ToString();
            lblRemainBalance.Text = remainBalance.ToString();
        }
        private void txtPayBalance_Leave(object sender, EventArgs e)
        {
            CLSValidityCheck.IsInt(txtInstalmentBalance);
        }

        //private bool InsertUpdateAction()
        //{
        //    if (CLSValidityCheck.IsInt(txtInstalmentBalance) && CLSValidityCheck.IsDate(mskPayDate))
        //    {
        //        string query = "";
        //        if (modeIsUpdate)
        //        {
        //            query += "update tbl_payment set ";
        //            query += "Pay_Balance =" + txtInstalmentBalance.Text.Trim() + ",";
        //            query += "Pay_Date = '" + (mskPayDate.Text.Trim()) + "'";
        //            query += " where Cust_No = " + cmbCust.Text;
        //        }
        //        else
        //        {
        //            string Cntr_No = "";
        //            try
        //            {
        //                Cntr_No = dataAccess.GetAccessDataSetByQuery("select Cntr_No from tbl_Cust where cust_No = " +
        //                cmbCust.Text).Tables[0].Rows[0]["Cntr_No"].ToString();
        //            }
        //            catch
        //            {
        //                MessageBox.Show("مشتری با این شماره وجود ندارد", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        //                return false;
        //            }
        //            query += "insert into tbl_payment (cust_No,cntr_No,Pay_Date,Pay_Balance)";
        //            query += " values (" + cmbCust.Text.Trim() +
        //                "," + Cntr_No +
        //                ",'" + mskPayDate.Text.Trim() +
        //                "'," + txtInstalmentBalance.Text +
        //                ")";
        //        }
        //        dataAccess.ExecuteAccess(query);
        //        if (!modeIsUpdate)
        //        {
        //            if (cmbCust.SelectedIndex < dtCust.Rows.Count - 1) cmbCust.SelectedIndex++;
        //            if (mskPayDate.Enabled)
        //            {
        //                mskPayDate.Select();
        //                mskPayDate.SelectAll();
        //            }
        //            else
        //            {
        //                txtInstalmentBalance.Select();
        //                txtInstalmentBalance.SelectAll();
        //            }
        //        }
        //        else
        //        {
        //            DialogResult = DialogResult.OK;
        //            //Close();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("لطفاً مقادیر وارد شده را تصحیح نمایید");
        //        return false;
        //    }
        //    return true;
        //}
        private void TextChangeEvent(object sender, EventArgs e)
        {
            RadTextBox txtBox = sender as RadTextBox;
            if (!frmMain.IsNumeric(txtBox.Text.Trim()))
            {
                txtBox.BackColor = frmMain.InvalidColor;
            }
            else
            {
                txtBox.BackColor = frmMain.ValidColor;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            bool valueIsValid = true;
            if (CLSValidityCheck.IsInt(txtInstalmentBalance))
            {
                if (int.Parse(txtInstalmentBalance.Text) <= 0) valueIsValid = false;
            }
            else valueIsValid = false;
            if (valueIsValid)
            {
                double newInstalment = double.Parse(txtInstalmentBalance.Text);
                if (newInstalment <= remainBalance)
                {
                    bool termExistsInTable = false;
                    DataRow drCurrentTerm = null;
                    foreach (DataRow dr in dtInstalment.Rows)
                    {
                        if (dr["Pay_Term_No"].ToString() == numTermNo.Value.ToString())
                        {
                            termExistsInTable = true;
                            drCurrentTerm = dr;
                        }
                    }
                    if (termExistsInTable)
                    {
                        if (MessageBox.Show("مبلغ " + drCurrentTerm["Balance"].ToString() + " برای این دوره ثبت گردیده است. مبلغ جدید به مبلغ قبلی اضافه شود؟", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            drCurrentTerm["Balance"] = double.Parse(drCurrentTerm["Balance"].ToString()) + newInstalment;
                            CalcFormBalances();
                        }
                    }
                    else
                    {
                        DataRow drNew = dtInstalment.Rows.Add();
                        drNew["Bill_Term_No"] = termNo;
                        drNew["Pay_Term_No"] = (int)numTermNo.Value;
                        drNew["Cntr_No"] = int.Parse(cntrNo);
                        drNew["Balance"] = newInstalment;
                        drNew["cmdDelete"] = "حذف";
                        instalmentSum += newInstalment;
                        lblInstalmentSum.Text = instalmentSum.ToString();
                        remainBalance = billBalance - instalmentSum;
                        lblRemainBalance.Text = remainBalance.ToString();
                        txtInstalmentBalance.Text = remainBalance.ToString();
                        numTermNo.Value++;
                    }
                }
                else
                {
                    MessageBox.Show("مقدار وارد شده از مبلغ باقیمانده قبض بیشتر است.");
                }
            }
            else MessageBox.Show("مقدار وارد شده معتبر نیست.");
            txtInstalmentBalance.Focus();
            txtInstalmentBalance.SelectAll();
        }
        private void frmInstalmentInsertUpdate_Load(object sender, EventArgs e)
        {
            dataModified = false;
        }

        private void grdInstalment_CommandCellClick(object sender, EventArgs e)
        {
            if (grdInstalment.CurrentColumn.Name == "cmdDelete" && MessageBox .Show ("ردیف "+(((int)grdInstalment .CurrentRow .Index) +1).ToString()+" حذف شود؟","", MessageBoxButtons.YesNo)==DialogResult .Yes)
            {
                instalmentSum -= double.Parse(dtInstalment.Rows[grdInstalment.CurrentRow.Index]["Balance"].ToString ());
                dtInstalment.Rows.RemoveAt(grdInstalment.CurrentRow.Index);
                lblInstalmentSum.Text = instalmentSum.ToString();
                remainBalance = billBalance - instalmentSum;
                lblRemainBalance.Text = remainBalance.ToString();
                txtInstalmentBalance.Text = remainBalance.ToString();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (modeIsUpdate && dataModified)
            {
                dataAccess.ExecuteAccess("delete from Tbl_Instalment where Bill_Term_No = " + termNo + " and Cntr_No = " + cntrNo);
            }

            if (!(modeIsUpdate && !dataModified))
            {
                foreach (DataRow dr in dtInstalment.Rows)
                {
                    string query = "insert into Tbl_Instalment (Bill_Term_No,Pay_Term_No,Cntr_No,Balance) values (" + dr["Bill_Term_No"].ToString() + "," + dr["Pay_Term_No"].ToString() + "," + dr["Cntr_No"].ToString() + "," + dr["Balance"].ToString() + ")";
                    dataAccess.ExecuteAccess(query);
                }
            }
        }

        private void grdInstalment_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            dataModified = true;
        }

        //void LoadUsage()
        //{
        //    if (dtCust != null && dtTermNo != null)
        //    {
        //        dtCurrentUsage = dataAccess.GetAccessDataSetByQuery("select * from tbl_usage where cntr_No = " + cmbCust.Text + " and term_no = " + "").Tables[0];
        //        if (dtCurrentUsage.Rows.Count > 0)
        //        {
        //            modeIsUpdate = true;
        //            txtCounter.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Cntr_Liter"]).ToString();
        //            txtPayBalance.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Garbage"]).ToString();
        //            SetWaterPriceText(null, null);
        //            SetPriceSumText(null, null);
        //        }
        //        else
        //        {
        //            modeIsUpdate = false;
        //        }
        //    }
        //}
        //private void cmbCust_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadUsage();
        //    SetPriceSumText(null, null);
        //}


        //private void lblTermNo_Click(object sender, EventArgs e)
        //{

        //}
    }
}
