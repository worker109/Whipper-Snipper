﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
  public class ActionServiceForm : Form
  {
    private Profile profile;
    private ActionService actionservice;
    private bool new_actionservice;
    private IContainer components;
    private TextBox txtFriendlyName;
    private Label label1;
    private Label label4;
    private ComboBox cboRevertStartup;
    private Label label5;
    private ComboBox cboEnableStartup;
    private TextBox txtServiceName;
    private Label label3;
    private Label label2;
    private Button btnDelete;
    private Button btnSave;
    private Button btnCancel;
    private ComboBox cboService;
    private Label label6;
    private ComboBox cboRevertAction;
    private Label label7;
    private ComboBox cboEnableAction;

    public ActionServiceForm(Profile profile, ActionService actionservice)
    {
      this.InitializeComponent();
      this.new_actionservice = false;
      this.profile = profile;
      this.actionservice = actionservice;
      this.Text = "Edit Service Action";
    }

    public ActionServiceForm(Profile profile)
    {
      this.InitializeComponent();
      this.Text = "New Service Action";
      this.new_actionservice = true;
      this.actionservice = new ActionService();
      this.profile = profile;
    }

    private void ActionServiceForm_Load(object sender, EventArgs e)
    {
      this.Icon = Resources.icon_service_icon;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      if (this.new_actionservice)
        this.btnDelete.Enabled = false;
      this.txtFriendlyName.Text = this.actionservice.friendlyname;
      this.cboService.Items.Clear();
      foreach (ServiceInfo service in Program.servicecontroller.getServiceList(this.profile, this.actionservice.serviceinfo))
      {
        this.cboService.Items.Add((object) service);
        if (service.servicename == this.actionservice.serviceinfo.servicename)
        {
          this.cboService.SelectedItem = (object) service;
          this.txtServiceName.Text = this.actionservice.serviceinfo.servicename;
        }
      }
      this.cboEnableAction.Items.Clear();
      this.cboEnableAction.Items.Add((object) "Start");
      this.cboEnableAction.Items.Add((object) "Stop");
      this.cboEnableAction.Items.Add((object) "(no action)");
      this.cboEnableAction.SelectedItem = (object) this.actionservice.action_enable;
      this.cboEnableStartup.Items.Clear();
      this.cboEnableStartup.Items.Add((object) "Automatic (Delayed Start)");
      this.cboEnableStartup.Items.Add((object) "Automatic");
      this.cboEnableStartup.Items.Add((object) "Manual");
      this.cboEnableStartup.Items.Add((object) "Disabled");
      this.cboEnableStartup.Items.Add((object) "(no action)");
      this.cboEnableStartup.SelectedItem = (object) this.actionservice.action_startup_enable;
      this.cboRevertAction.Items.Clear();
      this.cboRevertAction.Items.Add((object) "Start");
      this.cboRevertAction.Items.Add((object) "Stop");
      this.cboRevertAction.Items.Add((object) "(no action)");
      this.cboRevertAction.SelectedItem = (object) this.actionservice.action_revert;
      this.cboRevertStartup.Items.Clear();
      this.cboRevertStartup.Items.Add((object) "Automatic (Delayed Start)");
      this.cboRevertStartup.Items.Add((object) "Automatic");
      this.cboRevertStartup.Items.Add((object) "Manual");
      this.cboRevertStartup.Items.Add((object) "Disabled");
      this.cboRevertStartup.Items.Add((object) "(no action)");
      this.cboRevertStartup.SelectedItem = (object) this.actionservice.action_startup_revert;
    }

    private void btnSave_Click_1(object sender, EventArgs e)
    {
      string text = "";
      if (this.cboService.SelectedIndex == -1)
        text += "Choose a service\n";
      if (this.cboEnableAction.SelectedIndex == -1)
        text += "Choose an enable action\n";
      if (this.cboRevertAction.SelectedIndex == -1)
        text += "Choose a revert action (startup type)n\n";
      if (this.cboEnableStartup.SelectedIndex == -1)
        text += "Choose an enable action\n";
      if (this.cboRevertStartup.SelectedIndex == -1)
        text += "Choose a revert action (startup type)n\n";
      if (string.IsNullOrEmpty(text))
      {
        ServiceInfo selectedItem = (ServiceInfo) this.cboService.SelectedItem;
        this.actionservice.friendlyname = this.txtFriendlyName.Text.Trim();
        this.actionservice.serviceinfo = selectedItem;
        this.actionservice.action_enable = this.cboEnableAction.Text;
        this.actionservice.action_startup_enable = this.cboEnableStartup.Text;
        this.actionservice.action_revert = this.cboRevertAction.Text;
        this.actionservice.action_startup_revert = this.cboRevertStartup.Text;
        if (this.new_actionservice)
        {
          this.profile.addActionItem((Action) this.actionservice);
          Program.writeLog("Adding service action \"" + this.actionservice.getDispalyName() + "\"");
          Program.mainform.updateAfterSaveDelete();
        }
        else
        {
          Program.writeLog("Updating service action \"" + this.actionservice.getDispalyName() + "\"");
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
      if (DialogResult.Yes != MessageBox.Show("Delete service action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      this.profile.removeAction((Action) this.actionservice);
      Program.mainform.updateAfterSaveDelete();
      this.Close();
    }

    private void cboService_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cboService.SelectedItem.GetType() == typeof (ServiceInfo)))
        return;
      this.txtServiceName.Text = ((ServiceInfo) this.cboService.SelectedItem).servicename;
    }

    private void label5_Click(object sender, EventArgs e)
    {
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
      this.label4 = new Label();
      this.cboRevertStartup = new ComboBox();
      this.label5 = new Label();
      this.cboEnableStartup = new ComboBox();
      this.txtServiceName = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.btnDelete = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.cboService = new ComboBox();
      this.txtFriendlyName = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.label6.AutoSize = true;
      this.label6.Location = new Point(11, 204);
      this.label6.Name = "label6";
      this.label6.Size = new Size(104, 13);
      this.label6.TabIndex = 28;
      this.label6.Text = "Revert Profile Action";
      this.cboRevertAction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRevertAction.FormattingEnabled = true;
      this.cboRevertAction.Location = new Point(14, 220);
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
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 244);
      this.label4.Name = "label4";
      this.label4.Size = new Size(174, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "Revert Profile Action (Startup Type)";
      this.cboRevertStartup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRevertStartup.FormattingEnabled = true;
      this.cboRevertStartup.Location = new Point(14, 260);
      this.cboRevertStartup.Name = "cboRevertStartup";
      this.cboRevertStartup.Size = new Size(237, 21);
      this.cboRevertStartup.TabIndex = 23;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(11, 165);
      this.label5.Name = "label5";
      this.label5.Size = new Size(175, 13);
      this.label5.TabIndex = 22;
      this.label5.Text = "Enable Profile Action (Startup Type)";
      this.label5.Click += new EventHandler(this.label5_Click);
      this.cboEnableStartup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboEnableStartup.FormattingEnabled = true;
      this.cboEnableStartup.Location = new Point(14, 180);
      this.cboEnableStartup.Name = "cboEnableStartup";
      this.cboEnableStartup.Size = new Size(237, 21);
      this.cboEnableStartup.TabIndex = 21;
      this.txtServiceName.Location = new Point(15, 103);
      this.txtServiceName.Name = "txtServiceName";
      this.txtServiceName.ReadOnly = true;
      this.txtServiceName.Size = new Size(237, 20);
      this.txtServiceName.TabIndex = 17;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 87);
      this.label3.Name = "label3";
      this.label3.Size = new Size(43, 13);
      this.label3.TabIndex = 15;
      this.label3.Text = "Service";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Service Name";
      this.btnDelete.Location = new Point(176, 299);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 11;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click_1);
      this.btnSave.ImageKey = "(none)";
      this.btnSave.Location = new Point(14, 299);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 9;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click_1);
      this.btnCancel.Location = new Point(95, 299);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cboService.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboService.FormattingEnabled = true;
      this.cboService.Location = new Point(15, 63);
      this.cboService.Name = "cboService";
      this.cboService.Size = new Size(237, 21);
      this.cboService.TabIndex = 7;
      this.cboService.SelectedIndexChanged += new EventHandler(this.cboService_SelectedIndexChanged);
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
      this.ClientSize = new Size(268, 341);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.cboRevertAction);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.txtFriendlyName);
      this.Controls.Add((Control) this.cboEnableAction);
      this.Controls.Add((Control) this.cboService);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.cboRevertStartup);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.cboEnableStartup);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtServiceName);
      this.Controls.Add((Control) this.label3);
      this.Name = "ActionServiceForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ActionServiceForm";
      this.Load += new EventHandler(this.ActionServiceForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
