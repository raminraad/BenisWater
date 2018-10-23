namespace Benis
{
    partial class frmPaymentInsertUpdate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaymentInsertUpdate));
            this.cmbCust = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel9 = new Telerik.WinControls.UI.RadLabel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.btnLock = new Telerik.WinControls.UI.RadButton();
            this.mskPayDate = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.txtPayBalance = new Telerik.WinControls.UI.RadTextBox();
            this.breezeExtendedTheme = new Telerik.WinControls.Themes.BreezeExtendedTheme();
            this.breezeTheme = new Telerik.WinControls.Themes.BreezeTheme();
            this.btnReturn = new Telerik.WinControls.UI.RadButton();
            this.btnInsert = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mskPayDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayBalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInsert)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCust
            // 
            this.cmbCust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCust.DropDownMinSize = new System.Drawing.Size(650, 350);
            this.cmbCust.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            // 
            // cmbCust.NestedRadGridView
            // 
            this.cmbCust.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCust.EditorControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbCust.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbCust.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCust.EditorControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmbCust.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.cmbCust.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.cmbCust.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.cmbCust.EditorControl.MasterTemplate.AllowColumnChooser = false;
            gridViewTextBoxColumn1.FieldName = "Cust_No";
            gridViewTextBoxColumn1.HeaderText = "شماره اشتراک";
            gridViewTextBoxColumn1.Name = "Cust_No";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn2.FieldName = "Cntr_no";
            gridViewTextBoxColumn2.HeaderText = "شماره کنتور";
            gridViewTextBoxColumn2.Name = "Cntr_no";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 123;
            gridViewTextBoxColumn3.FieldName = "FName";
            gridViewTextBoxColumn3.HeaderText = "نام";
            gridViewTextBoxColumn3.Name = "FName";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.Width = 108;
            gridViewTextBoxColumn4.FieldName = "LName";
            gridViewTextBoxColumn4.HeaderText = "نام خانوادگی";
            gridViewTextBoxColumn4.Name = "LName";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.Width = 111;
            gridViewTextBoxColumn5.FieldName = "Addr";
            gridViewTextBoxColumn5.HeaderText = "آدرس";
            gridViewTextBoxColumn5.Name = "Addr";
            gridViewTextBoxColumn5.ReadOnly = true;
            gridViewTextBoxColumn5.Width = 284;
            this.cmbCust.EditorControl.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.cmbCust.EditorControl.MasterTemplate.EnableGrouping = false;
            this.cmbCust.EditorControl.MasterTemplate.ReadOnly = true;
            this.cmbCust.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.cmbCust.EditorControl.Name = "NestedRadGridView";
            this.cmbCust.EditorControl.ReadOnly = true;
            this.cmbCust.EditorControl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbCust.EditorControl.ShowGroupPanel = false;
            this.cmbCust.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.cmbCust.EditorControl.TabIndex = 0;
            this.cmbCust.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.cmbCust.Location = new System.Drawing.Point(76, 23);
            this.cmbCust.Margin = new System.Windows.Forms.Padding(4);
            this.cmbCust.Name = "cmbCust";
            // 
            // 
            // 
            this.cmbCust.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cmbCust.Size = new System.Drawing.Size(123, 25);
            this.cmbCust.TabIndex = 0;
            this.cmbCust.TabStop = false;
            this.cmbCust.ThemeName = "BreezeExtended";
            this.cmbCust.UseCompatibleTextRendering = false;
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.radLabel1.Location = new System.Drawing.Point(213, 26);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(100, 20);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "شماره اشتراک:";
            // 
            // radLabel2
            // 
            this.radLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.radLabel2.Location = new System.Drawing.Point(213, 143);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(98, 20);
            this.radLabel2.TabIndex = 3;
            this.radLabel2.Text = "مبلغ پرداختــی:";
            // 
            // radLabel9
            // 
            this.radLabel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.radLabel9.Location = new System.Drawing.Point(211, 85);
            this.radLabel9.Name = "radLabel9";
            this.radLabel9.Size = new System.Drawing.Size(101, 20);
            this.radLabel9.TabIndex = 10;
            this.radLabel9.Text = "تاریخ پرداخــــت:";
            // 
            // radPanel1
            // 
            this.radPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radPanel1.Controls.Add(this.btnLock);
            this.radPanel1.Controls.Add(this.mskPayDate);
            this.radPanel1.Controls.Add(this.txtPayBalance);
            this.radPanel1.Controls.Add(this.radLabel9);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.cmbCust);
            this.radPanel1.Location = new System.Drawing.Point(12, 12);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(305, 186);
            this.radPanel1.TabIndex = 0;
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLock.Image = ((System.Drawing.Image)(resources.GetObject("btnLock.Image")));
            this.btnLock.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLock.Location = new System.Drawing.Point(35, 76);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(35, 35);
            this.btnLock.TabIndex = 2;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // mskPayDate
            // 
            this.mskPayDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mskPayDate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mskPayDate.Culture = new System.Globalization.CultureInfo("fa-IR");
            this.mskPayDate.EnableTheming = false;
            this.mskPayDate.Location = new System.Drawing.Point(76, 83);
            this.mskPayDate.Mask = "##/##/##";
            this.mskPayDate.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.mskPayDate.Name = "mskPayDate";
            this.mskPayDate.Size = new System.Drawing.Size(123, 28);
            this.mskPayDate.TabIndex = 1;
            this.mskPayDate.TabStop = false;
            this.mskPayDate.Text = "__/__/__";
            this.mskPayDate.UseGenericBorderPaint = true;
            this.mskPayDate.Value = "";
            this.mskPayDate.Leave += new System.EventHandler(this.mskPayDate_Leave);
            // 
            // txtPayBalance
            // 
            this.txtPayBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPayBalance.BackColor = System.Drawing.Color.White;
            this.txtPayBalance.EnableTheming = false;
            this.txtPayBalance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtPayBalance.Location = new System.Drawing.Point(76, 140);
            this.txtPayBalance.Name = "txtPayBalance";
            this.txtPayBalance.Size = new System.Drawing.Size(123, 26);
            this.txtPayBalance.TabIndex = 3;
            this.txtPayBalance.TabStop = false;
            this.txtPayBalance.UseGenericBorderPaint = true;
            this.txtPayBalance.Leave += new System.EventHandler(this.txtPayBalance_Leave);
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReturn.Image = ((System.Drawing.Image)(resources.GetObject("btnReturn.Image")));
            this.btnReturn.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReturn.Location = new System.Drawing.Point(215, 207);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(48, 48);
            this.btnReturn.TabIndex = 2;
            // 
            // btnInsert
            // 
            this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsert.Image = ((System.Drawing.Image)(resources.GetObject("btnInsert.Image")));
            this.btnInsert.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnInsert.Location = new System.Drawing.Point(269, 207);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(48, 48);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // frmPaymentInsertUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnReturn;
            this.ClientSize = new System.Drawing.Size(329, 266);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.btnReturn);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPaymentInsertUpdate";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ثبت مبالغ پرداختی";
            ((System.ComponentModel.ISupportInitialize)(this.cmbCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mskPayDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayBalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInsert)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadMultiColumnComboBox cmbCust;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel9;
        //private Telerik.WinControls.UI.RadTextBox txtCounter;
        //private Telerik.WinControls.UI.RadTextBox txtRenovation;
        //private Telerik.WinControls.UI.RadTextBox txtGarbage;
        //private Telerik.WinControls.UI.RadTextBox txtOther;
        //private Telerik.WinControls.UI.RadButton btnInsertUsage;
        private Telerik.WinControls.UI.RadButton btnReturn;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.Themes.BreezeExtendedTheme breezeExtendedTheme;
        private Telerik.WinControls.Themes.BreezeTheme breezeTheme;
        private Telerik.WinControls.UI.RadTextBox txtPayBalance;
        private Telerik.WinControls.UI.RadButton btnInsert;
        private Telerik.WinControls.UI.RadMaskedEditBox mskPayDate;
        private Telerik.WinControls.UI.RadButton btnLock;
    }
}