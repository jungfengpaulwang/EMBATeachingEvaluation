using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;

namespace TeachingEvaluation.Forms
{
    public partial class frmMandrillApiKey : BaseForm
    {
        private AccessHelper Access;
        private ErrorProvider ErrorProvider1;
        private UDT.MandrillAPIKey MandrillAPIKey;

        public frmMandrillApiKey()
        {
            InitializeComponent();

            Access = new AccessHelper();
            ErrorProvider1 = new ErrorProvider();
            MandrillAPIKey = new UDT.MandrillAPIKey();
            this.Load += new EventHandler(Form_Load);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            List<UDT.MandrillAPIKey> MandrillAPIKeys = Access.Select<UDT.MandrillAPIKey>();
            if (MandrillAPIKeys.Count > 0)
            {
                this.MandrillAPIKey = MandrillAPIKeys.ElementAt(0);

                this.txtAPIKey.Text = this.MandrillAPIKey.APIKey;
            }
        }            

        private void Save_Click(object sender, EventArgs e)
        {
            bool is_validated = true;
            ErrorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(this.txtAPIKey.Text))
            {
                is_validated = false;
                ErrorProvider1.SetError(this.txtAPIKey, "必填");
            }
            if (!is_validated)
                return;

            this.MandrillAPIKey.APIKey = this.txtAPIKey.Text.Trim();

            try
            {
                this.MandrillAPIKey.Save();
                MessageBox.Show("儲存成功。");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
