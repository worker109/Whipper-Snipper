using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
  public class ActionHardwareForm : Form
  {
    private Profile profile;
    private ActionHardware actionhardware;
    private bool new_actionhardware;
    private IContainer components;
    private TextBox txtFriendlyName;
    private Label label1;
    private Label label4;
    private ComboBox cboActionRevert;
    private Label label5;
    private ComboBox cboActionEnable;
    private TextBox txtDeviceId;
    private Label label3;
    private Label label2;
    private Button btnDelete;
    private Button btnSave;
    private Button btnCancel;
    private ComboBox cboHardware;

    public ActionHardwareForm(Profile profile, ActionHardware actionhardware)
    {
      this.InitializeComponent();
      this.new_actionhardware = false;
      this.profile = profile;
      this.actionhardware = actionhardware;
      this.Text = "Edit Hardware Action";
    }

    public ActionHardwareForm(Profile profile)
    {
      this.InitializeComponent();
      this.Text = "New Hardware Action";
      this.new_actionhardware = true;
      this.actionhardware = new ActionHardware();
      this.profile = profile;
    }

    private void ActionHardwareForm_Load(object sender, EventArgs e)
    {
      this.Icon = Resources.icon_hardware_icon;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      if (this.new_actionhardware)
        this.btnDelete.Enabled = false;
      this.txtFriendlyName.Text = this.actionhardware.friendlyname;
      this.cboHardware.Items.Clear();
      foreach (HardwareInfo hardware in Program.devcon.getHardwareList(this.profile, this.actionhardware.hardwareinfo))
      {
        this.cboHardware.Items.Add((object) hardware);
        if (hardware.deviceid == this.actionhardware.hardwareinfo.deviceid)
        {
          this.cboHardware.SelectedItem = (object) hardware;
          this.txtDeviceId.Text = this.actionhardware.hardwareinfo.deviceid;
        }
      }
      this.cboActionEnable.Items.Clear();
      this.cboActionEnable.Items.Add((object) "Disable");
      this.cboActionEnable.Items.Add((object) "Enable");
      this.cboActionEnable.Items.Add((object) "(no action)");
      this.cboActionEnable.SelectedItem = (object) this.actionhardware.action_enable;
      this.cboActionRevert.Items.Clear();
      this.cboActionRevert.Items.Add((object) "Disable");
      this.cboActionRevert.Items.Add((object) "Enable");
      this.cboActionRevert.Items.Add((object) "(no action)");
      this.cboActionRevert.SelectedItem = (object) this.actionhardware.action_revert;
    }

    private void btnSave_Click_1(object sender, EventArgs e)
    {
      string text = "";
      if (this.cboHardware.SelectedIndex == -1)
        text += "Choose a hardware device\n";
      if (this.cboActionEnable.SelectedIndex == -1)
        text += "Choose an enable action\n";
      if (this.cboActionRevert.SelectedIndex == -1)
        text += "Choose a revert action\n";
      if (string.IsNullOrEmpty(text))
      {
        HardwareInfo selectedItem = (HardwareInfo) this.cboHardware.SelectedItem;
        this.actionhardware.friendlyname = this.txtFriendlyName.Text.Trim();
        this.actionhardware.hardwareinfo = selectedItem;
        this.actionhardware.action_enable = this.cboActionEnable.Text;
        this.actionhardware.action_revert = this.cboActionRevert.Text;
        if (this.new_actionhardware)
        {
          this.profile.addActionItem((Action) this.actionhardware);
          Program.writeLog("Adding hardware action \"" + this.actionhardware.getDispalyName() + "\"");
          Program.mainform.updateAfterSaveDelete();
        }
        else
        {
          Program.writeLog("Updating hardware action \"" + this.actionhardware.getDispalyName() + "\"");
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
      if (DialogResult.Yes != MessageBox.Show("Delete hardware action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      this.profile.removeAction((Action) this.actionhardware);
      Program.mainform.updateAfterSaveDelete();
      this.Close();
    }

    private void cboHardware_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cboHardware.SelectedItem.GetType() == typeof (HardwareInfo)))
        return;
      this.txtDeviceId.Text = ((HardwareInfo) this.cboHardware.SelectedItem).deviceid;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label4 = new Label();
      this.cboActionRevert = new ComboBox();
      this.label5 = new Label();
      this.cboActionEnable = new ComboBox();
      this.txtDeviceId = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.btnDelete = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.cboHardware = new ComboBox();
      this.txtFriendlyName = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 165);
      this.label4.Name = "label4";
      this.label4.Size = new Size(104, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "Revert Profile Action";
      this.cboActionRevert.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActionRevert.FormattingEnabled = true;
      this.cboActionRevert.Location = new Point(15, 181);
      this.cboActionRevert.Name = "cboActionRevert";
      this.cboActionRevert.Size = new Size(237, 21);
      this.cboActionRevert.TabIndex = 23;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 126);
      this.label5.Name = "label5";
      this.label5.Size = new Size(105, 13);
      this.label5.TabIndex = 22;
      this.label5.Text = "Enable Profile Action";
      this.cboActionEnable.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActionEnable.FormattingEnabled = true;
      this.cboActionEnable.Location = new Point(15, 141);
      this.cboActionEnable.Name = "cboActionEnable";
      this.cboActionEnable.Size = new Size(237, 21);
      this.cboActionEnable.TabIndex = 21;
      this.txtDeviceId.Location = new Point(15, 103);
      this.txtDeviceId.Name = "txtDeviceId";
      this.txtDeviceId.ReadOnly = true;
      this.txtDeviceId.Size = new Size(237, 20);
      this.txtDeviceId.TabIndex = 17;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 87);
      this.label3.Name = "label3";
      this.label3.Size = new Size(53, 13);
      this.label3.TabIndex = 15;
      this.label3.Text = "Device Id";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(90, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Hardware Device";
      this.btnDelete.Location = new Point(177, 220);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 11;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click_1);
      this.btnSave.ImageKey = "(none)";
      this.btnSave.Location = new Point(15, 220);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 9;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click_1);
      this.btnCancel.Location = new Point(96, 220);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cboHardware.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboHardware.FormattingEnabled = true;
      this.cboHardware.Location = new Point(15, 63);
      this.cboHardware.Name = "cboHardware";
      this.cboHardware.Size = new Size(237, 21);
      this.cboHardware.TabIndex = 7;
      this.cboHardware.SelectedIndexChanged += new EventHandler(this.cboHardware_SelectedIndexChanged);
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
      this.ClientSize = new Size(268, 267);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.cboActionRevert);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtFriendlyName);
      this.Controls.Add((Control) this.cboActionEnable);
      this.Controls.Add((Control) this.cboHardware);
      this.Controls.Add((Control) this.txtDeviceId);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnDelete);
      this.Name = "ActionHardwareForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Hardware";
      this.Load += new EventHandler(this.ActionHardwareForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
