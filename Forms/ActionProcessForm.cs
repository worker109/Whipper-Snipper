using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
  public class ActionProcessForm : Form
  {
    private Profile profile;
    private ActionProcess actionprocess;
    private bool new_actionprocess;
    private IContainer components;
    private TextBox txtFriendlyName;
    private Label label1;
    private TextBox txtProcessPath;
    private Label label3;
    private Label label2;
    private Button btnDelete;
    private Button btnSave;
    private Button btnCancel;
    private ComboBox cboProcessName;
    private Label label6;
    private ComboBox cboRevertAction;
    private Label label7;
    private ComboBox cboEnableAction;

    public ActionProcessForm(Profile profile, ActionProcess actionprocess)
    {
      this.InitializeComponent();
      this.new_actionprocess = false;
      this.profile = profile;
      this.actionprocess = actionprocess;
      this.Text = "Edit Process Action";
    }

    public ActionProcessForm(Profile profile)
    {
      this.InitializeComponent();
      this.Text = "New Process Action";
      this.new_actionprocess = true;
      this.actionprocess = new ActionProcess();
      this.profile = profile;
    }

    private void ActionProcessForm_Load(object sender, EventArgs e)
    {
      this.Icon = Resources.icon_process_icon;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      if (this.new_actionprocess)
        this.btnDelete.Enabled = false;
      this.txtFriendlyName.Text = this.actionprocess.friendlyname;
      this.cboProcessName.Items.Clear();
      foreach (object process in Program.processcontroller.getProcessList(this.profile, this.actionprocess.processinfo))
        this.cboProcessName.Items.Add(process);
      this.cboProcessName.Text = this.actionprocess.processinfo.exename;
      this.txtProcessPath.Text = this.actionprocess.processinfo.exepath;
      this.cboEnableAction.Items.Clear();
      this.cboEnableAction.Items.Add((object) "Kill");
      this.cboEnableAction.Items.Add((object) "Start");
      this.cboEnableAction.Items.Add((object) "(no action)");
      this.cboEnableAction.SelectedItem = (object) this.actionprocess.action_enable;
      this.cboRevertAction.Items.Clear();
      this.cboRevertAction.Items.Add((object) "Kill");
      this.cboRevertAction.Items.Add((object) "Start");
      this.cboRevertAction.Items.Add((object) "(no action)");
      this.cboRevertAction.SelectedItem = (object) this.actionprocess.action_revert;
    }

    private void btnSave_Click_1(object sender, EventArgs e)
    {
      string text = "";
      if (this.cboProcessName.Text.Trim() == "")
        text += "Enter a process\n";
      if (this.txtProcessPath.Text.Trim() == "")
        text += "Enter a process path\n";
      if (this.cboEnableAction.SelectedIndex == -1)
        text += "Choose an enable action\n";
      if (this.cboRevertAction.SelectedIndex == -1)
        text += "Choose a revert action\n";
      if (string.IsNullOrEmpty(text))
      {
        this.actionprocess.friendlyname = this.txtFriendlyName.Text.Trim();
        this.actionprocess.processinfo.exename = this.cboProcessName.Text.Trim();
        this.actionprocess.processinfo.exepath = this.txtProcessPath.Text.Trim();
        this.actionprocess.action_enable = this.cboEnableAction.Text;
        this.actionprocess.action_revert = this.cboRevertAction.Text;
        if (this.new_actionprocess)
        {
          this.profile.addActionItem((Action) this.actionprocess);
          Program.writeLog("Adding process action \"" + this.actionprocess.getDispalyName() + "\"");
          Program.mainform.updateAfterSaveDelete();
        }
        else
        {
          Program.writeLog("Updating process action \"" + this.actionprocess.getDispalyName() + "\"");
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
      if (DialogResult.Yes != MessageBox.Show("Delete process action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      this.profile.removeAction((Action) this.actionprocess);
      Program.mainform.updateAfterSaveDelete();
      this.Close();
    }

    private void cboProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cboProcessName.SelectedItem.GetType() == typeof (ProcessInfo)))
        return;
      this.txtProcessPath.Text = ((ProcessInfo) this.cboProcessName.SelectedItem).exepath;
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
            this.txtProcessPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboProcessName = new System.Windows.Forms.ComboBox();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Revert Profile Action";
            // 
            // cboRevertAction
            // 
            this.cboRevertAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRevertAction.FormattingEnabled = true;
            this.cboRevertAction.Location = new System.Drawing.Point(14, 181);
            this.cboRevertAction.Name = "cboRevertAction";
            this.cboRevertAction.Size = new System.Drawing.Size(237, 21);
            this.cboRevertAction.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Run Profile Action";
            // 
            // cboEnableAction
            // 
            this.cboEnableAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnableAction.FormattingEnabled = true;
            this.cboEnableAction.Location = new System.Drawing.Point(14, 141);
            this.cboEnableAction.Name = "cboEnableAction";
            this.cboEnableAction.Size = new System.Drawing.Size(237, 21);
            this.cboEnableAction.TabIndex = 25;
            // 
            // txtProcessPath
            // 
            this.txtProcessPath.Location = new System.Drawing.Point(15, 103);
            this.txtProcessPath.Name = "txtProcessPath";
            this.txtProcessPath.Size = new System.Drawing.Size(237, 20);
            this.txtProcessPath.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Process Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Process Name";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(176, 222);
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
            this.btnSave.Location = new System.Drawing.Point(14, 222);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(95, 222);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cboProcessName
            // 
            this.cboProcessName.FormattingEnabled = true;
            this.cboProcessName.Location = new System.Drawing.Point(15, 63);
            this.cboProcessName.Name = "cboProcessName";
            this.cboProcessName.Size = new System.Drawing.Size(237, 21);
            this.cboProcessName.TabIndex = 7;
            this.cboProcessName.SelectedIndexChanged += new System.EventHandler(this.cboProcess_SelectedIndexChanged);
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
            // ActionProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 261);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboRevertAction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtFriendlyName);
            this.Controls.Add(this.cboEnableAction);
            this.Controls.Add(this.cboProcessName);
            this.Controls.Add(this.txtProcessPath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Name = "ActionProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process";
            this.Load += new System.EventHandler(this.ActionProcessForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
  }
}
