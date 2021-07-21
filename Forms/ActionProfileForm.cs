using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
  public class ActionProfileForm : Form
  {
    private Profile profile;
    private ActionProfile actionprofile;
    private bool new_actionprofile;
    private IContainer components;
    private Label label2;
    private Button btnDelete;
    private Button btnSave;
    private Button btnCancel;
    private ComboBox cboProfile;

    public ActionProfileForm(Profile profile, ActionProfile actionprofile)
    {
      this.InitializeComponent();
      this.new_actionprofile = false;
      this.profile = profile;
      this.actionprofile = actionprofile;
      this.Text = "Edit Profile Action";
    }

    public ActionProfileForm(Profile profile)
    {
      this.InitializeComponent();
      this.Text = "New Profile Action";
      this.new_actionprofile = true;
      this.actionprofile = new ActionProfile();
      this.profile = profile;
    }

    private void ActionProfileForm_Load(object sender, EventArgs e)
    {
      this.Icon = Resources.icon_profile_icon;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      if (this.new_actionprofile)
        this.btnDelete.Enabled = false;
      this.cboProfile.Items.Clear();
      foreach (Profile profile in Program.profilelist.profiles)
      {
        if (profile.guid != this.profile.guid)
          this.cboProfile.Items.Add((object) profile);
      }
      this.cboProfile.SelectedItem = (object) this.actionprofile.getProfile();
    }

    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
    {
      Graphics graphics = e.Graphics;
      Pen darkGray = Pens.DarkGray;
      Rectangle clipRectangle = e.ClipRectangle;
      int left = clipRectangle.Left;
      clipRectangle = e.ClipRectangle;
      int y1 = clipRectangle.Bottom - 1;
      clipRectangle = e.ClipRectangle;
      int right = clipRectangle.Right;
      clipRectangle = e.ClipRectangle;
      int y2 = clipRectangle.Bottom - 1;
      graphics.DrawLine(darkGray, left, y1, right, y2);
      this.OnPaint(e);
    }

    private void btnSave_Click_1(object sender, EventArgs e)
    {
      string text = "";
      if (this.cboProfile.SelectedIndex == -1)
        text += "Choose a profile\n";
      if (this.cboProfile.SelectedItem.GetType() == typeof (Profile))
      {
        Profile selectedItem = (Profile) this.cboProfile.SelectedItem;
        bool flag = false;
        foreach (Action actionitem in selectedItem.actionitems)
        {
          if (actionitem.GetType() == typeof (ActionProfile) && ((ActionProfile) actionitem).profileguid == this.profile.guid)
            flag = true;
        }
        if (flag)
          text += "Action will result in infinite loop\n";
      }
      if (string.IsNullOrEmpty(text))
      {
        this.actionprofile.profileguid = ((Profile) this.cboProfile.SelectedItem).guid;
        if (this.new_actionprofile)
        {
          this.profile.addActionItem((Action) this.actionprofile);
          Program.writeLog("Adding profile action \"" + this.actionprofile.getDispalyName() + "\"");
          Program.mainform.updateAfterSaveDelete();
        }
        else
        {
          Program.writeLog("Updating profile action \"" + this.actionprofile.getDispalyName() + "\"");
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
      if (DialogResult.Yes != MessageBox.Show("Delete profile action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      this.profile.removeAction((Action) this.actionprofile);
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
      this.label2 = new Label();
      this.btnDelete = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.cboProfile = new ComboBox();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(36, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Profile";
      this.btnDelete.Location = new Point(177, 69);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 11;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click_1);
      this.btnSave.ImageKey = "(none)";
      this.btnSave.Location = new Point(15, 69);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 9;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click_1);
      this.btnCancel.Location = new Point(96, 69);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cboProfile.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboProfile.FormattingEnabled = true;
      this.cboProfile.Location = new Point(15, 25);
      this.cboProfile.Name = "cboProfile";
      this.cboProfile.Size = new Size(237, 21);
      this.cboProfile.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(269, 107);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.cboProfile);
      this.Controls.Add((Control) this.btnCancel);
      this.Name = "ActionProfileForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ActionProfileForm";
      this.Load += new EventHandler(this.ActionProfileForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
