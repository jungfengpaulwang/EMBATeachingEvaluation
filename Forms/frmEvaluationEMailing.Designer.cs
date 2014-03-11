namespace TeachingEvaluation.Forms
{
    partial class frmEvaluationEMailing
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.cboCourse = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.nudSchoolYear = new System.Windows.Forms.NumericUpDown();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.circularProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnSendEmail = new DevComponents.DotNetBar.ButtonX();
            this.btnSendSecondEmail = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.itemContainer5 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.txtSenderEMail = new DevComponents.DotNetBar.TextBoxItem();
            this.itemContainer6 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer2 = new DevComponents.DotNetBar.ItemContainer();
            this.txtSenderName = new DevComponents.DotNetBar.TextBoxItem();
            this.itemContainer7 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer3 = new DevComponents.DotNetBar.ItemContainer();
            this.txtCC = new DevComponents.DotNetBar.TextBoxItem();
            this.itemContainer4 = new DevComponents.DotNetBar.ItemContainer();
            this.labelItem4 = new DevComponents.DotNetBar.LabelItem();
            this.btnSave = new DevComponents.DotNetBar.ButtonItem();
            this.btnCancel = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem6 = new DevComponents.DotNetBar.LabelItem();
            this.labelItem7 = new DevComponents.DotNetBar.LabelItem();
            this.labelItem5 = new DevComponents.DotNetBar.LabelItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblMessage = new DevComponents.DotNetBar.LabelX();
            this.cSubjectName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cSubjectCode = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cNewSubjectCode = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cCourseName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cTeacherName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cOpeningTime = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cEndTime = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cAttendNo = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cSurveyNo = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.Status = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cEMailingTime = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.cSecondEMailingTime = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSchoolYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToOrderColumns = true;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cSubjectName,
            this.cSubjectCode,
            this.cNewSubjectCode,
            this.cCourseName,
            this.cTeacherName,
            this.cOpeningTime,
            this.cEndTime,
            this.cAttendNo,
            this.cSurveyNo,
            this.Status,
            this.cEMailingTime,
            this.cSecondEMailingTime});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvData.Location = new System.Drawing.Point(25, 68);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersWidth = 25;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(957, 432);
            this.dgvData.TabIndex = 82;
            this.dgvData.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.DataGridView_SortCompare);
            // 
            // cboCourse
            // 
            this.cboCourse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCourse.DisplayMember = "Text";
            this.cboCourse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCourse.FormattingEnabled = true;
            this.cboCourse.ItemHeight = 19;
            this.cboCourse.Location = new System.Drawing.Point(393, 25);
            this.cboCourse.Name = "cboCourse";
            this.cboCourse.Size = new System.Drawing.Size(589, 25);
            this.cboCourse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboCourse.TabIndex = 81;
            this.cboCourse.SelectedIndexChanged += new System.EventHandler(this.cboCourse_SelectedIndexChanged);
            // 
            // cboSemester
            // 
            this.cboSemester.DisplayMember = "Text";
            this.cboSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSemester.FormattingEnabled = true;
            this.cboSemester.ItemHeight = 19;
            this.cboSemester.Location = new System.Drawing.Point(220, 25);
            this.cboSemester.Name = "cboSemester";
            this.cboSemester.Size = new System.Drawing.Size(89, 25);
            this.cboSemester.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSemester.TabIndex = 80;
            this.cboSemester.SelectedIndexChanged += new System.EventHandler(this.cboSemester_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(184, 26);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(34, 21);
            this.labelX1.TabIndex = 79;
            this.labelX1.Text = "學期";
            // 
            // nudSchoolYear
            // 
            this.nudSchoolYear.Location = new System.Drawing.Point(74, 25);
            this.nudSchoolYear.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudSchoolYear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSchoolYear.Name = "nudSchoolYear";
            this.nudSchoolYear.Size = new System.Drawing.Size(89, 25);
            this.nudSchoolYear.TabIndex = 78;
            this.nudSchoolYear.Value = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.nudSchoolYear.ValueChanged += new System.EventHandler(this.nudSchoolYear_ValueChanged);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(25, 26);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(47, 21);
            this.labelX4.TabIndex = 77;
            this.labelX4.Text = "學年度";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(331, 27);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 21);
            this.labelX3.TabIndex = 76;
            this.labelX3.Text = "課程清單";
            // 
            // circularProgress
            // 
            this.circularProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.circularProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.circularProgress.BackgroundStyle.Class = "";
            this.circularProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circularProgress.Location = new System.Drawing.Point(430, 519);
            this.circularProgress.Name = "circularProgress";
            this.circularProgress.Size = new System.Drawing.Size(66, 23);
            this.circularProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circularProgress.TabIndex = 87;
            this.circularProgress.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(908, 519);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 86;
            this.btnExit.Text = "離　開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSendEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSendEmail.Location = new System.Drawing.Point(502, 519);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(187, 25);
            this.btnSendEmail.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSendEmail.TabIndex = 89;
            this.btnSendEmail.Text = "發送 Email 提醒通知";
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // btnSendSecondEmail
            // 
            this.btnSendSecondEmail.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSendSecondEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendSecondEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendSecondEmail.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSendSecondEmail.Location = new System.Drawing.Point(705, 519);
            this.btnSendSecondEmail.Name = "btnSendSecondEmail";
            this.btnSendSecondEmail.Size = new System.Drawing.Size(187, 25);
            this.btnSendSecondEmail.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSendSecondEmail.TabIndex = 90;
            this.btnSendSecondEmail.Text = "發送 Email 再次提醒通知";
            this.btnSendSecondEmail.Click += new System.EventHandler(this.btnSendSecondEmail_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonX1.AutoExpandOnClick = true;
            this.buttonX1.BackColor = System.Drawing.Color.Transparent;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(25, 519);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(187, 25);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer5,
            this.itemContainer1,
            this.itemContainer6,
            this.itemContainer2,
            this.itemContainer7,
            this.itemContainer3,
            this.itemContainer4,
            this.labelItem5});
            this.buttonX1.TabIndex = 91;
            this.buttonX1.Text = "設定寄件人及副本";
            // 
            // itemContainer5
            // 
            // 
            // 
            // 
            this.itemContainer5.BackgroundStyle.Class = "";
            this.itemContainer5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer5.Name = "itemContainer5";
            this.itemContainer5.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1});
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "　寄件人 Email";
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.Class = "";
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.HorizontalItemAlignment = DevComponents.DotNetBar.eHorizontalItemsAlignment.Center;
            this.itemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.ResizeItemsToFit = false;
            this.itemContainer1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.txtSenderEMail});
            // 
            // txtSenderEMail
            // 
            this.txtSenderEMail.AutoCollapseOnClick = false;
            this.txtSenderEMail.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.txtSenderEMail.Name = "txtSenderEMail";
            this.txtSenderEMail.Text = "textBoxItem1";
            this.txtSenderEMail.TextBoxWidth = 450;
            this.txtSenderEMail.WatermarkColor = System.Drawing.SystemColors.GrayText;
            // 
            // itemContainer6
            // 
            // 
            // 
            // 
            this.itemContainer6.BackgroundStyle.Class = "";
            this.itemContainer6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer6.Name = "itemContainer6";
            this.itemContainer6.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem2});
            // 
            // labelItem2
            // 
            this.labelItem2.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "　寄件人名稱";
            // 
            // itemContainer2
            // 
            // 
            // 
            // 
            this.itemContainer2.BackgroundStyle.Class = "";
            this.itemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer2.HorizontalItemAlignment = DevComponents.DotNetBar.eHorizontalItemsAlignment.Center;
            this.itemContainer2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer2.Name = "itemContainer2";
            this.itemContainer2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.txtSenderName});
            // 
            // txtSenderName
            // 
            this.txtSenderName.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.txtSenderName.Name = "txtSenderName";
            this.txtSenderName.Text = "textBoxItem2";
            this.txtSenderName.TextBoxWidth = 450;
            this.txtSenderName.WatermarkColor = System.Drawing.SystemColors.GrayText;
            // 
            // itemContainer7
            // 
            // 
            // 
            // 
            this.itemContainer7.BackgroundStyle.Class = "";
            this.itemContainer7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer7.Name = "itemContainer7";
            this.itemContainer7.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem3});
            // 
            // labelItem3
            // 
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Text = "　副本";
            // 
            // itemContainer3
            // 
            // 
            // 
            // 
            this.itemContainer3.BackgroundStyle.Class = "";
            this.itemContainer3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer3.HorizontalItemAlignment = DevComponents.DotNetBar.eHorizontalItemsAlignment.Center;
            this.itemContainer3.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer3.Name = "itemContainer3";
            this.itemContainer3.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.txtCC});
            // 
            // txtCC
            // 
            this.txtCC.AutoCollapseOnClick = false;
            this.txtCC.CanCustomize = false;
            this.txtCC.Name = "txtCC";
            this.txtCC.TextBoxWidth = 450;
            this.txtCC.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.txtCC.WatermarkText = "以半形逗號分隔多筆資料";
            // 
            // itemContainer4
            // 
            // 
            // 
            // 
            this.itemContainer4.BackgroundStyle.Class = "";
            this.itemContainer4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer4.HorizontalItemAlignment = DevComponents.DotNetBar.eHorizontalItemsAlignment.Right;
            this.itemContainer4.Name = "itemContainer4";
            this.itemContainer4.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem4,
            this.btnSave,
            this.btnCancel,
            this.labelItem6,
            this.labelItem7});
            // 
            // labelItem4
            // 
            this.labelItem4.Name = "labelItem4";
            this.labelItem4.Text = "　";
            // 
            // btnSave
            // 
            this.btnSave.AutoCollapseOnClick = false;
            this.btnSave.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.btnSave.Name = "btnSave";
            this.btnSave.Text = "儲　存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoCollapseOnClick = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Text = "取　消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelItem6
            // 
            this.labelItem6.Name = "labelItem6";
            // 
            // labelItem7
            // 
            this.labelItem7.Name = "labelItem7";
            // 
            // labelItem5
            // 
            this.labelItem5.GlobalItem = false;
            this.labelItem5.Height = 1;
            this.labelItem5.Name = "labelItem5";
            this.labelItem5.Width = 500;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblMessage.BackgroundStyle.Class = "";
            this.lblMessage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMessage.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Location = new System.Drawing.Point(220, 520);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(211, 23);
            this.lblMessage.TabIndex = 92;
            // 
            // cSubjectName
            // 
            this.cSubjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cSubjectName.HeaderText = "課程";
            this.cSubjectName.Name = "cSubjectName";
            this.cSubjectName.ReadOnly = true;
            this.cSubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cSubjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cSubjectName.Width = 59;
            // 
            // cSubjectCode
            // 
            this.cSubjectCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cSubjectCode.HeaderText = "課程識別碼";
            this.cSubjectCode.Name = "cSubjectCode";
            this.cSubjectCode.ReadOnly = true;
            this.cSubjectCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cSubjectCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cSubjectCode.Width = 98;
            // 
            // cNewSubjectCode
            // 
            this.cNewSubjectCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cNewSubjectCode.HeaderText = "課號";
            this.cNewSubjectCode.Name = "cNewSubjectCode";
            this.cNewSubjectCode.ReadOnly = true;
            this.cNewSubjectCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cNewSubjectCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cNewSubjectCode.Width = 59;
            // 
            // cCourseName
            // 
            this.cCourseName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cCourseName.HeaderText = "開課";
            this.cCourseName.Name = "cCourseName";
            this.cCourseName.ReadOnly = true;
            this.cCourseName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cCourseName.Width = 59;
            // 
            // cTeacherName
            // 
            this.cTeacherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cTeacherName.HeaderText = "授課教師";
            this.cTeacherName.Name = "cTeacherName";
            this.cTeacherName.ReadOnly = true;
            this.cTeacherName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cTeacherName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cTeacherName.Width = 85;
            // 
            // cOpeningTime
            // 
            this.cOpeningTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cOpeningTime.HeaderText = "問卷填寫開始時間";
            this.cOpeningTime.Name = "cOpeningTime";
            this.cOpeningTime.ReadOnly = true;
            this.cOpeningTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cOpeningTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cOpeningTime.Width = 137;
            // 
            // cEndTime
            // 
            this.cEndTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEndTime.HeaderText = "問卷填寫結束時間";
            this.cEndTime.Name = "cEndTime";
            this.cEndTime.ReadOnly = true;
            this.cEndTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cEndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cEndTime.Width = 137;
            // 
            // cAttendNo
            // 
            this.cAttendNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.cAttendNo.HeaderText = "修課學生數";
            this.cAttendNo.Name = "cAttendNo";
            this.cAttendNo.ReadOnly = true;
            this.cAttendNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cAttendNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cAttendNo.Width = 98;
            // 
            // cSurveyNo
            // 
            this.cSurveyNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.cSurveyNo.HeaderText = "填寫問卷數";
            this.cSurveyNo.Name = "cSurveyNo";
            this.cSurveyNo.ReadOnly = true;
            this.cSurveyNo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cSurveyNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cSurveyNo.Width = 98;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Status.HeaderText = "狀態";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Status.Width = 59;
            // 
            // cEMailingTime
            // 
            this.cEMailingTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cEMailingTime.HeaderText = "EMAIL提醒信寄出時間";
            this.cEMailingTime.Name = "cEMailingTime";
            this.cEMailingTime.ReadOnly = true;
            this.cEMailingTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cEMailingTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cEMailingTime.Width = 164;
            // 
            // cSecondEMailingTime
            // 
            this.cSecondEMailingTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cSecondEMailingTime.HeaderText = "EMAIL再次提醒信寄出時間";
            this.cSecondEMailingTime.Name = "cSecondEMailingTime";
            this.cSecondEMailingTime.ReadOnly = true;
            this.cSecondEMailingTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cSecondEMailingTime.Width = 190;
            // 
            // frmEvaluationEMailing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.btnSendSecondEmail);
            this.Controls.Add(this.btnSendEmail);
            this.Controls.Add(this.circularProgress);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.cboCourse);
            this.Controls.Add(this.cboSemester);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.nudSchoolYear);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.DoubleBuffered = true;
            this.MaximizeBox = true;
            this.Name = "frmEvaluationEMailing";
            this.Text = "教學評鑑意見調查EMAIL提醒";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSchoolYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvData;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboCourse;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSemester;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.NumericUpDown nudSchoolYear;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.CircularProgress circularProgress;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnSendEmail;
        private DevComponents.DotNetBar.ButtonX btnSendSecondEmail;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.ItemContainer itemContainer2;
        private DevComponents.DotNetBar.TextBoxItem txtSenderEMail;
        private DevComponents.DotNetBar.TextBoxItem txtSenderName;
        private DevComponents.DotNetBar.ItemContainer itemContainer3;
        private DevComponents.DotNetBar.TextBoxItem txtCC;
        private DevComponents.DotNetBar.ItemContainer itemContainer4;
        private DevComponents.DotNetBar.LabelItem labelItem4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.DotNetBar.LabelX lblMessage;
        private DevComponents.DotNetBar.ItemContainer itemContainer5;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.ItemContainer itemContainer6;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.ItemContainer itemContainer7;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private DevComponents.DotNetBar.ButtonItem btnSave;
        private DevComponents.DotNetBar.ButtonItem btnCancel;
        private DevComponents.DotNetBar.LabelItem labelItem6;
        private DevComponents.DotNetBar.LabelItem labelItem7;
        private DevComponents.DotNetBar.LabelItem labelItem5;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cSubjectName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cSubjectCode;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cNewSubjectCode;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cCourseName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cTeacherName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cOpeningTime;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cEndTime;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cAttendNo;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cSurveyNo;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn Status;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cEMailingTime;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn cSecondEMailingTime;
    }
}