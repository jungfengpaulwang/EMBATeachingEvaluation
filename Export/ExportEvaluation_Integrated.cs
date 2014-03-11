using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using DevComponents.DotNetBar;

namespace TeachingEvaluation.Export
{
    public partial class ExportEvaluation_Integrated : BaseForm
    {
        private bool ExpandedChanging;
        public ExportEvaluation_Integrated()
        {
            InitializeComponent();

            ExpandedChanging = true;

            this.expandablePanel_Subject.Dock = DockStyle.Fill;
            this.expandablePanel_Course.Dock = DockStyle.Bottom;
            this.expandablePanel_Teacher.Dock = DockStyle.Bottom;
            this.expandablePanel_Case.Dock = DockStyle.Bottom;

            this.expandablePanel_Subject.Expanded = true;
            this.expandablePanel_Course.Expanded = false;
            this.expandablePanel_Teacher.Expanded = false;
            this.expandablePanel_Case.Expanded = false;

            ExpandedChanging = false;

            this.Load += new System.EventHandler(this.ExportEvaluation_Integrated_Load);
            this.expandablePanel_Subject.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel_Subject_ExpandedChaning);
            this.expandablePanel_Course.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel_Course_ExpandedChaning);
            this.expandablePanel_Teacher.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel_Teacher_ExpandedChaning);
            this.expandablePanel_Case.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel_Case_ExpandedChaning);
        }

        private void ExportEvaluation_Integrated_Load(object sender, EventArgs e)
        {

        }

        private void expandablePanel_Subject_ExpandedChaning(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (ExpandedChanging)
                return;

            this.ExpandedChanging = true;
            this.expandablePanel_Subject.Dock = DockStyle.Fill;
            this.expandablePanel_Course.Dock = DockStyle.Bottom;
            this.expandablePanel_Teacher.Dock = DockStyle.Bottom;
            this.expandablePanel_Case.Dock = DockStyle.Bottom;
            this.expandablePanel_Subject.Expanded = true;
            this.expandablePanel_Course.Expanded = false;
            this.expandablePanel_Teacher.Expanded = false;
            this.expandablePanel_Case.Expanded = false;

            this.ExpandedChanging = false;
        }

        private void expandablePanel_Course_ExpandedChaning(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (ExpandedChanging)
                return;

            this.ExpandedChanging = true;
            this.expandablePanel_Subject.Dock = DockStyle.Top;
            this.expandablePanel_Course.Dock = DockStyle.Fill;
            this.expandablePanel_Teacher.Dock = DockStyle.Bottom;
            this.expandablePanel_Case.Dock = DockStyle.Bottom;
            this.expandablePanel_Subject.Expanded = false;
            this.expandablePanel_Course.Expanded = true;
            this.expandablePanel_Teacher.Expanded = false;
            this.expandablePanel_Case.Expanded = false;

            this.ExpandedChanging = false;
        }

        private void expandablePanel_Teacher_ExpandedChaning(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (ExpandedChanging)
                return;

            this.ExpandedChanging = true;
            this.expandablePanel_Subject.Dock = DockStyle.Top;
            this.expandablePanel_Course.Dock = DockStyle.Top;
            this.expandablePanel_Teacher.Dock = DockStyle.Fill;
            this.expandablePanel_Case.Dock = DockStyle.Bottom;
            this.expandablePanel_Subject.Expanded = false;
            this.expandablePanel_Course.Expanded = false;
            this.expandablePanel_Teacher.Expanded = true;
            this.expandablePanel_Case.Expanded = false;

            this.ExpandedChanging = false;
        }

        private void expandablePanel_Case_ExpandedChaning(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (ExpandedChanging)
                return;

            this.ExpandedChanging = true;
            this.expandablePanel_Subject.Dock = DockStyle.Top;
            this.expandablePanel_Course.Dock = DockStyle.Top;
            this.expandablePanel_Teacher.Dock = DockStyle.Top;
            this.expandablePanel_Case.Dock = DockStyle.Fill;
            this.expandablePanel_Subject.Expanded = false;
            this.expandablePanel_Course.Expanded = false;
            this.expandablePanel_Teacher.Expanded = false;
            this.expandablePanel_Case.Expanded = true;

            this.ExpandedChanging = false;
        }
    }
}
