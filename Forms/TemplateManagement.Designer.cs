namespace TeachingEvaluation.Forms
{
    partial class TemplateManagement
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
            this.TemplateName = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.Download = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Delete = new DevComponents.DotNetBar.ButtonX();
            this.Exit = new DevComponents.DotNetBar.ButtonX();
            this.Upload = new DevComponents.DotNetBar.ButtonX();
            this.Update = new DevComponents.DotNetBar.ButtonX();
            this.CircleProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TemplateName,
            this.Download});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersWidth = 25;
            this.dgvData.RowTemplate.Height = 27;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(481, 210);
            this.dgvData.TabIndex = 12;
            this.dgvData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellContentClick);
            // 
            // TemplateName
            // 
            this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TemplateName.HeaderText = "評鑑樣版名稱";
            this.TemplateName.Name = "TemplateName";
            this.TemplateName.ReadOnly = true;
            this.TemplateName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Download
            // 
            this.Download.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Download.HeaderText = "檢視";
            this.Download.Name = "Download";
            this.Download.ReadOnly = true;
            this.Download.Width = 40;
            // 
            // Delete
            // 
            this.Delete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Delete.BackColor = System.Drawing.Color.Transparent;
            this.Delete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Delete.Location = new System.Drawing.Point(316, 226);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(75, 25);
            this.Delete.TabIndex = 30;
            this.Delete.Text = "刪　除";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Exit
            // 
            this.Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Exit.BackColor = System.Drawing.Color.Transparent;
            this.Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Exit.Location = new System.Drawing.Point(405, 226);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 25);
            this.Exit.TabIndex = 29;
            this.Exit.Text = "離　開";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Upload
            // 
            this.Upload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Upload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Upload.BackColor = System.Drawing.Color.Transparent;
            this.Upload.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Upload.Location = new System.Drawing.Point(138, 226);
            this.Upload.Name = "Upload";
            this.Upload.Size = new System.Drawing.Size(75, 25);
            this.Upload.TabIndex = 31;
            this.Upload.Text = "新　增";
            this.Upload.Click += new System.EventHandler(this.Upload_Click);
            // 
            // Update
            // 
            this.Update.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Update.BackColor = System.Drawing.Color.Transparent;
            this.Update.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Update.Location = new System.Drawing.Point(227, 226);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(75, 25);
            this.Update.TabIndex = 32;
            this.Update.Text = "修　改";
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // CircleProgress
            // 
            this.CircleProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CircleProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.CircleProgress.BackgroundStyle.Class = "";
            this.CircleProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CircleProgress.Location = new System.Drawing.Point(95, 227);
            this.CircleProgress.Name = "CircleProgress";
            this.CircleProgress.Size = new System.Drawing.Size(35, 23);
            this.CircleProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.CircleProgress.TabIndex = 151;
            this.CircleProgress.Visible = false;
            // 
            // TemplateManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.CircleProgress);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.Upload);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.dgvData);
            this.DoubleBuffered = true;
            this.MaximizeBox = true;
            this.Name = "TemplateManagement";
            this.Text = "報表樣版管理";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvData;
        private DevComponents.DotNetBar.ButtonX Delete;
        private DevComponents.DotNetBar.ButtonX Exit;
        private DevComponents.DotNetBar.ButtonX Upload;
        private DevComponents.DotNetBar.ButtonX Update;
        private DevComponents.DotNetBar.Controls.CircularProgress CircleProgress;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn TemplateName;
        private System.Windows.Forms.DataGridViewLinkColumn Download;

    }
}