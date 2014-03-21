namespace TeachingEvaluation.Forms
{
    partial class ImportReportDocumentTemplate
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.Survey = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.Import = new DevComponents.DotNetBar.ButtonX();
            this.FileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.OpenFile = new DevComponents.DotNetBar.ButtonX();
            this.Exit = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(17, 53);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(37, 48);
            this.labelX4.TabIndex = 19;
            this.labelX4.Text = "評鑑樣版";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            this.labelX4.WordWrap = true;
            // 
            // Survey
            // 
            this.Survey.DisplayMember = "Text";
            this.Survey.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Survey.FormattingEnabled = true;
            this.Survey.ItemHeight = 19;
            this.Survey.Location = new System.Drawing.Point(60, 64);
            this.Survey.Name = "Survey";
            this.Survey.Size = new System.Drawing.Size(403, 25);
            this.Survey.TabIndex = 18;
            // 
            // Import
            // 
            this.Import.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Import.BackColor = System.Drawing.Color.Transparent;
            this.Import.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Import.Location = new System.Drawing.Point(288, 110);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(75, 25);
            this.Import.TabIndex = 17;
            this.Import.Text = "儲　存";
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // FileName
            // 
            // 
            // 
            // 
            this.FileName.Border.Class = "TextBoxBorder";
            this.FileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.FileName.Location = new System.Drawing.Point(59, 21);
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Size = new System.Drawing.Size(405, 25);
            this.FileName.TabIndex = 16;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(17, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(36, 25);
            this.labelX1.TabIndex = 15;
            this.labelX1.Text = "檔案";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // OpenFile
            // 
            this.OpenFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenFile.BackColor = System.Drawing.Color.Transparent;
            this.OpenFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.OpenFile.Location = new System.Drawing.Point(188, 110);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(75, 25);
            this.OpenFile.TabIndex = 14;
            this.OpenFile.Text = "瀏　覽";
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // Exit
            // 
            this.Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Exit.BackColor = System.Drawing.Color.Transparent;
            this.Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Exit.Location = new System.Drawing.Point(388, 110);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 25);
            this.Exit.TabIndex = 24;
            this.Exit.Text = "離　開";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // ImportReportDocumentTemplate
            // 
            this.ClientSize = new System.Drawing.Size(493, 151);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.Survey);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.OpenFile);
            this.DoubleBuffered = true;
            this.Name = "ImportReportDocumentTemplate";
            this.Text = "教學意見表樣版上載";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx Survey;
        private DevComponents.DotNetBar.ButtonX Import;
        private DevComponents.DotNetBar.Controls.TextBoxX FileName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX OpenFile;
        private DevComponents.DotNetBar.ButtonX Exit;
    }
}
