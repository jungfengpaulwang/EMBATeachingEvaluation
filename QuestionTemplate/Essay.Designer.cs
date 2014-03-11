namespace TeachingEvaluation.QuestionTemplate
{
    partial class Essay
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.nudDisplayOrder = new System.Windows.Forms.NumericUpDown();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkCase = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkSelfAssessment = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkRequired = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkNoneCalculated = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cboHierarchy = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.nudDisplayOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 72);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(34, 21);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "題號";
            // 
            // txtTitle
            // 
            // 
            // 
            // 
            this.txtTitle.Border.Class = "TextBoxBorder";
            this.txtTitle.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTitle.Location = new System.Drawing.Point(53, 115);
            this.txtTitle.Multiline = true;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(474, 214);
            this.txtTitle.TabIndex = 7;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(13, 115);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(34, 21);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "題目";
            // 
            // nudDisplayOrder
            // 
            this.nudDisplayOrder.Location = new System.Drawing.Point(54, 70);
            this.nudDisplayOrder.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudDisplayOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDisplayOrder.Name = "nudDisplayOrder";
            this.nudDisplayOrder.Size = new System.Drawing.Size(50, 25);
            this.nudDisplayOrder.TabIndex = 8;
            this.nudDisplayOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDisplayOrder.ValueChanged += new System.EventHandler(this.nudDisplayOrder_ValueChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // chkCase
            // 
            this.chkCase.AutoSize = true;
            // 
            // 
            // 
            this.chkCase.BackgroundStyle.Class = "";
            this.chkCase.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCase.Location = new System.Drawing.Point(291, 71);
            this.chkCase.Name = "chkCase";
            this.chkCase.Size = new System.Drawing.Size(54, 21);
            this.chkCase.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCase.TabIndex = 16;
            this.chkCase.Text = "個案";
            // 
            // chkSelfAssessment
            // 
            this.chkSelfAssessment.AutoSize = true;
            // 
            // 
            // 
            this.chkSelfAssessment.BackgroundStyle.Class = "";
            this.chkSelfAssessment.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSelfAssessment.Location = new System.Drawing.Point(213, 71);
            this.chkSelfAssessment.Name = "chkSelfAssessment";
            this.chkSelfAssessment.Size = new System.Drawing.Size(54, 21);
            this.chkSelfAssessment.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSelfAssessment.TabIndex = 15;
            this.chkSelfAssessment.Text = "自評";
            // 
            // chkRequired
            // 
            this.chkRequired.AutoSize = true;
            // 
            // 
            // 
            this.chkRequired.BackgroundStyle.Class = "";
            this.chkRequired.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkRequired.Location = new System.Drawing.Point(135, 71);
            this.chkRequired.Name = "chkRequired";
            this.chkRequired.Size = new System.Drawing.Size(54, 21);
            this.chkRequired.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkRequired.TabIndex = 14;
            this.chkRequired.Text = "必填";
            // 
            // chkNoneCalculated
            // 
            this.chkNoneCalculated.AutoSize = true;
            // 
            // 
            // 
            this.chkNoneCalculated.BackgroundStyle.Class = "";
            this.chkNoneCalculated.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoneCalculated.Checked = true;
            this.chkNoneCalculated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNoneCalculated.CheckValue = "Y";
            this.chkNoneCalculated.Location = new System.Drawing.Point(369, 71);
            this.chkNoneCalculated.Name = "chkNoneCalculated";
            this.chkNoneCalculated.Size = new System.Drawing.Size(134, 21);
            this.chkNoneCalculated.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoneCalculated.TabIndex = 17;
            this.chkNoneCalculated.Text = "不列入評鑑值計算";
            this.chkNoneCalculated.Visible = false;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(12, 25);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(34, 21);
            this.labelX4.TabIndex = 18;
            this.labelX4.Text = "標題";
            // 
            // cboHierarchy
            // 
            this.cboHierarchy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHierarchy.DisplayMember = "Text";
            this.cboHierarchy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHierarchy.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboHierarchy.FormattingEnabled = true;
            this.cboHierarchy.Location = new System.Drawing.Point(52, 26);
            this.cboHierarchy.Name = "cboHierarchy";
            this.cboHierarchy.Size = new System.Drawing.Size(475, 25);
            this.cboHierarchy.TabIndex = 30;
            // 
            // Essay
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.cboHierarchy);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.chkNoneCalculated);
            this.Controls.Add(this.chkCase);
            this.Controls.Add(this.chkSelfAssessment);
            this.Controls.Add(this.chkRequired);
            this.Controls.Add(this.nudDisplayOrder);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Name = "Essay";
            this.Size = new System.Drawing.Size(550, 350);
            ((System.ComponentModel.ISupportInitialize)(this.nudDisplayOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTitle;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.NumericUpDown nudDisplayOrder;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCase;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSelfAssessment;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkRequired;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkNoneCalculated;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboHierarchy;

    }
}