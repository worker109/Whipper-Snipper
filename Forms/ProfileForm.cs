using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
  public class ProfileForm : Form
  {
    private Profile profile;
    private bool new_profile;
    private IContainer components;
    private Label label1;
    private TextBox txtProfileName;
    private Button btnSave;
    private Button Cancel;
    private Button btnDelete;

    public ProfileForm(Profile profile)
    {
      this.InitializeComponent();
      this.new_profile = false;
      this.profile = profile;
      this.Text = "Edit Profile";
    }

    public ProfileForm()
    {
      this.InitializeComponent();
      this.Text = "New Profile";
      this.new_profile = true;
      this.profile = new Profile();
    }

    private void ProfileForm_Load(object sender, EventArgs e)
    {
      this.Icon = Resources.icon_profile_icon;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.txtProfileName.Text = this.profile.profilename;
      if (!this.new_profile)
        return;
      this.btnDelete.Enabled = false;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      string text = "";
      if (string.IsNullOrEmpty(this.txtProfileName.Text.Trim()))
        text += "Profile name cannot be blank\n";
      if (string.IsNullOrEmpty(text))
      {
        this.profile.setDisplayName(this.txtProfileName.Text.Trim());
        if (this.new_profile)
        {
          this.profile.setExpanded(true);
          Program.profilelist.addProfile(this.profile);
          Program.writeLog("Adding profile \"" + this.profile.getDispalyName() + "\"");
          Program.mainform.updateAfterSaveDelete();
          Program.mainform.setTreeSelectedProfile(this.profile);
        }
        else
        {
          Program.profilelist.sortProfiles();
          Program.writeLog("Updating profile \"" + this.profile.getDispalyName() + "\"");
          Program.mainform.updateAfterSaveDelete();
        }
        this.Close();
      }
      else
      {
        int num = (int) MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void Cancel_Click(object sender, EventArgs e) => this.Close();

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

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (DialogResult.Yes != MessageBox.Show("Delete profile?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      Program.profilelist.removeProfile(this.profile);
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
      this.label1 = new Label();
      this.txtProfileName = new TextBox();
      this.btnSave = new Button();
      this.Cancel = new Button();
      this.btnDelete = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(67, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Profile Name";
      this.txtProfileName.Location = new Point(15, 25);
      this.txtProfileName.Name = "txtProfileName";
      this.txtProfileName.Size = new Size(237, 20);
      this.txtProfileName.TabIndex = 3;
      this.btnSave.ImageKey = "(none)";
      this.btnSave.Location = new Point(15, 64);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.Cancel.Location = new Point(96, 64);
      this.Cancel.Name = "Cancel";
      this.Cancel.Size = new Size(75, 23);
      this.Cancel.TabIndex = 5;
      this.Cancel.Text = "Cancel";
      this.Cancel.UseVisualStyleBackColor = true;
      this.Cancel.Click += new EventHandler(this.Cancel_Click);
      this.btnDelete.Location = new Point(177, 64);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(75, 23);
      this.btnDelete.TabIndex = 6;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(272, 108);
      this.Controls.Add((Control) this.txtProfileName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.Cancel);
      this.Controls.Add((Control) this.btnSave);
      this.Name = "ProfileForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ProfileForm";
      this.Load += new EventHandler(this.ProfileForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
