using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;

namespace Benis
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int MaxTerm=0;
            try
            {
                MaxTerm = Convert.ToInt16((new CLSDataAccess()).GetAccessDataSetByQuery("select max(term_no) as MaxTerm from tbl_usage").Tables[0].Rows[0]["MaxTerm"]);
            }
            catch
            {
                MessageBox.Show("بانک اطلاعاتی سیستم خالی است. لطفاً پس از اجرای برنامه، فایل پشتیبان را بازگردانی نمایید");
                MaxTerm = 11;
            }
	        var maxTermAllowedPath = Application.StartupPath + "\\pg.mxt";
	        var maxAllowedTerm = System.IO.File.Exists(maxTermAllowedPath) ? Convert.ToInt16(System.IO.File.ReadAllText(maxTermAllowedPath).Replace("FastReportDllVersion:",string.Empty).Replace(".11.0",string.Empty)):1;
			if (!System.IO.File.Exists(Application.StartupPath + "\\pg.lcc") && MaxTerm >= maxAllowedTerm)
                {
                    MessageBox.Show(@"This file is not compatible with your operating system..","",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.RightAlign);
                    Application.Exit();
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    frmLogin frm = new frmLogin();
                    if (frm.ShowDialog() == DialogResult.OK)
                        Application.Run(new frmMain());
                    else
                        Application.Exit();
                }
            
        }
    }
}
