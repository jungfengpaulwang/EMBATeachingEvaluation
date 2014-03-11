namespace TeachingEvaluation.Forms
{
    partial class frmEMailingLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.CourseName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.TeacherName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.StudentNumber = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.StudentName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.SendingEMail = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.SendedEMail = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.ErrorMessage = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.RejectReason = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.CC = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.circularProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
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
            this.CourseName,
            this.TeacherName,
            this.StudentNumber,
            this.StudentName,
            this.SendingEMail,
            this.SendedEMail,
            this.ErrorMessage,
            this.RejectReason,
            this.CC});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvData.Location = new System.Drawing.Point(26, 25);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersWidth = 25;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(975, 643);
            this.dgvData.TabIndex = 83;
            // 
            // CourseName
            // 
            this.CourseName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CourseName.HeaderText = "開課";
            this.CourseName.Name = "CourseName";
            this.CourseName.ReadOnly = true;
            this.CourseName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CourseName.Width = 59;
            // 
            // TeacherName
            // 
            this.TeacherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TeacherName.HeaderText = "授課教師";
            this.TeacherName.Name = "TeacherName";
            this.TeacherName.ReadOnly = true;
            this.TeacherName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherName.Width = 85;
            // 
            // StudentNumber
            // 
            this.StudentNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StudentNumber.HeaderText = "學號";
            this.StudentNumber.Name = "StudentNumber";
            this.StudentNumber.ReadOnly = true;
            this.StudentNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StudentNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StudentNumber.Width = 59;
            // 
            // StudentName
            // 
            this.StudentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StudentName.HeaderText = "姓名";
            this.StudentName.Name = "StudentName";
            this.StudentName.ReadOnly = true;
            this.StudentName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StudentName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StudentName.Width = 59;
            // 
            // SendingEMail
            // 
            this.SendingEMail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SendingEMail.HeaderText = "待發送之電子郵件";
            this.SendingEMail.Name = "SendingEMail";
            this.SendingEMail.ReadOnly = true;
            this.SendingEMail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SendingEMail.Width = 137;
            // 
            // SendedEMail
            // 
            this.SendedEMail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SendedEMail.HeaderText = "已寄出之電子郵件";
            this.SendedEMail.Name = "SendedEMail";
            this.SendedEMail.ReadOnly = true;
            this.SendedEMail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SendedEMail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SendedEMail.Width = 137;
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ErrorMessage.HeaderText = "失敗訊息";
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.ReadOnly = true;
            this.ErrorMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ErrorMessage.Width = 85;
            // 
            // RejectReason
            // 
            this.RejectReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RejectReason.HeaderText = "退信原因";
            this.RejectReason.Name = "RejectReason";
            this.RejectReason.ReadOnly = true;
            this.RejectReason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RejectReason.Width = 85;
            // 
            // CC
            // 
            this.CC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CC.HeaderText = "副本";
            this.CC.Name = "CC";
            this.CC.ReadOnly = true;
            this.CC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CC.Width = 59;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(926, 688);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 87;
            this.btnExit.Text = "離　開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(815, 688);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExport.TabIndex = 88;
            this.btnExport.Text = "匯　出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            this.circularProgress.Location = new System.Drawing.Point(720, 688);
            this.circularProgress.Name = "circularProgress";
            this.circularProgress.Size = new System.Drawing.Size(89, 23);
            this.circularProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circularProgress.TabIndex = 89;
            this.circularProgress.Visible = false;
            // 
            // frmEMailingLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 729);
            this.Controls.Add(this.circularProgress);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dgvData);
            this.DoubleBuffered = true;
            this.Name = "frmEMailingLog";
            this.Text = "發送 Email 提醒通知 Log";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvData;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn CourseName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn TeacherName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn StudentNumber;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn StudentName;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn SendingEMail;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn SendedEMail;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn ErrorMessage;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn RejectReason;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn CC;
        private DevComponents.DotNetBar.Controls.CircularProgress circularProgress;
    }
}