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
    public partial class frmCustInsertUpdate : Form
    {
        CLSDataAccess dataAccess = new CLSDataAccess();
        bool modeIsUpdate = false;
        string cntr_No;

        public frmCustInsertUpdate(string Cntr_No)
        {
            InitializeComponent();
            txtCust_No.ReadOnly = true;
            //txtCntr_No.ReadOnly = true;
            cntr_No = Cntr_No;
            modeIsUpdate = true;
            //txtCntr_No.Enabled =
            txtCust_No.Enabled = false;
            txtFName.Select();
            LoadCust();
        }
        public frmCustInsertUpdate()
        {
            InitializeComponent();
            modeIsUpdate = false;
        }

        private void LoadCust()
        {
            try
            {
                DataTable dt = dataAccess.GetAccessDataSetByQuery("select * from tbl_cust where cntr_no = " + cntr_No).Tables[0];
                txtCntr_No.Text = cntr_No;
                txtCust_No.Text = dt.Rows[0]["Cust_No"].ToString();
                txtFName.Text = dt.Rows[0]["FName"].ToString();
                txtLName.Text = dt.Rows[0]["LName"].ToString();
                txtAddr.Text = dt.Rows[0]["Addr"].ToString();
            }
            catch
            {
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!(txtFName.Text.Trim() == "" && txtLName.Text.Trim() == ""))
            {
                string query = "";
                if (modeIsUpdate)
                {
                    bool cntr_noIsChanged = false;
                    if (txtCntr_No.Text.Trim() != cntr_No) cntr_noIsChanged = true;
                    if (cntr_noIsChanged && frmMain.ExistsInTable("tbl_cust", "cntr_No", txtCntr_No.Text.Trim(), true))
                    {
                        MessageBox.Show("این شماره کنتور قبلاً برای فرد دیگری ثبت شده است");
                        txtCntr_No.Select();
                        txtCntr_No.SelectAll();
                    }
                    else
                    {
                        if (txtAddr.Text.Trim() == "") txtAddr.Text = "-----------";
                        query += "update tbl_cust set ";
                        query += "cntr_No='" + (txtCntr_No.Text.Trim()) + "',";
                        query += "fname='" + (txtFName.Text.Trim()) + "',";
                        query += "lname='" + (txtLName.Text.Trim()) + "',";
                        query += "addr='" + (txtAddr.Text.Trim()) + "' ";
                        query += " where cntr_No = " + cntr_No;
                        dataAccess.ExecuteAccess(query);
                        if (cntr_noIsChanged)
                        {
                            query = "update tbl_usage set cntr_No=" + txtCntr_No.Text.Trim() + " where cntr_no=" + cntr_No;// +"'";
                            dataAccess.ExecuteAccess(query);
                            query = "update tbl_payment set cntr_No=" + txtCntr_No.Text.Trim() + " where cntr_no=" + cntr_No;// +"'";
                            dataAccess.ExecuteAccess(query);
                        }
                      
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    if (txtCntr_No.Text.Trim() != "" || txtCust_No.Text.Trim() != "")
                    {
                        if (frmMain.ExistsInTable("tbl_cust", "cntr_No", txtCntr_No.Text.Trim(), true))
                        {
                            MessageBox.Show("این شماره کنتور قبلاً برای فرد دیگری ثبت شده است");
                            txtCntr_No.Select();
                            txtCntr_No.SelectAll();
                        }
                        else if (frmMain.ExistsInTable("tbl_cust", "Cust_No", txtCust_No.Text.Trim(), true))
                        {
                            MessageBox.Show("این شماره اشتراک قبلاً برای فرد دیگری ثبت شده است");
                            txtCust_No.Select();
                            txtCust_No.SelectAll();
                        }
                        else if (txtAddr.Text.Trim() == "")
                        {
                            MessageBox.Show("آدرس نباید خالی باشد");
                            txtAddr.Select();
                            txtAddr.SelectAll();
                        }
                        else
                        {
                            query += "insert into tbl_cust (Cust_No,cntr_No,fname,lname,addr)";
                            query += " values (" + txtCust_No.Text.Trim() + ","
                                + txtCntr_No.Text.Trim() +
                                ",'" + txtFName.Text.Trim() +
                                "','" + txtLName.Text.Trim() +
                                "','" + txtAddr.Text.Trim() +
                                "')";
                            dataAccess.ExecuteAccess(query);
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("لطفاً شماره کنتور و شماره اشتراک را وارد نمایید");
                        txtCntr_No.Select();
                    }
                }
            }
            else
            {
                MessageBox.Show("لطفاً نام و نام خانوادگی را وارد نمایید");
            }
        }
    }
}
