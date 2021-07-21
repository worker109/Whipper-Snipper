using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Whipper_Snipper
{
  public class ActionPickerForm : Form
  {
    private Profile profile;
    private IContainer components;
    private Button btnHardware;
    private Button btnProcess;
    private Button btnService;
    private Label label3;
    private Label label2;
    private Label label1;
    private Label label4;
    private Button btnExecutable;
    private Label label5;
    private Button btnProfile;

    public ActionPickerForm(Profile profile)
    {
      this.profile = profile;
      this.InitializeComponent();
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

    private void ActionPickerForm_Load(object sender, EventArgs e)
    {
      this.ShowIcon = false;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.btnService.ImageList = MainForm.icons;
      this.btnService.ImageKey = "icon_service_small";
      this.btnProcess.ImageList = MainForm.icons;
      this.btnProcess.ImageKey = "icon_process_small";
      this.btnExecutable.ImageList = MainForm.icons;
      this.btnExecutable.ImageKey = "icon_executable_small";
      this.btnHardware.ImageList = MainForm.icons;
      this.btnHardware.ImageKey = "icon_hardware_small";
      this.btnProfile.ImageList = MainForm.icons;
      this.btnProfile.ImageKey = "icon_profile_small";
    }

    private void btnHardware_Click(object sender, EventArgs e)
    {
      Program.mainform.loadActionHardwareForm(this.profile);
      this.Close();
    }

    private void btnService_Click(object sender, EventArgs e)
    {
      Program.mainform.loadActionServiceForm(this.profile);
      this.Close();
    }

    private void btnProcess_Click(object sender, EventArgs e)
    {
      Program.mainform.loadActionProcessForm(this.profile);
      this.Close();
    }

    private void btnExecutable_Click(object sender, EventArgs e)
    {
       Program.mainform.loadActionExecutableForm(this.profile);
       this.Close();
    }

    private void btnProfile_Click(object sender, EventArgs e)
    {
      Program.mainform.loadActionProfileForm(this.profile);
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
            this.label4 = new System.Windows.Forms.Label();
            this.btnProfile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnService = new System.Windows.Forms.Button();
            this.btnHardware = new System.Windows.Forms.Button();
            this.btnExecutable = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Link another profile";
            // 
            // btnProfile
            // 
            this.btnProfile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProfile.ImageKey = "(none)";
            this.btnProfile.Location = new System.Drawing.Point(12, 164);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(138, 32);
            this.btnProfile.TabIndex = 6;
            this.btnProfile.Text = "Profile";
            this.btnProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProfile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Enable or disable hardware";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Kill or start processes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start and stop windows services, change the startup type";
            // 
            // btnProcess
            // 
            this.btnProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcess.ImageKey = "(none)";
            this.btnProcess.Location = new System.Drawing.Point(12, 88);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(138, 32);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "Process";
            this.btnProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnService
            // 
            this.btnService.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnService.ImageKey = "(none)";
            this.btnService.Location = new System.Drawing.Point(12, 12);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(138, 32);
            this.btnService.TabIndex = 1;
            this.btnService.Text = "Service";
            this.btnService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnService.UseVisualStyleBackColor = true;
            this.btnService.Click += new System.EventHandler(this.btnService_Click);
            // 
            // btnHardware
            // 
            this.btnHardware.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHardware.ImageKey = "(none)";
            this.btnHardware.Location = new System.Drawing.Point(12, 50);
            this.btnHardware.Name = "btnHardware";
            this.btnHardware.Size = new System.Drawing.Size(138, 32);
            this.btnHardware.TabIndex = 0;
            this.btnHardware.Text = "Hardware";
            this.btnHardware.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHardware.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHardware.UseVisualStyleBackColor = true;
            this.btnHardware.Click += new System.EventHandler(this.btnHardware_Click);
            // 
            // btnExecutable
            // 
            this.btnExecutable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExecutable.ImageKey = "(none)";
            this.btnExecutable.Location = new System.Drawing.Point(12, 126);
            this.btnExecutable.Name = "btnExecutable";
            this.btnExecutable.Size = new System.Drawing.Size(138, 32);
            this.btnExecutable.TabIndex = 8;
            this.btnExecutable.Text = "Executable";
            this.btnExecutable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExecutable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExecutable.UseVisualStyleBackColor = true;
            this.btnExecutable.Click += new System.EventHandler(this.btnExecutable_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(156, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Kill or start executable";
            // 
            // ActionPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 208);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnExecutable);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.btnService);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnHardware);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.label1);
            this.Name = "ActionPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose action type";
            this.Load += new System.EventHandler(this.ActionPickerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

        
    }
}
