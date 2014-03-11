namespace TeachingEvaluation.Forms
{
    partial class frmSurveyTemplate
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
            this.dgvData_Attend = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.SubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Export = new DevComponents.DotNetBar.ButtonX();
            this.Exit = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData_Attend)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData_Attend
            // 
            this.dgvData_Attend.AllowUserToAddRows = false;
            this.dgvData_Attend.AllowUserToDeleteRows = false;
            this.dgvData_Attend.AllowUserToOrderColumns = true;
            this.dgvData_Attend.BackgroundColor = System.Drawing.Color.White;
            this.dgvData_Attend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData_Attend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData_Attend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SubjectName,
            this.Level,
            this.ClassName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData_Attend.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData_Attend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvData_Attend.Location = new System.Drawing.Point(22, 22);
            this.dgvData_Attend.Name = "dgvData_Attend";
            this.dgvData_Attend.ReadOnly = true;
            this.dgvData_Attend.RowHeadersWidth = 25;
            this.dgvData_Attend.RowTemplate.Height = 24;
            this.dgvData_Attend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData_Attend.Size = new System.Drawing.Size(547, 141);
            this.dgvData_Attend.TabIndex = 66;
            // 
            // SubjectName
            // 
            this.SubjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SubjectName.HeaderText = "評鑑名稱";
            this.SubjectName.Name = "SubjectName";
            this.SubjectName.ReadOnly = true;
            this.SubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Level
            // 
            this.Level.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Level.HeaderText = "簡述說明";
            this.Level.Name = "Level";
            this.Level.ReadOnly = true;
            this.Level.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ClassName
            // 
            this.ClassName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ClassName.HeaderText = "指定問卷樣版";
            this.ClassName.Name = "ClassName";
            this.ClassName.ReadOnly = true;
            this.ClassName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ClassName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ClassName.Width = 111;
            // 
            // Export
            // 
            this.Export.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Export.BackColor = System.Drawing.Color.Transparent;
            this.Export.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Export.Location = new System.Drawing.Point(440, 178);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(54, 23);
            this.Export.TabIndex = 68;
            this.Export.Text = "儲存";
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // Exit
            // 
            this.Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Exit.BackColor = System.Drawing.Color.Transparent;
            this.Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Exit.Location = new System.Drawing.Point(515, 178);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(54, 23);
            this.Exit.TabIndex = 67;
            this.Exit.Text = "離開";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // frmSurveyTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 208);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.dgvData_Attend);
            this.DoubleBuffered = true;
            this.Name = "frmSurveyTemplate";
            this.Text = "問卷管理";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData_Attend)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvData_Attend;
        private DevComponents.DotNetBar.ButtonX Export;
        private DevComponents.DotNetBar.ButtonX Exit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Level;
        private System.Windows.Forms.DataGridViewLinkColumn ClassName;
    }
}