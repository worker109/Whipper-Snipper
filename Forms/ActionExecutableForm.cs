using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
    public class ActionExecutableForm : Form
    {
        private Profile profile;
        private ActionExecutable actionexecutable;
        private bool new_actionexecutable;
        private IContainer components;
        private TextBox txtFriendlyName;
        private Label label1;
        private TextBox txtExecutablePath;
        private Label label3;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;
        private Label label6;
        private ComboBox cboRevertAction;
        private Label label7;
        private LinkLabel lnkBrowse;
        private OpenFileDialog openFileDialog1;
        private ComboBox cboEnableAction;

        public ActionExecutableForm(Profile profile, ActionExecutable actionexecutable)
        {
            this.InitializeComponent();
            this.new_actionexecutable = false;
            this.profile = profile;
            this.actionexecutable = actionexecutable;
            this.Text = "Edit Executable Action";
        }

        public ActionExecutableForm(Profile profile)
        {
            this.InitializeComponent();
            this.Text = "New Executable Action";
            this.new_actionexecutable = true;
            this.actionexecutable = new ActionExecutable();
            this.profile = profile;
        }

        private void ActionExecutableForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.icon_executable_icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            if (this.new_actionexecutable)
            this.btnDelete.Enabled = false;
            this.txtFriendlyName.Text = this.actionexecutable.friendlyname;      
            this.txtExecutablePath.Text = this.actionexecutable.executableinfo.exepath;
            this.cboEnableAction.Items.Clear();
            this.cboEnableAction.Items.Add((object) "Kill");
            this.cboEnableAction.Items.Add((object) "Start");
            this.cboEnableAction.Items.Add((object) "(no action)");
            this.cboEnableAction.SelectedItem = this.actionexecutable.action_enable;
            this.cboRevertAction.Items.Clear();
            this.cboRevertAction.Items.Add((object) "Kill");
            this.cboRevertAction.Items.Add((object) "Start");
            this.cboRevertAction.Items.Add((object) "(no action)");
            this.cboRevertAction.SelectedItem = (object) this.actionexecutable.action_revert;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string text = "";
            //if (this.cboExecutableName.Text.Trim() == "")
            //  text += "Enter an executable\n";
            if (this.txtExecutablePath.Text.Trim() == "")
            text += "Enter an executable path\n";
            if (this.cboEnableAction.SelectedIndex == -1)
            text += "Choose an enable action\n";
            if (this.cboRevertAction.SelectedIndex == -1)
            text += "Choose a revert action\n";
            if (string.IsNullOrEmpty(text))
            {

                this.actionexecutable.friendlyname = this.txtFriendlyName.Text.Trim();
                //this.actionexecutable.executableinfo.exename = this.cboExecutableName.Text.Trim();
                this.actionexecutable.executableinfo.exepath = this.txtExecutablePath.Text.Trim();
                this.actionexecutable.action_enable = this.cboEnableAction.Text;
                this.actionexecutable.action_revert = this.cboRevertAction.Text;
                if (this.new_actionexecutable)
                {
                    this.profile.addActionItem((Action) this.actionexecutable);
                    Program.writeLog("Adding executable action \"" + this.actionexecutable.getDispalyName() + "\"");
                    Program.mainform.updateAfterSaveDelete();
                }
                else
                {
                    Program.writeLog("Updating executable action \"" + this.actionexecutable.getDispalyName() + "\"");
                    Program.mainform.updateAfterSaveDelete();
                }
                this.Close();
            }
            else
            {
                int num = (int) MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Delete executable action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
            return;
            this.profile.removeAction((Action) this.actionexecutable);
            Program.mainform.updateAfterSaveDelete();
            this.Close();
        }
           

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.cboRevertAction = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboEnableAction = new System.Windows.Forms.ComboBox();
            this.txtExecutablePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkBrowse = new System.Windows.Forms.LinkLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Revert Profile Action";
            // 
            // cboRevertAction
            // 
            this.cboRevertAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRevertAction.FormattingEnabled = true;
            this.cboRevertAction.Location = new System.Drawing.Point(15, 142);
            this.cboRevertAction.Name = "cboRevertAction";
            this.cboRevertAction.Size = new System.Drawing.Size(237, 21);
            this.cboRevertAction.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Run Profile Action";
            // 
            // cboEnableAction
            // 
            this.cboEnableAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnableAction.FormattingEnabled = true;
            this.cboEnableAction.Location = new System.Drawing.Point(15, 102);
            this.cboEnableAction.Name = "cboEnableAction";
            this.cboEnableAction.Size = new System.Drawing.Size(237, 21);
            this.cboEnableAction.TabIndex = 25;
            this.cboEnableAction.SelectedIndexChanged += new System.EventHandler(this.cboEnableAction_SelectedIndexChanged);
            // 
            // txtExecutablePath
            // 
            this.txtExecutablePath.Location = new System.Drawing.Point(15, 64);
            this.txtExecutablePath.Name = "txtExecutablePath";
            this.txtExecutablePath.Size = new System.Drawing.Size(189, 20);
            this.txtExecutablePath.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Executable Path";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(177, 183);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.ImageKey = "(none)";
            this.btnSave.Location = new System.Drawing.Point(15, 183);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(96, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtFriendlyName
            // 
            this.txtFriendlyName.Location = new System.Drawing.Point(15, 25);
            this.txtFriendlyName.Name = "txtFriendlyName";
            this.txtFriendlyName.Size = new System.Drawing.Size(237, 20);
            this.txtFriendlyName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Friendly name (optional)";
            // 
            // lnkBrowse
            // 
            this.lnkBrowse.AutoSize = true;
            this.lnkBrowse.Location = new System.Drawing.Point(210, 67);
            this.lnkBrowse.Name = "lnkBrowse";
            this.lnkBrowse.Size = new System.Drawing.Size(42, 13);
            this.lnkBrowse.TabIndex = 29;
            this.lnkBrowse.TabStop = true;
            this.lnkBrowse.Text = "Browse";
            this.lnkBrowse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBrowse_LinkClicked);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Executable Files|*.exe";
            // 
            // ActionExecutableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 220);
            this.Controls.Add(this.lnkBrowse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboRevertAction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtFriendlyName);
            this.Controls.Add(this.cboEnableAction);
            this.Controls.Add(this.txtExecutablePath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Name = "ActionExecutableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Executable";
            this.Load += new System.EventHandler(this.ActionExecutableForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            }   

            private void lnkBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
                openFileDialog1.ShowDialog();
                txtExecutablePath.Text = openFileDialog1.FileName;
            }

            private void cboEnableAction_SelectedIndexChanged(object sender, EventArgs e)
            {

            }
        }
}
