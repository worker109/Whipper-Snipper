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
      this.label6 = new Label();
      this.cboRevertAction = new ComboBox();
      this.label7 = new Label();
      this.cboEnableAction = new ComboBox();
      this.txtProcessPath = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.btnDelete = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.cboProcessName = new ComboBox();
      this.txtFriendlyName = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.label6.AutoSize = true;
      this.label6.Location = new Point(11, 165);
      this.label6.Name = "label6";
      this.label6.Size = new Size(104, 13);
      this.label6.TabIndex = 28;
      this.label6.Text = "Revert Profile Action";
      this.cboRevertAction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRevertAction.FormattingEnabled = true;
      this.cboRevertAction.Location = new Point(14, 181);
      this.cboRevertAction.Name = "cboRevertAction";
      this.cboRevertAction.Size = new Size(237, 21);
      this.cboRevertAction.TabIndex = 27;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(11, 126);
      this.label7.Name = "label7";
      this.label7.Size = new Size(105, 13);
      this.label7.TabIndex = 26;
      this.label7.Text = "Enable Profile Action";
      this.cboEnableAction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboEnableAction.FormattingEnabled = true;
      this.cboEnableAction.Location = new Point(14, 141);
      this.cboEnableAction.Name = "cboEnableAction";
      this.cboEnableAction.Size = new Size(237, 21);
      this.cboEnableAction.TabIndex = 25;
      this.txtProcessPath.Location = new Point(15, 103);
      this.txtProcessPath.Name = "txtProcessPath";
      this.txtProcessPath.Size = new Size(237, 20);
      this.txtProcessPath.TabIndex = 17;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 87);
      this.label3.Name = "label3";
      this.label3.Size = new Size(70, 13);
      this.label3.TabIndex = 15;
      this.label3.Text = "Process Path";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(76, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Process Name";
      this.btnDelete.Location = new Point(176, 222);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 11;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click_1);
      this.btnSave.ImageKey = "(none)";
      this.btnSave.Location = new Point(14, 222);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 9;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click_1);
      this.btnCancel.Location = new Point(95, 222);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cboProcessName.FormattingEnabled = true;
      this.cboProcessName.Location = new Point(15, 63);
      this.cboProcessName.Name = "cboProcessName";
      this.cboProcessName.Size = new Size(237, 21);
      this.cboProcessName.TabIndex = 7;
      this.cboProcessName.SelectedIndexChanged += new EventHandler(this.cboProcess_SelectedIndexChanged);
      this.txtFriendlyName.Location = new Point(15, 25);
      this.txtFriendlyName.Name = "txtFriendlyName";
      this.txtFriendlyName.Size = new Size(237, 20);
      this.txtFriendlyName.TabIndex = 3;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(118, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Friendly name (optional)";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(264, 261);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.cboRevertAction);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.txtFriendlyName);
      this.Controls.Add((Control) this.cboEnableAction);
      this.Controls.Add((Control) this.cboProcessName);
      this.Controls.Add((Control) this.txtProcessPath);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnDelete);
      this.Name = "ActionProcessForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Process";
      this.Load += new EventHandler(this.ActionProcessForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
