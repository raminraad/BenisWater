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
    public partial class frmUsageInsert : Form
    {
        CLSDataAccess dataAccess = new CLSDataAccess();
        DataTable dtCust;
        DataTable dtTermNo;
        bool usageExistsInDB = false;
        DataTable dtCurrentUsage;
        frmMain mainForm;

        public frmUsageInsert(DataTable DtCustomer,DataTable DtTermNo,frmMain FrmCaller,string SelectedTerm_No)
        {
            InitializeComponent();
            mainForm = FrmCaller;
            dtCust = DtCustomer;
            cmbCust.DisplayMember = "Cust_no";
            cmbTermNo.DisplayMember = "Term_No";
            cmbCust.DataSource = dtCust;
            cmbCust.SelectedIndex = 0;
            dtTermNo = DtTermNo;
            cmbTermNo.DataSource = dtTermNo;
            cmbTermNo.Text = SelectedTerm_No;

            txtCounter.Leave += new EventHandler(SetWaterPriceText);
            txtCounter.Leave += new EventHandler(SetPriceSumText);
            txtCommunion.Leave += new EventHandler(SetPriceSumText);
            txtGarbage.Leave += new EventHandler(SetPriceSumText);
            txtOther.Leave += new EventHandler(SetPriceSumText);
            txtPartnership.Leave += new EventHandler(SetPriceSumText);
            txtRenovation.Leave += new EventHandler(SetPriceSumText);
            txtSubscription.Leave += new EventHandler(SetPriceSumText);
            //txt.Leave += new EventHandler(SetPriceSumText);
            txtDiscount.Leave += new EventHandler(SetPriceSumText);
            //txtSubscription.Leave += new EventHandler(SetPriceSumText);

            txtCounter.TextChanged += new EventHandler(TextChangeEvent);
            txtCommunion.TextChanged += new EventHandler(TextChangeEvent);
            txtGarbage.TextChanged += new EventHandler(TextChangeEvent);
            txtOther.TextChanged += new EventHandler(TextChangeEvent);
            txtPartnership.TextChanged += new EventHandler(TextChangeEvent);
            txtPartnership.TextChanged += new EventHandler(TextChangeEvent);
            txtRenovation.TextChanged += new EventHandler(TextChangeEvent);
            txtSubscription.TextChanged += new EventHandler(TextChangeEvent);
            //txtDiscount.TextChanged += new EventHandler(TextChangeEvent);
            //txtSubscription.TextChanged += new EventHandler(TextChangeEvent);

        }

        private void SetWaterPriceText(object sender, EventArgs e)
        {
            try
            {
                txtWaterPrice.Text = mainForm.CalcWaterPrice(cmbTermNo.Text, cmbCust.Text, Convert.ToDouble(txtCounter.Text)).ToString();
            }
            catch
            {
            }
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
        private void SetPriceSumText(object sender, EventArgs e)
        {
            RadTextBox txtSender = (sender as RadTextBox);
            if (txtSender!=null && txtSender.Text.Trim() == "") txtSender.Text = "0";
            try
            {
                txtPriceSum.Text = (Convert.ToDouble(txtWaterPrice.Text.Trim()) + Convert.ToDouble(txtSubscription.Text.Trim()) +
                    Convert.ToDouble(txtPartnership.Text.Trim()) + Convert.ToDouble(txtGarbage.Text.Trim()) +
                    Convert.ToDouble(txtCommunion.Text.Trim()) + Convert.ToDouble(txtRenovation.Text.Trim()) +
                    Convert.ToDouble(txtOther.Text.Trim()) -
                    Convert.ToDouble(txtDiscount.Text.Trim())).ToString();
            }
            catch
            {
                txtPriceSum.Text = "0";
            }
        }
        private void cmbTermNo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            LoadUsage();
            SetPriceSumText(null, null);
        }
        void LoadUsage()
        {
            if (dtCust != null && dtTermNo != null)
            {
                dtCurrentUsage = dataAccess.GetAccessDataSetByQuery("select * from tbl_usage where Cust_No = " + cmbCust.Text + " and term_no = " + cmbTermNo.Text).Tables[0];
                if (dtCurrentUsage.Rows.Count > 0)
                {
                    usageExistsInDB = true;
                    txtCounter.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Cntr_Liter"]).ToString();
                    txtWaterPrice.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Wtr_Price"]).ToString();
                    txtGarbage.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Garbage"]).ToString();
                    txtPartnership.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Partnership"]).ToString();
                    txtRenovation.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Renovation"]).ToString();
                    txtCommunion.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Communion"]).ToString();
                    txtOther.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Others"]).ToString();
                    txtDiscount.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Discount"]).ToString();
                    txtSubscription.Text = Convert.ToInt32(dtCurrentUsage.Rows[0]["Subscription"]).ToString();
                    SetWaterPriceText(null, null);
                    SetPriceSumText(null, null);
                }
                else
                {
                    txtCounter.Text = mainForm.GetCntr_Liter((Convert.ToDouble(cmbTermNo.Text) - 1).ToString(), cmbCust.Text).ToString();
                    if (chkClearForm.Checked)
                    {
                        txtWaterPrice.Text = txtRenovation.Text = txtPartnership.Text = txtGarbage.Text =
                       txtCommunion.Text = txtOther.Text = txtDiscount.Text = txtSubscription.Text = "0";
                    }
                    usageExistsInDB = false;
                }
            }
        }
        private bool InsertUpdateAction()
        {
            string query = "";
            if (usageExistsInDB)
            {
                query += "update tbl_usage set ";
                query += "Cntr_Liter=" + (txtCounter.Text.Trim()) + ",";
                query += "Wtr_Price=" + (txtWaterPrice.Text.Trim()) + ",";
                query += "Garbage=" + (txtGarbage.Text.Trim()) + ",";
                query += "Partnership=" + (txtPartnership.Text.Trim()) + ",";
                query += "Renovation=" + (txtRenovation.Text.Trim()) + ",";
                query += "Communion=" + (txtCommunion.Text.Trim()) + ",";
                query += "Others=" + (txtOther.Text.Trim()) + ",";
                query += "Discount=" + (txtDiscount.Text.Trim());
                query += " where cust_No = " + cmbCust.Text;
                query += " and term_no = " + cmbTermNo.Text;
            }
            else
            {
                string Cntr_No="";
                try
                {
                     Cntr_No= dataAccess.GetAccessDataSetByQuery("select Cntr_No from tbl_Cust where cust_No = " +
                        cmbCust.Text).Tables[0].Rows[0]["Cntr_No"].ToString();
                }
                catch
                {
                    MessageBox.Show("مشتری با این شماره وجود ندارد", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    return false;
                }
                query += "insert into Tbl_Usage (Cntr_Liter,Wtr_Price,Garbage,Partnership,Renovation,Communion," +
                    "Others,Discount,Subscription,term_no,Cntr_no,Cust_No)";
                query += " values (" + txtCounter.Text.Trim() +
                    "," + txtWaterPrice.Text.Trim() +
                    "," + txtGarbage.Text.Trim() +
                    "," + txtPartnership.Text.Trim() +
                    "," + txtRenovation.Text.Trim() +
                    "," + txtCommunion.Text.Trim() +
                    "," + txtOther.Text.Trim() +
                    "," + txtDiscount.Text.Trim() +
                    "," + txtSubscription.Text.Trim() +
                    "," + cmbTermNo.Text +
                    "," + Cntr_No +
                    "," + cmbCust.Text +
                    ")";
            }
            dataAccess.ExecuteAccess(query);
            if (cmbCust.SelectedIndex < dtCust.Rows.Count - 1) cmbCust.SelectedIndex++;
            txtCounter.Select();
            return true;
        }
        private void cmbCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsage();
            SetPriceSumText(null,null);
        }
        private void btnInsertUsage_Click(object sender, EventArgs e)
        {
            InsertUpdateAction();
        }
    }
}
