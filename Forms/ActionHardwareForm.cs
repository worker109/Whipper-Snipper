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
            this.label4 = new System.Windows.Forms.Label();
            this.cboActionRevert = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboActionEnable = new System.Windows.Forms.ComboBox();
            this.txtDeviceId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboHardware = new System.Windows.Forms.ComboBox();
            this.txtFriendlyName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Revert Profile Action";
            // 
            // cboActionRevert
            // 
            this.cboActionRevert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActionRevert.FormattingEnabled = true;
            this.cboActionRevert.Location = new System.Drawing.Point(15, 181);
            this.cboActionRevert.Name = "cboActionRevert";
            this.cboActionRevert.Size = new System.Drawing.Size(237, 21);
            this.cboActionRevert.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Run Profile Action";
            // 
            // cboActionEnable
            // 
            this.cboActionEnable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActionEnable.FormattingEnabled = true;
            this.cboActionEnable.Location = new System.Drawing.Point(15, 141);
            this.cboActionEnable.Name = "cboActionEnable";
            this.cboActionEnable.Size = new System.Drawing.Size(237, 21);
            this.cboActionEnable.TabIndex = 21;
            // 
            // txtDeviceId
            // 
            this.txtDeviceId.Location = new System.Drawing.Point(15, 103);
            this.txtDeviceId.Name = "txtDeviceId";
            this.txtDeviceId.ReadOnly = true;
            this.txtDeviceId.Size = new System.Drawing.Size(237, 20);
            this.txtDeviceId.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Device Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Hardware Device";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(177, 220);
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
            this.btnSave.Location = new System.Drawing.Point(15, 220);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(96, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cboHardware
            // 
            this.cboHardware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHardware.FormattingEnabled = true;
            this.cboHardware.Location = new System.Drawing.Point(15, 63);
            this.cboHardware.Name = "cboHardware";
            this.cboHardware.Size = new System.Drawing.Size(237, 21);
            this.cboHardware.TabIndex = 7;
            this.cboHardware.SelectedIndexChanged += new System.EventHandler(this.cboHardware_SelectedIndexChanged);
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
            // ActionHardwareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 267);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboActionRevert);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtFriendlyName);
            this.Controls.Add(this.cboActionEnable);
            this.Controls.Add(this.cboHardware);
            this.Controls.Add(this.txtDeviceId);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Name = "ActionHardwareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hardware";
            this.Load += new System.EventHandler(this.ActionHardwareForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    }
}
