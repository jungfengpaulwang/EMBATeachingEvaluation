namespace TeachingEvaluation.Forms
{
    partial class frmStatistics_SingleForm
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
            this.lstQuestions = new System.Windows.Forms.ListView();
            this.Sure = new DevComponents.DotNetBar.ButtonX();
            this.Exit = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.chkSelectAll = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtStatisticsGroup = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cboSurvey = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // lstQuestions
            // 
            this.lstQuestions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstQuestions.CheckBoxes = true;
            this.lstQuestions.Location = new System.Drawing.Point(68, 118);
            this.lstQuestions.Name = "lstQuestions";
            this.lstQuestions.Size = new System.Drawing.Size(686, 401);
            this.lstQuestions.TabIndex = 0;
            this.lstQuestions.UseCompatibleStateImageBehavior = false;
            this.lstQuestions.View = System.Windows.Forms.View.List;
            // 
            // Sure
            // 
            this.Sure.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Sure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Sure.BackColor = System.Drawing.Color.Transparent;
            this.Sure.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Sure.Location = new System.Drawing.Point(625, 531);
            this.Sure.Name = "Sure";
            this.Sure.Size = new System.Drawing.Size(54, 23);
            this.Sure.TabIndex = 70;
            this.Sure.Text = "儲存";
            this.Sure.Click += new System.EventHandler(this.Save_Click);
            // 
            // Exit
            // 
            this.Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Exit.BackColor = System.Drawing.Color.Transparent;
            this.Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Exit.Location = new System.Drawing.Point(700, 531);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(54, 23);
            this.Exit.TabIndex = 69;
            this.Exit.Text = "離開";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
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
            this.labelX1.Location = new System.Drawing.Point(4, 23);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 21);
            this.labelX1.TabIndex = 72;
            this.labelX1.Text = "評鑑樣版";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
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
            this.labelX4.Location = new System.Drawing.Point(30, 118);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(34, 21);
            this.labelX4.TabIndex = 73;
            this.labelX4.Text = "題目";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAll.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkSelectAll.BackgroundStyle.Class = "";
            this.chkSelectAll.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSelectAll.Location = new System.Drawing.Point(64, 525);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(54, 21);
            this.chkSelectAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSelectAll.TabIndex = 90;
            this.chkSelectAll.Text = "全選";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // txtStatisticsGroup
            // 
            this.txtStatisticsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtStatisticsGroup.Border.Class = "TextBoxBorder";
            this.txtStatisticsGroup.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtStatisticsGroup.Location = new System.Drawing.Point(68, 71);
            this.txtStatisticsGroup.Name = "txtStatisticsGroup";
            this.txtStatisticsGroup.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatisticsGroup.Size = new System.Drawing.Size(686, 25);
            this.txtStatisticsGroup.TabIndex = 91;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(4, 71);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 92;
            this.labelX2.Text = "群組名稱";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // cboSurvey
            // 
            this.cboSurvey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSurvey.DisplayMember = "Text";
            this.cboSurvey.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSurvey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSurvey.FormattingEnabled = true;
            this.cboSurvey.ItemHeight = 19;
            this.cboSurvey.Location = new System.Drawing.Point(68, 20);
            this.cboSurvey.Name = "cboSurvey";
            this.cboSurvey.Size = new System.Drawing.Size(686, 25);
            this.cboSurvey.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSurvey.TabIndex = 93;
            this.cboSurvey.SelectedIndexChanged += new System.EventHandler(this.cboSurvey_SelectedIndexChanged);
            // 
            // frmStatistics_SingleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.cboSurvey);
            this.Controls.Add(this.txtStatisticsGroup);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.Sure);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.lstQuestions);
            this.DoubleBuffered = true;
            this.Name = "frmStatistics_SingleForm";
            this.Text = "新增／修改統計群組";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstQuestions;
        private DevComponents.DotNetBar.ButtonX Sure;
        private DevComponents.DotNetBar.ButtonX Exit;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSelectAll;
        private DevComponents.DotNetBar.Controls.TextBoxX txtStatisticsGroup;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSurvey;
    }
}