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
    public partial class frmPaymentInsertUpdate : Form
    {
        CLSDataAccess dataAccess = new CLSDataAccess();
        DataTable dtCust;
        bool modeIsUpdate = false;
        frmMain mainForm;
        string original_Pay_Balance = "";
        string original_Pay_Date = "";
        public frmPaymentInsertUpdate(DataTable DtCustomer, frmMain FrmCaller,string Cust_No,string Pay_Balance,string Pay_Date) // Update Mode
        {
            InitializeComponent();
            original_Pay_Balance = Pay_Balance;
            original_Pay_Date = Pay_Date;
            modeIsUpdate = true;
            cmbCust.Text = Cust_No;
            mskPayDate.Text = Pay_Date;
            txtPayBalance.Text = Pay_Balance;
            cmbCust.Enabled = false;
            Text = "ویرایش مبالغ پرداختی";
            btnLock.Visible = false;
            Width = 284;
        }
        public frmPaymentInsertUpdate(DataTable DtCustomer, frmMain FrmCaller)
        {
            InitializeComponent();
            mainForm = FrmCaller;
            dtCust = DtCustomer;
            cmbCust.DisplayMember = "Cust_no";
            cmbCust.DataSource = dtCust;
            cmbCust.SelectedIndex = 0;
            modeIsUpdate = false;
            txtPayBalance.TextChanged += new EventHandler(TextChangeEvent);
            //txtGarbage.TextChanged += new EventHandler(TextChangeEvent);
            //txtDiscount.TextChanged += new EventHandler(TextChangeEvent);
            //txtSubscription.TextChanged += new EventHandler(TextChangeEvent);

        }

        private bool InsertUpdateAction()
        {
            if (CLSValidityCheck.IsInt(txtPayBalance) && CLSValidityCheck.IsDate(mskPayDate))
            {
                string query = "";
                if (modeIsUpdate)
                {
                    query += "update tbl_payment set ";
                    query += "Pay_Balance =" + txtPayBalance.Text.Trim() + ",";
                    query += "Pay_Date = '" + (mskPayDate.Text.Trim()) + "'";
                    query += " where Cust_No = " + cmbCust.Text + " and Pay_Date = '" + original_Pay_Date + "' and Pay_Balance = " + original_Pay_Balance;
                }
                else
                {
                    string Cntr_No = "";
                    try
                    {
                        Cntr_No = dataAccess.GetAccessDataSetByQuery("select Cntr_No from tbl_Cust where cust_No = " +
                        cmbCust.Text).Tables[0].Rows[0]["Cntr_No"].ToString();
                    }
                    catch
                    {
                        MessageBox.Show("مشتری با این شماره وجود ندارد", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        return false;
                    }
                    query += "insert into tbl_payment (cust_No,cntr_No,Pay_Date,Pay_Balance)";
                    query += " values (" + cmbCust.Text.Trim() +
                        "," + Cntr_No +
                        ",'" + mskPayDate.Text.Trim() +
                        "'," + txtPayBalance.Text +
                        ")";
                }
                dataAccess.ExecuteAccess(query);
                if (!modeIsUpdate)
                {
                    if (cmbCust.SelectedIndex < dtCust.Rows.Count - 1) cmbCust.SelectedIndex++;
                    if (mskPayDate.Enabled)
                    {
                        mskPayDate.Select();
                        mskPayDate.SelectAll();
                    }
                    else
                    {
                        txtPayBalance.Select();
                        txtPayBalance.SelectAll();
                    }
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    //Close();
                }
            }
            else
            {
                MessageBox.Show("لطفاً مقادیر وارد شده را تصحیح نمایید");
                return false;
            }
            return true;
        }
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
            InsertUpdateAction();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (mskPayDate.Enabled)
                btnLock.Image = Benis.Properties.Resources.Unlock32;
            else
                btnLock.Image = Benis.Properties.Resources.Lock32;
            mskPayDate.Enabled = !mskPayDate.Enabled;
        }

        private void mskPayDate_Leave(object sender, EventArgs e)
        {
            CLSValidityCheck.IsDate(mskPayDate);
        }

        private void txtPayBalance_Leave(object sender, EventArgs e)
        {
            CLSValidityCheck.IsInt(txtPayBalance);
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
