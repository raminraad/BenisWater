using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
//using FarsiLibrary.Win.Controls;
using Telerik.WinControls.UI;


namespace Benis
{
    public static class CLSValidityCheck
    {
        #region Global Members
        static Color colorValid = Color.White;
        static Color colorInvalid = Color.Yellow;
       static Color colorEmpty = Color.Tomato;
        public static DialogResult dialogResult = DialogResult.None;
        #endregion

        #region Methods
        public static bool IsInt(TextBox txt)
        {
            txt.Text = txt.Text.Trim();
            Int64 i;
            try
            {
                i = Convert.ToInt64(txt.Text);
                txt.BackColor = colorValid;
                return true;
            }
            catch
            {
                txt.BackColor = colorInvalid;
                return false;
            }
        }
        public static bool IsInt(RadTextBox txt)
        {
            txt.Text = txt.Text.Trim();
            Int64 i;
            try
            {
                i = Convert.ToInt64(txt.Text);
                txt.BackColor = colorValid;
                return true;
            }
            catch
            {
                txt.BackColor = colorInvalid;
                return false;
            }
        }
        public static bool IsInt(string txt)
        {
            txt = txt.Trim();
            Int64 i;
            try
            {
                i = Convert.ToInt64(txt);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //public static bool IsDate(FADatePicker cmbDate)
        //{
        //    if (cmbDate.Text.Trim() == "") 
        //    {
        //        cmbDate.Text = "[هیج مقداری انتخاب نشده]";
        //        return true;
        //    }
        //    else if (cmbDate.Text.Trim() == "[هیج مقداری انتخاب نشده]")
        //    {
        //        return true;
        //    }
        //    try
        //    {
        //        FarsiLibrary.Utils.PersianDate pDate = FarsiLibrary.Utils.PersianDate.Parse(cmbDate.Text);
        //        cmbDate.BackColor = colorValid;
        //        return true;
        //    }
        //    catch
        //    {
        //        cmbDate.BackColor = colorInvalid;
        //        return false;
        //    }
        //}
        public static bool IsDate(string strInput)
        {
            try
            {
                FarsiLibrary.Utils.PersianDate pDate = FarsiLibrary.Utils.PersianDate.Parse(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDate(RadMaskedEditBox mskDate)
        {
            bool isValid = false;
            if (mskDate.Value.ToString() == "")
            {
                mskDate.BackColor = colorValid;
                return true;
            }
            try
            {
            if ((Convert.ToInt16(mskDate.Text.Substring(3, 2)) > 12 || Convert.ToInt16(mskDate.Text.Substring(3, 2)) < 1) ||
            (Convert.ToInt16(mskDate.Text.Substring(6, 2)) > 31 || Convert.ToInt16(mskDate.Text.Substring(6, 2)) < 1))
            {
                mskDate.BackColor = colorInvalid;
                return false;
            }
            else
            {
                try
                {
                    FarsiLibrary.Utils.PersianDate pDate = FarsiLibrary.Utils.PersianDate.Parse("13" + mskDate.Text);
                    isValid = true;
                    mskDate.BackColor = colorValid;
                }
                catch
                {
                    isValid = false;
                }
            }
            }
            catch
            {
                mskDate.BackColor = colorInvalid;
                return false;
            }
            
            if (isValid) mskDate.BackColor = colorValid;
            else mskDate.BackColor = colorInvalid;
            return isValid;
        }
        public static bool IsTime(RadMaskedEditBox msk)
        {
            bool isvalid = true;
            string strInput=msk.Value.ToString();
            if (strInput.Length == 0) isvalid = true;
            else if (strInput.Length < 4) isvalid = false;
            else
            {
                try
                {
                    int hour = Convert.ToInt16(strInput.Substring(0, 2));
                    int min = Convert.ToInt16(strInput.Substring(2, 2));
                    if (hour > 23 || hour < 0 || min > 59 || min < 0) isvalid = false;
                }
                catch
                {
                    isvalid = false;
                }
            }
            if (isvalid) msk.BackColor = colorValid;
            else msk.BackColor = colorInvalid;
            return isvalid;
        }

        public static bool HasText(TextBox txt)
        {
            txt.Text = txt.Text.Trim();
            if (txt.Text != "")
            {
                txt.BackColor = colorValid;
                return true;
            }
            else
            {
                txt.BackColor = colorEmpty;
                return false;
            }
        }
        public static bool HasText(RadMaskedEditBox msk)
        {
            msk.Text = msk.Text.Trim();
            if (msk.Text != "__:__")
            {
                msk.BackColor = colorValid;
                return true;
            }
            else
            {
                msk.BackColor = colorEmpty;
                return false;
            }
        }
        public static bool HasTextDontTrim(TextBox txt)
        {
            if (txt.Text != "")
            {
                txt.BackColor = colorValid;
                return true;
            }
            else
            {
                txt.BackColor = colorEmpty;
                return false;
            }
        }
        public static bool HasText(ComboBox cmb)
        {
            if (cmb.Text != "")
            {
                cmb.BackColor = colorValid;
                return true;
            }
            else
            {
                cmb.BackColor = colorEmpty;
                return false;
            }
        }
        public static bool HasText(RadComboBox cmb)
        {
            if (cmb.Text != "")
            {
                cmb.BackColor = colorValid;
                return true;
            }
            else
            {
                cmb.BackColor = colorEmpty;
                return false;
            }
        }
        //public static bool HasText(FADatePicker cmbDate)
        //{
        //    if (cmbDate.Text != "" && cmbDate.Text != "[هیج مقداری انتخاب نشده]")
        //    {
        //        cmbDate.BackColor = colorValid;
        //        return true;
        //    }
        //    else
        //    {
        //        cmbDate.BackColor = colorEmpty;
        //        return false;
        //    }
        //}
        public static void SetColorToValid(TextBox txt)
        {
            txt.BackColor = colorValid;
        }
        public static bool ExistInTable(string TableName, string CulumnToSearch, string SearchValue)
        {
            CLSDataAccess dataaAccess = new CLSDataAccess();
            if (dataaAccess.GetSQLDataSetByQuery("SELECT * FROM " + TableName + " WHERE " + CulumnToSearch + " = " + SearchValue).Tables[0].Rows.Count>0) return true;
            else return false;
        }
        public static bool ExistInTable(string TableName, string Criteria)
        {
            CLSDataAccess dataaAccess = new CLSDataAccess();
            if (dataaAccess.GetSQLDataSetByQuery("SELECT * FROM " + TableName + " WHERE " + Criteria).Tables[0].Rows.Count > 0) return true;
            else return false;
        }
        public static bool HasRows(string TableName)
        {
            CLSDataAccess dataaAccess = new CLSDataAccess();
            if (dataaAccess.GetSQLDataSetByQuery("SELECT * FROM " + TableName ).Tables[0].Rows.Count > 0) return true;
            else return false;
        }
        public static string GenerateValidityDate(string ExportDate)
        {
            return (Convert.ToInt32(ExportDate.Substring(0,4))+5).ToString()+ExportDate.Substring(4,6);
        }

        /*
        #region Messages
        public void ShowMessageFieldIsNotValid()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً نوع داده فیلدهای مشخص شده با رنگ زرد را تصحیح فرمایید");
        }
        public void ShowMessageServiceDoesNotExist()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage( "سرویس حرکت برای این تور تعریف نشده است. لطفاً ابتدا از قسمت ویرایش تور ، سرویسهای حرکت تور را تعریف نمایید");
        }
        public void ShowMessageCityTableIsEmpty()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً قبل از ورود به این قسمت ، کشورها و شهرهای مورد نظر را وارد نمایید.");
        }
        public void ShowMessageFieldsAreEmpty()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage( "پر نمودن فیلدهای مشخص شده با رنگ قرمز الزامی میباشد. لطفاً مقادیر مربوطه را وارد نمایید");
        }
        public void ShowMessageSeatHasOwner(BOCCustomer SeatOwner)
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage("این شماره صندلی قبلاً برای "+SeatOwner.FName+" " +SeatOwner.LName+" ثبت شده است. لطفاً شماره صندلی را تغییر دهید.");
        }
        public void ShowMessageSeatHasOwner()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage("شماره صندلی های این قرارداد ، با شماره صندلی های ثبت شده در قراردادهای دیگر اشتراک دارد. لطفاً شماره صندلی ها و یا سرویس حرکت را تغییر دهید.");
        }
        public void ShowMessageSetToUpdateModeTrip()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage( "لطفاً جهت اعمال تغییرات ، ابتدا ویرایش تور را کلیک کنید");
        }
        public void ShowMessageSetToUpdateModeTicket()
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً جهت اعمال تغییرات ، ابتدا ویرایش بلیط را کلیک کنید");
        }
        public void ShowMessagePassengerHasAlreadyAddedToService(BOCContract Contract)
        {
            frmClassLibraryMessage message = new frmClassLibraryMessage("این مسافر قبلاً در قرارداد شماره "+Contract.ContractCode+" برای این تور با همین سرویس حرکت ثبت نام کرده است.");
        }
        #endregion
        */
        /*
        #region Form Show Validity Check
        public bool frmShowIsValidTicketMng(bool ShowMessage)
        {
            if (HasRows("TblTourCity"))
                return true;
            else
            {
                frmClassLibraryMessage frm = new frmClassLibraryMessage("لطفاً ابتدا اطلاعات کشورها و شهرهای مربوطه را وارد نمایید.");
            }
            return false;
        }
        #endregion
        */
        /*
        #region Insert Validity Check
        public BOCCustomer InsertIsValidBusPassenger(BOCCustomer Customer,BOCTripService Service,TextBox txtBusNo,TextBox  txtSeatNo,DataTable DtPassenger)
        {
            BOCCustomer seatOwner=new BOCCustomer();
            foreach (DataRow datarow in DtPassenger.Rows)
            {
                if (datarow["SeatNo"].ToString() == txtSeatNo.Text && datarow["BusNo"].ToString() == txtBusNo.Text && datarow["PassengerID"].ToString() != Customer.ID)
                {
                    seatOwner.ID=datarow["PassengerID"].ToString();
                    txtBusNo.BackColor = txtSeatNo.BackColor = colorInvalid;
                }
            }
            if (seatOwner.ID == "0") 
            {
                CLSDataAccess dataaccess = new CLSDataAccess();
                DataTable dt = dataaccess.GetSQLDataSetByQuery("SELECT * FROM TblTourContractCustomer WHERE CustomerID <> " + Customer.ID + " AND SeatNo = " + txtSeatNo.Text + " AND BusNo = " + txtBusNo.Text + " AND ContractID IN (SELECT ID FROM TblTourContract WHERE TripServiceID = " + Service.ID + ")").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    seatOwner.ID = dt.Rows[0]["CustomerID"].ToString();
                    txtBusNo.BackColor = txtSeatNo.BackColor = colorInvalid;
                }
            }
            return seatOwner;
        }
        public bool InsertIsValidBusPassenger(BOCTripService Service,BOCContract Contract,DataTable DtPassenger,bool ShowMessage)
        {
            CLSDataAccess dataaccess = new CLSDataAccess();
            foreach (DataRow datarow in DtPassenger.Rows)
            {
                DataTable dt = dataaccess.GetSQLDataSetByQuery("SELECT ID FROM TblTourContractCustomer WHERE SeatNo = " + datarow["SeatNo"].ToString() + " AND BusNo = " + datarow["BusNo"].ToString() + " AND ContractID IN (SELECT ID FROM TblTourContract WHERE TripServiceID = " + Service.ID + " AND ID <> "+Contract.ID+")").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (ShowMessage) ShowMessageSeatHasOwner();
                    return false;
                }
            }
            return true;
        }
        public bool InsertIsValidPassenger(BOCTripService Service, BOCContract Contract, BOCCustomer Customer, bool ShowMessage)
        {
            CLSDataAccess dataaccess = new CLSDataAccess();
                DataTable dt = dataaccess.GetSQLDataSetByQuery("SELECT ContractID FROM TblTourContractCustomer WHERE CustomerID = "+Customer.ID+ " AND ContractID IN (SELECT ID FROM TblTourContract WHERE TripServiceID = " + Service.ID + " AND ID <> " + Contract.ID + ")").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    BOCContract contract = new BOCContract(dt.Rows[0]["ContractID"].ToString());
                    if (ShowMessage) ShowMessagePassengerHasAlreadyAddedToService(contract);
                    return false;
                }
            
            return true;
        }
        public bool InsertIsValidUser(BOCUser User, bool ShowMessage)
        {
            CLSDataAccess dataaccess = new CLSDataAccess();
            if (dataaccess.GetSQLDataSetByQuery("SELECT * FROM TblTourUser WHERE Username = N'" + User.Username + "'").Tables[0].Rows.Count > 0)
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage frm = new frmClassLibraryMessage("این نام کاربری قبلاً تعریف شده است. لطفاً یک نام جدید انتخاب نمایید.");
                }
                return false;
            }
            return true;
        }
        #endregion

        #region Remove Validity Check
        public bool RemoveContract(BOCContract Contract,bool ShowMessage)
        {
            if (ExistInTable("TblTourChequeContract", "ContractID", Contract.ID) || ExistInTable("TblTourSettleContract", "ContractID", Contract.ID) || ExistInTable("TblTourCashContract", "ContractID", Contract.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه اطلاعات مالی قرارداد را حذف نمایید.");
                }
                return false;
            }
            else
            {
                frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                message.ShowMessageContractRemove();
                message.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    CLSDataAccess dataaccess = new CLSDataAccess();
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourContractCustomer WHERE ContractID = " + Contract.ID);
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourContract WHERE ID = " + Contract.ID);
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveTicket(BOCTicket Ticket, bool ShowMessageNotValid,bool ShowMessageConfirm)
        {
            if (ExistInTable("TblTourChequeTicket", "TicketID", Ticket.ID) || ExistInTable("TblTourSettleTicket", "TicketID", Ticket.ID) || ExistInTable("TblTourCashTicket", "TicketID", Ticket.ID))
            {
                if (ShowMessageNotValid)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه اطلاعات مالی بلیط شماره " + Ticket.TicketCodeL + "  را حذف نمایید.");
                }
                return false;
            }
            else
            {
                if (ShowMessageConfirm)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                    message.ShowMessageTicketRemove();
                    message.ShowDialog();
                }
                else dialogResult = DialogResult.Yes;
                if (dialogResult == DialogResult.Yes)
                {
                    CLSDataAccess dataaccess = new CLSDataAccess();
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTicket WHERE ID = " + Ticket.ID);
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveTicketForTrip(BOCTicket Ticket, bool ShowMessageNotValid, bool ShowMessageConfirm)
        {
            //if (ExistInTable("TblTourChequeTicket", "TicketID", Ticket.ID) || ExistInTable("TblTourSettleTicket", "TicketID", Ticket.ID) || ExistInTable("TblTourCashTicket", "TicketID", Ticket.ID))
            //{
            //    if (ShowMessageNotValid)
            //    {
            //        frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه اطلاعات مالی بلیط شماره " + Ticket.TicketCodeL + "  را حذف نمایید.");
            //    }
            //    return false;
            //}
            //else
            //{
                if (ShowMessageConfirm)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                    message.ShowMessageTicketRemove();
                    message.ShowDialog();
                }
                else dialogResult = DialogResult.Yes;
                if (dialogResult == DialogResult.Yes)
                {
                    CLSDataAccess dataaccess = new CLSDataAccess();
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTicketForTrip WHERE ID = " + Ticket.ID);
                    return true;
                }
                else return false;
            //}
        }
        public bool RemoveCustomer(BOCCustomer Customer, bool ShowMessage)
        {
            if (ExistInTable("TblTourContractCustomer", "CustomerID", Customer.ID) || ExistInTable("TblTourContract", "OwnerID", Customer.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه قراردادهای مربوط به این مشتری را حذف نمایید.");
                }
                return false;
            }
            else if (ExistInTable("TblTourTicket", "CustomerID", Customer.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه بلیط های صادر شده برای این مشتری را حذف نمایید.");
                }
                return false;
            }
            else if (ExistInTable("TblTourCheque", "AccountOwnerID", Customer.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه چک های مربوط به حساب بانکی این مشتری را حذف نمایید.");
                }
                return false;
            }
            else if (ExistInTable("TblTourCasheerTransactionCash", "CustomerID", Customer.ID) || ExistInTable("TblTourCasheerTransactionCheque", "CustomerID", Customer.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه دریافت ها و پرداخت های این مشتری را حذف نمایید.");
                }
                return false;
            }
            else
            {
                frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                message.ShowMessageRemoveCustomer();
                message.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    CLSDataAccess dataaccess = new CLSDataAccess();
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTripServiceReservation WHERE CustomerID = " + Customer.ID);
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourCustomer WHERE ID = " + Customer.ID);
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveTripService(BOCTripService Service, bool ShowMessage)
        {
            if ( ExistInTable("TblTourContract", "TripServiceID", Service.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه قراردادهای مربوط به این سرویس را حذف نمایید.");
                }
                return false;
            }
            else
            {
                frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                message.ShowMessageRemoveTripService();
                message.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    CLSDataAccess dataaccess = new CLSDataAccess();
                    dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTripService WHERE ID = " + Service.ID);
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveTrip(BOCTrip Trip, bool ShowMessage)
        {
            CLSDataAccess dataaccess = new CLSDataAccess();
            foreach (DataRow datarow in dataaccess.GetSQLDataSetByQuery("SELECT ID FROM TblTourTripService WHERE TripID = " + Trip.ID).Tables[0].Rows) 
            {
                if (ExistInTable("TblTourContract", "TripServiceID", datarow["ID"].ToString())) 
                {
                    if (ShowMessage)
                    {
                        frmClassLibraryMessage message = new frmClassLibraryMessage("لطفاً ابتدا کلیه قراردادهای مربوط به این تور را حذف نمایید.");
                    }
                    return false;
                }
            }

            frmClassLibraryMessage messageConfirm = new frmClassLibraryMessage(this, "");
            messageConfirm.ShowMessageRemoveTrip();
            messageConfirm.ShowDialog();
            if (dialogResult == DialogResult.Yes)
            {
                dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTripService WHERE TripID = " + Trip.ID);
                dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTripCity WHERE TripID = " + Trip.ID);
                dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourUserTrip WHERE TripID = " + Trip.ID);
                dataaccess.ExecuteSQLNonQuery("DELETE FROM TblTourTrip WHERE ID = " + Trip.ID);
                return true;
            }
            else return false;

        }
        public bool RemoveCity(BOCCity City, bool ShowMessageInvalid,bool ShowMessageConfirm)
        {
            if (ExistInTable("TblTourTripCity", "CityID", City.ID))
            {
                if (ShowMessageInvalid)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("برای حذف این شهر ابتدا باید تورهای مربوط به آن را حذف نمایید.");
                }
                return false;
            }
            else if (ExistInTable("TblTourTicket", "SourceCityID", City.ID) || ExistInTable("TblTourTicket", "DestinationCityID", City.ID))
            {
                if (ShowMessageInvalid)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("برای حذف این شهر ابتدا باید بلیط های مربوط به آن را حذف نمایید.");
                }
                return false;
            }
            else
            {
                dialogResult = DialogResult.Yes;
                if (ShowMessageConfirm)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                    message.ShowMessageRemoveCity();
                    message.ShowDialog();
                }
                if (dialogResult == DialogResult.Yes)
                {
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveCountry(BOCCountry Country, bool ShowMessage)
        {
            bool removeIsValid = true;
            foreach (BOCCity city in Country.City)
                if (!RemoveCity(city,false,false))
                {
                    removeIsValid = false;
                }
            if (removeIsValid)
            {
                frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                message.ShowMessageRemoveCountry();
                message.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("برای حذف این کشور ابتدا باید کلیه تورها و بلیط های مربوط به شهرهای آن را حذف نمایید.");
                }
                return false;
            }
        }
        public bool RemoveBranch(BOCBranch Branch, bool ShowMessage)
        {
            if (ExistInTable("TblTourCheque", "BranchID", Branch.ID))
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("برای حذف این شعبه ، ابتدا باید چک های مربوط به آن را حذف نمایید.");
                }
                return false;
            }
            else
            {
                frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                message.ShowMessageRemoveBranch();
                message.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveBank(BOCBank Bank, bool ShowMessage)
        {
            bool removeIsValid = true;
            foreach (BOCBranch branch in Bank.Branch)
                if (ExistInTable("TblTourCheque", "BranchID", branch.ID))
                {
                    removeIsValid = false;
                }
            if (removeIsValid)
            {
                frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                message.ShowMessageRemoveBank();
                message.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (ShowMessage)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("برای حذف این بانک ، ابتدا باید کلیه چک های مربوط به شعبه های آن را حذف نمایید.");
                }
                return false;
            }
        }
        public bool RemoveCheque(BOCCheque Cheque, bool ShowMessageNotValid, bool ShowMessageConfirm)
        {
            bool RemoveIsValid = true;
            if (!RemoveIsValid)
            {
                if (ShowMessageNotValid)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("Cheque Remove Is Not Valid");
                }
                return false;
            }
            else
            {
                if (ShowMessageConfirm)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                    message.ShowMessageChequeRemove();
                    message.ShowDialog();
                }
                else dialogResult = DialogResult.Yes;
                if (dialogResult == DialogResult.Yes)
                {
                    Cheque.Delete();
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveSettle(BOCSettle Settle, bool ShowMessageNotValid, bool ShowMessageConfirm)
        {
            bool RemoveIsValid = true;
            if (!RemoveIsValid)
            {
                if (ShowMessageNotValid)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("Settle Remove Is Not Valid");
                }
                return false;
            }
            else
            {
                if (ShowMessageConfirm)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                    message.ShowMessageSettleRemove();
                    message.ShowDialog();
                }
                else dialogResult = DialogResult.Yes;
                if (dialogResult == DialogResult.Yes)
                {
                    Settle.Delete();
                    return true;
                }
                else return false;
            }
        }
        public bool RemoveCash(BOCCash Cash, bool ShowMessageNotValid, bool ShowMessageConfirm)
        {
            bool RemoveIsValid = true;
            if (!RemoveIsValid)
            {
                if (ShowMessageNotValid)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage("Cash Remove Is Not Valid");
                }
                return false;
            }
            else
            {
                if (ShowMessageConfirm)
                {
                    frmClassLibraryMessage message = new frmClassLibraryMessage(this, "");
                    message.ShowMessageCashRemove();
                    message.ShowDialog();
                }
                else dialogResult = DialogResult.Yes;
                if (dialogResult == DialogResult.Yes)
                {
                    Cash.Delete();
                    return true;
                }
                else return false;
            }
        }
        #endregion

        #region License Validity Check
        public bool LicenseIsOK(bool ShowMessage,string StopPoint)
        {
            bool trust = false;
            if (System.IO.File.Exists(Application.StartupPath + @"\HardwareSetting.log"))
            {
                CLSStringEncoder clsStrEnc = new CLSStringEncoder("1RaMin1");
                clsStrEnc.Input = System.IO.File.ReadAllText(Application.StartupPath + @"\HardwareSetting.log");
                clsStrEnc.KeyValue = 5;
                if (Module.GetCPUSerial() == clsStrEnc.OutputDecode && StopPoint == "Break my heart but not my lock!")
                    trust = true;
            }
            if (!trust && ShowMessage)
            {
                frmClassLibraryMessage frm = new frmClassLibraryMessage("این کپی از نرم افزار غیر مجاز می باشد. لطفاً با شرکت تولید کننده نرم افزار تماس حاصل فرمایید.");
            }
            return trust;
        }
        public bool RememberFileIsOK(string StopPoint)
        {
            bool isOK = false;
            CLSStringEncoder clsStrEnc = new CLSStringEncoder("1RaMin1");
            DataTable dt = new DataTable();
            dt.ReadXmlSchema(Application.StartupPath + @"\Login.scm");
            dt.ReadXml(Application.StartupPath + @"\Login.log");
            clsStrEnc.Input = dt.Rows[0]["HWS"].ToString();
            clsStrEnc.KeyValue = 6;
            if (Module.GetCPUSerial() == clsStrEnc.OutputDecode && StopPoint == "Break my heart but not my lock!")
                isOK = true;
            return isOK;
        }
        #endregion

         */
        #endregion
         
    }
}
