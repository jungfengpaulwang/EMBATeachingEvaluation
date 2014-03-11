namespace TeachingEvaluation.QuestionTemplate
{
    partial class SingleChoice
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
            this.cboHierarchy = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.chkNoneCalculated = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkCase = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkSelfAssessment = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkRequired = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.nudDisplayOrder = new System.Windows.Forms.NumericUpDown();
            this.txtTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.dgvData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.cTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDisplayOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudDisplayOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboHierarchy
            // 
            this.cboHierarchy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHierarchy.DisplayMember = "Text";
            this.cboHierarchy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHierarchy.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboHierarchy.FormattingEnabled = true;
            this.cboHierarchy.Location = new System.Drawing.Point(54, 25);
            this.cboHierarchy.Name = "cboHierarchy";
            this.cboHierarchy.Size = new System.Drawing.Size(473, 25);
            this.cboHierarchy.TabIndex = 17;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(13, 25);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(34, 21);
            this.labelX4.TabIndex = 15;
            this.labelX4.Text = "標題";
            // 
            // chkNoneCalculated
            // 
            this.chkNoneCalculated.AutoSize = true;
            // 
            // 
            // 
            this.chkNoneCalculated.BackgroundStyle.Class = "";
            this.chkNoneCalculated.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkNoneCalculated.Location = new System.Drawing.Point(369, 68);
            this.chkNoneCalculated.Name = "chkNoneCalculated";
            this.chkNoneCalculated.Size = new System.Drawing.Size(134, 21);
            this.chkNoneCalculated.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkNoneCalculated.TabIndex = 14;
            this.chkNoneCalculated.Text = "不列入評鑑值計算";
            // 
            // chkCase
            // 
            this.chkCase.AutoSize = true;
            // 
            // 
            // 
            this.chkCase.BackgroundStyle.Class = "";
            this.chkCase.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCase.Location = new System.Drawing.Point(291, 68);
            this.chkCase.Name = "chkCase";
            this.chkCase.Size = new System.Drawing.Size(54, 21);
            this.chkCase.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCase.TabIndex = 13;
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
            this.chkSelfAssessment.Location = new System.Drawing.Point(213, 68);
            this.chkSelfAssessment.Name = "chkSelfAssessment";
            this.chkSelfAssessment.Size = new System.Drawing.Size(54, 21);
            this.chkSelfAssessment.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSelfAssessment.TabIndex = 12;
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
            this.chkRequired.Checked = true;
            this.chkRequired.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRequired.CheckValue = "Y";
            this.chkRequired.Location = new System.Drawing.Point(135, 68);
            this.chkRequired.Name = "chkRequired";
            this.chkRequired.Size = new System.Drawing.Size(54, 21);
            this.chkRequired.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkRequired.TabIndex = 11;
            this.chkRequired.Text = "必填";
            // 
            // nudDisplayOrder
            // 
            this.nudDisplayOrder.Location = new System.Drawing.Point(54, 67);
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
            // txtTitle
            // 
            // 
            // 
            // 
            this.txtTitle.Border.Class = "TextBoxBorder";
            this.txtTitle.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTitle.Location = new System.Drawing.Point(53, 112);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(474, 25);
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
            this.labelX2.Location = new System.Drawing.Point(13, 112);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(34, 21);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "題目";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(13, 160);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(34, 21);
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "選項";
            // 
            // dgvData
            // 
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTitle,
            this.cDisplayOrder});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dgvData.Location = new System.Drawing.Point(53, 158);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidth = 25;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.Size = new System.Drawing.Size(474, 169);
            this.dgvData.TabIndex = 4;
            // 
            // cTitle
            // 
            this.cTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cTitle.HeaderText = "項目";
            this.cTitle.Name = "cTitle";
            // 
            // cDisplayOrder
            // 
            this.cDisplayOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.cDisplayOrder.HeaderText = "順序";
            this.cDisplayOrder.Name = "cDisplayOrder";
            this.cDisplayOrder.ToolTipText = "報表輸出順序(順序由小到大排序，標題從左到右，從上到下顯示)";
            this.cDisplayOrder.Width = 59;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 69);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(34, 21);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "題號";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SingleChoice
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.cboHierarchy);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.chkNoneCalculated);
            this.Controls.Add(this.chkCase);
            this.Controls.Add(this.chkSelfAssessment);
            this.Controls.Add(this.chkRequired);
            this.Controls.Add(this.nudDisplayOrder);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.labelX1);
            this.Name = "SingleChoice";
            this.Size = new System.Drawing.Size(550, 350);
            ((System.ComponentModel.ISupportInitialize)(this.nudDisplayOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvData;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTitle;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.NumericUpDown nudDisplayOrder;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkRequired;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCase;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSelfAssessment;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDisplayOrder;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkNoneCalculated;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboHierarchy;

    }
}